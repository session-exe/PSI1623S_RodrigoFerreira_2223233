using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Data;
using System.Net.Mail;
using System.Text.RegularExpressions;


namespace OfiPecas
{
    public static class AuthService
    {
        // Cria um administrador
        public static (bool success, string message) CreateAdmin(
            string username,
            string email,
            string plainPassword,
            string recoveryKey)
        {
            if (string.IsNullOrWhiteSpace(username))
                return (false, "O campo 'Utilizador' é obrigatório.");
            if (string.IsNullOrWhiteSpace(email))
                return (false, "O campo 'Email' é obrigatório.");
            if (!IsValidEmail(email))
                return (false, "O formato do email é inválido.");
            if (string.IsNullOrWhiteSpace(plainPassword))
                return (false, "O campo 'Senha' é obrigatório.");
            if (string.IsNullOrWhiteSpace(recoveryKey))
                return (false, "A 'Chave de recuperação' é obrigatória.");

            var senhaHash = HashPassword(plainPassword);
            const string sql = @"
                INSERT INTO dbo.UTILIZADOR
                    (username, email, senha, chave_recuperacao, is_admin)
                VALUES
                    (@Username, @Email, @Senha, @ChaveRecuperacao, 1);
            ";
            try
            {
                using var conn = DatabaseConnection.GetConnection();
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Senha", senhaHash);
                cmd.Parameters.AddWithValue("@ChaveRecuperacao", recoveryKey);
                cmd.ExecuteNonQuery();
                return (true, "Administrador criado com sucesso.");
            }
            catch (SqlException ex) when (ex.Number == 2627)
            {
                if (ex.Message.Contains("UQ_UTILIZADOR_EMAIL"))
                    return (false, "Email já está associado a outra conta.");
                if (ex.Message.Contains("UQ_UTILIZADOR_USERNAME"))
                    return (false, "Utilizador já está em uso.");
                return (false, "Erro ao criar administrador: " + ex.Message);
            }
        }

        // Cria um cliente (utilizador normal)
        public static (bool success, string message) CreateUser(
            string username,
            string email,
            string plainPassword,
            string recoveryKey,
            string endereco,
            string nomeEmpresa,
            string telefone = null)
        {
            if (string.IsNullOrWhiteSpace(username))
                return (false, "O campo 'Utilizador' é obrigatório.");
            if (string.IsNullOrWhiteSpace(email))
                return (false, "O campo 'Email' é obrigatório.");
            if (!IsValidEmail(email))
                return (false, "O formato do email é inválido.");
            if (string.IsNullOrWhiteSpace(plainPassword))
                return (false, "O campo 'Senha' é obrigatório.");
            if (string.IsNullOrWhiteSpace(recoveryKey))
                return (false, "A 'Chave de recuperação' é obrigatória.");
            if (string.IsNullOrWhiteSpace(endereco))
                return (false, "O campo 'Endereço' é obrigatório.");
            if (string.IsNullOrWhiteSpace(nomeEmpresa))
                return (false, "O campo 'Nome da Empresa' é obrigatório.");
            if (!string.IsNullOrWhiteSpace(telefone) && !IsValidPhone(telefone))
                return (false, "O formato do telefone é inválido.");

            var senhaHash = HashPassword(plainPassword);
            const string sql = @"
                INSERT INTO dbo.UTILIZADOR
                    (username, email, senha, chave_recuperacao, is_admin, endereco, nome_empresa, telefone)
                VALUES
                    (@Username, @Email, @Senha, @ChaveRecuperacao, 0, @Endereco, @NomeEmpresa, @Telefone);
            ";
            try
            {
                using var conn = DatabaseConnection.GetConnection();
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Senha", senhaHash);
                cmd.Parameters.AddWithValue("@ChaveRecuperacao", recoveryKey);
                cmd.Parameters.AddWithValue("@Endereco", endereco);
                cmd.Parameters.AddWithValue("@NomeEmpresa", nomeEmpresa);
                cmd.Parameters.AddWithValue("@Telefone", (object)telefone ?? DBNull.Value);
                cmd.ExecuteNonQuery();
                return (true, "Utilizador criado com sucesso.");
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627)
                {
                    if (ex.Message.Contains("UQ_UTILIZADOR_EMAIL"))
                        return (false, "Email já está associado a outra conta.");
                    if (ex.Message.Contains("UQ_UTILIZADOR_USERNAME"))
                        return (false, "Utilizador já está em uso.");
                }
                if (ex.Number == 515)
                    return (false, "Não foi possível criar a conta: campos obrigatórios ausentes.");
                return (false, "Erro ao criar utilizador: " + ex.Message);
            }
        }

        // Faz login por utilizador ou email
        public static (bool success, string message, int userId, bool isAdmin) Login(
            string login,
            string plainPassword)
        {
            const string sql = @"
                SELECT id_utilizador, senha, is_admin
                FROM dbo.UTILIZADOR
                WHERE username = @Login OR email = @Login;
            ";
            try
            {
                using var conn = DatabaseConnection.GetConnection();
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Login", login);
                using var reader = cmd.ExecuteReader();
                if (!reader.Read())
                    return (false, "Utilizador não encontrado.", 0, false);

                int userId = reader.GetInt32(0);
                string storedHash = reader.GetString(1);
                bool isAdmin = reader.GetBoolean(2);
                if (!VerifyPassword(plainPassword, storedHash))
                    return (false, "Senha incorreta.", 0, false);
                return (true, "Login efetuado com sucesso.", userId, isAdmin);
            }
            catch (Exception ex)
            {
                return (false, "Erro ao efetuar login: " + ex.Message, 0, false);
            }
        }

        // Valida formato de email
        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        // Valida formato de telefone (apenas dígitos, entre 9 e 15 caracteres)
        private static bool IsValidPhone(string phone)
        {
            var digitsOnly = Regex.Replace(phone, "\\D", "");
            return digitsOnly.Length >= 9 && digitsOnly.Length <= 15;
        }

        // Gera hash da senha com salt e 100k iterações
        private static string HashPassword(string plain)
        {
            byte[] salt = new byte[16];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt);
            using var pbkdf2 = new Rfc2898DeriveBytes(plain, salt, 100000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(32);
            byte[] result = new byte[48];
            Array.Copy(salt, 0, result, 0, 16);
            Array.Copy(hash, 0, result, 16, 32);
            return Convert.ToBase64String(result);
        }

        // Verifica se a senha corresponde ao hash armazenado
        public static bool VerifyPassword(string plain, string storedSaltHash)
        {
            byte[] saltHash = Convert.FromBase64String(storedSaltHash);
            byte[] salt = new byte[16];
            Array.Copy(saltHash, 0, salt, 0, 16);
            byte[] storedHash = new byte[32];
            Array.Copy(saltHash, 16, storedHash, 0, 32);
            using var pbkdf2 = new Rfc2898DeriveBytes(plain, salt, 100000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(32);
            for (int i = 0; i < 32; i++)
                if (hash[i] != storedHash[i])
                    return false;
            return true;
        }
    }
}

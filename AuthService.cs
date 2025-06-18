using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;


namespace OfiPecas.Services
{
    public static class AuthService
    {
  

        /// Cria um utilizador administrador
        public static (bool success, string message) CreateAdmin(
            string username,
            string email,
            string plainPassword,
            string recoveryKey)
        {
            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(plainPassword) ||
                string.IsNullOrWhiteSpace(recoveryKey))
            {
                return (false, "Username, email, senha e recovery key são obrigatórios.");
            }

            var senhaHash = HashPassword(plainPassword);

            const string sql = @"
                INSERT INTO dbo.UTILIZADOR (username, email, senha, chave_recuperacao, is_admin)
                VALUES (@Username, @Email, @Senha, @ChaveRecuperacao, 1);
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
                    return (false, "Username já está em uso.");
                return (false, "Violação de dados: " + ex.Message);
            }
            catch (Exception ex)
            {
                return (false, "Erro: " + ex.Message);
            }
        }


        /// Cria um utilizador normal (cliente)
        public static (bool success, string message) CreateUser(
            string username,
            string email,
            string plainPassword,
            string recoveryKey,
            string endereco,
            string nomeEmpresa,
            string telefone = "")
        {
            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(plainPassword) ||
                string.IsNullOrWhiteSpace(recoveryKey))
            {
                return (false, "Username, email, senha e recovery key são obrigatórios.");
            }
            if (string.IsNullOrWhiteSpace(endereco) || string.IsNullOrWhiteSpace(nomeEmpresa))
                return (false, "Endereço e nome da empresa são obrigatórios.");

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
            catch (SqlException ex) when (ex.Number == 2627)
            {
                if (ex.Message.Contains("UQ_UTILIZADOR_EMAIL"))
                    return (false, "Email já está associado a outra conta.");
                if (ex.Message.Contains("UQ_UTILIZADOR_USERNAME"))
                    return (false, "Username já está em uso.");
                return (false, "Violação de dados: " + ex.Message);
            }
            catch (Exception ex)
            {
                return (false, "Erro: " + ex.Message);
            }
        }


        /// Efetua login de um utilizador (por username ou email).
        public static (bool success, string message, int userId, bool isAdmin) Login(string login, string plainPassword)
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
                    return (false, "Usuário não encontrado.", 0, false);

                int userId = reader.GetInt32(0);
                string storedHash = reader.GetString(1);
                bool isAdmin = reader.GetBoolean(2);

                if (!VerifyPassword(plainPassword, storedHash))
                    return (false, "Senha incorreta.", 0, false);

                return (true, "Login efetuado com sucesso.", userId, isAdmin);
            }
            catch (Exception ex)
            {
                return (false, "Erro: " + ex.Message, 0, false);
            }
        }


        /// Gera um hash da senha usando PBKDF2 + SHA256 + salt.
        private static string HashPassword(string plain)
        {
            byte[] salt = new byte[16];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt);
            using var pbkdf2 = new Rfc2898DeriveBytes(plain, salt, 10000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(32);
            byte[] res = new byte[48];
            Array.Copy(salt, 0, res, 0, 16);
            Array.Copy(hash, 0, res, 16, 32);
            return Convert.ToBase64String(res);
        }


        /// Verifica se a senha corresponde ao hash armazenado.
        public static bool VerifyPassword(string plain, string storedSaltHash)
        {
            byte[] saltHash = Convert.FromBase64String(storedSaltHash);
            byte[] salt = new byte[16];
            Array.Copy(saltHash, 0, salt, 0, 16);
            byte[] storedHash = new byte[32];
            Array.Copy(saltHash, 16, storedHash, 0, 32);
            using var pbkdf2 = new Rfc2898DeriveBytes(plain, salt, 10000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(32);
            for (int i = 0; i < 32; i++)
                if (hash[i] != storedHash[i])
                    return false;
            return true;
        }


    }
}

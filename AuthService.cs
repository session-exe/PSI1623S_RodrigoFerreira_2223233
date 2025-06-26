using Microsoft.Data.SqlClient;
using System;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Net.Mail; // Necessário para a validação de email

namespace OfiPecas
{
    // Esta classe estática centraliza toda a lógica de autenticação.
    public static class AuthService
    {
        // Cria um novo utilizador com permissões de administrador.
        internal static (bool success, string message) CreateAdmin(string username, string email, string plainPassword, string recoveryKey)
        {
            // Validações iniciais para garantir que os dados essenciais não estão vazios.
            if (string.IsNullOrWhiteSpace(username)) return (false, "O campo 'Utilizador' é obrigatório.");
            if (string.IsNullOrWhiteSpace(email)) return (false, "O campo 'Email' é obrigatório.");
            if (!IsValidEmail(email)) return (false, "O formato do email é inválido.");
            if (string.IsNullOrWhiteSpace(plainPassword)) return (false, "O campo 'Senha' é obrigatório.");
            if (string.IsNullOrWhiteSpace(recoveryKey)) return (false, "A 'Chave de recuperação' é obrigatória.");

            // Transforma a password em texto simples num "hash" seguro antes de a guardar.
            var senhaHash = HashPassword(plainPassword);

            // Query SQL para inserir o novo administrador na base de dados.
            const string sql = @"
                INSERT INTO dbo.UTILIZADOR (username, email, senha, chave_recuperacao, is_admin)
                VALUES (@Username, @Email, @Senha, @ChaveRecuperacao, 1);
            ";
            try
            {
                using var conn = DatabaseConnection.GetConnection();
                using var cmd = new SqlCommand(sql, conn);
                // Adiciona os parâmetros ao comando para evitar SQL Injection.
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Senha", senhaHash);
                cmd.Parameters.AddWithValue("@ChaveRecuperacao", recoveryKey);

                cmd.ExecuteNonQuery(); // Executa o comando na base de dados.
                return (true, "Administrador criado com sucesso.");
            }
            // Captura erros específicos do SQL Server, como a tentativa de inserir um username ou email que já existe.
            catch (SqlException ex) when (ex.Number == 2627)
            {
                if (ex.Message.Contains("UQ_UTILIZADOR_EMAIL")) return (false, "Email já está associado a outra conta.");
                if (ex.Message.Contains("UQ_UTILIZADOR_USERNAME")) return (false, "Utilizador já está em uso.");
                return (false, "Erro ao criar administrador: " + ex.Message);
            }
        }

        // Cria um novo utilizador (cliente).
        public static (bool success, string message) CreateUser(string username, string email, string plainPassword, string recoveryKey, string endereco, string nomeEmpresa, string telefone = null)
        {
            // Validações semelhantes ao CreateAdmin, mas com os campos extra do cliente.
            if (string.IsNullOrWhiteSpace(username)) return (false, "O campo 'Utilizador' é obrigatório.");
            if (string.IsNullOrWhiteSpace(email)) return (false, "O campo 'Email' é obrigatório.");
            if (!IsValidEmail(email)) return (false, "O formato do email é inválido.");
            if (string.IsNullOrWhiteSpace(plainPassword)) return (false, "O campo 'Senha' é obrigatório.");
            if (string.IsNullOrWhiteSpace(recoveryKey)) return (false, "A 'Chave de recuperação' é obrigatória.");
            if (string.IsNullOrWhiteSpace(endereco)) return (false, "O campo 'Endereço' é obrigatório.");
            if (string.IsNullOrWhiteSpace(nomeEmpresa)) return (false, "O campo 'Nome da Empresa' é obrigatório.");
            if (!string.IsNullOrWhiteSpace(telefone) && !IsValidPhone(telefone)) return (false, "O formato do telefone é inválido.");

            var senhaHash = HashPassword(plainPassword);
            const string sql = @"
                INSERT INTO dbo.UTILIZADOR (username, email, senha, chave_recuperacao, is_admin, endereco, nome_empresa, telefone)
                VALUES (@Username, @Email, @Senha, @ChaveRecuperacao, 0, @Endereco, @NomeEmpresa, @Telefone);
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
                // Trata o campo opcional 'telefone'. Se for nulo ou vazio, insere DBNull na base de dados.
                cmd.Parameters.AddWithValue("@Telefone", (object)telefone ?? DBNull.Value);

                cmd.ExecuteNonQuery();
                return (true, "Utilizador criado com sucesso.");
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627) // Username ou Email duplicado
                {
                    if (ex.Message.Contains("UQ_UTILIZADOR_EMAIL")) return (false, "Email já está associado a outra conta.");
                    if (ex.Message.Contains("UQ_UTILIZADOR_USERNAME")) return (false, "Utilizador já está em uso.");
                }
                if (ex.Number == 515) return (false, "Não foi possível criar a conta: campos obrigatórios ausentes.");
                return (false, "Erro ao criar utilizador: " + ex.Message);
            }
        }

        // Verifica as credenciais do utilizador para permitir o acesso.
        public static (bool success, string message, int userId, bool isAdmin) Login(string login, string plainPassword)
        {
            // O utilizador pode fazer login com o seu username OU com o seu email.
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
                // Se não encontrar nenhuma linha, o utilizador não existe.
                if (!reader.Read())
                {
                    return (false, "Utilizador não encontrado.", 0, false);
                }

                // Lê os dados da base de dados.
                int userId = reader.GetInt32(0);
                string storedHash = reader.GetString(1);
                bool isAdmin = reader.GetBoolean(2);

                // Compara a password fornecida com o hash guardado.
                if (!VerifyPassword(plainPassword, storedHash))
                {
                    return (false, "Senha incorreta.", 0, false);
                }

                // Se tudo estiver correto, devolve sucesso e os dados do utilizador.
                return (true, "Login efetuado com sucesso.", userId, isAdmin);
            }
            catch (Exception ex)
            {
                return (false, "Erro ao efetuar login: " + ex.Message, 0, false);
            }
        }

        // Permite ao utilizador definir uma nova password se fornecer os dados de recuperação corretos.
        public static (bool success, string message) RecoverPassword(string login, string recoveryKey, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(login)) return (false, "O campo 'Utilizador' é obrigatório.");
            if (string.IsNullOrWhiteSpace(recoveryKey)) return (false, "A 'Chave de recuperação' é obrigatória.");
            if (string.IsNullOrWhiteSpace(newPassword)) return (false, "O campo 'Nova Password' é obrigatório.");

            // Primeiro, verifica se existe um utilizador que corresponda ao login E à chave de recuperação.
            const string selectSql = @"
                SELECT id_utilizador FROM dbo.UTILIZADOR
                WHERE (username = @Login OR email = @Login) AND chave_recuperacao = @Key;
            ";
            using var conn = DatabaseConnection.GetConnection();
            using var selectCmd = new SqlCommand(selectSql, conn);
            selectCmd.Parameters.AddWithValue("@Login", login);
            selectCmd.Parameters.AddWithValue("@Key", recoveryKey);

            var userIdObj = selectCmd.ExecuteScalar(); // ExecuteScalar é eficiente para obter um único valor.
            if (userIdObj == null)
            {
                return (false, "Dados de recuperação inválidos.");
            }

            // Se os dados estiverem corretos, atualiza a password para o novo hash.
            int userId = Convert.ToInt32(userIdObj);
            var senhaHash = HashPassword(newPassword);

            const string updateSql = "UPDATE dbo.UTILIZADOR SET senha = @Senha WHERE id_utilizador = @UserId;";
            using var updateCmd = new SqlCommand(updateSql, conn);
            updateCmd.Parameters.AddWithValue("@Senha", senhaHash);
            updateCmd.Parameters.AddWithValue("@UserId", userId);

            int rows = updateCmd.ExecuteNonQuery();
            return rows == 1 ? (true, "Senha alterada com sucesso.") : (false, "Erro ao atualizar a senha.");
        }

        // --- MÉTODOS DE AJUDA INTERNOS ---

        // Valida o formato de um email. É 'internal' para poder ser usado por outras classes de serviço.
        internal static bool IsValidEmail(string email)
        {
            try { var addr = new MailAddress(email); return addr.Address == email; }
            catch { return false; }
        }

        // Valida o formato de um telefone.
        internal static bool IsValidPhone(string phone)
        {
            var digitsOnly = Regex.Replace(phone, "\\D", "");
            return digitsOnly.Length >= 9 && digitsOnly.Length <= 15;
        }

        // Gera um hash seguro de uma password 
        internal static string HashPassword(string plain)
        {
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create()) { rng.GetBytes(salt); }
            using (var pbkdf2 = new Rfc2898DeriveBytes(plain, salt, 100000, HashAlgorithmName.SHA256))
            {
                byte[] hash = pbkdf2.GetBytes(32);
                byte[] result = new byte[48];
                Buffer.BlockCopy(salt, 0, result, 0, 16);
                Buffer.BlockCopy(hash, 0, result, 16, 32);
                return Convert.ToBase64String(result);
            }
        }

        // Verifica se uma password em texto simples corresponde a um hash guardado.
        public static bool VerifyPassword(string plain, string storedSaltHash)
        {
            try
            {
                byte[] saltHash = Convert.FromBase64String(storedSaltHash);
                byte[] salt = new byte[16];
                Buffer.BlockCopy(saltHash, 0, salt, 0, 16);
                byte[] storedHashBytes = new byte[32];
                Buffer.BlockCopy(saltHash, 16, storedHashBytes, 0, 32);

                // Gera um novo hash da password fornecida, usando o mesmo "salt" que foi guardado.
                using (var pbkdf2 = new Rfc2898DeriveBytes(plain, salt, 100000, HashAlgorithmName.SHA256))
                {
                    byte[] hash = pbkdf2.GetBytes(32);
                    // Compara os dois hashes. Se forem idênticos, a password está correta.
                    for (int i = 0; i < 32; i++)
                    {
                        if (hash[i] != storedHashBytes[i]) return false;
                    }
                }
                return true;
            }
            catch { return false; } // Se o formato do hash for inválido, retorna falso por segurança.
        }
    }
}

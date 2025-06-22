using Microsoft.Data.SqlClient;
using System;
using System.Windows.Forms;

namespace OfiPecas
{
    // Responsável por todas as operações relacionadas com os dados do utilizador
    public static class UserService 
    {
        // Vai buscar os dados de um utilizador específico à BD
        public static UserInfo GetUserData(int userId)
        {
            UserInfo userData = null;
            string sql = "SELECT username, email, nome_empresa, endereco, telefone FROM dbo.UTILIZADOR WHERE id_utilizador = @UserId";

            try
            {
                using var conn = DatabaseConnection.GetConnection();
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@UserId", userId);

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    userData = new UserInfo
                    {
                        Username = reader["username"].ToString(),
                        Email = reader["email"].ToString(),
                        NomeEmpresa = reader["nome_empresa"].ToString(),
                        Endereco = reader["endereco"].ToString(),
                        Telefone = reader["telefone"].ToString()
                    };
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar dados do utilizador: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return userData;
        }

        // Atualiza os dados pessoais de um utilizador
        public static (bool success, string message) UpdateUserData(int userId, string email, string nomeEmpresa, string endereco, string telefone)
        {
            // Validações básicas
            if (!AuthService.IsValidEmail(email)) return (false, "O formato do email é inválido.");
            if (!string.IsNullOrWhiteSpace(telefone) && !AuthService.IsValidPhone(telefone)) return (false, "O formato do telefone é inválido.");

            string sql = @"
                UPDATE dbo.UTILIZADOR 
                SET email = @Email, nome_empresa = @NomeEmpresa, endereco = @Endereco, telefone = @Telefone 
                WHERE id_utilizador = @UserId";
            try
            {
                using var conn = DatabaseConnection.GetConnection();
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@NomeEmpresa", nomeEmpresa);
                cmd.Parameters.AddWithValue("@Endereco", endereco);
                cmd.Parameters.AddWithValue("@Telefone", (object)telefone ?? DBNull.Value);

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0
                    ? (true, "Dados atualizados com sucesso!")
                    : (false, "Não foi possível atualizar os dados.");
            }
            catch (SqlException ex) when (ex.Number == 2627) // Erro de chave única (email duplicado)
            {
                return (false, "O email introduzido já está associado a outra conta.");
            }
            catch (Exception ex)
            {
                return (false, $"Erro ao atualizar dados: {ex.Message}");
            }
        }

        // Altera a password de um utilizador
        public static (bool success, string message) ChangePassword(int userId, string oldPassword, string newPassword)
        {
            // Validações
            if (string.IsNullOrWhiteSpace(oldPassword) || string.IsNullOrWhiteSpace(newPassword))
            {
                return (false, "Todos os campos de password são obrigatórios.");
            }

            // Vai buscar a password atual à BD para a verificar
            string storedHash = "";
            string sqlSelect = "SELECT senha FROM dbo.UTILIZADOR WHERE id_utilizador = @UserId";
            try
            {
                using var conn = DatabaseConnection.GetConnection();
                using var cmdSelect = new SqlCommand(sqlSelect, conn);
                cmdSelect.Parameters.AddWithValue("@UserId", userId);
                var result = cmdSelect.ExecuteScalar();
                if (result != null)
                {
                    storedHash = result.ToString();
                }
                else
                {
                    return (false, "Utilizador não encontrado.");
                }

                // Verifica se a password antiga está correta usando o método do AuthService
                if (!AuthService.VerifyPassword(oldPassword, storedHash))
                {
                    return (false, "A sua password atual está incorreta.");
                }

                // Se estiver correta, faz o hash da nova password e atualiza na BD
                string newHash = AuthService.HashPassword(newPassword);
                string sqlUpdate = "UPDATE dbo.UTILIZADOR SET senha = @NewPassword WHERE id_utilizador = @UserId";
                using var cmdUpdate = new SqlCommand(sqlUpdate, conn);
                cmdUpdate.Parameters.AddWithValue("@NewPassword", newHash);
                cmdUpdate.Parameters.AddWithValue("@UserId", userId);

                cmdUpdate.ExecuteNonQuery();
                return (true, "Password alterada com sucesso!");
            }
            catch (Exception ex)
            {
                return (false, $"Erro ao alterar a password: {ex.Message}");
            }
        }

        // Apaga a conta de um utilizador
        public static (bool success, string message) DeleteAccount(int userId, string password)
        {
            // Primeiro, verifica se a password fornecida está correta
            string storedHash = "";
            string sqlSelect = "SELECT senha FROM dbo.UTILIZADOR WHERE id_utilizador = @UserId";
            try
            {
                using var conn = DatabaseConnection.GetConnection();
                using var cmdSelect = new SqlCommand(sqlSelect, conn);
                cmdSelect.Parameters.AddWithValue("@UserId", userId);
                var result = cmdSelect.ExecuteScalar();

                if (result == null || !AuthService.VerifyPassword(password, result.ToString()))
                {
                    return (false, "Password incorreta. A conta não foi apagada.");
                }

                // Se a password estiver correta, apaga o utilizador.
                // Graças ao 'ON DELETE CASCADE' na tabela CARRINHO, o carrinho e os seus itens serão apagados automaticamente.
                // As encomendas NÃO serão apagadas (ON DELETE NO ACTION).
                string sqlDelete = "DELETE FROM dbo.UTILIZADOR WHERE id_utilizador = @UserId";
                using var cmdDelete = new SqlCommand(sqlDelete, conn);
                cmdDelete.Parameters.AddWithValue("@UserId", userId);

                cmdDelete.ExecuteNonQuery();
                return (true, "Conta apagada com sucesso.");

            }
            catch (Exception ex)
            {
                return (false, $"Erro ao apagar a conta: {ex.Message}");
            }
        }
    }
}

using Microsoft.Data.SqlClient;
using System;
using System.Windows.Forms;

namespace OfiPecas
{
    // Responsável por todas as operações relacionadas com os dados do utilizador
    public static class UserService
    {
        // Vai buscar os dados completos do utilizador
        public static UserInfo GetUserData(int userId)
        {
            UserInfo userData = null;
            string sql = "SELECT username, email, chave_recuperacao, nome_empresa, endereco, telefone FROM dbo.UTILIZADOR WHERE id_utilizador = @UserId";

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
                        RecoveryKey = reader["chave_recuperacao"].ToString(),
                        Email = reader["email"].ToString(),
                        NomeEmpresa = reader["nome_empresa"].ToString(),
                        Endereco = reader["endereco"].ToString(),
                        Telefone = reader["telefone"].ToString()
                    };
                }
            }
            catch (Exception ex) { MessageBox.Show($"Erro ao carregar dados do utilizador: {ex.Message}"); }
            return userData;
        }

        // Atualiza os dados pessoais de um utilizador
        public static (bool success, string message) UpdateUserData(int userId, string email, string nomeEmpresa, string endereco, string telefone)
        {
            if (!AuthService.IsValidEmail(email)) return (false, "O formato do email é inválido.");
            if (!string.IsNullOrWhiteSpace(telefone) && !AuthService.IsValidPhone(telefone)) return (false, "O formato do telefone é inválido.");

            string sql = "UPDATE dbo.UTILIZADOR SET email = @Email, nome_empresa = @NomeEmpresa, endereco = @Endereco, telefone = @Telefone WHERE id_utilizador = @UserId";
            try
            {
                using var conn = DatabaseConnection.GetConnection();
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@NomeEmpresa", nomeEmpresa);
                cmd.Parameters.AddWithValue("@Endereco", endereco);
                cmd.Parameters.AddWithValue("@Telefone", (object)telefone ?? DBNull.Value);

                cmd.ExecuteNonQuery();
                return (true, "Dados atualizados com sucesso!");
            }
            catch (SqlException ex) when (ex.Number == 2627) { return (false, "O email introduzido já está associado a outra conta."); }
            catch (Exception ex) { return (false, $"Erro ao atualizar dados: {ex.Message}"); }
        }

        // Altera a password de um utilizador
        public static (bool success, string message) ChangePassword(int userId, string oldPassword, string newPassword)
        {
            string sqlSelect = "SELECT senha FROM dbo.UTILIZADOR WHERE id_utilizador = @UserId";
            try
            {
                using var conn = DatabaseConnection.GetConnection();
                using var cmdSelect = new SqlCommand(sqlSelect, conn);
                cmdSelect.Parameters.AddWithValue("@UserId", userId);
                var result = cmdSelect.ExecuteScalar();

                if (result == null || !AuthService.VerifyPassword(oldPassword, result.ToString()))
                {
                    return (false, "A sua password atual está incorreta.");
                }

                string newHash = AuthService.HashPassword(newPassword);
                string sqlUpdate = "UPDATE dbo.UTILIZADOR SET senha = @NewPassword WHERE id_utilizador = @UserId";
                using var cmdUpdate = new SqlCommand(sqlUpdate, conn);
                cmdUpdate.Parameters.AddWithValue("@NewPassword", newHash);
                cmdUpdate.Parameters.AddWithValue("@UserId", userId);
                cmdUpdate.ExecuteNonQuery();
                return (true, "Password alterada com sucesso!");
            }
            catch (Exception ex) { return (false, $"Erro ao alterar a password: {ex.Message}"); }
        }

        // Apaga a conta de um utilizador de forma segura
        public static (bool success, string message) DeleteAccount(int userId, string password)
        {
            using var conn = DatabaseConnection.GetConnection();
            try
            {
                string sqlSelect = "SELECT senha FROM dbo.UTILIZADOR WHERE id_utilizador = @UserId";
                using var cmdSelect = new SqlCommand(sqlSelect, conn);
                cmdSelect.Parameters.AddWithValue("@UserId", userId);
                var result = cmdSelect.ExecuteScalar();

                if (result == null || !AuthService.VerifyPassword(password, result.ToString()))
                {
                    return (false, "Password incorreta. A conta não foi apagada.");
                }

                conn.Close(); // Fecha a conexão para começar uma nova com transação
                conn.Open();

                using (var transaction = conn.BeginTransaction())
                {
                    string sqlDeleteItensEncomenda = "DELETE FROM dbo.ITEM_ENCOMENDA WHERE id_encomenda IN (SELECT id_encomenda FROM dbo.ENCOMENDA WHERE id_utilizador = @UserId)";
                    using (var cmd = new SqlCommand(sqlDeleteItensEncomenda, conn, transaction)) { cmd.Parameters.AddWithValue("@UserId", userId); cmd.ExecuteNonQuery(); }

                    string sqlDeleteEncomendas = "DELETE FROM dbo.ENCOMENDA WHERE id_utilizador = @UserId";
                    using (var cmd = new SqlCommand(sqlDeleteEncomendas, conn, transaction)) { cmd.Parameters.AddWithValue("@UserId", userId); cmd.ExecuteNonQuery(); }

                    string sqlDeleteUser = "DELETE FROM dbo.UTILIZADOR WHERE id_utilizador = @UserId";
                    using (var cmd = new SqlCommand(sqlDeleteUser, conn, transaction)) { cmd.Parameters.AddWithValue("@UserId", userId); cmd.ExecuteNonQuery(); }

                    transaction.Commit();
                    return (true, "Conta apagada com sucesso.");
                }
            }
            catch (Exception ex)
            {
                return (false, $"Erro ao apagar a conta: {ex.Message}");
            }
        }
    }
}

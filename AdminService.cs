using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace OfiPecas
{
    // Responsável por todas as operações do painel de administração
    public static class AdminService
    {
        // --- GESTÃO DE PEÇAS ---
        public static (bool success, string message) GuardarPeca(int pecaId, string nome, decimal preco, int estoque, int categoriaId, byte[] imagemBytes)
        {
            // ... (código existente, sem alterações)
            return (true, ""); // Placeholder
        }

        public static (bool success, string message) ApagarPeca(int pecaId)
        {
            // ... (código existente, sem alterações)
            return (true, ""); // Placeholder
        }

        // --- GESTÃO DE CATEGORIAS ---
        public static (bool success, string message) GuardarCategoria(int categoriaId, string nome)
        {
            // ... (código existente, sem alterações)
            return (true, ""); // Placeholder
        }

        public static (bool success, string message) ApagarCategoria(int categoriaId)
        {
            // ... (código existente, sem alterações)
            return (true, ""); // Placeholder
        }


        // --- GESTÃO DE UTILIZADORES ---

        // Devolve a lista de todos os utilizadores, AGORA USANDO A CLASSE UserInfo
        public static List<UserInfo> GetUtilizadores(string searchTerm = null)
        {
            var users = new List<UserInfo>();
            // Query atualizada para ir buscar todos os campos necessários
            string sql = "SELECT id_utilizador, username, email, is_admin FROM dbo.UTILIZADOR";
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                sql += " WHERE username LIKE @SearchTerm OR email LIKE @SearchTerm";
            }

            try
            {
                using var conn = DatabaseConnection.GetConnection();
                using var cmd = new SqlCommand(sql, conn);
                if (!string.IsNullOrWhiteSpace(searchTerm)) { cmd.Parameters.AddWithValue("@SearchTerm", $"%{searchTerm}%"); }

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    // Preenche o nosso modelo UserInfo completo
                    users.Add(new UserInfo
                    {
                        Id = reader.GetInt32("id_utilizador"),
                        Username = reader.GetString("username"),
                        Email = reader.GetString("email"),
                        IsAdmin = reader.GetBoolean("is_admin")
                        // As outras propriedades (morada, telefone, etc.) não são precisas para a lista, por isso ficam vazias
                    });
                }
            }
            catch (Exception ex) { MessageBox.Show($"Erro ao buscar utilizadores: {ex.Message}"); }
            return users;
        }

        // Atribui ou remove o status de administrador
        public static (bool success, string message) SetAdminStatus(int userId, bool isAdmin)
        {
            string sql = "UPDATE dbo.UTILIZADOR SET is_admin = @IsAdmin WHERE id_utilizador = @UserId";
            try
            {
                using var conn = DatabaseConnection.GetConnection();
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@IsAdmin", isAdmin);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.ExecuteNonQuery();
                return (true, "Status de administrador atualizado com sucesso.");
            }
            catch (Exception ex) { return (false, $"Erro ao atualizar status: {ex.Message}"); }
        }
    }
}

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
            if (string.IsNullOrWhiteSpace(nome) || preco <= 0 || categoriaId <= 0)
            {
                return (false, "Nome, preço e categoria são obrigatórios.");
            }

            try
            {
                using var conn = DatabaseConnection.GetConnection();
                string sql;
                bool isCreating = (pecaId == 0);

                if (isCreating)
                {
                    if (imagemBytes == null || imagemBytes.Length == 0)
                        return (false, "Uma imagem é obrigatória ao criar uma nova peça.");

                    sql = "INSERT INTO dbo.PECA (nome, preco, estoque, id_categoria, imagem) VALUES (@Nome, @Preco, @Estoque, @CategoriaId, @Imagem)";
                }
                else
                {
                    sql = (imagemBytes != null && imagemBytes.Length > 0)
                        ? "UPDATE dbo.PECA SET nome = @Nome, preco = @Preco, estoque = @Estoque, id_categoria = @CategoriaId, imagem = @Imagem WHERE id_peca = @PecaId"
                        : "UPDATE dbo.PECA SET nome = @Nome, preco = @Preco, estoque = @Estoque, id_categoria = @CategoriaId WHERE id_peca = @PecaId";
                }

                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Nome", nome);
                cmd.Parameters.AddWithValue("@Preco", preco);
                cmd.Parameters.AddWithValue("@Estoque", estoque);
                cmd.Parameters.AddWithValue("@CategoriaId", categoriaId);

                if (!isCreating) cmd.Parameters.AddWithValue("@PecaId", pecaId);
                if (imagemBytes != null && imagemBytes.Length > 0) cmd.Parameters.AddWithValue("@Imagem", imagemBytes);

                cmd.ExecuteNonQuery();
                return (true, "Peça guardada com sucesso!");
            }
            catch (Exception ex)
            {
                return (false, $"Erro ao guardar a peça: {ex.Message}");
            }
        }

        public static (bool success, string message) ApagarPeca(int pecaId)
        {
            if (pecaId == 0) return (false, "Nenhuma peça selecionada.");
            string sql = "DELETE FROM dbo.PECA WHERE id_peca = @PecaId";
            try
            {
                using var conn = DatabaseConnection.GetConnection();
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@PecaId", pecaId);
                cmd.ExecuteNonQuery();
                return (true, "Peça apagada com sucesso.");
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                return (false, "Não é possível apagar esta peça, pois ela já está associada a encomendas ou carrinhos.");
            }
            catch (Exception ex)
            {
                return (false, $"Erro ao apagar a peça: {ex.Message}");
            }
        }

        // --- GESTÃO DE CATEGORIAS ---

        public static (bool success, string message) GuardarCategoria(int categoriaId, string nome)
        {
            if (string.IsNullOrWhiteSpace(nome)) return (false, "O nome da categoria é obrigatório.");

            string sql = (categoriaId == 0)
                ? "INSERT INTO dbo.CATEGORIA (nome) VALUES (@Nome)"
                : "UPDATE dbo.CATEGORIA SET nome = @Nome WHERE id_categoria = @Id";

            try
            {
                using var conn = DatabaseConnection.GetConnection();
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Nome", nome);
                if (categoriaId > 0) cmd.Parameters.AddWithValue("@Id", categoriaId);
                cmd.ExecuteNonQuery();
                return (true, "Categoria guardada com sucesso.");
            }
            catch (Exception ex) { return (false, $"Erro ao guardar categoria: {ex.Message}"); }
        }

        public static (bool success, string message) ApagarCategoria(int categoriaId)
        {
            if (categoriaId == 0) return (false, "Nenhuma categoria selecionada.");
            string sql = "DELETE FROM dbo.CATEGORIA WHERE id_categoria = @Id";
            try
            {
                using var conn = DatabaseConnection.GetConnection();
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", categoriaId);
                cmd.ExecuteNonQuery();
                return (true, "Categoria apagada com sucesso.");
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                return (false, "Não pode apagar uma categoria que está a ser utilizada por peças.");
            }
            catch (Exception ex) { return (false, $"Erro ao apagar categoria: {ex.Message}"); }
        }

        // --- GESTÃO DE UTILIZADORES ---

        public static List<UserInfo> GetUtilizadores(string searchTerm = null)
        {
            var users = new List<UserInfo>();
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
                    users.Add(new UserInfo
                    {
                        Id = reader.GetInt32("id_utilizador"),
                        Username = reader.GetString("username"),
                        Email = reader.GetString("email"),
                        IsAdmin = reader.GetBoolean("is_admin")
                    });
                }
            }
            catch (Exception ex) { MessageBox.Show($"Erro ao buscar utilizadores: {ex.Message}"); }
            return users;
        }

        public static (bool success, string message) SetAdminStatus(int userId, bool isAdmin)
        {
            if (userId == 0) return (false, "Nenhum utilizador selecionado.");
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms; 
using Microsoft.Data.SqlClient;


namespace OfiPecas
{
    // Classe para transportar dados da Peça
    public class Peca
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public int Estoque { get; set; }
        public byte[] ImagemBytes { get; set; }

        public Image GetImagem()
        {
            if (ImagemBytes == null || ImagemBytes.Length == 0)
            {
                return null;
            }
            using (var ms = new MemoryStream(ImagemBytes))
            {
                return Image.FromStream(ms);
            }
        }
    }

    // Classe para transportar dados da Categoria
    public class CategoriaInfo
    {
        public int Id { get; set; }
        public string Nome { get; set; }
    }

    public static class StoreService
    {
        public static List<Peca> GetPecas()
        {
            return PesquisarPecas(null);
        }

        public static List<Peca> PesquisarPecas(string searchTerm)
        {
            var pecas = new List<Peca>();
            string sql = "SELECT id_peca, nome, preco, estoque, imagem FROM dbo.PECA";

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                sql += " WHERE nome LIKE @SearchTerm";
            }

            try
            {
                using var conn = DatabaseConnection.GetConnection();
                using var cmd = new SqlCommand(sql, conn);

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    cmd.Parameters.AddWithValue("@SearchTerm", $"%{searchTerm}%");
                }

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var peca = new Peca
                    {
                        Id = reader.GetInt32("id_peca"),
                        Nome = reader.GetString("nome"),
                        Preco = reader.GetDecimal("preco"),
                        Estoque = reader.GetInt32("estoque"),
                        ImagemBytes = (byte[])reader["imagem"]
                    };
                    pecas.Add(peca);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao aceder às peças: {ex.Message}", "Erro de Base de Dados", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return pecas;
        }

        public static List<Peca> GetPecasPorCategoria(int idCategoria)
        {
            var pecas = new List<Peca>();
            string sql = "SELECT id_peca, nome, preco, estoque, imagem FROM dbo.PECA WHERE id_categoria = @CategoriaId";

            try
            {
                using var conn = DatabaseConnection.GetConnection();
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@CategoriaId", idCategoria);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var peca = new Peca
                    {
                        Id = reader.GetInt32("id_peca"),
                        Nome = reader.GetString("nome"),
                        Preco = reader.GetDecimal("preco"),
                        Estoque = reader.GetInt32("estoque"),
                        ImagemBytes = (byte[])reader["imagem"]
                    };
                    pecas.Add(peca);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao aceder às peças: {ex.Message}", "Erro de Base de Dados", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return pecas;
        }

        public static List<CategoriaInfo> GetCategorias()
        {
            var categorias = new List<CategoriaInfo>();
            string sql = "SELECT id_categoria, nome FROM dbo.CATEGORIA ORDER BY nome";

            try
            {
                using var conn = DatabaseConnection.GetConnection();
                using var cmd = new SqlCommand(sql, conn);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var categoria = new CategoriaInfo
                    {
                        Id = reader.GetInt32("id_categoria"),
                        Nome = reader.GetString("nome")
                    };
                    categorias.Add(categoria);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao buscar categorias: {ex.Message}", "Erro de Base de Dados", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return categorias;
        }

        public static (bool success, string message) AdicionarAoCarrinho(int userId, int pecaId)
        {
            using var conn = DatabaseConnection.GetConnection();
            using var transaction = conn.BeginTransaction();

            try
            {
                // Passo 1: Obter o ID do carrinho do utilizador (ou criar um novo)
                int carrinhoId;
                string sqlGetCart = "SELECT id_carrinho FROM dbo.CARRINHO WHERE id_utilizador = @UserId";
                using (var cmdGetCart = new SqlCommand(sqlGetCart, conn, transaction))
                {
                    cmdGetCart.Parameters.AddWithValue("@UserId", userId);
                    var result = cmdGetCart.ExecuteScalar();

                    if (result != null)
                    {
                        carrinhoId = (int)result;
                    }
                    else
                    {
                        string sqlCreateCart = "INSERT INTO dbo.CARRINHO (id_utilizador) VALUES (@UserId); SELECT SCOPE_IDENTITY();";
                        using (var cmdCreateCart = new SqlCommand(sqlCreateCart, conn, transaction))
                        {
                            cmdCreateCart.Parameters.AddWithValue("@UserId", userId);
                            carrinhoId = Convert.ToInt32(cmdCreateCart.ExecuteScalar());
                        }
                    }
                }

                // Passo 2: Verificar se o item já existe no carrinho
                int? itemId = null;
                int quantidadeAtual = 0;
                string sqlCheckItem = "SELECT id_item, quantidade FROM dbo.ITEM_CARRINHO WHERE id_carrinho = @CarrinhoId AND id_peca = @PecaId";
                using (var cmdCheckItem = new SqlCommand(sqlCheckItem, conn, transaction))
                {
                    cmdCheckItem.Parameters.AddWithValue("@CarrinhoId", carrinhoId);
                    cmdCheckItem.Parameters.AddWithValue("@PecaId", pecaId);
                    using (var reader = cmdCheckItem.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            itemId = reader.GetInt32("id_item");
                            quantidadeAtual = reader.GetInt32("quantidade");
                        }
                    }
                }

                // Passo 3: Inserir novo item ou atualizar a quantidade do existente
                if (itemId.HasValue)
                {
                    string sqlUpdate = "UPDATE dbo.ITEM_CARRINHO SET quantidade = @NovaQuantidade WHERE id_item = @ItemId";
                    using (var cmdUpdate = new SqlCommand(sqlUpdate, conn, transaction))
                    {
                        cmdUpdate.Parameters.AddWithValue("@NovaQuantidade", quantidadeAtual + 1);
                        cmdUpdate.Parameters.AddWithValue("@ItemId", itemId.Value);
                        cmdUpdate.ExecuteNonQuery();
                    }
                }
                else
                {
                    string sqlInsert = "INSERT INTO dbo.ITEM_CARRINHO (id_carrinho, id_peca, quantidade) VALUES (@CarrinhoId, @PecaId, 1)";
                    using (var cmdInsert = new SqlCommand(sqlInsert, conn, transaction))
                    {
                        cmdInsert.Parameters.AddWithValue("@CarrinhoId", carrinhoId);
                        cmdInsert.Parameters.AddWithValue("@PecaId", pecaId);
                        cmdInsert.ExecuteNonQuery();
                    }
                }

                transaction.Commit();
                return (true, "Produto adicionado ao carrinho com sucesso!");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return (false, $"Erro ao adicionar ao carrinho: {ex.Message}");
            }
        }
    }
}
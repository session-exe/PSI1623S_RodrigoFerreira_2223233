using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace OfiPecas
{
    // --- CLASSES PARA TRANSPORTAR DADOS ---

    // Representa uma Peça
    public class Peca
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public int Estoque { get; set; }
        public byte[] ImagemBytes { get; set; }

        public Image GetImagem()
        {
            if (ImagemBytes == null || ImagemBytes.Length == 0) return null;
            using (var ms = new MemoryStream(ImagemBytes)) { return Image.FromStream(ms); }
        }
    }

    // Representa uma Categoria
    public class CategoriaInfo
    {
        public int Id { get; set; }
        public string Nome { get; set; }
    }

    // Representa um Item no Carrinho
    public class ItemCarrinhoInfo
    {
        public int ItemId { get; set; }
        public int PecaId { get; set; }
        public string Nome { get; set; }
        public decimal PrecoUnitario { get; set; }
        public int Quantidade { get; set; }
        public byte[] ImagemBytes { get; set; }
        public decimal Subtotal => PrecoUnitario * Quantidade;

        public Image GetImagem()
        {
            if (ImagemBytes == null || ImagemBytes.Length == 0) return null;
            using (var ms = new MemoryStream(ImagemBytes)) { return Image.FromStream(ms); }
        }
    }

    // --- CLASSE DE SERVIÇO COM ACESSO À BASE DE DADOS ---

    public static class StoreService
    {
        // --- MÉTODOS DA LOJA ---

        // Devolve todas as peças
        public static List<Peca> GetPecas() { return PesquisarPecas(null); }

        // Pesquisa peças por nome
        public static List<Peca> PesquisarPecas(string searchTerm)
        {
            var pecas = new List<Peca>();
            string sql = "SELECT id_peca, nome, preco, estoque, imagem FROM dbo.PECA";
            if (!string.IsNullOrWhiteSpace(searchTerm)) { sql += " WHERE nome LIKE @SearchTerm"; }

            try
            {
                using var conn = DatabaseConnection.GetConnection();
                using var cmd = new SqlCommand(sql, conn);
                if (!string.IsNullOrWhiteSpace(searchTerm)) { cmd.Parameters.AddWithValue("@SearchTerm", $"%{searchTerm}%"); }

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    pecas.Add(new Peca
                    {
                        Id = reader.GetInt32("id_peca"),
                        Nome = reader.GetString("nome"),
                        Preco = reader.GetDecimal("preco"),
                        Estoque = reader.GetInt32("estoque"),
                        ImagemBytes = (byte[])reader["imagem"]
                    });
                }
            }
            catch (Exception ex) { MessageBox.Show($"Erro ao aceder às peças: {ex.Message}"); }
            return pecas;
        }

        // Devolve peças de uma categoria específica
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
                    pecas.Add(new Peca
                    {
                        Id = reader.GetInt32("id_peca"),
                        Nome = reader.GetString("nome"),
                        Preco = reader.GetDecimal("preco"),
                        Estoque = reader.GetInt32("estoque"),
                        ImagemBytes = (byte[])reader["imagem"]
                    });
                }
            }
            catch (Exception ex) { MessageBox.Show($"Erro ao aceder às peças por categoria: {ex.Message}"); }
            return pecas;
        }

        // Devolve todas as categorias
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
                    categorias.Add(new CategoriaInfo { Id = reader.GetInt32("id_categoria"), Nome = reader.GetString("nome") });
                }
            }
            catch (Exception ex) { MessageBox.Show($"Erro ao buscar categorias: {ex.Message}"); }
            return categorias;
        }

        // --- MÉTODOS DO CARRINHO E ENCOMENDAS ---

        // Adiciona um produto ao carrinho de um utilizador
        public static (bool success, string message) AdicionarAoCarrinho(int userId, int pecaId)
        {
            using var conn = DatabaseConnection.GetConnection();
            using var transaction = conn.BeginTransaction();
            try
            {
                int carrinhoId;
                string sqlGetCart = "SELECT id_carrinho FROM dbo.CARRINHO WHERE id_utilizador = @UserId";
                using (var cmdGetCart = new SqlCommand(sqlGetCart, conn, transaction))
                {
                    cmdGetCart.Parameters.AddWithValue("@UserId", userId);
                    var result = cmdGetCart.ExecuteScalar();

                    if (result != null) { carrinhoId = (int)result; }
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

        // Devolve os itens do carrinho de um utilizador
        public static List<ItemCarrinhoInfo> GetItensDoCarrinho(int userId)
        {
            var itens = new List<ItemCarrinhoInfo>();
            string sql = @"
                SELECT ic.id_item, p.id_peca, p.nome, p.preco, ic.quantidade, p.imagem
                FROM dbo.ITEM_CARRINHO ic
                JOIN dbo.CARRINHO c ON ic.id_carrinho = c.id_carrinho
                JOIN dbo.PECA p ON ic.id_peca = p.id_peca
                WHERE c.id_utilizador = @UserId";
            try
            {
                using var conn = DatabaseConnection.GetConnection();
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@UserId", userId);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    itens.Add(new ItemCarrinhoInfo
                    {
                        ItemId = reader.GetInt32("id_item"),
                        PecaId = reader.GetInt32("id_peca"),
                        Nome = reader.GetString("nome"),
                        PrecoUnitario = reader.GetDecimal("preco"),
                        Quantidade = reader.GetInt32("quantidade"),
                        ImagemBytes = (byte[])reader["imagem"]
                    });
                }
            }
            catch (Exception ex) { MessageBox.Show($"Erro ao buscar itens do carrinho: {ex.Message}"); }
            return itens;
        }

        // Atualiza a quantidade de um item no carrinho
        public static void AtualizarQuantidadeItem(int itemId, int novaQuantidade)
        {
            if (novaQuantidade <= 0) { RemoverItemDoCarrinho(itemId); return; }

            string sql = "UPDATE dbo.ITEM_CARRINHO SET quantidade = @Quantidade WHERE id_item = @ItemId";
            try
            {
                using var conn = DatabaseConnection.GetConnection();
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Quantidade", novaQuantidade);
                cmd.Parameters.AddWithValue("@ItemId", itemId);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex) { MessageBox.Show($"Erro ao atualizar quantidade: {ex.Message}"); }
        }

        // Remove um item do carrinho
        public static void RemoverItemDoCarrinho(int itemId)
        {
            string sql = "DELETE FROM dbo.ITEM_CARRINHO WHERE id_item = @ItemId";
            try
            {
                using var conn = DatabaseConnection.GetConnection();
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ItemId", itemId);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex) { MessageBox.Show($"Erro ao remover item: {ex.Message}"); }
        }

        // Finaliza a compra: cria uma encomenda e limpa o carrinho
        public static (bool success, string message) FinalizarEncomenda(int userId)
        {
            var itensCarrinho = GetItensDoCarrinho(userId);
            if (itensCarrinho.Count == 0) return (false, "O carrinho está vazio.");

            using var conn = DatabaseConnection.GetConnection();
            using var transaction = conn.BeginTransaction();
            try
            {
                decimal valorTotal = itensCarrinho.Sum(item => item.Subtotal);

                string sqlEncomenda = "INSERT INTO dbo.ENCOMENDA (id_utilizador, valor_total, estado) VALUES (@UserId, @ValorTotal, 'Pendente'); SELECT SCOPE_IDENTITY();";
                int novaEncomendaId;
                using (var cmdEncomenda = new SqlCommand(sqlEncomenda, conn, transaction))
                {
                    cmdEncomenda.Parameters.AddWithValue("@UserId", userId);
                    cmdEncomenda.Parameters.AddWithValue("@ValorTotal", valorTotal);
                    novaEncomendaId = Convert.ToInt32(cmdEncomenda.ExecuteScalar());
                }

                string sqlItemEncomenda = "INSERT INTO dbo.ITEM_ENCOMENDA (id_encomenda, id_peca, quantidade, preco_unitario) VALUES (@EncomendaId, @PecaId, @Quantidade, @PrecoUnitario)";
                foreach (var item in itensCarrinho)
                {
                    using (var cmdItem = new SqlCommand(sqlItemEncomenda, conn, transaction))
                    {
                        cmdItem.Parameters.AddWithValue("@EncomendaId", novaEncomendaId);
                        cmdItem.Parameters.AddWithValue("@PecaId", item.PecaId);
                        cmdItem.Parameters.AddWithValue("@Quantidade", item.Quantidade);
                        cmdItem.Parameters.AddWithValue("@PrecoUnitario", item.PrecoUnitario);
                        cmdItem.ExecuteNonQuery();
                    }
                }

                string sqlLimparCarrinho = "DELETE FROM dbo.ITEM_CARRINHO WHERE id_carrinho = (SELECT id_carrinho FROM dbo.CARRINHO WHERE id_utilizador = @UserId)";
                using (var cmdLimpar = new SqlCommand(sqlLimparCarrinho, conn, transaction))
                {
                    cmdLimpar.Parameters.AddWithValue("@UserId", userId);
                    cmdLimpar.ExecuteNonQuery();
                }

                transaction.Commit();
                return (true, $"Encomenda nº {novaEncomendaId} criada com sucesso!");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return (false, $"Ocorreu um erro: {ex.Message}");
            }
        }
    }
}

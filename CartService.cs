using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace OfiPecas
{
    // Responsável por todas as operações do carrinho de compras
    public static class CartService
    {
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
                    var sqlUpdate = "UPDATE dbo.ITEM_CARRINHO SET quantidade = @NovaQuantidade WHERE id_item = @ItemId";
                    using (var cmdUpdate = new SqlCommand(sqlUpdate, conn, transaction))
                    {
                        cmdUpdate.Parameters.AddWithValue("@NovaQuantidade", quantidadeAtual + 1);
                        cmdUpdate.Parameters.AddWithValue("@ItemId", itemId.Value);
                        cmdUpdate.ExecuteNonQuery();
                    }
                }
                else
                {
                    var sqlInsert = "INSERT INTO dbo.ITEM_CARRINHO (id_carrinho, id_peca, quantidade) VALUES (@CarrinhoId, @PecaId, 1)";
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
                SELECT ic.id_item, p.id_peca, p.nome, p.preco, ic.quantidade, p.imagem, p.estoque
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
                        Estoque = reader.GetInt32("estoque"),
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
    }
}

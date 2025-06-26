using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace OfiPecas
{
    // Esta classe estática trata de todas as operações relacionadas com o carrinho de compras.
    // Ela é responsável por adicionar, ver, atualizar e remover itens.
    public static class CartService
    {
        // Adiciona um produto ao carrinho de um utilizador.
        public static (bool success, string message) AdicionarAoCarrinho(int userId, int pecaId)
        {
            // 'using' garante que a conexão e a transação são fechadas corretamente, mesmo se ocorrer um erro.
            using var conn = DatabaseConnection.GetConnection();
            // Uma transação garante que todas as operações seguintes ou são bem-sucedidas em conjunto, ou nenhuma é.
            using var transaction = conn.BeginTransaction();
            try
            {
                int carrinhoId;
                // Primeiro, verifica se o utilizador já tem um carrinho.
                string sqlGetCart = "SELECT id_carrinho FROM dbo.CARRINHO WHERE id_utilizador = @UserId";
                using (var cmdGetCart = new SqlCommand(sqlGetCart, conn, transaction))
                {
                    cmdGetCart.Parameters.AddWithValue("@UserId", userId);
                    var result = cmdGetCart.ExecuteScalar();
                    if (result != null) { carrinhoId = (int)result; }
                    else // Se não tiver, cria um novo carrinho para ele.
                    {
                        string sqlCreateCart = "INSERT INTO dbo.CARRINHO (id_utilizador) VALUES (@UserId); SELECT SCOPE_IDENTITY();";
                        using (var cmdCreateCart = new SqlCommand(sqlCreateCart, conn, transaction))
                        {
                            cmdCreateCart.Parameters.AddWithValue("@UserId", userId);
                            carrinhoId = Convert.ToInt32(cmdCreateCart.ExecuteScalar());
                        }
                    }
                }

                // Depois, verifica se o produto já existe no carrinho.
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

                // Se o item já existe, apenas aumenta a sua quantidade.
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
                else // Se não existe, insere um novo item no carrinho.
                {
                    var sqlInsert = "INSERT INTO dbo.ITEM_CARRINHO (id_carrinho, id_peca, quantidade) VALUES (@CarrinhoId, @PecaId, 1)";
                    using (var cmdInsert = new SqlCommand(sqlInsert, conn, transaction))
                    {
                        cmdInsert.Parameters.AddWithValue("@CarrinhoId", carrinhoId);
                        cmdInsert.Parameters.AddWithValue("@PecaId", pecaId);
                        cmdInsert.ExecuteNonQuery();
                    }
                }

                transaction.Commit(); // Confirma todas as alterações na base de dados.
                return (true, "Produto adicionado ao carrinho com sucesso!");
            }
            catch (Exception ex)
            {
                transaction.Rollback(); // Se ocorrer um erro, desfaz todas as alterações.
                return (false, $"Erro ao adicionar ao carrinho: {ex.Message}");
            }
        }

        // Devolve uma lista com todos os itens do carrinho de um utilizador específico.
        public static List<ItemCarrinhoInfo> GetItensDoCarrinho(int userId)
        {
            var itens = new List<ItemCarrinhoInfo>();
            // Query SQL que junta as tabelas de itens, carrinho e peças para obter toda a informação.
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
                    // Para cada linha da base de dados, cria um objeto 'ItemCarrinhoInfo' e adiciona à lista.
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

        // Atualiza a quantidade de um item no carrinho.
        public static void AtualizarQuantidadeItem(int itemId, int novaQuantidade)
        {
            // Se a nova quantidade for zero ou menos, o item é removido.
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

        // Remove um item específico do carrinho.
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

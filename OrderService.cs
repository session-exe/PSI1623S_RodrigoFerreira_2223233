using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace OfiPecas
{
    // Responsável por todas as operações de encomendas
    public static class OrderService
    {
        // Finaliza a compra: cria uma encomenda e limpa o carrinho
        public static (bool success, string message) FinalizarEncomenda(int userId)
        {
            // Este método agora usa o CartService para obter os itens.
            var itensCarrinho = CartService.GetItensDoCarrinho(userId);
            if (itensCarrinho.Count == 0)
            {
                return (false, "O carrinho está vazio.");
            }

            using var conn = DatabaseConnection.GetConnection();
            using var transaction = conn.BeginTransaction();
            try
            {
                // 1. Verificação de stock
                foreach (var item in itensCarrinho)
                {
                    if (item.Quantidade > item.Estoque)
                    {
                        transaction.Rollback();
                        return (false, $"Stock insuficiente para o produto: '{item.Nome}'. Disponível: {item.Estoque}, Pedido: {item.Quantidade}.");
                    }
                }

                // 2. Criação da encomenda
                decimal valorTotal = itensCarrinho.Sum(item => item.Subtotal);
                string sqlEncomenda = "INSERT INTO dbo.ENCOMENDA (id_utilizador, valor_total, estado) VALUES (@UserId, @ValorTotal, 'Pendente'); SELECT SCOPE_IDENTITY();";
                int novaEncomendaId;
                using (var cmdEncomenda = new SqlCommand(sqlEncomenda, conn, transaction))
                {
                    cmdEncomenda.Parameters.AddWithValue("@UserId", userId);
                    cmdEncomenda.Parameters.AddWithValue("@ValorTotal", valorTotal);
                    novaEncomendaId = Convert.ToInt32(cmdEncomenda.ExecuteScalar());
                }

                // 3. Inserção dos itens da encomenda e atualização do stock
                string sqlItemEncomenda = "INSERT INTO dbo.ITEM_ENCOMENDA (id_encomenda, id_peca, quantidade, preco_unitario) VALUES (@EncomendaId, @PecaId, @Quantidade, @PrecoUnitario)";
                string sqlAtualizaStock = "UPDATE dbo.PECA SET estoque = estoque - @Quantidade WHERE id_peca = @PecaId";
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
                    using (var cmdStock = new SqlCommand(sqlAtualizaStock, conn, transaction))
                    {
                        cmdStock.Parameters.AddWithValue("@Quantidade", item.Quantidade);
                        cmdStock.Parameters.AddWithValue("@PecaId", item.PecaId);
                        cmdStock.ExecuteNonQuery();
                    }
                }

                // 4. Limpeza do carrinho
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
                return (false, $"Ocorreu um erro crítico ao finalizar a encomenda: {ex.Message}");
            }
        }

        // Devolve a lista de todas as encomendas de um utilizador
        public static List<EncomendaInfo> GetHistoricoEncomendas(int userId)
        {
            var encomendas = new List<EncomendaInfo>();
            string sql = "SELECT id_encomenda, data_encomenda, valor_total, estado FROM dbo.ENCOMENDA WHERE id_utilizador = @UserId ORDER BY data_encomenda DESC";

            try
            {
                using var conn = DatabaseConnection.GetConnection();
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@UserId", userId);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    encomendas.Add(new EncomendaInfo
                    {
                        Id = reader.GetInt32("id_encomenda"),
                        Data = reader.GetDateTime("data_encomenda"),
                        ValorTotal = reader.GetDecimal("valor_total"),
                        Estado = reader.GetString("estado")
                    });
                }
            }
            catch (Exception ex) { MessageBox.Show($"Erro ao buscar histórico: {ex.Message}"); }
            return encomendas;
        }

        // Devolve os itens detalhados de uma única encomenda
        public static List<ItemEncomendaInfo> GetDetalhesEncomenda(int encomendaId)
        {
            var itens = new List<ItemEncomendaInfo>();
            string sql = @"
                SELECT p.nome, ie.quantidade, ie.preco_unitario
                FROM dbo.ITEM_ENCOMENDA ie
                JOIN dbo.PECA p ON ie.id_peca = p.id_peca
                WHERE ie.id_encomenda = @EncomendaId";

            try
            {
                using var conn = DatabaseConnection.GetConnection();
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@EncomendaId", encomendaId);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    itens.Add(new ItemEncomendaInfo
                    {
                        NomePeca = reader.GetString("nome"),
                        Quantidade = reader.GetInt32("quantidade"),
                        PrecoUnitario = reader.GetDecimal("preco_unitario")
                    });
                }
            }
            catch (Exception ex) { MessageBox.Show($"Erro ao buscar detalhes da encomenda: {ex.Message}"); }
            return itens;
        }


    }
}

using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace OfiPecas
{
    // Responsável por todas as operações de encomendas.
    public static class OrderService
    {
        // Finaliza a compra: cria uma encomenda, atualiza o stock e limpa o carrinho.
        public static (bool success, string message) FinalizarEncomenda(int userId)
        {
            // Pede ao CartService a lista de itens no carrinho do utilizador.
            var itensCarrinho = CartService.GetItensDoCarrinho(userId);
            if (itensCarrinho.Count == 0)
            {
                return (false, "O carrinho está vazio.");
            }

            // Usa uma transação para garantir que todas as operações são executadas com sucesso, ou nenhuma é.
            using var conn = DatabaseConnection.GetConnection();
            using var transaction = conn.BeginTransaction();
            try
            {
                // 1. Verificação de stock antes de qualquer alteração na BD.
                foreach (var item in itensCarrinho)
                {
                    if (item.Quantidade > item.Estoque)
                    {
                        transaction.Rollback(); // Cancela a transação.
                        return (false, $"Stock insuficiente para o produto: '{item.Nome}'. Disponível: {item.Estoque}, Pedido: {item.Quantidade}.");
                    }
                }

                // 2. Criação do registo principal da encomenda.
                decimal valorTotal = itensCarrinho.Sum(item => item.Subtotal);
                string sqlEncomenda = "INSERT INTO dbo.ENCOMENDA (id_utilizador, valor_total, estado) VALUES (@UserId, @ValorTotal, 'Pendente'); SELECT SCOPE_IDENTITY();";
                int novaEncomendaId;
                using (var cmdEncomenda = new SqlCommand(sqlEncomenda, conn, transaction))
                {
                    cmdEncomenda.Parameters.AddWithValue("@UserId", userId);
                    cmdEncomenda.Parameters.AddWithValue("@ValorTotal", valorTotal);
                    novaEncomendaId = Convert.ToInt32(cmdEncomenda.ExecuteScalar());
                }

                // 3. Inserção dos itens da encomenda e atualização do stock de cada peça.
                string sqlItemEncomenda = "INSERT INTO dbo.ITEM_ENCOMENDA (id_encomenda, id_peca, quantidade, preco_unitario) VALUES (@EncomendaId, @PecaId, @Quantidade, @PrecoUnitario)";
                string sqlAtualizaStock = "UPDATE dbo.PECA SET estoque = estoque - @Quantidade WHERE id_peca = @PecaId";
                foreach (var item in itensCarrinho)
                {
                    // Insere o item na tabela de encomendas.
                    using (var cmdItem = new SqlCommand(sqlItemEncomenda, conn, transaction))
                    {
                        cmdItem.Parameters.AddWithValue("@EncomendaId", novaEncomendaId);
                        cmdItem.Parameters.AddWithValue("@PecaId", item.PecaId);
                        cmdItem.Parameters.AddWithValue("@Quantidade", item.Quantidade);
                        cmdItem.Parameters.AddWithValue("@PrecoUnitario", item.PrecoUnitario);
                        cmdItem.ExecuteNonQuery();
                    }
                    // Abate a quantidade comprada ao stock da peça correspondente.
                    using (var cmdStock = new SqlCommand(sqlAtualizaStock, conn, transaction))
                    {
                        cmdStock.Parameters.AddWithValue("@Quantidade", item.Quantidade);
                        cmdStock.Parameters.AddWithValue("@PecaId", item.PecaId);
                        cmdStock.ExecuteNonQuery();
                    }
                }

                // 4. Limpeza do carrinho do utilizador.
                string sqlLimparCarrinho = "DELETE FROM dbo.ITEM_CARRINHO WHERE id_carrinho = (SELECT id_carrinho FROM dbo.CARRINHO WHERE id_utilizador = @UserId)";
                using (var cmdLimpar = new SqlCommand(sqlLimparCarrinho, conn, transaction))
                {
                    cmdLimpar.Parameters.AddWithValue("@UserId", userId);
                    cmdLimpar.ExecuteNonQuery();
                }

                transaction.Commit(); // Confirma todas as alterações na base de dados.
                return (true, $"Encomenda nº {novaEncomendaId} criada com sucesso!");
            }
            catch (Exception ex)
            {
                transaction.Rollback(); // Se ocorrer um erro, desfaz todas as alterações.
                return (false, $"Ocorreu um erro crítico ao finalizar a encomenda: {ex.Message}");
            }
        }

        // Devolve o histórico de todas as encomendas de um utilizador.
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
                    // Cria um objeto EncomendaInfo para cada registo e adiciona à lista.
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

        // Devolve os itens detalhados de uma única encomenda (para o PDF da fatura).
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

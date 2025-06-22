using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace OfiPecas
{
    // Responsável apenas pela gestão do catálogo de peças e categorias
    public static class StoreService
    {
        // Devolve todas as peças (agora com os dados da categoria)
        public static List<Peca> GetPecas() { return PesquisarPecas(null); }

        // Pesquisa peças por nome (agora com os dados da categoria)
        public static List<Peca> PesquisarPecas(string searchTerm)
        {
            var pecas = new List<Peca>();
            // --- QUERY CORRIGIDA COM JOIN PARA IR BUSCAR OS DADOS DA CATEGORIA ---
            string sql = @"
                SELECT 
                    p.id_peca, p.nome, p.preco, p.estoque, p.id_categoria, p.imagem, 
                    c.nome as NomeCategoria
                FROM dbo.PECA p
                LEFT JOIN dbo.CATEGORIA c ON p.id_categoria = c.id_categoria";

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                sql += " WHERE p.nome LIKE @SearchTerm";
            }

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
                        CategoriaId = reader.GetInt32("id_categoria"),
                        NomeCategoria = reader["NomeCategoria"] as string, // Preenche a nova propriedade
                        ImagemBytes = (byte[])reader["imagem"]
                    });
                }
            }
            catch (Exception ex) { MessageBox.Show($"Erro ao aceder às peças: {ex.Message}"); }
            return pecas;
        }

        // Devolve peças de uma categoria específica (aqui não precisamos do nome, a loja já o sabe)
        public static List<Peca> GetPecasPorCategoria(int idCategoria)
        {
            var pecas = new List<Peca>();
            string sql = "SELECT id_peca, nome, preco, estoque, id_categoria, imagem FROM dbo.PECA WHERE id_categoria = @CategoriaId";
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
                        CategoriaId = reader.GetInt32("id_categoria"),
                        ImagemBytes = (byte[])reader["imagem"]
                    });
                }
            }
            catch (Exception ex) { MessageBox.Show($"Erro ao aceder às peças por categoria: {ex.Message}"); }
            return pecas;
        }

        // Devolve todas as categorias (este método está correto)
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
    }
}

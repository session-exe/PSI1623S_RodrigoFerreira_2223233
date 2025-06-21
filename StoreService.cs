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
    }
}
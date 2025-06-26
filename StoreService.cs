using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace OfiPecas
{
    // Serviço responsável pela gestão do catálogo de peças e categorias.
    public static class StoreService
    {
        // Devolve a lista completa de peças.
        public static List<Peca> GetPecas() { return PesquisarPecas(null); }

        // Pesquisa peças por nome. Se o termo for nulo, devolve todas as peças.
        public static List<Peca> PesquisarPecas(string searchTerm)
        {
            var pecas = new List<Peca>();
            // Query SQL com JOIN para obter também o nome da categoria.
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
                // Itera pelos resultados e cria uma lista de objetos Peca.
                while (reader.Read())
                {
                    pecas.Add(new Peca
                    {
                        Id = reader.GetInt32("id_peca"),
                        Nome = reader.GetString("nome"),
                        Preco = reader.GetDecimal("preco"),
                        Estoque = reader.GetInt32("estoque"),
                        CategoriaId = reader.GetInt32("id_categoria"),
                        NomeCategoria = reader["NomeCategoria"] as string,
                        ImagemBytes = (byte[])reader["imagem"]
                    });
                }
            }
            catch (Exception ex) { MessageBox.Show($"Erro ao aceder às peças: {ex.Message}"); }
            return pecas;
        }

        // Devolve as peças de uma categoria específica.
        public static List<Peca> GetPecasPorCategoria(int idCategoria)
        {
            var pecas = new List<Peca>();
            // Este método não precisa do JOIN porque a categoria já é conhecida.
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

        // Devolve a lista de todas as categorias.
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

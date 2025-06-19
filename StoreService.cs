using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// StoreService.cs
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms; // Adicionado para o MessageBox

namespace OfiPecas
{
    // Uma classe simples para transportar os dados da peça da BD para a UI
    public class Peca
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public int Estoque { get; set; }
        public byte[] ImagemBytes { get; set; }

        // Converte o array de bytes para um objeto Image que pode ser usado na PictureBox
        public Image GetImagem()
        {
            if (ImagemBytes == null || ImagemBytes.Length == 0)
            {
                return null; // ou retorna uma imagem placeholder
            }
            using (var ms = new MemoryStream(ImagemBytes))
            {
                return Image.FromStream(ms);
            }
        }
    }

    public static class StoreService
    {
        /// <summary>
        /// Vai buscar todas as peças à base de dados.
        /// </summary>
        public static List<Peca> GetPecas()
        {
            // Usa a string de pesquisa nula para indicar que queremos todos os produtos
            return PesquisarPecas(null);
        }

        /// <summary>
        /// Pesquisa peças por um termo no nome ou retorna todas se o termo for nulo/vazio.
        /// </summary>
        public static List<Peca> PesquisarPecas(string searchTerm)
        {
            var pecas = new List<Peca>();

            // A query base seleciona todas as peças
            string sql = "SELECT id_peca, nome, preco, estoque, imagem FROM dbo.PECA";

            // Se existir um termo de pesquisa, adiciona a cláusula WHERE
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                sql += " WHERE nome LIKE @SearchTerm";
            }

            try
            {
                using var conn = DatabaseConnection.GetConnection();
                using var cmd = new SqlCommand(sql, conn);

                // Adiciona o parâmetro de pesquisa, se necessário
                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    // Os '%' são wildcards que permitem encontrar o termo em qualquer parte do nome
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
                        // O campo da imagem é lido como um array de bytes (byte[])
                        ImagemBytes = (byte[])reader["imagem"]
                    };
                    pecas.Add(peca);
                }
            }
            catch (SqlException ex)
            {
                // Em caso de erro, mostra uma mensagem. Idealmente, poderias ter um sistema de logs.
                MessageBox.Show($"Erro ao aceder às peças: {ex.Message}", "Erro de Base de Dados", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return pecas;
        }

        // TODO: Implementar o método para buscar por categoria quando necessário.
        // public static List<Peca> GetPecasPorCategoria(int idCategoria) { ... }
    }
}
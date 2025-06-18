using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetEnv;
using Microsoft.Data.SqlClient;

namespace OfiPecas
{
    internal static class DatabaseConnection 
    {
        private static readonly string _connectionString;

        static DatabaseConnection()
        {
            try
            {
                // Carrega variáveis do arquivo .env
                Env.Load();

                // Constrói a string de conexão
                var builder = new SqlConnectionStringBuilder
                {
                    DataSource = Env.GetString("DB_SERVER"),
                    UserID = Env.GetString("DB_USER"),
                    Password = Env.GetString("DB_PASSWORD"),
                    InitialCatalog = Env.GetString("DB_NAME"),
                    ConnectTimeout = 30,
                    TrustServerCertificate = true
                };
                _connectionString = builder.ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar configurações: {ex.Message}");
                throw;
            }
        }

        public static SqlConnection GetConnection()
        {
            var conn = new SqlConnection(_connectionString);
            conn.Open();
            return conn;
        }

        public static bool TestarConexao()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    return conn.State == System.Data.ConnectionState.Open;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro de conexão: " + ex.Message, "ERRO");
                return false;
            }
        }


    }
}

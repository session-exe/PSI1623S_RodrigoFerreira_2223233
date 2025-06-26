using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetEnv; // Biblioteca para carregar variáveis de ambiente a partir de um ficheiro .env
using Microsoft.Data.SqlClient;
using System.Windows.Forms; // Necessário para o MessageBox

namespace OfiPecas
{

    internal static class DatabaseConnection
    {
        // A string de conexão é 'readonly', só pode ser definida uma vez.
        private static readonly string _connectionString;


        static DatabaseConnection()
        {
            try
            {
                // Carrega as variáveis (servidor, utilizador, password) do ficheiro .env.
                // Isto é uma boa prática de segurança para não deixar as credenciais no código.
                Env.Load();

                // Usa o SqlConnectionStringBuilder para construir a string de conexão de forma segura.
                var builder = new SqlConnectionStringBuilder
                {
                    DataSource = Env.GetString("DB_SERVER"),
                    UserID = Env.GetString("DB_USER"),
                    Password = Env.GetString("DB_PASSWORD"),
                    InitialCatalog = Env.GetString("DB_NAME"),
                    ConnectTimeout = 30, // Tempo máximo de espera para conectar.
                    TrustServerCertificate = true // Necessário para conexões locais sem SSL.
                };
                // Guarda a string de conexão final.
                _connectionString = builder.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar configurações da base de dados: {ex.Message}");
                throw; 
            }
        }

        // Método público que outras classes usam para obter uma nova conexão à base de dados.
        public static SqlConnection GetConnection()
        {
            // Cria uma nova instância da conexão.
            var conn = new SqlConnection(_connectionString);
            // Abre a conexão antes de a devolver.
            conn.Open();
            return conn;
        }

        // Um método de ajuda para verificar se a conexão com a base de dados está a funcionar.

        public static bool TestarConexao()
        {
            try
            {
                // O 'using' garante que a conexão é fechada automaticamente no final.
                using (var conn = GetConnection())
                {
                    // Se o estado for 'Open', a conexão foi bem-sucedida.
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

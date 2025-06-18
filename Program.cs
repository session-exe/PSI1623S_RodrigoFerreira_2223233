using System;
using System.Windows.Forms;
using OfiPecas;
using DotNetEnv;
using System.Diagnostics;

namespace OfiPecas
{
    internal static class Program
    {

        [STAThread]
        static void Main()
        {
            Env.Load();

            // Verifica conexão antes de iniciar
            if (!DatabaseConnection.TestarConexao())
            {
                MessageBox.Show("Erro ao conectar ao banco de dados!");
                return;
            }

            ApplicationConfiguration.Initialize();
            Application.Run(new Login());
        }
    }
}
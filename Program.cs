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
            // Verifica se o aplicativo já está em execução e fecha.
            KillDuplicateProcesses();

            // Configura o ambiente
            ApplicationConfiguration.Initialize();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Carrega variáveis de ambiente
            try
            {
                Env.Load();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar configurações: {ex.Message}");
                ForceShutdown();
            }

            // Verifica conexão com o basde de dados
            if (!DatabaseConnection.TestarConexao())
            {
                MessageBox.Show("Erro ao conectar ao banco de dados!");
                ForceShutdown();
                return;
            }

            var loginForm = new Login();
 
            Application.Run(loginForm);
        }

        // Fecha processos duplicados
        static void KillDuplicateProcesses()
        {
            try
            {
                var currentProcess = Process.GetCurrentProcess();
                foreach (var process in Process.GetProcessesByName(currentProcess.ProcessName))
                {
                    if (process.Id != currentProcess.Id)
                    {
                        process.Kill();
                        process.WaitForExit(500); 
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao Fechar processos duplicados: {ex.Message}");
                ForceShutdown();

            }
        }

        // Força o encerramento
        static void ForceShutdown()
        {
            // Força o encerramento
            Environment.Exit(0);
            Process.GetCurrentProcess().Kill();
        }
    }
}


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OfiPecas
{
    // Este é o formulário para um novo utilizador criar a sua conta.
    public partial class CriarConta : Form
    {
        // Construtor do formulário. É chamado quando uma nova janela de 'CriarConta' é criada.
        public CriarConta()
        {
            InitializeComponent();
        }

        // Evento do clique no botão para gerar uma chave de recuperação aleatória.
        private void guna_ImageButton_Gerar_Click_1(object sender, EventArgs e)
        {
            // Chama a função para gerar uma chave aleatória e coloca-a na TextBox.
            TextBox_ChaveRecuperacao.Text = GenerateRandomKey(15);
        }

        // Evento principal, executado quando o botão "Criar Conta" é clicado.
        private void Button_CriarConta_Click(object sender, EventArgs e)
        {
            // Validação local: verifica se a password e a sua confirmação coincidem.
            if (TextBox_Password.Text != TextBox_RepetirPassword.Text)
            {
                MessageBox.Show("As senhas não coincidem.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Para a execução do método se as senhas não coincidirem.
            }

            // Recolhe todos os dados dos campos do formulário.
            // O '.Trim()' remove espaços em branco no início e no fim.
            string username = TextBox_Utilizador.Text.Trim();
            string email = TextBox_Email.Text.Trim();
            string password = TextBox_Password.Text;
            string recoveryKey = TextBox_ChaveRecuperacao.Text;
            string endereco = TextBox_Endereço.Text.Trim();
            string nomeEmpresa = TextBox_NomeEmpresa.Text.Trim();
            string telefone = TextBox_Telefone.Text.Trim();

            // Chama o método 'CreateUser' do nosso AuthService para tentar criar o utilizador na base de dados.
            // A resposta do método (se foi bem-sucedido e a mensagem) é guardada em duas variáveis.
            var (success, message) = AuthService.CreateUser(
                username, email, password, recoveryKey, endereco, nomeEmpresa, telefone);

            // Mostra o resultado da operação (sucesso ou erro) ao utilizador.
            MessageBox.Show(message, success ? "Sucesso" : "Erro",
                MessageBoxButtons.OK,
                success ? MessageBoxIcon.Information : MessageBoxIcon.Error);

            // Se a conta foi criada com sucesso...
            if (success)
            {
                // ...cria uma nova instância do formulário de Login,
                var loginForm = new Login();
                // mostra-o,
                loginForm.Show();
                // e fecha o formulário atual de criação de conta.
                this.Close();
            }
        }

        // Método auxiliar para gerar uma chave aleatória e segura.
        private string GenerateRandomKey(int length)
        {
            // Define o conjunto de caracteres que podem ser usados na chave.
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var data = new byte[length];
            // Usa a classe criptográfica para gerar bytes verdadeiramente aleatórios.
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(data);

            var sb = new System.Text.StringBuilder(length);
            // Constrói a string final, escolhendo um caracter do conjunto para cada byte aleatório.
            foreach (byte b in data)
            {
                sb.Append(chars[b % chars.Length]);
            }
            return sb.ToString();
        }

        // Evento do botão "Voltar".
        private void ImageButton_Back_Click(object sender, EventArgs e)
        {
            // Cria e mostra o formulário de Login e fecha o formulário atual.
            var login = new Login();
            login.Show();
            this.Close();
        }
    }
}

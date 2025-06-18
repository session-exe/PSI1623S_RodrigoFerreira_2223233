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
    public partial class CriarConta : Form
    {
        public CriarConta()
        {
            InitializeComponent();
        }

        private void guna_ImageButton_Gerar_Click_1(object sender, EventArgs e)
        {
            TextBox_ChaveRecuperacao.Text = GenerateRandomKey(15);
        }

        private void Button_CriarConta_Click(object sender, EventArgs e)
        {
            // Verifica se as senhas coincidem 
            if (TextBox_Password.Text != TextBox_RepetirPassword.Text)
            {
                MessageBox.Show("As senhas não coincidem.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string username = TextBox_Utilizador.Text.Trim();
            string email = TextBox_Email.Text.Trim();
            string password = TextBox_Password.Text;
            string recoveryKey = TextBox_ChaveRecuperacao.Text;
            string endereco = TextBox_Endereço.Text.Trim();
            string nomeEmpresa = TextBox_NomeEmpresa.Text.Trim();
            string telefone = TextBox_Telefone.Text.Trim();

            var (success, message) = AuthService.CreateUser(
                username, email, password, recoveryKey, endereco, nomeEmpresa, telefone);

            // Exibe mensagem uma única vez
            MessageBox.Show(message, success ? "Sucesso" : "Erro",
                MessageBoxButtons.OK,
                success ? MessageBoxIcon.Information : MessageBoxIcon.Error);

            if (success)
            {
                // Vai para o login sem chamar o CreateConta de novo
                var loginForm = new Login();
                loginForm.Show();
                this.Close();
            }
        }

        private string GenerateRandomKey(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var data = new byte[length];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(data);
            var sb = new System.Text.StringBuilder(length);
            foreach (byte b in data)
                sb.Append(chars[b % chars.Length]);
            return sb.ToString();
        }

        private void Button_Back_Click(object sender, EventArgs e)
        {
            var login = new Login();
            login.Show();
            this.Close();
        }

        
    }
}





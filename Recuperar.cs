using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OfiPecas
{
    public partial class Recuperar : Form
    {
        public Recuperar()
        {
            InitializeComponent();
        }

        private void Button_Recuperar_Click(object sender, EventArgs e)
        {
            string username = TextBox_Utilizador.Text.Trim();
            string key = TextBox_ChaveRecuperacao.Text.Trim();
            string newPassword = TextBox_NovaPassword.Text;
            string repeatPassword = TextBox_RepetirPassword.Text;

            // Validação local
            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(key) ||
                string.IsNullOrWhiteSpace(newPassword) ||
                string.IsNullOrWhiteSpace(repeatPassword))
            {
                MessageBox.Show("Todos os campos são obrigatórios.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (newPassword != repeatPassword)
            {
                MessageBox.Show("As senhas não coincidem.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Chama o metodo de recuperação
            var (success, message) = AuthService.RecoverPassword(username, key, newPassword);

            MessageBox.Show(message, success ? "Sucesso" : "Erro",
                MessageBoxButtons.OK,
                success ? MessageBoxIcon.Information : MessageBoxIcon.Error);

            if (success)
            {
                var login = new Login();
                login.Show();
                this.Close();
            }
        }

        private void ImageButton_Back_Click(object sender, EventArgs e)
        {
            var login = new Login();
            login.Show();
            this.Close();
        }
    }
}

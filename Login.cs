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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        // Butão para fazer login
        private void Button_Login_Click(object sender, EventArgs e)
        {
            string login = TextBox_User_Email.Text.Trim();
            string senha = TextBox_Pass.Text;

            var (success, message, userId, isAdmin) = AuthService.Login(login, senha);
            MessageBox.Show(message);

            if (success)
            {
                // passa os dados do utilizador para a janela principal
                var Main = new Loja(userId, isAdmin);
                Main.Show();
                this.Hide();
            }
        }

        // Criar conta
        private void label_CriarConta_Click(object sender, EventArgs e)
        {
            CriarConta criarConta = new CriarConta();
            criarConta.Show();
            this.Hide();
        }

        // Recuperar senha
        private void label_RecuperarPass_Click(object sender, EventArgs e)
        {
            Recuperar recuperar = new Recuperar();
            recuperar.Show();
            this.Hide();
        }
    }
}

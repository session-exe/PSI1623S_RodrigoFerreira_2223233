using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OfiPecas.Services;

namespace OfiPecas
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Button_Login_Click(object sender, EventArgs e)
        {
            string login = TextBox_User_Email.Text.Trim();
            string senha = TextBox_Pass.Text;

            var (success, message, userId, isAdmin) = AuthService.Login(login, senha);
            MessageBox.Show(message);

            if (success)
            {
                // Exemplo: abrir o formulário principal
                //var Main = new Main(userId, isAdmin);
                var Main = new Main();
                Main.Show();
                this.Hide();
            }
        }
    }
}

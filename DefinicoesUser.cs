using System;
using System.Windows.Forms;

namespace OfiPecas
{
    public partial class DefinicoesUser : Form
    {
        private readonly int _userId;

        public DefinicoesUser(int userId)
        {
            InitializeComponent();
            _userId = userId;
            this.Load += DefinicoesUser_Load;
        }

        private void DefinicoesUser_Load(object sender, EventArgs e)
        {
            CarregarDadosDoUtilizador();
        }

        // Método central para carregar ou recarregar os dados
        private void CarregarDadosDoUtilizador()
        {
            var userData = UserService.GetUserData(_userId);
            if (userData != null)
            {
                TextBox_Username.Text = userData.Username;
                TextBox_RecoveryKey.Text = userData.RecoveryKey;
                TextBox_Email.Text = userData.Email;
                TextBox_NomeEmpresa.Text = userData.NomeEmpresa;
                TextBox_Endereco.Text = userData.Endereco;
                TextBox_Telefone.Text = userData.Telefone;
            }
            else
            {
                MessageBox.Show("Não foi possível carregar os dados do utilizador.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void Button_GuardarDados_Click(object sender, EventArgs e)
        {
            // Valida se os campos obrigatórios estão preenchidos
            if (string.IsNullOrWhiteSpace(TextBox_Email.Text) || string.IsNullOrWhiteSpace(TextBox_NomeEmpresa.Text) || string.IsNullOrWhiteSpace(TextBox_Endereco.Text))
            {
                MessageBox.Show("Os campos Email, Nome da Empresa e Endereço são obrigatórios.", "Campos em Falta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                // CORREÇÃO: Recarrega os dados para restaurar os campos
                CarregarDadosDoUtilizador();
                return;
            }

            // Se a validação passar, tenta atualizar na base de dados
            var (success, message) = UserService.UpdateUserData(
                _userId,
                TextBox_Email.Text.Trim(),
                TextBox_NomeEmpresa.Text.Trim(),
                TextBox_Endereco.Text.Trim(),
                TextBox_Telefone.Text.Trim()
            );

            MessageBox.Show(message, success ? "Sucesso" : "Erro", MessageBoxButtons.OK, success ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

            // Se a atualização na BD falhar (ex: email duplicado), também recarrega
            if (!success)
            {
                CarregarDadosDoUtilizador();
            }
        }

        private void Button_AlterarPassword_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TextBox_PasswordAtual.Text) || string.IsNullOrWhiteSpace(TextBox_NovaPassword.Text) || string.IsNullOrWhiteSpace(TextBox_ConfirmarPassword.Text))
            {
                MessageBox.Show("Todos os campos de password devem ser preenchidos.", "Campos em Falta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (TextBox_NovaPassword.Text != TextBox_ConfirmarPassword.Text)
            {
                MessageBox.Show("A nova password e a sua confirmação não coincidem.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var (success, message) = UserService.ChangePassword(_userId, TextBox_PasswordAtual.Text, TextBox_NovaPassword.Text);
            MessageBox.Show(message, success ? "Sucesso" : "Erro", MessageBoxButtons.OK, success ? MessageBoxIcon.Information : MessageBoxIcon.Error);

            // Limpa sempre os campos de password após a tentativa
            TextBox_PasswordAtual.Clear();
            TextBox_NovaPassword.Clear();
            TextBox_ConfirmarPassword.Clear();
        }

        private void Button_ApagarConta_Click(object sender, EventArgs e)
        {
            string password = TextBox_PasswordConfirmarApagar.Text;
            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Para apagar a sua conta, por favor, insira a sua password atual no campo de confirmação.", "Confirmação Necessária", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TextBox_PasswordConfirmarApagar.Focus();
                return;
            }

            var confirmacaoFinal = MessageBox.Show(
                "TEM A CERTEZA ABSOLUTA?\n\nEsta ação é permanente e não pode ser desfeita.",
                "Confirmar Remoção Permanente da Conta",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2
            );

            if (confirmacaoFinal == DialogResult.No) return;

            var (success, message) = UserService.DeleteAccount(_userId, password);
            MessageBox.Show(message, success ? "Sucesso" : "Erro", MessageBoxButtons.OK, success ? MessageBoxIcon.Information : MessageBoxIcon.Error);

            if (success)
            {
                MessageBox.Show("A sua conta foi apagada com sucesso. A aplicação será reiniciada.", "Conta Apagada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Restart();
            }
            else
            {
                // Se a password para apagar a conta estiver errada, limpa o campo
                TextBox_PasswordConfirmarApagar.Clear();
            }
        }
    }
}

using System;
using System.Windows.Forms;

namespace OfiPecas
{
    // Formulário para o utilizador ver e editar as suas próprias definições.
    public partial class DefinicoesUser : Form
    {
        // Guarda o ID do utilizador que está a usar este formulário.
        private readonly int _userId;

        // Construtor que recebe o ID do utilizador vindo do formulário da Loja.
        public DefinicoesUser(int userId)
        {
            InitializeComponent();
            _userId = userId;
            // Liga o evento de carregamento do formulário ao método de inicialização.
            this.Load += DefinicoesUser_Load;
        }

        // Método executado quando o formulário é aberto.
        private void DefinicoesUser_Load(object sender, EventArgs e)
        {
            CarregarDadosDoUtilizador();
        }

        // Vai à base de dados buscar e preencher os dados do utilizador.
        private void CarregarDadosDoUtilizador()
        {
            // Pede ao UserService os dados do utilizador atual.
            var userData = UserService.GetUserData(_userId);
            if (userData != null)
            {
                // Se encontrou os dados, preenche os campos do formulário.
                TextBox_Username.Text = userData.Username;
                TextBox_RecoveryKey.Text = userData.RecoveryKey;
                TextBox_Email.Text = userData.Email;
                TextBox_NomeEmpresa.Text = userData.NomeEmpresa;
                TextBox_Endereco.Text = userData.Endereco;
                TextBox_Telefone.Text = userData.Telefone;
            }
            else
            {
                // Se ocorrer um erro, avisa e fecha o formulário.
                MessageBox.Show("Não foi possível carregar os dados do utilizador.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        // Evento do clique no botão "Guardar Dados".
        private void Button_GuardarDados_Click(object sender, EventArgs e)
        {
            // Valida se os campos obrigatórios estão preenchidos.
            if (string.IsNullOrWhiteSpace(TextBox_Email.Text) || string.IsNullOrWhiteSpace(TextBox_NomeEmpresa.Text) || string.IsNullOrWhiteSpace(TextBox_Endereco.Text))
            {
                MessageBox.Show("Os campos Email, Nome da Empresa e Endereço são obrigatórios.", "Campos em Falta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                // Se a validação falhar, recarrega os dados originais.
                CarregarDadosDoUtilizador();
                return;
            }

            // Tenta atualizar os dados na base de dados através do UserService.
            var (success, message) = UserService.UpdateUserData(
                _userId,
                TextBox_Email.Text.Trim(),
                TextBox_NomeEmpresa.Text.Trim(),
                TextBox_Endereco.Text.Trim(),
                TextBox_Telefone.Text.Trim()
            );

            MessageBox.Show(message, success ? "Sucesso" : "Erro", MessageBoxButtons.OK, success ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

            // Se a atualização na BD falhar (ex: email duplicado), recarrega os dados.
            if (!success)
            {
                CarregarDadosDoUtilizador();
            }
        }

        // Evento do clique no botão "Alterar Password".
        private void Button_AlterarPassword_Click(object sender, EventArgs e)
        {
            // Valida se todos os campos de password foram preenchidos.
            if (string.IsNullOrWhiteSpace(TextBox_PasswordAtual.Text) || string.IsNullOrWhiteSpace(TextBox_NovaPassword.Text) || string.IsNullOrWhiteSpace(TextBox_ConfirmarPassword.Text))
            {
                MessageBox.Show("Todos os campos de password devem ser preenchidos.", "Campos em Falta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Valida se a nova password e a sua confirmação coincidem.
            if (TextBox_NovaPassword.Text != TextBox_ConfirmarPassword.Text)
            {
                MessageBox.Show("A nova password e a sua confirmação não coincidem.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Chama o serviço para tentar alterar a password.
            var (success, message) = UserService.ChangePassword(_userId, TextBox_PasswordAtual.Text, TextBox_NovaPassword.Text);
            MessageBox.Show(message, success ? "Sucesso" : "Erro", MessageBoxButtons.OK, success ? MessageBoxIcon.Information : MessageBoxIcon.Error);

            // Por segurança, limpa sempre os campos de password após a tentativa.
            TextBox_PasswordAtual.Clear();
            TextBox_NovaPassword.Clear();
            TextBox_ConfirmarPassword.Clear();
        }

        // Evento do clique no botão "Apagar Conta".
        private void Button_ApagarConta_Click(object sender, EventArgs e)
        {
            // Valida se a password de confirmação foi inserida.
            string password = TextBox_PasswordConfirmarApagar.Text;
            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Para apagar a sua conta, por favor, insira a sua password atual no campo de confirmação.", "Confirmação Necessária", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TextBox_PasswordConfirmarApagar.Focus();
                return;
            }

            // Mostra um aviso final sobre a permanência da ação.
            var confirmacaoFinal = MessageBox.Show(
                "TEM A CERTEZA ABSOLUTA?\n\nEsta ação é permanente e não pode ser desfeita.",
                "Confirmar Remoção Permanente da Conta",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2
            );

            if (confirmacaoFinal == DialogResult.No) return;

            // Chama o serviço para tentar apagar a conta, validando a password.
            var (success, message) = UserService.DeleteAccount(_userId, password);
            MessageBox.Show(message, success ? "Sucesso" : "Erro", MessageBoxButtons.OK, success ? MessageBoxIcon.Information : MessageBoxIcon.Error);

            if (success)
            {
                // Se a conta for apagada, reinicia a aplicação para forçar o logout.
                MessageBox.Show("A sua conta foi apagada com sucesso. A aplicação será reiniciada.", "Conta Apagada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Restart();
            }
            else
            {
                // Se a password estiver errada, limpa o campo.
                TextBox_PasswordConfirmarApagar.Clear();
            }
        }
    }
}

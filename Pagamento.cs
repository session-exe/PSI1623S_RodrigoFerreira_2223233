using System;
using System.Linq;
using System.Windows.Forms;

namespace OfiPecas
{
    // Formulário para o utilizador inserir os dados de pagamento simulados.
    public partial class Pagamento : Form
    {
        // Armazena o ID do utilizador que está a fazer a compra.
        private readonly int _userId;

        // Construtor que recebe o ID do utilizador vindo do formulário do Carrinho.
        public Pagamento(int userId)
        {
            InitializeComponent();
            _userId = userId;
            // Configura o DateTimePicker para não permitir a seleção de datas passadas.
            DateTimePicker_Card.MinDate = DateTime.Today;
        }

        // Handler para o clique do botão "Pagar".
        private void Button_Pagar_Click(object sender, EventArgs e)
        {
            // --- VALIDAÇÃO INDIVIDUAL DE CADA CAMPO ---

            if (string.IsNullOrWhiteSpace(TextBox_Nome.Text))
            {
                MessageBox.Show("O campo 'Nome do Titular' é obrigatório.", "Erro de Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TextBox_Nome.Focus(); // Coloca o cursor no campo inválido.
                return; // Para a execução.
            }

            if (string.IsNullOrWhiteSpace(TextBox_Morada.Text))
            {
                MessageBox.Show("O campo 'Morada de Faturação' é obrigatório.", "Erro de Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TextBox_Morada.Focus();
                return;
            }

            // Valida se o número do cartão tem 16 dígitos numéricos.
            if (string.IsNullOrWhiteSpace(TextBox_NrCartao.Text) || TextBox_NrCartao.Text.Replace(" ", "").Length != 16 || !TextBox_NrCartao.Text.Replace(" ", "").All(char.IsDigit))
            {
                MessageBox.Show("Por favor, insira um número de cartão válido com 16 dígitos.", "Erro de Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TextBox_NrCartao.Focus();
                return;
            }

            // Valida se o CVV tem 3 dígitos numéricos.
            if (string.IsNullOrWhiteSpace(TextBox_CVV.Text) || TextBox_CVV.Text.Length != 3 || !TextBox_CVV.Text.All(char.IsDigit))
            {
                MessageBox.Show("Por favor, insira um CVV válido com 3 dígitos.", "Erro de Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TextBox_CVV.Focus();
                return;
            }

            // --- FIM DAS VALIDAÇÕES ---

            // Se todas as validações passarem, chama o serviço para finalizar a encomenda.
            var (success, message) = OrderService.FinalizarEncomenda(_userId);

            MessageBox.Show(message, success ? "Sucesso" : "Erro", MessageBoxButtons.OK, success ? MessageBoxIcon.Information : MessageBoxIcon.Error);

            // Se a encomenda for criada com sucesso na base de dados...
            if (success)
            {
                // ...define o resultado do diálogo como OK para notificar o formulário do Carrinho.
                this.DialogResult = DialogResult.OK;
                // Fecha este formulário.
                this.Close();
            }
        }
    }
}

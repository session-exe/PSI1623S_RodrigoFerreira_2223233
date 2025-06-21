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
    public partial class Pagamento : Form
    {
        private readonly int _userId;

        public Pagamento(int userId)
        {
            InitializeComponent();
            _userId = userId;

            // Configura o DateTimePicker para não permitir datas passadas
            DateTimePicker_Card.MinDate = DateTime.Today;
        }

        private void Button_Pagar_Click(object sender, EventArgs e)
        {
            // --- VALIDAÇÃO CAMPO A CAMPO ---

            // Validação do Nome
            if (string.IsNullOrWhiteSpace(TextBox_Nome.Text))
            {
                MessageBox.Show("O campo 'Nome do Titular' é obrigatório.", "Erro de Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TextBox_Nome.Focus();
                return;
            }

            // Validação da Morada
            if (string.IsNullOrWhiteSpace(TextBox_Morada.Text))
            {
                MessageBox.Show("O campo 'Morada de Faturação' é obrigatório.", "Erro de Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TextBox_Morada.Focus();
                return;
            }

            // Validação do Número do Cartão
            if (string.IsNullOrWhiteSpace(TextBox_NrCartao.Text) || TextBox_NrCartao.Text.Replace(" ", "").Length != 16 || !TextBox_NrCartao.Text.Replace(" ", "").All(char.IsDigit))
            {
                MessageBox.Show("Por favor, insira um número de cartão válido com 16 dígitos.", "Erro de Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TextBox_NrCartao.Focus();
                return;
            }

            // Validação do CVV
            if (string.IsNullOrWhiteSpace(TextBox_CVV.Text) || TextBox_CVV.Text.Length != 3 || !TextBox_CVV.Text.All(char.IsDigit))
            {
                MessageBox.Show("Por favor, insira um CVV válido com 3 dígitos.", "Erro de Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TextBox_CVV.Focus();
                return;
            }

            // Validação da Data de Validade
            // (A propriedade MinDate do DateTimePicker já previne datas passadas)
            // Se quiseres uma verificação extra:
            DateTime primeiroDiaDoMesSelecionado = new DateTime(DateTimePicker_Card.Value.Year, DateTimePicker_Card.Value.Month, 1);
            DateTime primeiroDiaDoMesAtual = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            if (primeiroDiaDoMesSelecionado < primeiroDiaDoMesAtual)
            {
                MessageBox.Show("A data de validade do cartão não pode ser no passado.", "Erro de Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DateTimePicker_Card.Focus();
                return;
            }

            // --- SE TODAS AS VALIDAÇÕES PASSAREM ---

            // Chama o serviço para criar a encomenda na base de dados
            var (success, message) = StoreService.FinalizarEncomenda(_userId);

            MessageBox.Show(message, success ? "Sucesso" : "Erro", MessageBoxButtons.OK, success ? MessageBoxIcon.Information : MessageBoxIcon.Error);

            if (success)
            {
                // Informa o formulário do carrinho que a compra foi bem-sucedida
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
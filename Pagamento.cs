using System;
using System.Linq;
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
            DateTimePicker_Card.MinDate = DateTime.Today;
        }

        private void Button_Pagar_Click(object sender, EventArgs e)
        {
            // Validações...
            if (string.IsNullOrWhiteSpace(TextBox_Nome.Text)) { /* ... */ return; }
            if (string.IsNullOrWhiteSpace(TextBox_Morada.Text)) { /* ... */ return; }
            // ... resto das validações ...

            // **CHAMADA CORRIGIDA**
            var (success, message) = OrderService.FinalizarEncomenda(_userId);

            MessageBox.Show(message, success ? "Sucesso" : "Erro", MessageBoxButtons.OK, success ? MessageBoxIcon.Information : MessageBoxIcon.Error);

            if (success)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}

using System;
using System.Windows.Forms;

namespace OfiPecas
{
    // A palavra 'partial' é crucial para ligar este ficheiro ao ficheiro .Designer.cs
    public partial class EncomendaCard : UserControl
    {
        public EncomendaInfo InfoEncomenda { get; private set; }
        public event EventHandler BaixarFaturaClicked;

        public EncomendaCard(EncomendaInfo encomenda)
        {
            // Este método é definido no ficheiro .Designer.cs e cria todos os controlos
            InitializeComponent();

            this.InfoEncomenda = encomenda;

            // Preenche os controlos com os dados. Agora o código vai encontrá-los.
            lblIdEncomenda.Text = $"Encomenda #{encomenda.Id}";
            lblData.Text = $"Data: {encomenda.Data:dd/MM/yyyy}";
            lblValor.Text = $"Total: {encomenda.ValorTotal:C}";
            lblEstado.Text = $"Estado: {encomenda.Estado}";

            // Liga o evento de clique ao botão
            btnBaixarFatura.Click += BtnBaixarFatura_Click;
        }

        private void BtnBaixarFatura_Click(object sender, EventArgs e)
        {
            BaixarFaturaClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}

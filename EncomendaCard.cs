using System;
using System.Windows.Forms;

namespace OfiPecas
{
    // UserControl para exibir o resumo de uma encomenda.
    public partial class EncomendaCard : UserControl
    {
        // Propriedade pública para armazenar os dados da encomenda.
        public EncomendaInfo InfoEncomenda { get; private set; }

        // Evento para notificar o formulário pai quando o botão de download é clicado.
        public event EventHandler BaixarFaturaClicked;

        // Construtor que recebe um objeto EncomendaInfo para configurar o controlo.
        public EncomendaCard(EncomendaInfo encomenda)
        {
            InitializeComponent();
            this.InfoEncomenda = encomenda;

            // Preenche os controlos da interface com os dados da encomenda.
            lblIdEncomenda.Text = $"Encomenda #{encomenda.Id}";
            lblData.Text = $"Data: {encomenda.Data:dd/MM/yyyy}"; // Formata a data
            lblValor.Text = $"Total: {encomenda.ValorTotal:C}";   // Formata como moeda
            lblEstado.Text = $"Estado: {encomenda.Estado}";

            // Associa o evento de clique do botão ao método handler.
            btnBaixarFatura.Click += BtnBaixarFatura_Click;
        }

        // Handler para o clique do botão de baixar fatura.
        private void BtnBaixarFatura_Click(object sender, EventArgs e)
        {
            // Dispara o evento 'BaixarFaturaClicked'.
            BaixarFaturaClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}

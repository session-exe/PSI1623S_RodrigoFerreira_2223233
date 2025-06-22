using System;
using System.Windows.Forms;

namespace OfiPecas
{
    public partial class HistoricoEncomendas : Form
    {
        private readonly int _userId;

        public HistoricoEncomendas(int userId)
        {
            InitializeComponent();
            _userId = userId;

            // Ativa o scroll automático no painel
            flowPanel_Encomendas.AutoScroll = true;

            // Associa o carregamento do formulário ao nosso método
            this.Load += HistoricoEncomendas_Load;
        }

        private void HistoricoEncomendas_Load(object sender, EventArgs e)
        {
            // Limpa o painel antes de adicionar novos controlos
            flowPanel_Encomendas.Controls.Clear();

            // Vai buscar a lista de encomendas do utilizador
            var encomendas = OrderService.GetHistoricoEncomendas(_userId);

            if (encomendas.Count == 0)
            {
                // Mostra uma mensagem se não houver encomendas
                Label lblVazio = new Label
                {
                    Text = "Ainda não efetuou nenhuma encomenda.",
                    AutoSize = true,
                    Font = new System.Drawing.Font("Segoe UI", 12F),
                    ForeColor = System.Drawing.Color.White
                };
                flowPanel_Encomendas.Controls.Add(lblVazio);
            }
            else
            {
                // Cria um EncomendaCard para cada encomenda na lista
                foreach (var encomenda in encomendas)
                {
                    var control = new EncomendaCard(encomenda);
                    // Subscreve ao evento do botão do PDF
                    control.BaixarFaturaClicked += EncomendaCard_BaixarFaturaClicked;
                    flowPanel_Encomendas.Controls.Add(control);
                }
            }
        }

        // Este método é chamado quando o botão "Baixar Fatura" de um dos cards é clicado
        private void EncomendaCard_BaixarFaturaClicked(object sender, EventArgs e)
        {
            if (sender is EncomendaCard control)
            {
                // Busca os detalhes (os produtos) da encomenda específica
                var itens = OrderService.GetDetalhesEncomenda(control.InfoEncomenda.Id);

                // Chama o serviço de PDF para gerar a fatura
                PdfService.GerarFaturaPdf(control.InfoEncomenda, itens);
            }
        }
    }
}
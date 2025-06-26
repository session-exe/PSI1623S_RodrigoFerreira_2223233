using System;
using System.Windows.Forms;

namespace OfiPecas
{
    // Formulário para exibir o histórico de encomendas do utilizador.
    public partial class HistoricoEncomendas : Form
    {
        // Armazena o ID do utilizador com sessão iniciada.
        private readonly int _userId;

        // Construtor que recebe o ID do utilizador para saber quais encomendas mostrar.
        public HistoricoEncomendas(int userId)
        {
            InitializeComponent();
            _userId = userId;

            // Ativa a barra de scroll no painel se o conteúdo for maior que a área visível.
            flowPanel_Encomendas.AutoScroll = true;

            // Associa o evento de carregamento do formulário ao método de inicialização.
            this.Load += HistoricoEncomendas_Load;
        }

        // Método executado quando o formulário é carregado.
        private void HistoricoEncomendas_Load(object sender, EventArgs e)
        {
            // Chama o método para popular o formulário com os dados.
            CarregarHistorico();
        }

        // Vai à base de dados buscar e exibir as encomendas do utilizador.
        private void CarregarHistorico()
        {
            // Limpa o painel para evitar duplicados ao recarregar.
            flowPanel_Encomendas.Controls.Clear();

            // Pede ao OrderService a lista de encomendas do utilizador.
            var encomendas = OrderService.GetHistoricoEncomendas(_userId);

            // Verifica se existem encomendas para exibir.
            if (encomendas.Count == 0)
            {
                // Se não houver, mostra uma mensagem informativa.
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
                // Se houver, cria um UserControl 'EncomendaCard' para cada uma.
                foreach (var encomenda in encomendas)
                {
                    var control = new EncomendaCard(encomenda);
                    // Subscreve ao evento de clique do botão do PDF no UserControl.
                    control.BaixarFaturaClicked += EncomendaCard_BaixarFaturaClicked;
                    // Adiciona o card ao painel.
                    flowPanel_Encomendas.Controls.Add(control);
                }
            }
        }

        // Handler executado quando o botão "Baixar Fatura" de um dos cards é clicado.
        private void EncomendaCard_BaixarFaturaClicked(object sender, EventArgs e)
        {
            // 'sender' é o EncomendaCard que disparou o evento.
            if (sender is EncomendaCard control)
            {
                // Pede ao OrderService os detalhes (produtos) da encomenda específica.
                var itens = OrderService.GetDetalhesEncomenda(control.InfoEncomenda.Id);

                // Chama o PdfService para gerar o ficheiro da fatura.
                PdfService.GerarFaturaPdf(control.InfoEncomenda, itens);
            }
        }
    }
}

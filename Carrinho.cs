using System;
using System.Linq;
using System.Windows.Forms;

namespace OfiPecas
{
    // Formulário para gerir o carrinho de compras.
    public partial class Carrinho : Form
    {
        // Guarda o ID do utilizador atual.
        private readonly int _userId;

        // Construtor que recebe o ID do utilizador da loja.
        public Carrinho(int userId)
        {
            InitializeComponent();
            _userId = userId;
            flowPanel_Produtos.AutoScroll = true; // Ativa o scroll se houver muitos itens.
            this.Load += Carrinho_Load;
        }

        // Método executado quando o formulário é carregado.
        private void Carrinho_Load(object sender, EventArgs e)
        {
            CarregarItensDoCarrinho();
        }

        // Vai buscar os itens à base de dados e cria os controlos visuais.
        private void CarregarItensDoCarrinho()
        {
            flowPanel_Produtos.Controls.Clear(); // Limpa o painel.

            // Pede ao CartService a lista de itens do utilizador.
            var itens = CartService.GetItensDoCarrinho(_userId);

            if (itens.Count == 0)
            {
                // Se o carrinho estiver vazio, mostra uma mensagem.
                Label lblVazio = new Label { Text = "O seu carrinho está vazio.", AutoSize = true, ForeColor = System.Drawing.Color.White };
                flowPanel_Produtos.Controls.Add(lblVazio);
                Button_Comprar.Enabled = false;
            }
            else
            {
                // Para cada item, cria um UserControl 'ItemCarrinho'.
                foreach (var item in itens)
                {
                    var itemControl = new ItemCarrinho(item);

                    // Liga os eventos do UserControl (remover, mudar quantidade) aos métodos deste formulário.
                    itemControl.RemoverClicked += ItemControl_RemoverClicked;
                    itemControl.QuantidadeChanged += ItemControl_QuantidadeChanged;

                    flowPanel_Produtos.Controls.Add(itemControl);
                }
                Button_Comprar.Enabled = true;
            }
            // Atualiza os totais no final.
            AtualizarTotais();
        }

        // Calcula e exibe os totais da compra.
        private void AtualizarTotais()
        {
            int totalProdutos = 0;
            decimal valorTotal = 0;

            // Itera por cada UserControl 'ItemCarrinho' no painel.
            foreach (ItemCarrinho itemControl in flowPanel_Produtos.Controls.OfType<ItemCarrinho>())
            {
                // Usa as propriedades públicas do UserControl para obter os dados.
                totalProdutos += itemControl.Quantidade;
                valorTotal += itemControl.PrecoUnitario * itemControl.Quantidade;
            }

            // Atualiza o texto das labels.
            lbl_Total_Produtos.Text = $"Total de Produtos: {totalProdutos}";
            lbl_Total.Text = $"Total: {valorTotal:C}"; // O formato 'C' mostra o símbolo da moeda (€).
        }

        // Chamado quando o utilizador muda a quantidade de um item.
        private void ItemControl_QuantidadeChanged(object sender, EventArgs e)
        {
            if (sender is ItemCarrinho itemControl)
            {
                CartService.AtualizarQuantidadeItem(itemControl.ItemId, itemControl.Quantidade);
                AtualizarTotais();
            }
        }

        // Chamado quando o utilizador remove um item.
        private void ItemControl_RemoverClicked(object sender, EventArgs e)
        {
            if (sender is ItemCarrinho itemControl)
            {
                var confirmResult = MessageBox.Show("Tem a certeza?", "Confirmar Remoção", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmResult == DialogResult.Yes)
                {
                    CartService.RemoverItemDoCarrinho(itemControl.ItemId);
                    // Recarrega o carrinho para atualizar a vista.
                    CarregarItensDoCarrinho();
                }
            }
        }

        // Inicia o processo de checkout.
        private void Button_Comprar_Click(object sender, EventArgs e)
        {
            // Abre o formulário de pagamento.
            var formPagamento = new Pagamento(_userId);
            DialogResult resultado = formPagamento.ShowDialog();

            // Se o pagamento for bem-sucedido, fecha o carrinho.
            if (resultado == DialogResult.OK)
            {
                this.Close();
            }
        }

        // Fecha o formulário do carrinho.
        private void ImageButton_Back_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

using System;
using System.Linq;
using System.Windows.Forms;

namespace OfiPecas
{
    public partial class Carrinho : Form
    {
        private readonly int _userId;

        public Carrinho(int userId)
        {
            InitializeComponent();
            _userId = userId;
            flowPanel_Produtos.AutoScroll = true;
            this.Load += Carrinho_Load;
        }

        private void Carrinho_Load(object sender, EventArgs e)
        {
            CarregarItensDoCarrinho();
        }

        private void CarregarItensDoCarrinho()
        {
            flowPanel_Produtos.Controls.Clear();

            // **CHAMADA CORRIGIDA**
            var itens = CartService.GetItensDoCarrinho(_userId);

            if (itens.Count == 0)
            {
                Label lblVazio = new Label { Text = "O seu carrinho está vazio.", AutoSize = true, ForeColor = System.Drawing.Color.White };
                flowPanel_Produtos.Controls.Add(lblVazio);
                Button_Comprar.Enabled = false;
            }
            else
            {
                foreach (var item in itens)
                {
                    var itemControl = new ItemCarrinho(item);
                    itemControl.RemoverClicked += ItemControl_RemoverClicked;
                    itemControl.QuantidadeChanged += ItemControl_QuantidadeChanged;
                    flowPanel_Produtos.Controls.Add(itemControl);
                }
                Button_Comprar.Enabled = true;
            }
            AtualizarTotais();
        }

        private void AtualizarTotais()
        {
            int totalProdutos = 0;
            decimal valorTotal = 0;
            foreach (ItemCarrinho itemControl in flowPanel_Produtos.Controls.OfType<ItemCarrinho>())
            {
                totalProdutos += itemControl.Quantidade;
                valorTotal += itemControl.PrecoUnitario * itemControl.Quantidade;
            }
            lbl_Total_Produtos.Text = $"Total de Produtos: {totalProdutos}";
            lbl_Total.Text = $"Total: {valorTotal:C}";
        }

        private void ItemControl_QuantidadeChanged(object sender, EventArgs e)
        {
            if (sender is ItemCarrinho itemControl)
            {
                // **CHAMADA CORRIGIDA**
                CartService.AtualizarQuantidadeItem(itemControl.ItemId, itemControl.Quantidade);
                AtualizarTotais();
            }
        }

        private void ItemControl_RemoverClicked(object sender, EventArgs e)
        {
            if (sender is ItemCarrinho itemControl)
            {
                var confirmResult = MessageBox.Show("Tem a certeza?", "Confirmar Remoção", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmResult == DialogResult.Yes)
                {
                    // **CHAMADA CORRIGIDA**
                    CartService.RemoverItemDoCarrinho(itemControl.ItemId);
                    CarregarItensDoCarrinho();
                }
            }
        }

        private void Button_Comprar_Click(object sender, EventArgs e)
        {
            var formPagamento = new Pagamento(_userId);
            DialogResult resultado = formPagamento.ShowDialog();
            if (resultado == DialogResult.OK)
            {
                this.Close();
            }
        }

        private void ImageButton_Back_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

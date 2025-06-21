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
    public partial class ItemCarrinho : UserControl
    {
        // Propriedades Públicas para acesso fácil
        public int ItemId { get; private set; }
        public decimal PrecoUnitario { get; private set; }

        // Propriedade pública que lê diretamente o valor do controlo e o converte
        public int Quantidade => (int)NumUpDwn_Quantidade.Value;

        // Eventos para comunicar com o formulário Carrinho
        public event EventHandler RemoverClicked;
        public event EventHandler QuantidadeChanged;

        public ItemCarrinho(ItemCarrinhoInfo item)
        {
            InitializeComponent();

            this.ItemId = item.ItemId;
            this.PrecoUnitario = item.PrecoUnitario; // Guardamos o preço

            picProduto.Image = item.GetImagem();
            picProduto.SizeMode = PictureBoxSizeMode.Zoom;
            lblNome.Text = item.Nome;
            lblPreco.Text = $"{item.PrecoUnitario:C}";
            NumUpDwn_Quantidade.Value = item.Quantidade;
            NumUpDwn_Quantidade.Maximum = item.Estoque;

            // Define o valor (só se for menor ou igual ao stock)
            if (item.Quantidade <= NumUpDwn_Quantidade.Maximum)
            {
                NumUpDwn_Quantidade.Value = item.Quantidade;
            }
            else
            {
                NumUpDwn_Quantidade.Value = NumUpDwn_Quantidade.Maximum;
            }

            AtualizarSubtotal();

            btnRemover.Click += BtnRemover_Click;
            NumUpDwn_Quantidade.ValueChanged += NumUpDwn_Quantidade_ValueChanged;
        }

        private void AtualizarSubtotal()
        {
            decimal subtotal = this.PrecoUnitario * NumUpDwn_Quantidade.Value;
            lblSubtotal.Text = $"{subtotal:C}";
        }

        private void NumUpDwn_Quantidade_ValueChanged(object sender, EventArgs e)
        {
            AtualizarSubtotal();
            QuantidadeChanged?.Invoke(this, EventArgs.Empty);
        }

        private void BtnRemover_Click(object sender, EventArgs e)
        {
            RemoverClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
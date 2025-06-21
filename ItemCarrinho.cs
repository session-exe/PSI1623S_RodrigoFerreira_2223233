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
    // Nome da classe corrigido para corresponder ao teu projeto
    public partial class ItemCarrinho : UserControl
    {
        // Propriedades para guardar informação importante
        public int ItemId { get; private set; }
        private decimal _precoUnitario;

        // Eventos para comunicar com o formulário Carrinho
        public event EventHandler RemoverClicked;
        public event EventHandler QuantidadeChanged;

        public ItemCarrinho(ItemCarrinhoInfo item)
        {
            InitializeComponent();

            // Guardar dados importantes
            this.ItemId = item.ItemId;
            this._precoUnitario = item.PrecoUnitario;

            // Preencher os controlos com os dados do item
            picProduto.Image = item.GetImagem();
            picProduto.SizeMode = PictureBoxSizeMode.Zoom;
            lblNome.Text = item.Nome;
            lblPreco.Text = $"{item.PrecoUnitario:C}"; // Formato de moeda (€)

            // Define o valor do NumericUpDown sem disparar o evento de mudança
            NumUpDwn_Quantidade.Value = item.Quantidade;

            // Calcula e mostra o subtotal inicial
            AtualizarSubtotal();

            // Ligar os eventos dos controlos aos nossos métodos
            btnRemover.Click += BtnRemover_Click;
            NumUpDwn_Quantidade.ValueChanged += NumUpDwn_Quantidade_ValueChanged;
        }

        // Método privado para calcular e atualizar a label do subtotal
        private void AtualizarSubtotal()
        {
            decimal subtotal = _precoUnitario * NumUpDwn_Quantidade.Value;
            lblSubtotal.Text = $"{subtotal:C}";
        }

        // Dispara o evento quando a quantidade muda
        private void NumUpDwn_Quantidade_ValueChanged(object sender, EventArgs e)
        {
            AtualizarSubtotal();
            QuantidadeChanged?.Invoke(this, EventArgs.Empty); // Avisa o Carrinho que algo mudou
        }

        // Dispara o evento quando o botão de remover é clicado
        private void BtnRemover_Click(object sender, EventArgs e)
        {
            RemoverClicked?.Invoke(this, EventArgs.Empty); // Avisa o Carrinho para remover este item
        }
    }
}
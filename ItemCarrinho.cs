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
    // Este é um UserControl reutilizável que representa um item no carrinho de compras.
    public partial class ItemCarrinho : UserControl
    {
        // Propriedades públicas para que o formulário do Carrinho possa aceder aos dados deste item.
        public int ItemId { get; private set; }
        public decimal PrecoUnitario { get; private set; }

        // Propriedade que lê o valor do controlo numérico e o converte para inteiro.
        public int Quantidade => (int)NumUpDwn_Quantidade.Value;

        // Eventos personalizados para comunicar com o formulário Carrinho.
        public event EventHandler RemoverClicked;
        public event EventHandler QuantidadeChanged;

        // Construtor que recebe os dados de um item para configurar o controlo.
        public ItemCarrinho(ItemCarrinhoInfo item)
        {
            InitializeComponent();

            // Armazena os dados importantes.
            this.ItemId = item.ItemId;
            this.PrecoUnitario = item.PrecoUnitario;

            // Preenche os controlos visuais com a informação do item.
            picProduto.Image = item.GetImagem();
            picProduto.SizeMode = PictureBoxSizeMode.Zoom;
            lblNome.Text = item.Nome;
            lblPreco.Text = $"{item.PrecoUnitario:C}"; // Formata como moeda (€).

            // Define o limite máximo do controlo de quantidade igual ao stock disponível.
            NumUpDwn_Quantidade.Maximum = item.Estoque;
            // Define a quantidade inicial, garantindo que não ultrapassa o stock.
            if (item.Quantidade <= NumUpDwn_Quantidade.Maximum)
            {
                NumUpDwn_Quantidade.Value = item.Quantidade;
            }
            else
            {
                NumUpDwn_Quantidade.Value = NumUpDwn_Quantidade.Maximum;
            }

            // Calcula e mostra o subtotal inicial.
            AtualizarSubtotal();

            // Liga os eventos de clique e mudança de valor aos seus métodos correspondentes.
            btnRemover.Click += BtnRemover_Click;
            NumUpDwn_Quantidade.ValueChanged += NumUpDwn_Quantidade_ValueChanged;
        }

        // Método privado para calcular e atualizar a label do subtotal.
        private void AtualizarSubtotal()
        {
            decimal subtotal = this.PrecoUnitario * NumUpDwn_Quantidade.Value;
            lblSubtotal.Text = $"{subtotal:C}";
        }

        // Chamado quando o valor do controlo de quantidade é alterado.
        private void NumUpDwn_Quantidade_ValueChanged(object sender, EventArgs e)
        {
            // Atualiza o subtotal e "avisa" o formulário do Carrinho que a quantidade mudou.
            AtualizarSubtotal();
            QuantidadeChanged?.Invoke(this, EventArgs.Empty);
        }

        // Chamado quando o botão "Remover" é clicado.
        private void BtnRemover_Click(object sender, EventArgs e)
        {
            // "Avisa" o formulário do Carrinho que este item deve ser removido.
            RemoverClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}

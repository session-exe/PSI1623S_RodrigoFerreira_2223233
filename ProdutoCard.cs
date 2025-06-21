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
    public partial class ProdutoCard : UserControl
    {
        public int PecaId { get; private set; }
        private int _userId;

        // Evento que "avisa" a Loja que o botão foi clicado
        public event EventHandler AdicionarAoCarrinhoClicked;

        public ProdutoCard(int produtoId, string nome, decimal preco, int stock, Image imagem, int userId)
        {
            InitializeComponent();

            this.PecaId = produtoId;
            _userId = userId;

            if (stock > 0)
            {
                lblStock.Text = $"Stock: {stock}";
                btnAdicionar.Enabled = true;
            }
            else
            {
                lblStock.Text = "Esgotado";
                btnAdicionar.Enabled = false;
            }

            lblNome.Text = nome;
            lblPreco.Text = $"{preco:C}";
            picProduto.Image = imagem;
            picProduto.SizeMode = PictureBoxSizeMode.Zoom;
        }

        // Este método é chamado pelo clique do botão (ligado no Designer).
        // Ele apenas dispara o evento para a Loja tratar do resto.
        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            AdicionarAoCarrinhoClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}

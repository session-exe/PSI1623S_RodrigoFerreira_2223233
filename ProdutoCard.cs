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
        // SOLUÇÃO: Propriedade pública para o ID da Peça.
        // A Loja precisa disto para saber qual produto adicionar ao carrinho.
        public int PecaId { get; private set; }
        private int _userId;

        // SOLUÇÃO: Evento público.
        // O card usa isto para "avisar" a Loja que o botão foi clicado.
        public event EventHandler AdicionarAoCarrinhoClicked;

        public ProdutoCard(int produtoId, string nome, decimal preco, int stock, Image imagem, int userId)
        {
            InitializeComponent();

            this.PecaId = produtoId; // Atribui o ID à propriedade pública
            _userId = userId;

            // Desativa o botão se o produto estiver esgotado
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
            lblPreco.Text = $"{preco:C}"; // Formatação para moeda (ex: 65,50 €)
            picProduto.Image = imagem;
            picProduto.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            // SOLUÇÃO: Em vez de mostrar uma MessageBox, disparamos o evento.
            AdicionarAoCarrinhoClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}

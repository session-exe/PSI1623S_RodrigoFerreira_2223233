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
    // UserControl para exibir um produto individual no catálogo da loja.
    public partial class ProdutoCard : UserControl
    {
        // Propriedades para armazenar o ID da peça e do utilizador.
        public int PecaId { get; private set; }
        private int _userId;

        // Evento para notificar o formulário Loja quando o botão 'Adicionar' é clicado.
        public event EventHandler AdicionarAoCarrinhoClicked;

        // Construtor que recebe os dados da peça para configurar os controlos.
        public ProdutoCard(int produtoId, string nome, decimal preco, int stock, Image imagem, int userId)
        {
            InitializeComponent();

            this.PecaId = produtoId;
            _userId = userId;

            // Define o texto de stock e o estado do botão com base na disponibilidade.
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

            // Preenche os controlos da interface com os dados da peça.
            lblNome.Text = nome;
            lblPreco.Text = $"{preco:C}"; // Formata como moeda.
            picProduto.Image = imagem;
            picProduto.SizeMode = PictureBoxSizeMode.Zoom;
        }

        // Handler para o clique do botão 'Adicionar'.
        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            // Dispara o evento para o formulário Loja tratar da lógica.
            AdicionarAoCarrinhoClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}

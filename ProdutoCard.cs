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
        private int _produtoId;
        private int _userId;

        public ProdutoCard(int produtoId, string nome, decimal preco, Image imagem, int userId)
        {
            InitializeComponent();
            _produtoId = produtoId;
            _userId = userId;

            lblNome.Text = nome;
            lblPreco.Text = $"{preco:0.00} €";
            picProduto.Image = imagem;

            btnAdicionar.Click += BtnAdicionar_Click;
        }

        private void BtnAdicionar_Click(object sender, EventArgs e)
        {
            // TODO: Adicionar ao carrinho na BD
            MessageBox.Show("Produto adicionado ao carrinho!", "Carrinho", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}

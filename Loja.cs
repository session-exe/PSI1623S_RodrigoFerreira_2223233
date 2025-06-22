using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OfiPecas
{
    public partial class Loja : Form
    {
        private readonly int _userId;
        private readonly bool _isAdmin;

        public Loja(int userId, bool isAdmin)
        {
            InitializeComponent();
            _userId = userId;
            _isAdmin = isAdmin;

            // --- CORREÇÃO DE SEGURANÇA 1 ---
            // O botão de administração só fica visível para administradores.
            ImageButton_Admin.Visible = _isAdmin;

            this.Load += Loja_Load;
        }

        private void Loja_Load(object sender, EventArgs e)
        {
            CarregarPecas();
            CarregarCategorias();
        }

        private void CarregarPecas(List<Peca> pecas = null)
        {
            flowLayoutPanel_Produtos.Controls.Clear();
            if (pecas == null)
            {
                pecas = StoreService.GetPecas();
            }
            if (pecas.Count == 0)
            {
                Label lblEmpty = new Label { Text = "Nenhuma peça encontrada.", AutoSize = true, ForeColor = System.Drawing.Color.Gray };
                flowLayoutPanel_Produtos.Controls.Add(lblEmpty);
                return;
            }
            foreach (var peca in pecas)
            {
                var card = new ProdutoCard(peca.Id, peca.Nome, peca.Preco, peca.Estoque, peca.GetImagem(), _userId);
                card.AdicionarAoCarrinhoClicked += Card_AdicionarAoCarrinhoClicked;
                flowLayoutPanel_Produtos.Controls.Add(card);
            }
        }

        private void CarregarCategorias()
        {
            flowLayoutPanel_Sidebar.Controls.Clear();
            var todas = new Categoria(0, "Todas as Peças");
            todas.CategoriaClicked += Categoria_Clicked;
            flowLayoutPanel_Sidebar.Controls.Add(todas);

            var categorias = StoreService.GetCategorias();
            foreach (var cat in categorias)
            {
                var categoriaControl = new Categoria(cat.Id, cat.Nome);
                categoriaControl.CategoriaClicked += Categoria_Clicked;
                flowLayoutPanel_Sidebar.Controls.Add(categoriaControl);
            }
        }

        private void Categoria_Clicked(object sender, EventArgs e)
        {
            if (sender is Categoria categoriaControl)
            {
                if (categoriaControl.CategoriaId == 0)
                {
                    CarregarPecas();
                }
                else
                {
                    var pecasFiltradas = StoreService.GetPecasPorCategoria(categoriaControl.CategoriaId);
                    CarregarPecas(pecasFiltradas);
                }
            }
        }

        private void Card_AdicionarAoCarrinhoClicked(object sender, EventArgs e)
        {
            if (sender is ProdutoCard card)
            {
                var (success, message) = CartService.AdicionarAoCarrinho(_userId, card.PecaId);
                MessageBox.Show(message, success ? "Sucesso" : "Erro", MessageBoxButtons.OK, success ? MessageBoxIcon.Information : MessageBoxIcon.Error);
            }
        }

        private void ImageButton_Pesquisa_Click(object sender, EventArgs e)
        {
            string termoPesquisa = TextBox_Pesquisa.Text.Trim();
            var pecasEncontradas = StoreService.PesquisarPecas(termoPesquisa);
            CarregarPecas(pecasEncontradas);
        }

        private void ImageButton_Cart_Click(object sender, EventArgs e)
        {
            var carrinho = new Carrinho(_userId);
            carrinho.ShowDialog();
            Loja_Load(this, EventArgs.Empty);
        }

        private void ImageButton_SidePanel_Click(object sender, EventArgs e)
        {
            flowLayoutPanel_Sidebar.Visible = !flowLayoutPanel_Sidebar.Visible;
        }

        // A tua funcionalidade de voltar ao Login está preservada
        private void PictureBox_Logo_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }

        private void ImageButton_HistoricoEcomendas_Click(object sender, EventArgs e)
        {
            var formHistorico = new HistoricoEncomendas(_userId);
            formHistorico.ShowDialog();
        }

        private void ImageButton_Settings_Click(object sender, EventArgs e)
        {
            var formDefinicoes = new DefinicoesUser(_userId);
            formDefinicoes.ShowDialog();
        }

        private void ImageButton_Admin_Click(object sender, EventArgs e)
        {
            // --- CORREÇÃO DE SEGURANÇA 2 ---
            // Passamos a permissão de admin para o construtor do painel.
            var formAdmin = new PainelAdmin(_isAdmin);
            formAdmin.ShowDialog();

            // Atualiza a loja quando o painel de admin for fechado.
            Loja_Load(this, EventArgs.Empty);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic.ApplicationServices;

namespace OfiPecas
{
    public partial class Loja : Form
    {
        // Campos para guardar a informação do utilizador que fez login
        private readonly int _userId;
        private readonly bool _isAdmin;

        public Loja(int userId, bool isAdmin)
        {
            InitializeComponent();
            _userId = userId;
            _isAdmin = isAdmin;

            // Esconde o botão de admin se o utilizador não for administrador
            //ImageButton_Admin.Visible = _isAdmin;

            // Associa o evento Load do formulário ao nosso método de inicialização
            this.Load += Loja_Load;
        }

        private void Loja_Load(object sender, EventArgs e)
        {
            CarregarPecas();      // Carrega todos os produtos por defeito
            CarregarCategorias(); // Carrega as categorias na barra lateral
        }

        #region Carregamento de Controlos Dinâmicos

        private void CarregarPecas(List<Peca> pecas = null)
        {
            // Limpa os controlos antigos antes de adicionar novos
            flowLayoutPanel_Produtos.Controls.Clear();

            // Se não for passada uma lista de peças, vai buscar todas à base de dados
            if (pecas == null)
            {
                pecas = StoreService.GetPecas();
            }

            // Se, mesmo assim, a lista estiver vazia (nenhum produto encontrado), mostra uma mensagem.
            if (pecas.Count == 0)
            {
                Label lblEmpty = new Label
                {
                    Text = "Nenhuma peça encontrada.",
                    AutoSize = true,
                    Font = new System.Drawing.Font("Segoe UI", 12F),
                    ForeColor = System.Drawing.Color.Gray,
                    Margin = new Padding(10)
                };
                flowLayoutPanel_Produtos.Controls.Add(lblEmpty);
                return;
            }

            // Itera por cada peça na lista
            foreach (var peca in pecas)
            {
                var card = new ProdutoCard(
                    peca.Id,
                    peca.Nome,
                    peca.Preco,
                    peca.Estoque,
                    peca.GetImagem(),
                    _userId
                );

                // Subscreve ao evento do card. Quando for disparado, chama o método correspondente.
                card.AdicionarAoCarrinhoClicked += Card_AdicionarAoCarrinhoClicked;

                // Adiciona o card ao painel
                flowLayoutPanel_Produtos.Controls.Add(card);
            }
        }

        private void CarregarCategorias()
        {
            flowLayoutPanel_Sidebar.Controls.Clear();

            // Adiciona uma opção para "Todas as Peças" para limpar o filtro
            var todas = new Categoria(0, "Todas as Peças"); // ID 0 é um caso especial
            todas.CategoriaClicked += Categoria_Clicked;
            flowLayoutPanel_Sidebar.Controls.Add(todas);

            // Busca as categorias reais da base de dados
            var categorias = StoreService.GetCategorias();

            foreach (var cat in categorias)
            {
                var categoriaControl = new Categoria(cat.Id, cat.Nome);
                categoriaControl.CategoriaClicked += Categoria_Clicked;
                flowLayoutPanel_Sidebar.Controls.Add(categoriaControl);
            }
        }

        #endregion

        #region Handlers de Eventos dos Controlos Dinâmicos

        private void Categoria_Clicked(object sender, EventArgs e)
        {
            if (sender is Categoria categoriaControl)
            {
                // Se a categoria clicada for "Todas as Peças" (nosso caso especial ID 0)
                if (categoriaControl.CategoriaId == 0)
                {
                    CarregarPecas(); // Carrega todas as peças sem filtro
                }
                else
                {
                    // Caso contrário, busca as peças para a categoria específica
                    var pecasFiltradas = StoreService.GetPecasPorCategoria(categoriaControl.CategoriaId);
                    CarregarPecas(pecasFiltradas);
                }
            }
        }

        private void Card_AdicionarAoCarrinhoClicked(object sender, EventArgs e)
        {
            if (sender is ProdutoCard card)
            {
                // Chamamos o método do serviço para adicionar ao carrinho
                var (success, message) = StoreService.AdicionarAoCarrinho(_userId, card.PecaId);

                // Mostramos o resultado ao utilizador.
                MessageBox.Show(message, success ? "Sucesso" : "Erro", MessageBoxButtons.OK, success ? MessageBoxIcon.Information : MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Handlers de Eventos dos Botões do Formulário

        private void ImageButton_Pesquisa_Click(object sender, EventArgs e)
        {
            string termoPesquisa = TextBox_Pesquisa.Text.Trim();
            var pecasEncontradas = StoreService.PesquisarPecas(termoPesquisa);
            CarregarPecas(pecasEncontradas);
        }

        private void ImageButton_SidePanel_Click(object sender, EventArgs e)
        {
            // Lógica simples para mostrar/esconder a barra de categorias
            flowLayoutPanel_Sidebar.Visible = !flowLayoutPanel_Sidebar.Visible;
        }

        private void ImageButton_Cart_Click(object sender, EventArgs e)
        {
            // Cria uma nova instância do formulário Carrinho, passando o ID do utilizador
            var formCarrinho = new Carrinho(_userId);

            // Mostra o formulário. Usamos ShowDialog() para que o utilizador
            // não possa interagir com a loja enquanto o carrinho estiver aberto.
            formCarrinho.ShowDialog();
        }

        private void PictureBox_Logo_Click(object sender, EventArgs e)
        {
            var login = new Login();
            login.Show();
            this.Close();
        }

        // --- Outros botões (a implementar no futuro) ---

        private void ImageButton_Admin_Click(object sender, EventArgs e)
        {
            // TODO: Abrir o painel de administração
        }

        private void ImageButton_Settings_Click(object sender, EventArgs e)
        {
            // TODO: Abrir as definições do utilizador
        }


        #endregion

    }
}

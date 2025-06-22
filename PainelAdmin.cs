using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace OfiPecas
{
    public partial class PainelAdmin : Form
    {
        private int _selectedPecaId = 0;
        private int _selectedCategoriaId = 0;
        private int _selectedUserId = 0;
        private byte[] _imagemPecaBytes = null;

        public PainelAdmin(bool isAdmin)
        {
            InitializeComponent();

            if (!isAdmin)
            {
                MessageBox.Show("Acesso negado. Apenas administradores podem aceder a este painel.", "Acesso Restrito", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Load += (s, e) => this.Close();
                return;
            }

            this.NumericUpDown_PecaPreco.Maximum = 999999;
            this.NumericUpDown_PecaEstoque.Maximum = 99999;

            this.Load += PainelAdmin_Load;
        }

        private void PainelAdmin_Load(object sender, EventArgs e)
        {
            if (this.IsDisposed) return;
            RefreshPecasTab();
            RefreshCategoriasTab();
            RefreshUtilizadoresTab();
        }

        // --- MÉTODOS DE REFRESH PARA CADA ABA ---

        private void RefreshPecasTab()
        {
            var pecas = StoreService.GetPecas();
            DataGridView_Pecas.DataSource = null;
            DataGridView_Pecas.DataSource = pecas;

            var categorias = StoreService.GetCategorias();
            ComboBox_PecaCategoria.DataSource = null;
            ComboBox_PecaCategoria.DataSource = categorias;
            ComboBox_PecaCategoria.DisplayMember = "Nome";
            ComboBox_PecaCategoria.ValueMember = "Id";

            LimparFormularioPeca();
        }

        private void RefreshCategoriasTab()
        {
            var categorias = StoreService.GetCategorias();
            ComboBox_Categorias.DataSource = null;
            ComboBox_Categorias.DataSource = categorias;
            ComboBox_Categorias.DisplayMember = "Nome";
            ComboBox_Categorias.ValueMember = "Id";
            LimparFormularioCategoria();
        }

        private void RefreshUtilizadoresTab()
        {
            DataGridView_Utilizadores.DataSource = AdminService.GetUtilizadores();
            _selectedUserId = 0;
        }

        // --- ABA 1: GESTÃO DE PEÇAS ---

        // Este método será chamado pelo evento que acabaste de ligar
        private void DataGridView_Pecas_SelectionChanged(object sender, EventArgs e)
        {
            if (DataGridView_Pecas.CurrentRow?.DataBoundItem is Peca peca)
            {
                _selectedPecaId = peca.Id;
                TextBox_PecaNome.Text = peca.Nome;
                NumericUpDown_PecaPreco.Value = peca.Preco;
                NumericUpDown_PecaEstoque.Value = peca.Estoque;

                // --- CORREÇÃO ROBUSTA AQUI ---
                // Em vez de usar SelectedValue, encontramos o índice do item correspondente.
                int categoriaIdParaSelecionar = peca.CategoriaId;
                for (int i = 0; i < ComboBox_PecaCategoria.Items.Count; i++)
                {
                    if (ComboBox_PecaCategoria.Items[i] is CategoriaInfo item && item.Id == categoriaIdParaSelecionar)
                    {
                        ComboBox_PecaCategoria.SelectedIndex = i;
                        break; // Encontrámos, podemos parar o loop
                    }
                }

                _imagemPecaBytes = peca.ImagemBytes;
                PictureBox_PecaImagem.Image = peca.GetImagem();
            }
        }

        private void Button_Novo_Click(object sender, EventArgs e)
        {
            LimparFormularioPeca();
        }

        private void Button_Guardar_Click(object sender, EventArgs e)
        {
            if (ComboBox_PecaCategoria.SelectedValue == null)
            {
                MessageBox.Show("Por favor, selecione uma categoria para a peça.", "Erro de Validação");
                return;
            }
            int categoriaId = Convert.ToInt32(ComboBox_PecaCategoria.SelectedValue);

            var (success, message) = AdminService.GuardarPeca(
                _selectedPecaId,
                TextBox_PecaNome.Text,
                NumericUpDown_PecaPreco.Value,
                (int)NumericUpDown_PecaEstoque.Value,
                categoriaId,
                _imagemPecaBytes
            );
            MessageBox.Show(message, success ? "Sucesso" : "Erro");
            if (success) RefreshPecasTab();
        }

        private void Button_Apagar_Click(object sender, EventArgs e)
        {
            if (_selectedPecaId == 0)
            {
                MessageBox.Show("Por favor, selecione uma peça para apagar.", "Aviso");
                return;
            }
            var confirm = MessageBox.Show($"Tem a certeza que quer apagar a peça selecionada?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm == DialogResult.Yes)
            {
                var (success, message) = AdminService.ApagarPeca(_selectedPecaId);
                MessageBox.Show(message, success ? "Sucesso" : "Erro");
                if (success) RefreshPecasTab();
            }
        }

        private void Button_CarregarImagem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog { Filter = "Ficheiros de Imagem|*.jpg;*.jpeg;*.png;*.bmp" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        _imagemPecaBytes = File.ReadAllBytes(ofd.FileName);
                        PictureBox_PecaImagem.Image = Image.FromFile(ofd.FileName);
                    }
                    catch (Exception ex) { MessageBox.Show($"Erro ao carregar imagem: {ex.Message}"); }
                }
            }
        }

        private void LimparFormularioPeca()
        {
            _selectedPecaId = 0;
            TextBox_PecaNome.Clear();
            NumericUpDown_PecaPreco.Value = 0;
            NumericUpDown_PecaEstoque.Value = 0;
            ComboBox_PecaCategoria.SelectedIndex = -1;
            PictureBox_PecaImagem.Image = null;
            _imagemPecaBytes = null;
            DataGridView_Pecas.ClearSelection();
        }

        // --- ABA 2: GESTÃO DE CATEGORIAS ---

        private void ComboBox_Categorias_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComboBox_Categorias.SelectedItem is CategoriaInfo categoria)
            {
                _selectedCategoriaId = categoria.Id;
                TextBox_CategoriaNome.Text = categoria.Nome;
            }
        }

        private void Button_CategoriaNovo_Click(object sender, EventArgs e)
        {
            LimparFormularioCategoria();
        }

        private void Button_CategoriaGuardar_Click(object sender, EventArgs e)
        {
            var (success, message) = AdminService.GuardarCategoria(_selectedCategoriaId, TextBox_CategoriaNome.Text);
            MessageBox.Show(message, success ? "Sucesso" : "Erro");
            if (success)
            {
                RefreshCategoriasTab();
                RefreshPecasTab();
            }
        }

        private void Button_CategoriaApagar_Click(object sender, EventArgs e)
        {
            if (_selectedCategoriaId == 0)
            {
                MessageBox.Show("Por favor, selecione uma categoria para apagar.", "Aviso");
                return;
            }
            var confirm = MessageBox.Show($"Tem a certeza que quer apagar a categoria selecionada?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm == DialogResult.Yes)
            {
                var (success, message) = AdminService.ApagarCategoria(_selectedCategoriaId);
                MessageBox.Show(message, success ? "Sucesso" : "Erro");
                if (success)
                {
                    RefreshCategoriasTab();
                    RefreshPecasTab();
                }
            }
        }

        private void LimparFormularioCategoria()
        {
            _selectedCategoriaId = 0;
            TextBox_CategoriaNome.Clear();
            ComboBox_Categorias.SelectedIndex = -1;
        }

        // --- ABA 3: GESTÃO DE UTILIZADORES ---

        private void DataGridView_Utilizadores_SelectionChanged(object sender, EventArgs e)
        {
            if (DataGridView_Utilizadores.CurrentRow?.DataBoundItem is UserInfo user)
            {
                _selectedUserId = user.Id;
            }
        }

        private void TextBox_PesquisarUser_TextChanged(object sender, EventArgs e)
        {
            DataGridView_Utilizadores.DataSource = AdminService.GetUtilizadores(TextBox_PesquisarUser.Text);
        }

        private void Button_TornarAdmin_Click(object sender, EventArgs e)
        {
            var (success, message) = AdminService.SetAdminStatus(_selectedUserId, true);
            MessageBox.Show(message, success ? "Sucesso" : "Erro");
            if (success) RefreshUtilizadoresTab();
        }

        private void Button_RemoverAdmin_Click(object sender, EventArgs e)
        {
            var (success, message) = AdminService.SetAdminStatus(_selectedUserId, false);
            MessageBox.Show(message, success ? "Sucesso" : "Erro");
            if (success) RefreshUtilizadoresTab();
        }
    }
}

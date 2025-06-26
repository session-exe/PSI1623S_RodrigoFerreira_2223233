using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace OfiPecas
{
    // Formulário de gestão para administradores.
    public partial class PainelAdmin : Form
    {
        // IDs dos itens selecionados para edição ou remoção.
        private int _selectedPecaId = 0;
        private int _selectedCategoriaId = 0;
        private int _selectedUserId = 0;
        // Armazena os bytes da imagem carregada.
        private byte[] _imagemPecaBytes = null;

        // Construtor que recebe a permissão do utilizador.
        public PainelAdmin(bool isAdmin)
        {
            InitializeComponent();
            // Verifica se o utilizador tem permissão para aceder.
            if (!isAdmin)
            {
                MessageBox.Show("Acesso negado. Apenas administradores podem aceder a este painel.", "Acesso Restrito", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Load += (s, e) => this.Close();
                return;
            }
            // Aumenta o valor máximo dos controlos numéricos.
            this.NumericUpDown_PecaPreco.Maximum = 999999;
            this.NumericUpDown_PecaEstoque.Maximum = 99999;
            // Associa o evento de carregamento do formulário.
            this.Load += PainelAdmin_Load;
        }

        // Executado quando o formulário é carregado.
        private void PainelAdmin_Load(object sender, EventArgs e)
        {
            if (this.IsDisposed) return;
            // Carrega os dados iniciais de todas as abas.
            RefreshPecasTab();
            RefreshCategoriasTab();
            RefreshUtilizadoresTab();
        }

        // --- MÉTODOS DE ATUALIZAÇÃO DAS ABAS ---

        // Atualiza a tab de "Gestão de Peças".
        private void RefreshPecasTab()
        {
            DataGridView_Pecas.DataSource = StoreService.GetPecas();
            var categorias = StoreService.GetCategorias();
            ComboBox_PecaCategoria.DataSource = null;
            ComboBox_PecaCategoria.DataSource = categorias;
            ComboBox_PecaCategoria.DisplayMember = "Nome";
            ComboBox_PecaCategoria.ValueMember = "Id";
            LimparFormularioPeca();
        }

        // Atualiza a tab de "Gestão de Categorias".
        private void RefreshCategoriasTab()
        {
            var categorias = StoreService.GetCategorias();
            ComboBox_Categorias.DataSource = null;
            ComboBox_Categorias.DataSource = categorias;
            ComboBox_Categorias.DisplayMember = "Nome";
            ComboBox_Categorias.ValueMember = "Id";
            LimparFormularioCategoria();
        }

        // Atualiza a aba de "Gestão de Utilizadores".
        private void RefreshUtilizadoresTab()
        {
            DataGridView_Utilizadores.DataSource = AdminService.GetUtilizadores();
            _selectedUserId = 0;
        }

        // --- ABA 1: GESTÃO DE PEÇAS ---

        // Evento disparado ao selecionar uma linha na tabela.
        private void DataGridView_Pecas_SelectionChanged(object sender, EventArgs e)
        {
            if (DataGridView_Pecas.CurrentRow?.DataBoundItem is Peca peca)
            {
                // Preenche o formulário com os dados da peça selecionada.
                _selectedPecaId = peca.Id;
                TextBox_PecaNome.Text = peca.Nome;
                NumericUpDown_PecaPreco.Value = peca.Preco;
                NumericUpDown_PecaEstoque.Value = peca.Estoque;
                ComboBox_PecaCategoria.SelectedValue = peca.CategoriaId;
                _imagemPecaBytes = peca.ImagemBytes;
                PictureBox_PecaImagem.Image = peca.GetImagem();
            }
        }

        // Limpa o formulário de peças.
        private void Button_Novo_Click(object sender, EventArgs e)
        {
            LimparFormularioPeca();
            LimparFormularioPeca();
        }

        // Guarda uma peça nova ou as alterações de uma existente.
        private void Button_Guardar_Click(object sender, EventArgs e)
        {
            if (ComboBox_PecaCategoria.SelectedValue == null)
            {
                MessageBox.Show("Por favor, selecione uma categoria para a peça.", "Erro de Validação");
                return;
            }
            int categoriaId = Convert.ToInt32(ComboBox_PecaCategoria.SelectedValue);
            var (success, message) = AdminService.GuardarPeca(_selectedPecaId, TextBox_PecaNome.Text, NumericUpDown_PecaPreco.Value, (int)NumericUpDown_PecaEstoque.Value, categoriaId, _imagemPecaBytes);
            MessageBox.Show(message, success ? "Sucesso" : "Erro");
            if (success) RefreshPecasTab();
        }

        // Apaga a peça selecionada.
        private void Button_Apagar_Click(object sender, EventArgs e)
        {
            if (_selectedPecaId == 0) { MessageBox.Show("Por favor, selecione uma peça para apagar.", "Aviso"); return; }
            var confirm = MessageBox.Show($"Tem a certeza que quer apagar a peça selecionada?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm == DialogResult.Yes)
            {
                var (success, message) = AdminService.ApagarPeca(_selectedPecaId);
                MessageBox.Show(message, success ? "Sucesso" : "Erro");
                if (success) RefreshPecasTab();
            }
        }

        // Abre uma janela para carregar um ficheiro de imagem.
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

        // Método auxiliar para limpar o formulário de peças.
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

        // Evento disparado ao selecionar uma categoria.
        private void ComboBox_Categorias_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComboBox_Categorias.SelectedItem is CategoriaInfo categoria)
            {
                _selectedCategoriaId = categoria.Id;
                TextBox_CategoriaNome.Text = categoria.Nome;
            }
        }

        // Limpa o formulário de categorias.
        private void Button_CategoriaNovo_Click(object sender, EventArgs e)
        {
            // Solução funcional para o problema de eventos da ComboBox.
            LimparFormularioCategoria();
            LimparFormularioCategoria();
        }

        // Guarda uma nova categoria ou as alterações.
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

        // Apaga a categoria selecionada.
        private void Button_CategoriaApagar_Click(object sender, EventArgs e)
        {
            if (_selectedCategoriaId == 0) { MessageBox.Show("Por favor, selecione uma categoria para apagar.", "Aviso"); return; }
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

        // Método auxiliar para limpar o formulário de categorias.
        private void LimparFormularioCategoria()
        {
            _selectedCategoriaId = 0;
            TextBox_CategoriaNome.Clear();
            ComboBox_Categorias.SelectedIndex = -1;
        }

        // --- ABA 3: GESTÃO DE UTILIZADORES ---

        // Guarda o ID do utilizador selecionado.
        private void DataGridView_Utilizadores_SelectionChanged(object sender, EventArgs e)
        {
            if (DataGridView_Utilizadores.CurrentRow?.DataBoundItem is UserInfo user)
            {
                _selectedUserId = user.Id;
            }
        }

        // Filtra a lista de utilizadores.
        private void TextBox_PesquisarUser_TextChanged(object sender, EventArgs e)
        {
            DataGridView_Utilizadores.DataSource = AdminService.GetUtilizadores(TextBox_PesquisarUser.Text);
        }

        // Atribui permissões de administrador.
        private void Button_TornarAdmin_Click(object sender, EventArgs e)
        {
            var (success, message) = AdminService.SetAdminStatus(_selectedUserId, true);
            MessageBox.Show(message, success ? "Sucesso" : "Erro");
            if (success) RefreshUtilizadoresTab();
        }

        // Remove permissões de administrador.
        private void Button_RemoverAdmin_Click(object sender, EventArgs e)
        {
            var (success, message) = AdminService.SetAdminStatus(_selectedUserId, false);
            MessageBox.Show(message, success ? "Sucesso" : "Erro");
            if (success) RefreshUtilizadoresTab();
        }
    }
}

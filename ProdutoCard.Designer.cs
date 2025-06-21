namespace OfiPecas
{
    partial class ProdutoCard
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            picProduto = new Guna.UI2.WinForms.Guna2PictureBox();
            lblNome = new Guna.UI2.WinForms.Guna2HtmlLabel();
            lblPreco = new Guna.UI2.WinForms.Guna2HtmlLabel();
            btnAdicionar = new Guna.UI2.WinForms.Guna2Button();
            lblStock = new Guna.UI2.WinForms.Guna2HtmlLabel();
            guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            ((System.ComponentModel.ISupportInitialize)picProduto).BeginInit();
            guna2Panel1.SuspendLayout();
            SuspendLayout();
            // 
            // picProduto
            // 
            picProduto.BorderRadius = 6;
            picProduto.CustomizableEdges = customizableEdges1;
            picProduto.ImageRotate = 0F;
            picProduto.Location = new Point(12, 12);
            picProduto.Name = "picProduto";
            picProduto.ShadowDecoration.CustomizableEdges = customizableEdges2;
            picProduto.Size = new Size(236, 161);
            picProduto.TabIndex = 0;
            picProduto.TabStop = false;
            // 
            // lblNome
            // 
            lblNome.BackColor = Color.FromArgb(40, 40, 40);
            lblNome.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNome.ForeColor = Color.White;
            lblNome.Location = new Point(11, 181);
            lblNome.Name = "lblNome";
            lblNome.Size = new Size(61, 22);
            lblNome.TabIndex = 1;
            lblNome.Text = "Nomeee";
            // 
            // lblPreco
            // 
            lblPreco.BackColor = Color.FromArgb(40, 40, 40);
            lblPreco.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPreco.ForeColor = Color.White;
            lblPreco.Location = new Point(184, 181);
            lblPreco.Name = "lblPreco";
            lblPreco.Size = new Size(35, 22);
            lblPreco.TabIndex = 2;
            lblPreco.Text = "€€€€";
            // 
            // btnAdicionar
            // 
            btnAdicionar.BackColor = Color.FromArgb(40, 40, 40);
            btnAdicionar.BorderRadius = 6;
            btnAdicionar.CustomizableEdges = customizableEdges3;
            btnAdicionar.DisabledState.BorderColor = Color.DarkGray;
            btnAdicionar.DisabledState.CustomBorderColor = Color.DarkGray;
            btnAdicionar.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnAdicionar.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnAdicionar.FillColor = Color.LimeGreen;
            btnAdicionar.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAdicionar.ForeColor = Color.White;
            btnAdicionar.Image = Properties.Resources.icon_shopping_cart;
            btnAdicionar.ImageAlign = HorizontalAlignment.Right;
            btnAdicionar.Location = new Point(132, 215);
            btnAdicionar.Name = "btnAdicionar";
            btnAdicionar.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnAdicionar.Size = new Size(115, 30);
            btnAdicionar.TabIndex = 3;
            btnAdicionar.Text = "Adicionar";
            btnAdicionar.TextAlign = HorizontalAlignment.Left;
            btnAdicionar.Click += btnAdicionar_Click;
            // 
            // lblStock
            // 
            lblStock.BackColor = Color.FromArgb(40, 40, 40);
            lblStock.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblStock.ForeColor = Color.White;
            lblStock.Location = new Point(12, 223);
            lblStock.Name = "lblStock";
            lblStock.Size = new Size(27, 22);
            lblStock.TabIndex = 5;
            lblStock.Text = "000";
            // 
            // guna2Panel1
            // 
            guna2Panel1.BackColor = Color.Transparent;
            guna2Panel1.BorderColor = Color.DodgerBlue;
            guna2Panel1.BorderRadius = 12;
            guna2Panel1.BorderThickness = 2;
            guna2Panel1.Controls.Add(picProduto);
            guna2Panel1.Controls.Add(lblStock);
            guna2Panel1.CustomizableEdges = customizableEdges5;
            guna2Panel1.Dock = DockStyle.Fill;
            guna2Panel1.FillColor = Color.FromArgb(40, 40, 40);
            guna2Panel1.Location = new Point(0, 0);
            guna2Panel1.Name = "guna2Panel1";
            guna2Panel1.ShadowDecoration.CustomizableEdges = customizableEdges6;
            guna2Panel1.Size = new Size(260, 260);
            guna2Panel1.TabIndex = 6;
            // 
            // ProdutoCard
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Transparent;
            Controls.Add(lblPreco);
            Controls.Add(btnAdicionar);
            Controls.Add(lblNome);
            Controls.Add(guna2Panel1);
            ForeColor = SystemColors.ControlText;
            Name = "ProdutoCard";
            Size = new Size(260, 260);
            ((System.ComponentModel.ISupportInitialize)picProduto).EndInit();
            guna2Panel1.ResumeLayout(false);
            guna2Panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Guna.UI2.WinForms.Guna2PictureBox picProduto;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblNome;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblPreco;
        private Guna.UI2.WinForms.Guna2Button btnAdicionar;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblStock;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
    }
}

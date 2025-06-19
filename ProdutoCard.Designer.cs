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
            picProduto = new Guna.UI2.WinForms.Guna2PictureBox();
            lblNome = new Guna.UI2.WinForms.Guna2HtmlLabel();
            lblPreco = new Guna.UI2.WinForms.Guna2HtmlLabel();
            btnAdicionar = new Guna.UI2.WinForms.Guna2Button();
            ((System.ComponentModel.ISupportInitialize)picProduto).BeginInit();
            SuspendLayout();
            // 
            // picProduto
            // 
            picProduto.BorderRadius = 6;
            picProduto.CustomizableEdges = customizableEdges1;
            picProduto.ImageRotate = 0F;
            picProduto.Location = new Point(8, 9);
            picProduto.Name = "picProduto";
            picProduto.ShadowDecoration.CustomizableEdges = customizableEdges2;
            picProduto.Size = new Size(246, 166);
            picProduto.TabIndex = 0;
            picProduto.TabStop = false;
            // 
            // lblNome
            // 
            lblNome.BackColor = Color.Transparent;
            lblNome.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblNome.Location = new Point(11, 181);
            lblNome.Name = "lblNome";
            lblNome.Size = new Size(121, 22);
            lblNome.TabIndex = 1;
            lblNome.Text = "guna2HtmlLabel1";
            // 
            // lblPreco
            // 
            lblPreco.BackColor = Color.Transparent;
            lblPreco.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblPreco.Location = new Point(165, 181);
            lblPreco.Name = "lblPreco";
            lblPreco.Size = new Size(35, 22);
            lblPreco.TabIndex = 2;
            lblPreco.Text = "€€€€";
            // 
            // btnAdicionar
            // 
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
            // 
            // ProdutoCard
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.DodgerBlue;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(btnAdicionar);
            Controls.Add(lblPreco);
            Controls.Add(lblNome);
            Controls.Add(picProduto);
            ForeColor = SystemColors.ControlText;
            Name = "ProdutoCard";
            Size = new Size(263, 258);
            ((System.ComponentModel.ISupportInitialize)picProduto).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Guna.UI2.WinForms.Guna2PictureBox picProduto;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblNome;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblPreco;
        private Guna.UI2.WinForms.Guna2Button btnAdicionar;
    }
}

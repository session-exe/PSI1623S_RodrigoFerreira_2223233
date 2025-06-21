namespace OfiPecas
{
    partial class ItemCarrinho
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            btnRemover = new Guna.UI2.WinForms.Guna2Button();
            guna2PictureBox1 = new Guna.UI2.WinForms.Guna2PictureBox();
            lblNome = new Guna.UI2.WinForms.Guna2HtmlLabel();
            lblPreco = new Guna.UI2.WinForms.Guna2HtmlLabel();
            guna2Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)guna2PictureBox1).BeginInit();
            SuspendLayout();
            // 
            // guna2Panel1
            // 
            guna2Panel1.BorderColor = Color.DodgerBlue;
            guna2Panel1.BorderRadius = 12;
            guna2Panel1.BorderThickness = 2;
            guna2Panel1.Controls.Add(lblPreco);
            guna2Panel1.Controls.Add(lblNome);
            guna2Panel1.Controls.Add(guna2PictureBox1);
            guna2Panel1.Controls.Add(btnRemover);
            guna2Panel1.CustomizableEdges = customizableEdges5;
            guna2Panel1.Dock = DockStyle.Fill;
            guna2Panel1.FillColor = Color.FromArgb(40, 40, 40);
            guna2Panel1.Location = new Point(0, 0);
            guna2Panel1.Name = "guna2Panel1";
            guna2Panel1.ShadowDecoration.CustomizableEdges = customizableEdges6;
            guna2Panel1.Size = new Size(500, 120);
            guna2Panel1.TabIndex = 0;
            // 
            // btnRemover
            // 
            btnRemover.BackColor = Color.FromArgb(40, 40, 40);
            btnRemover.BorderRadius = 6;
            btnRemover.CustomizableEdges = customizableEdges3;
            btnRemover.DisabledState.BorderColor = Color.DarkGray;
            btnRemover.DisabledState.CustomBorderColor = Color.DarkGray;
            btnRemover.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnRemover.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnRemover.FillColor = Color.Red;
            btnRemover.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRemover.ForeColor = Color.White;
            btnRemover.Image = Properties.Resources.icon_shopping_cart;
            btnRemover.ImageAlign = HorizontalAlignment.Right;
            btnRemover.Location = new Point(372, 77);
            btnRemover.Name = "btnRemover";
            btnRemover.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnRemover.Size = new Size(110, 30);
            btnRemover.TabIndex = 4;
            btnRemover.Text = "Remover";
            btnRemover.TextAlign = HorizontalAlignment.Left;
            // 
            // guna2PictureBox1
            // 
            guna2PictureBox1.BorderRadius = 6;
            guna2PictureBox1.CustomizableEdges = customizableEdges1;
            guna2PictureBox1.ImageRotate = 0F;
            guna2PictureBox1.Location = new Point(14, 12);
            guna2PictureBox1.Name = "guna2PictureBox1";
            guna2PictureBox1.ShadowDecoration.CustomizableEdges = customizableEdges2;
            guna2PictureBox1.Size = new Size(142, 95);
            guna2PictureBox1.TabIndex = 5;
            guna2PictureBox1.TabStop = false;
            // 
            // lblNome
            // 
            lblNome.BackColor = Color.FromArgb(40, 40, 40);
            lblNome.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNome.ForeColor = Color.White;
            lblNome.Location = new Point(186, 12);
            lblNome.Name = "lblNome";
            lblNome.Size = new Size(61, 22);
            lblNome.TabIndex = 6;
            lblNome.Text = "Nomeee";
            // 
            // lblPreco
            // 
            lblPreco.BackColor = Color.FromArgb(40, 40, 40);
            lblPreco.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPreco.ForeColor = Color.White;
            lblPreco.Location = new Point(186, 50);
            lblPreco.Name = "lblPreco";
            lblPreco.Size = new Size(35, 22);
            lblPreco.TabIndex = 7;
            lblPreco.Text = "€€€€";
            // 
            // ItemCarrinho
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Transparent;
            Controls.Add(guna2Panel1);
            Name = "ItemCarrinho";
            Size = new Size(500, 120);
            guna2Panel1.ResumeLayout(false);
            guna2Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)guna2PictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Guna.UI2.WinForms.Guna2Button btnRemover;
        private Guna.UI2.WinForms.Guna2PictureBox guna2PictureBox1;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblNome;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblPreco;
    }
}

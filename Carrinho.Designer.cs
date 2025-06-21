namespace OfiPecas
{
    partial class Carrinho
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            flowPanel_ProdutosCarrinho = new FlowLayoutPanel();
            Panel_Total = new Guna.UI2.WinForms.Guna2Panel();
            lbl_Total = new Guna.UI2.WinForms.Guna2HtmlLabel();
            lbl_Total_Produtos = new Guna.UI2.WinForms.Guna2HtmlLabel();
            Button_Comprar = new Guna.UI2.WinForms.Guna2Button();
            Panel_Total.SuspendLayout();
            SuspendLayout();
            // 
            // flowPanel_ProdutosCarrinho
            // 
            flowPanel_ProdutosCarrinho.Location = new Point(141, 49);
            flowPanel_ProdutosCarrinho.Name = "flowPanel_ProdutosCarrinho";
            flowPanel_ProdutosCarrinho.Size = new Size(520, 500);
            flowPanel_ProdutosCarrinho.TabIndex = 0;
            // 
            // Panel_Total
            // 
            Panel_Total.BackColor = Color.Transparent;
            Panel_Total.BorderRadius = 20;
            Panel_Total.Controls.Add(lbl_Total);
            Panel_Total.Controls.Add(lbl_Total_Produtos);
            Panel_Total.Controls.Add(Button_Comprar);
            Panel_Total.CustomizableEdges = customizableEdges11;
            Panel_Total.FillColor = Color.FromArgb(45, 45, 45);
            Panel_Total.Location = new Point(707, 49);
            Panel_Total.Name = "Panel_Total";
            Panel_Total.ShadowDecoration.CustomizableEdges = customizableEdges12;
            Panel_Total.Size = new Size(339, 276);
            Panel_Total.TabIndex = 1;
            // 
            // lbl_Total
            // 
            lbl_Total.BackColor = Color.Transparent;
            lbl_Total.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            lbl_Total.ForeColor = Color.White;
            lbl_Total.Location = new Point(24, 100);
            lbl_Total.Name = "lbl_Total";
            lbl_Total.Size = new Size(41, 22);
            lbl_Total.TabIndex = 2;
            lbl_Total.Text = "Total: ";
            // 
            // lbl_Total_Produtos
            // 
            lbl_Total_Produtos.BackColor = Color.Transparent;
            lbl_Total_Produtos.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            lbl_Total_Produtos.ForeColor = Color.White;
            lbl_Total_Produtos.Location = new Point(24, 54);
            lbl_Total_Produtos.Name = "lbl_Total_Produtos";
            lbl_Total_Produtos.Size = new Size(128, 22);
            lbl_Total_Produtos.TabIndex = 1;
            lbl_Total_Produtos.Text = "Total de Produtos: ";
            // 
            // Button_Comprar
            // 
            Button_Comprar.BackColor = Color.Transparent;
            Button_Comprar.BorderRadius = 10;
            Button_Comprar.CustomizableEdges = customizableEdges9;
            Button_Comprar.DisabledState.BorderColor = Color.DarkGray;
            Button_Comprar.DisabledState.CustomBorderColor = Color.DarkGray;
            Button_Comprar.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            Button_Comprar.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            Button_Comprar.FillColor = Color.FromArgb(150, 30, 144, 255);
            Button_Comprar.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Button_Comprar.ForeColor = Color.White;
            Button_Comprar.Location = new Point(77, 204);
            Button_Comprar.Name = "Button_Comprar";
            Button_Comprar.ShadowDecoration.CustomizableEdges = customizableEdges10;
            Button_Comprar.Size = new Size(180, 45);
            Button_Comprar.TabIndex = 0;
            Button_Comprar.Text = "Comprar";
            // 
            // Carrinho
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(30, 30, 30);
            ClientSize = new Size(1084, 561);
            Controls.Add(Panel_Total);
            Controls.Add(flowPanel_ProdutosCarrinho);
            Name = "Carrinho";
            Text = "OfiPeças";
            Panel_Total.ResumeLayout(false);
            Panel_Total.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private FlowLayoutPanel flowPanel_ProdutosCarrinho;
        private Guna.UI2.WinForms.Guna2Panel Panel_Total;
        private Guna.UI2.WinForms.Guna2Button Button_Comprar;
        private Guna.UI2.WinForms.Guna2HtmlLabel lbl_Total_Produtos;
        private Guna.UI2.WinForms.Guna2HtmlLabel lbl_Total;
    }
}
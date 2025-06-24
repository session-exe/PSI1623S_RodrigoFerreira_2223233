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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Carrinho));
            flowPanel_Produtos = new FlowLayoutPanel();
            Panel_Comprar = new Guna.UI2.WinForms.Guna2Panel();
            lbl_Total = new Guna.UI2.WinForms.Guna2HtmlLabel();
            lbl_Total_Produtos = new Guna.UI2.WinForms.Guna2HtmlLabel();
            Button_Comprar = new Guna.UI2.WinForms.Guna2Button();
            ImageButton_Back = new Guna.UI2.WinForms.Guna2ImageButton();
            Panel_Comprar.SuspendLayout();
            SuspendLayout();
            // 
            // flowPanel_Produtos
            // 
            flowPanel_Produtos.Location = new Point(161, 65);
            flowPanel_Produtos.Margin = new Padding(3, 4, 3, 4);
            flowPanel_Produtos.Name = "flowPanel_Produtos";
            flowPanel_Produtos.Size = new Size(594, 667);
            flowPanel_Produtos.TabIndex = 0;
            // 
            // Panel_Comprar
            // 
            Panel_Comprar.BackColor = Color.Transparent;
            Panel_Comprar.BorderRadius = 20;
            Panel_Comprar.Controls.Add(lbl_Total);
            Panel_Comprar.Controls.Add(lbl_Total_Produtos);
            Panel_Comprar.Controls.Add(Button_Comprar);
            Panel_Comprar.CustomizableEdges = customizableEdges3;
            Panel_Comprar.FillColor = Color.FromArgb(45, 45, 45);
            Panel_Comprar.Location = new Point(808, 65);
            Panel_Comprar.Margin = new Padding(3, 4, 3, 4);
            Panel_Comprar.Name = "Panel_Comprar";
            Panel_Comprar.ShadowDecoration.CustomizableEdges = customizableEdges4;
            Panel_Comprar.Size = new Size(387, 368);
            Panel_Comprar.TabIndex = 1;
            // 
            // lbl_Total
            // 
            lbl_Total.BackColor = Color.Transparent;
            lbl_Total.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            lbl_Total.ForeColor = Color.White;
            lbl_Total.Location = new Point(27, 133);
            lbl_Total.Margin = new Padding(3, 4, 3, 4);
            lbl_Total.Name = "lbl_Total";
            lbl_Total.Size = new Size(49, 27);
            lbl_Total.TabIndex = 2;
            lbl_Total.Text = "Total: ";
            // 
            // lbl_Total_Produtos
            // 
            lbl_Total_Produtos.BackColor = Color.Transparent;
            lbl_Total_Produtos.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            lbl_Total_Produtos.ForeColor = Color.White;
            lbl_Total_Produtos.Location = new Point(27, 72);
            lbl_Total_Produtos.Margin = new Padding(3, 4, 3, 4);
            lbl_Total_Produtos.Name = "lbl_Total_Produtos";
            lbl_Total_Produtos.Size = new Size(157, 27);
            lbl_Total_Produtos.TabIndex = 1;
            lbl_Total_Produtos.Text = "Total de Produtos: ";
            // 
            // Button_Comprar
            // 
            Button_Comprar.BackColor = Color.Transparent;
            Button_Comprar.BorderRadius = 10;
            Button_Comprar.CustomizableEdges = customizableEdges1;
            Button_Comprar.DisabledState.BorderColor = Color.DarkGray;
            Button_Comprar.DisabledState.CustomBorderColor = Color.DarkGray;
            Button_Comprar.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            Button_Comprar.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            Button_Comprar.FillColor = Color.FromArgb(150, 30, 144, 255);
            Button_Comprar.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Button_Comprar.ForeColor = Color.White;
            Button_Comprar.Location = new Point(88, 272);
            Button_Comprar.Margin = new Padding(3, 4, 3, 4);
            Button_Comprar.Name = "Button_Comprar";
            Button_Comprar.ShadowDecoration.CustomizableEdges = customizableEdges2;
            Button_Comprar.Size = new Size(206, 60);
            Button_Comprar.TabIndex = 0;
            Button_Comprar.Text = "Comprar";
            Button_Comprar.Click += Button_Comprar_Click;
            // 
            // ImageButton_Back
            // 
            ImageButton_Back.CheckedState.ImageSize = new Size(64, 64);
            ImageButton_Back.HoverState.ImageSize = new Size(27, 27);
            ImageButton_Back.Image = Properties.Resources.angle_left;
            ImageButton_Back.ImageOffset = new Point(0, 0);
            ImageButton_Back.ImageRotate = 0F;
            ImageButton_Back.ImageSize = new Size(30, 30);
            ImageButton_Back.Location = new Point(14, 16);
            ImageButton_Back.Margin = new Padding(3, 4, 3, 4);
            ImageButton_Back.Name = "ImageButton_Back";
            ImageButton_Back.PressedState.ImageSize = new Size(27, 27);
            ImageButton_Back.ShadowDecoration.CustomizableEdges = customizableEdges5;
            ImageButton_Back.Size = new Size(40, 47);
            ImageButton_Back.TabIndex = 2;
            ImageButton_Back.Click += ImageButton_Back_Click;
            // 
            // Carrinho
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(30, 30, 30);
            ClientSize = new Size(1239, 748);
            Controls.Add(ImageButton_Back);
            Controls.Add(Panel_Comprar);
            Controls.Add(flowPanel_Produtos);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 4, 3, 4);
            MaximumSize = new Size(1257, 795);
            MinimumSize = new Size(1257, 795);
            Name = "Carrinho";
            Text = "OfiPeças";
            Panel_Comprar.ResumeLayout(false);
            Panel_Comprar.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private FlowLayoutPanel flowPanel_Produtos;
        private Guna.UI2.WinForms.Guna2Panel Panel_Comprar;
        private Guna.UI2.WinForms.Guna2Button Button_Comprar;
        private Guna.UI2.WinForms.Guna2HtmlLabel lbl_Total_Produtos;
        private Guna.UI2.WinForms.Guna2HtmlLabel lbl_Total;
        private Guna.UI2.WinForms.Guna2ImageButton ImageButton_Back;
    }
}
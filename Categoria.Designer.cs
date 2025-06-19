namespace OfiPecas
{
    partial class Categoria
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
            panelCategoria = new Guna.UI2.WinForms.Guna2Panel();
            guna2HtmlLabel1 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            panelCategoria.SuspendLayout();
            SuspendLayout();
            // 
            // panelCategoria
            // 
            panelCategoria.BackColor = Color.DodgerBlue;
            panelCategoria.BorderColor = Color.FromArgb(224, 224, 224);
            panelCategoria.BorderRadius = 6;
            panelCategoria.BorderThickness = 2;
            panelCategoria.Controls.Add(guna2HtmlLabel1);
            panelCategoria.CustomizableEdges = customizableEdges1;
            panelCategoria.Dock = DockStyle.Fill;
            panelCategoria.Location = new Point(0, 0);
            panelCategoria.Name = "panelCategoria";
            panelCategoria.ShadowDecoration.CustomizableEdges = customizableEdges2;
            panelCategoria.Size = new Size(130, 30);
            panelCategoria.TabIndex = 0;
            // 
            // guna2HtmlLabel1
            // 
            guna2HtmlLabel1.BackColor = Color.Transparent;
            guna2HtmlLabel1.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2HtmlLabel1.Location = new Point(7, 5);
            guna2HtmlLabel1.Name = "guna2HtmlLabel1";
            guna2HtmlLabel1.Size = new Size(112, 19);
            guna2HtmlLabel1.TabIndex = 0;
            guna2HtmlLabel1.Text = "guna2HtmlLabel1";
            // 
            // Categoria
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Transparent;
            Controls.Add(panelCategoria);
            Name = "Categoria";
            Size = new Size(130, 30);
            panelCategoria.ResumeLayout(false);
            panelCategoria.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel panelCategoria;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel1;
    }
}

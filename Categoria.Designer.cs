﻿namespace OfiPecas
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
            lblCategoria = new Guna.UI2.WinForms.Guna2HtmlLabel();
            panelCategoria.SuspendLayout();
            SuspendLayout();
            // 
            // panelCategoria
            // 
            panelCategoria.BackColor = Color.Transparent;
            panelCategoria.BorderColor = Color.DodgerBlue;
            panelCategoria.BorderRadius = 6;
            panelCategoria.BorderThickness = 2;
            panelCategoria.Controls.Add(lblCategoria);
            panelCategoria.CustomizableEdges = customizableEdges1;
            panelCategoria.Dock = DockStyle.Fill;
            panelCategoria.FillColor = Color.DodgerBlue;
            panelCategoria.Location = new Point(0, 0);
            panelCategoria.Margin = new Padding(3, 4, 3, 4);
            panelCategoria.Name = "panelCategoria";
            panelCategoria.ShadowDecoration.CustomizableEdges = customizableEdges2;
            panelCategoria.Size = new Size(214, 40);
            panelCategoria.TabIndex = 0;
            // 
            // lblCategoria
            // 
            lblCategoria.BackColor = Color.Transparent;
            lblCategoria.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCategoria.Location = new Point(8, 4);
            lblCategoria.Margin = new Padding(3, 4, 3, 4);
            lblCategoria.Name = "lblCategoria";
            lblCategoria.Size = new Size(89, 30);
            lblCategoria.TabIndex = 0;
            lblCategoria.Text = "Categoria";
            // 
            // Categoria
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Transparent;
            Controls.Add(panelCategoria);
            Margin = new Padding(3, 4, 3, 4);
            Name = "Categoria";
            Size = new Size(214, 40);
            panelCategoria.ResumeLayout(false);
            panelCategoria.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel panelCategoria;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblCategoria;
    }
}

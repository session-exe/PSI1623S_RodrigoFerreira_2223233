﻿namespace OfiPecas
{
    partial class HistoricoEncomendas
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HistoricoEncomendas));
            flowPanel_Encomendas = new FlowLayoutPanel();
            lbl_Total_Produtos = new Guna.UI2.WinForms.Guna2HtmlLabel();
            SuspendLayout();
            // 
            // flowPanel_Encomendas
            // 
            flowPanel_Encomendas.AutoScroll = true;
            flowPanel_Encomendas.Location = new Point(58, 58);
            flowPanel_Encomendas.Name = "flowPanel_Encomendas";
            flowPanel_Encomendas.Size = new Size(415, 415);
            flowPanel_Encomendas.TabIndex = 0;
            // 
            // lbl_Total_Produtos
            // 
            lbl_Total_Produtos.BackColor = Color.Transparent;
            lbl_Total_Produtos.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
            lbl_Total_Produtos.ForeColor = Color.White;
            lbl_Total_Produtos.Location = new Point(58, 12);
            lbl_Total_Produtos.Name = "lbl_Total_Produtos";
            lbl_Total_Produtos.Size = new Size(198, 27);
            lbl_Total_Produtos.TabIndex = 2;
            lbl_Total_Produtos.Text = "Historico Encomendas:";
            // 
            // HistoricoEncomendas
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(30, 30, 30);
            ClientSize = new Size(536, 517);
            Controls.Add(lbl_Total_Produtos);
            Controls.Add(flowPanel_Encomendas);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximumSize = new Size(552, 556);
            MinimumSize = new Size(552, 556);
            Name = "HistoricoEncomendas";
            Text = "OfiPeças";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private FlowLayoutPanel flowPanel_Encomendas;
        private Guna.UI2.WinForms.Guna2HtmlLabel lbl_Total_Produtos;
    }
}
namespace OfiPecas
{
    partial class EcomendaCard
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            btnBaixarFatura = new Guna.UI2.WinForms.Guna2Button();
            lblIdEncomenda = new Guna.UI2.WinForms.Guna2HtmlLabel();
            lblData = new Guna.UI2.WinForms.Guna2HtmlLabel();
            lblEstado = new Guna.UI2.WinForms.Guna2HtmlLabel();
            lblValor = new Guna.UI2.WinForms.Guna2HtmlLabel();
            guna2Panel1.SuspendLayout();
            SuspendLayout();
            // 
            // guna2Panel1
            // 
            guna2Panel1.BorderColor = Color.DodgerBlue;
            guna2Panel1.BorderRadius = 9;
            guna2Panel1.BorderThickness = 2;
            guna2Panel1.Controls.Add(lblValor);
            guna2Panel1.Controls.Add(lblEstado);
            guna2Panel1.Controls.Add(lblData);
            guna2Panel1.Controls.Add(lblIdEncomenda);
            guna2Panel1.Controls.Add(btnBaixarFatura);
            guna2Panel1.CustomizableEdges = customizableEdges3;
            guna2Panel1.Dock = DockStyle.Fill;
            guna2Panel1.FillColor = Color.FromArgb(40, 40, 40);
            guna2Panel1.Location = new Point(0, 0);
            guna2Panel1.Name = "guna2Panel1";
            guna2Panel1.ShadowDecoration.CustomizableEdges = customizableEdges4;
            guna2Panel1.Size = new Size(350, 90);
            guna2Panel1.TabIndex = 0;
            // 
            // btnBaixarFatura
            // 
            btnBaixarFatura.BackColor = Color.FromArgb(40, 40, 40);
            btnBaixarFatura.BorderRadius = 6;
            btnBaixarFatura.CustomizableEdges = customizableEdges1;
            btnBaixarFatura.DisabledState.BorderColor = Color.DarkGray;
            btnBaixarFatura.DisabledState.CustomBorderColor = Color.DarkGray;
            btnBaixarFatura.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnBaixarFatura.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnBaixarFatura.FillColor = Color.RoyalBlue;
            btnBaixarFatura.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnBaixarFatura.ForeColor = Color.White;
            btnBaixarFatura.Image = Properties.Resources.icon_download_pdf;
            btnBaixarFatura.ImageAlign = HorizontalAlignment.Left;
            btnBaixarFatura.Location = new Point(240, 48);
            btnBaixarFatura.Name = "btnBaixarFatura";
            btnBaixarFatura.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnBaixarFatura.Size = new Size(96, 30);
            btnBaixarFatura.TabIndex = 4;
            btnBaixarFatura.Text = "Export";
            btnBaixarFatura.TextAlign = HorizontalAlignment.Right;
            // 
            // lblIdEncomenda
            // 
            lblIdEncomenda.BackColor = Color.FromArgb(40, 40, 40);
            lblIdEncomenda.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblIdEncomenda.ForeColor = Color.White;
            lblIdEncomenda.Location = new Point(13, 11);
            lblIdEncomenda.Name = "lblIdEncomenda";
            lblIdEncomenda.Size = new Size(96, 22);
            lblIdEncomenda.TabIndex = 5;
            lblIdEncomenda.Text = "IdEncomenda";
            // 
            // lblData
            // 
            lblData.BackColor = Color.FromArgb(40, 40, 40);
            lblData.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblData.ForeColor = Color.White;
            lblData.Location = new Point(240, 11);
            lblData.Name = "lblData";
            lblData.Size = new Size(33, 22);
            lblData.TabIndex = 6;
            lblData.Text = "data";
            // 
            // lblEstado
            // 
            lblEstado.BackColor = Color.FromArgb(40, 40, 40);
            lblEstado.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblEstado.ForeColor = Color.White;
            lblEstado.Location = new Point(13, 56);
            lblEstado.Name = "lblEstado";
            lblEstado.Size = new Size(48, 22);
            lblEstado.TabIndex = 7;
            lblEstado.Text = "Estado";
            // 
            // lblValor
            // 
            lblValor.BackColor = Color.FromArgb(40, 40, 40);
            lblValor.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblValor.ForeColor = Color.White;
            lblValor.Location = new Point(124, 56);
            lblValor.Name = "lblValor";
            lblValor.Size = new Size(38, 22);
            lblValor.TabIndex = 8;
            lblValor.Text = "valor";
            // 
            // EcomendaCard
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Transparent;
            Controls.Add(guna2Panel1);
            Name = "EcomendaCard";
            Size = new Size(350, 90);
            guna2Panel1.ResumeLayout(false);
            guna2Panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Guna.UI2.WinForms.Guna2Button btnBaixarFatura;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblValor;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblEstado;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblData;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblIdEncomenda;
    }
}

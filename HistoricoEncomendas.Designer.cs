namespace OfiPecas
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
            flowPanel_Encomendas = new FlowLayoutPanel();
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
            // HistoricoEncomendas
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(30, 30, 30);
            ClientSize = new Size(534, 511);
            Controls.Add(flowPanel_Encomendas);
            Name = "HistoricoEncomendas";
            Text = "HistoricoEncomendas";
            ResumeLayout(false);
        }

        #endregion

        private FlowLayoutPanel flowPanel_Encomendas;
    }
}
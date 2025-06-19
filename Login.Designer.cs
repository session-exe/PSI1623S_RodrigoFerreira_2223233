namespace OfiPecas
{
    partial class Login
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            label_RecuperarPass = new Label();
            Button_Login = new Guna.UI2.WinForms.Guna2Button();
            label_CriarConta = new Label();
            label3 = new Label();
            TextBox_User_Email = new Guna.UI2.WinForms.Guna2TextBox();
            label4 = new Label();
            TextBox_Pass = new Guna.UI2.WinForms.Guna2TextBox();
            guna2HtmlLabel3 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            guna2PictureBox1 = new Guna.UI2.WinForms.Guna2PictureBox();
            ((System.ComponentModel.ISupportInitialize)guna2PictureBox1).BeginInit();
            SuspendLayout();
            // 
            // label_RecuperarPass
            // 
            label_RecuperarPass.AutoSize = true;
            label_RecuperarPass.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label_RecuperarPass.ForeColor = Color.White;
            label_RecuperarPass.Location = new Point(415, 293);
            label_RecuperarPass.Name = "label_RecuperarPass";
            label_RecuperarPass.Size = new Size(143, 13);
            label_RecuperarPass.TabIndex = 4;
            label_RecuperarPass.Text = "Não sabe a sua password?";
            label_RecuperarPass.Click += label_RecuperarPass_Click;
            // 
            // Button_Login
            // 
            Button_Login.BorderRadius = 6;
            Button_Login.CustomizableEdges = customizableEdges1;
            Button_Login.DisabledState.BorderColor = Color.DarkGray;
            Button_Login.DisabledState.CustomBorderColor = Color.DarkGray;
            Button_Login.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            Button_Login.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            Button_Login.FillColor = Color.DodgerBlue;
            Button_Login.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Button_Login.ForeColor = Color.White;
            Button_Login.Location = new Point(415, 340);
            Button_Login.Name = "Button_Login";
            Button_Login.ShadowDecoration.CustomizableEdges = customizableEdges2;
            Button_Login.Size = new Size(210, 30);
            Button_Login.TabIndex = 5;
            Button_Login.Text = "Login";
            Button_Login.Click += Button_Login_Click;
            // 
            // label_CriarConta
            // 
            label_CriarConta.AutoSize = true;
            label_CriarConta.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label_CriarConta.ForeColor = Color.DodgerBlue;
            label_CriarConta.Location = new Point(417, 373);
            label_CriarConta.Name = "label_CriarConta";
            label_CriarConta.Size = new Size(138, 13);
            label_CriarConta.TabIndex = 8;
            label_CriarConta.Text = "Não tem conta? Crie uma.";
            label_CriarConta.Click += label_CriarConta_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.DodgerBlue;
            label3.Location = new Point(424, 207);
            label3.Name = "label3";
            label3.Padding = new Padding(1, 0, 1, 0);
            label3.Size = new Size(102, 17);
            label3.TabIndex = 20;
            label3.Text = "Utilizador/Email";
            // 
            // TextBox_User_Email
            // 
            TextBox_User_Email.BackColor = Color.Transparent;
            TextBox_User_Email.BorderColor = Color.DodgerBlue;
            TextBox_User_Email.BorderRadius = 6;
            TextBox_User_Email.BorderThickness = 2;
            TextBox_User_Email.CustomizableEdges = customizableEdges3;
            TextBox_User_Email.DefaultText = "";
            TextBox_User_Email.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            TextBox_User_Email.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            TextBox_User_Email.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            TextBox_User_Email.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            TextBox_User_Email.FillColor = Color.FromArgb(30, 30, 30);
            TextBox_User_Email.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            TextBox_User_Email.Font = new Font("Segoe UI", 9F);
            TextBox_User_Email.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            TextBox_User_Email.Location = new Point(414, 216);
            TextBox_User_Email.Margin = new Padding(3, 4, 3, 4);
            TextBox_User_Email.Name = "TextBox_User_Email";
            TextBox_User_Email.PlaceholderText = "";
            TextBox_User_Email.SelectedText = "";
            TextBox_User_Email.ShadowDecoration.CustomizableEdges = customizableEdges4;
            TextBox_User_Email.Size = new Size(210, 30);
            TextBox_User_Email.TabIndex = 19;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.ForeColor = Color.DodgerBlue;
            label4.Location = new Point(424, 251);
            label4.Name = "label4";
            label4.Padding = new Padding(1, 0, 1, 0);
            label4.Size = new Size(66, 17);
            label4.TabIndex = 18;
            label4.Text = "Password";
            // 
            // TextBox_Pass
            // 
            TextBox_Pass.BackColor = Color.Transparent;
            TextBox_Pass.BorderColor = Color.DodgerBlue;
            TextBox_Pass.BorderRadius = 6;
            TextBox_Pass.BorderThickness = 2;
            TextBox_Pass.CustomizableEdges = customizableEdges5;
            TextBox_Pass.DefaultText = "";
            TextBox_Pass.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            TextBox_Pass.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            TextBox_Pass.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            TextBox_Pass.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            TextBox_Pass.FillColor = Color.FromArgb(30, 30, 30);
            TextBox_Pass.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            TextBox_Pass.Font = new Font("Segoe UI", 9F);
            TextBox_Pass.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            TextBox_Pass.Location = new Point(414, 260);
            TextBox_Pass.Margin = new Padding(3, 4, 3, 4);
            TextBox_Pass.Name = "TextBox_Pass";
            TextBox_Pass.PlaceholderText = "";
            TextBox_Pass.SelectedText = "";
            TextBox_Pass.ShadowDecoration.CustomizableEdges = customizableEdges6;
            TextBox_Pass.Size = new Size(210, 30);
            TextBox_Pass.TabIndex = 17;
            TextBox_Pass.UseSystemPasswordChar = true;
            // 
            // guna2HtmlLabel3
            // 
            guna2HtmlLabel3.BackColor = Color.Transparent;
            guna2HtmlLabel3.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            guna2HtmlLabel3.ForeColor = Color.DodgerBlue;
            guna2HtmlLabel3.Location = new Point(402, 116);
            guna2HtmlLabel3.Name = "guna2HtmlLabel3";
            guna2HtmlLabel3.Size = new Size(140, 34);
            guna2HtmlLabel3.TabIndex = 21;
            guna2HtmlLabel3.Text = "Bem Vindo a";
            // 
            // guna2PictureBox1
            // 
            guna2PictureBox1.CustomizableEdges = customizableEdges7;
            guna2PictureBox1.Image = Properties.Resources.logo;
            guna2PictureBox1.ImageRotate = 0F;
            guna2PictureBox1.Location = new Point(524, 76);
            guna2PictureBox1.Name = "guna2PictureBox1";
            guna2PictureBox1.ShadowDecoration.CustomizableEdges = customizableEdges8;
            guna2PictureBox1.Size = new Size(152, 114);
            guna2PictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            guna2PictureBox1.TabIndex = 22;
            guna2PictureBox1.TabStop = false;
            // 
            // Login
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(30, 30, 30);
            ClientSize = new Size(1084, 561);
            Controls.Add(guna2HtmlLabel3);
            Controls.Add(guna2PictureBox1);
            Controls.Add(label3);
            Controls.Add(TextBox_User_Email);
            Controls.Add(label4);
            Controls.Add(TextBox_Pass);
            Controls.Add(label_CriarConta);
            Controls.Add(Button_Login);
            Controls.Add(label_RecuperarPass);
            Name = "Login";
            Text = "OfiPeças";
            ((System.ComponentModel.ISupportInitialize)guna2PictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label_RecuperarPass;
        private Guna.UI2.WinForms.Guna2Button Button_Login;
        private Label label_CriarConta;
        private Label label3;
        private Guna.UI2.WinForms.Guna2TextBox TextBox_User_Email;
        private Label label4;
        private Guna.UI2.WinForms.Guna2TextBox TextBox_Pass;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel3;
        private Guna.UI2.WinForms.Guna2PictureBox guna2PictureBox1;
    }
}

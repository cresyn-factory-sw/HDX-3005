namespace threadTest01
{
    partial class passwordbox
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
            this.label1 = new System.Windows.Forms.Label();
            this.tb_password = new System.Windows.Forms.TextBox();
            this.lb_pwerror = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 28.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(106, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 51);
            this.label1.TabIndex = 0;
            this.label1.Text = "FAIL";
            // 
            // tb_password
            // 
            this.tb_password.Location = new System.Drawing.Point(24, 86);
            this.tb_password.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_password.Name = "tb_password";
            this.tb_password.Size = new System.Drawing.Size(278, 21);
            this.tb_password.TabIndex = 2;
            this.tb_password.UseSystemPasswordChar = true;
            this.tb_password.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tb_password_KeyPress);
            // 
            // lb_pwerror
            // 
            this.lb_pwerror.AutoSize = true;
            this.lb_pwerror.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_pwerror.ForeColor = System.Drawing.Color.Blue;
            this.lb_pwerror.Location = new System.Drawing.Point(20, 116);
            this.lb_pwerror.Name = "lb_pwerror";
            this.lb_pwerror.Size = new System.Drawing.Size(261, 63);
            this.lb_pwerror.TabIndex = 3;
            this.lb_pwerror.Text = "Error : \r\nPlease Check Config and Fail data\r\nTry Again";
            // 
            // passwordbox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(341, 190);
            this.ControlBox = false;
            this.Controls.Add(this.lb_pwerror);
            this.Controls.Add(this.tb_password);
            this.Controls.Add(this.label1);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "passwordbox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "passwordbox";
            this.TopMost = true;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.passwordbox_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_password;
        private System.Windows.Forms.Label lb_pwerror;
    }
}
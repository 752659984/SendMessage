namespace SendMessage
{
    partial class FrmClient
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
            this.rtxtReceive = new System.Windows.Forms.RichTextBox();
            this.rtxtSend = new System.Windows.Forms.RichTextBox();
            this.cmbUsers = new System.Windows.Forms.ComboBox();
            this.btnConnection = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // rtxtReceive
            // 
            this.rtxtReceive.HideSelection = false;
            this.rtxtReceive.Location = new System.Drawing.Point(24, 26);
            this.rtxtReceive.Name = "rtxtReceive";
            this.rtxtReceive.Size = new System.Drawing.Size(356, 408);
            this.rtxtReceive.TabIndex = 0;
            this.rtxtReceive.Text = "";
            // 
            // rtxtSend
            // 
            this.rtxtSend.Location = new System.Drawing.Point(24, 467);
            this.rtxtSend.Name = "rtxtSend";
            this.rtxtSend.Size = new System.Drawing.Size(356, 96);
            this.rtxtSend.TabIndex = 1;
            this.rtxtSend.Text = "";
            // 
            // cmbUsers
            // 
            this.cmbUsers.FormattingEnabled = true;
            this.cmbUsers.Location = new System.Drawing.Point(24, 583);
            this.cmbUsers.Name = "cmbUsers";
            this.cmbUsers.Size = new System.Drawing.Size(121, 20);
            this.cmbUsers.TabIndex = 2;
            // 
            // btnConnection
            // 
            this.btnConnection.Location = new System.Drawing.Point(186, 583);
            this.btnConnection.Name = "btnConnection";
            this.btnConnection.Size = new System.Drawing.Size(75, 23);
            this.btnConnection.TabIndex = 3;
            this.btnConnection.Text = "Connection";
            this.btnConnection.UseVisualStyleBackColor = true;
            this.btnConnection.Click += new System.EventHandler(this.btnConnection_Click);
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(305, 583);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 4;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(24, 616);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(237, 21);
            this.txtUserName.TabIndex = 5;
            // 
            // FrmClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(415, 649);
            this.Controls.Add(this.txtUserName);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.btnConnection);
            this.Controls.Add(this.cmbUsers);
            this.Controls.Add(this.rtxtSend);
            this.Controls.Add(this.rtxtReceive);
            this.Name = "FrmClient";
            this.Text = "FrmClient";
            this.Load += new System.EventHandler(this.FrmClient_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtxtReceive;
        private System.Windows.Forms.RichTextBox rtxtSend;
        private System.Windows.Forms.ComboBox cmbUsers;
        private System.Windows.Forms.Button btnConnection;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox txtUserName;
    }
}
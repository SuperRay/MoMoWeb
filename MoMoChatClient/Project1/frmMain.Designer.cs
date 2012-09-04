namespace MoMoChatClient
{
    partial class frmMain
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lstChatters = new System.Windows.Forms.ListView();
            this.lblcurrPerson = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lstChatters);
            this.groupBox1.Location = new System.Drawing.Point(34, 55);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(544, 276);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "click the face you wanna to chat to:";
            // 
            // lstChatters
            // 
            this.lstChatters.Location = new System.Drawing.Point(6, 20);
            this.lstChatters.Name = "lstChatters";
            this.lstChatters.Size = new System.Drawing.Size(532, 250);
            this.lstChatters.TabIndex = 0;
            this.lstChatters.UseCompatibleStateImageBehavior = false;
            this.lstChatters.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lstChatters_MouseClick);
            // 
            // lblcurrPerson
            // 
            this.lblcurrPerson.AutoSize = true;
            this.lblcurrPerson.Location = new System.Drawing.Point(60, 22);
            this.lblcurrPerson.Name = "lblcurrPerson";
            this.lblcurrPerson.Size = new System.Drawing.Size(0, 12);
            this.lblcurrPerson.TabIndex = 1;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(619, 369);
            this.Controls.Add(this.lblcurrPerson);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmMain";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView lstChatters;
        private System.Windows.Forms.Label lblcurrPerson;
    }
}
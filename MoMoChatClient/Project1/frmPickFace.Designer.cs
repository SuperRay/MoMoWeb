namespace MoMoChatClient
{
    partial class frmPickFace
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
            this.lstFaces = new System.Windows.Forms.ListView();
            this.btnOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lstFaces
            // 
            this.lstFaces.Location = new System.Drawing.Point(12, 12);
            this.lstFaces.Name = "lstFaces";
            this.lstFaces.Size = new System.Drawing.Size(579, 387);
            this.lstFaces.TabIndex = 0;
            this.lstFaces.UseCompatibleStateImageBehavior = false;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(492, 405);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // frmPickFace
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(603, 438);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lstFaces);
            this.Name = "frmPickFace";
            this.Text = "frmPickFace";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lstFaces;
        private System.Windows.Forms.Button btnOK;
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MoMoChatClient.ServiceReferenceTB;

namespace MoMoChatClient
{
    public partial class frmLogin : Form
    {
        frmPickFace pickface = new frmPickFace();
        Person currentPerson;
        string imgurl;

        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnChoose_Click(object sender, EventArgs e)
        {
            pickface.Show();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            imgurl = this.pickface.ImageUrl;
            if ((tbName.Text.ToString() != "") && (imgurl != null))
            {
                this.Visible = false;
                currentPerson = new Person();
                currentPerson.ImageURL = imgurl;
                currentPerson.Name = tbName.Text.ToString();
                frmMain mainwindow = new frmMain(currentPerson);
                mainwindow.Show();
            }
            else
            {
                MessageBox.Show("Please choose a face and then Enter your name before continue!", "Retry", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {

        }

        // 当关闭选择图像的窗体时，在Login窗体中加载所选择的图片
        private void ShowFace(object sender, EventArgs e)
        {
            if (pickface.Visible == false)
            {
                this.pbFace.Image = Image.FromFile(pickface.ImageUrl);
            }
        }
    }
}

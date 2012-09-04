using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MoMoChatClient
{
    public partial class frmPickFace : Form
    {
        private string imageurl;
        public frmPickFace()
        {
            InitializeComponent();
            LoadImage();
        }

        public string ImageUrl
        {
            get { return imageurl; }
            set { imageurl = value; }
        }

        // 加载指定路径下各种类型的图片
        private void LoadImage()
        {
            string path = "C:\\Users\\Administrator.RD-WUJIANLONG\\Documents\\Tencent Files\\All Users\\QQ\\Misc\\com.tencent.qqshow\\qqshow8";
            DirectoryInfo di = new DirectoryInfo(path);
            //获得的各种类型文件存放的路径存放于FileInfo类型的数组中
            FileInfo[] jpgfis = di.GetFiles("*.jpg");
            FileInfo[] pngfis = di.GetFiles("*.png");
            FileInfo[] giffis = di.GetFiles("*.gif");
            FileInfo[] bmpfis = di.GetFiles("*.bmp");
            string[] faces_path =
            new String[jpgfis.Length + pngfis.Length + giffis.Length + bmpfis.Length];
            int i = 0, j = 0;
            foreach (FileInfo jpgisitem in jpgfis)
            {
                faces_path[i++] = jpgfis[j++].FullName;
            }
            j = 0;
            foreach (FileInfo giffisitem in giffis)
            {
                faces_path[i++] = giffis[j++].FullName;
            }
            j = 0;
            foreach (FileInfo pngfisitem in pngfis)
            {
                faces_path[i++] = pngfis[j++].FullName;
            }
            j = 0;
            foreach (FileInfo bmpfisitem in bmpfis)
            {
                faces_path[i++] = bmpfis[j++].FullName;
            }
            //i值记录相应图片在ImageList中的索引值
            i = 0;
            ImageList largeimageList = new ImageList();
            ImageList smallimageList = new ImageList();
            largeimageList.ImageSize = new Size(40, 40);
            smallimageList.ImageSize = new Size(20, 20);
            foreach (string facepath in faces_path)
            {
                ListViewItem item = new ListViewItem("", i);
                //将图片路径存储到Tag属性里
                item.Tag = facepath as object;
                lstFaces.Items.AddRange(new ListViewItem[] { item });
                largeimageList.Images.Add(Bitmap.FromFile(facepath));
                smallimageList.Images.Add(Bitmap.FromFile(facepath));
                i++;
            }
            //设置lstFaces的LargeImageList与SmallImageList属性，
            //用于控件显示为大、小图标时使用
            lstFaces.SmallImageList = smallimageList;
            lstFaces.LargeImageList = largeimageList;
        }

        // 确定选择的头像后，隐藏窗体并将选中头像的Tag值
        //（存储了该头像图片的路径）赋值给imageurl
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (lstFaces.SelectedItems.Count != 0)
            {
                imageurl = lstFaces.SelectedItems[0].Tag.ToString();
                this.Visible = false;
            }
            else
            {
                MessageBox.Show("Please click one face at least before press OK!",
                "Retry", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

    }


}

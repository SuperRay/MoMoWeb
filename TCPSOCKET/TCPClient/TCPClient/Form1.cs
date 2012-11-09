using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace TCPClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            TextBox.CheckForIllegalCrossThreadCalls = false;
        }

        Socket sokClient = null;//负责与服务端通信的套接字
        Thread threadClient = null;//负责 监听 服务端发送来的消息的线程
        bool isRec = true;//是否循环接收服务端数据

        private void btnConnect_Click(object sender, EventArgs e)
        {
            //实例化 套接字
            sokClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //创建 ip对象
            IPAddress address = IPAddress.Parse(txtIP.Text.Trim());
            //创建网络节点对象 包含 ip和port
            IPEndPoint endpoint = new IPEndPoint(address, int.Parse(txtPort.Text.Trim()));
            //连接 服务端监听套接字
            sokClient.Connect(endpoint);

            //创建负责接收 服务端发送来数据的 线程
            threadClient = new Thread(ReceiveMsg);
            threadClient.IsBackground = true;
            //如果在win7下要通过 某个线程 来调用 文件选择框的代码，就需要设置如下
            threadClient.SetApartmentState(ApartmentState.STA);
            threadClient.Start();
        }

        /// <summary>
        /// 接收服务端发送来的消息数据
        /// </summary>
        void ReceiveMsg()
        {
            while (isRec)
            {
                byte[] msgArr = new byte[1024 * 1024 * 1];//接收到的消息的缓冲区
                int length = 0;
                //接收服务端发送来的消息数据
                length = sokClient.Receive(msgArr);//Receive会阻断线程
                if (msgArr[0] == 0)//发送来的是文字
                {
                    string strMsg = System.Text.Encoding.UTF8.GetString(msgArr, 1, length - 1);
                    txtShow.AppendText(strMsg + "\r\n");
                }
                else if (msgArr[0] == 1)
                { //发送来的是文件
                    SaveFileDialog sfd = new SaveFileDialog();
                    //弹出文件保存选择框
                    if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        //创建文件流
                        using (FileStream fs = new FileStream(sfd.FileName, FileMode.OpenOrCreate))
                        {
                            fs.Write(msgArr, 1, length - 1);
                            MessageBox.Show("文件保存成功！");
                        }
                    }
                }
                else if (msgArr[0] == 2)
                {
                    ShakeWindow();
                }
            }

        }

        private void ShakeWindow()
        {
            Random ran = new Random();
            //保存 窗体原坐标
            System.Drawing.Point point = this.Location;
            for (int i = 0; i < 30; i++)
            {
                //随机 坐标
                this.Location = new System.Drawing.Point(point.X + ran.Next(8), point.Y + ran.Next(8));
                System.Threading.Thread.Sleep(15);//休息15毫秒
                this.Location = point;//还原 原坐标(窗体回到原坐标)
                System.Threading.Thread.Sleep(15);//休息15毫秒
            }
        }

        //发送消息
        private void btnSend_Click(object sender, EventArgs e)
        {
            byte[] arrMsg = System.Text.Encoding.UTF8.GetBytes(txtInput.Text.Trim());
            sokClient.Send(arrMsg);
        }
    }
}

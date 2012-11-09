using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace TCPLocationTest
{
    public partial class Test : Form
    {
        Socket newsock = null;//负责监听 客户端段 连接请求的  套接字
        Thread threadWatch = null;//负责 调用套接字， 执行 监听请求的线程
        public delegate void ShowConnectMsg(string msg);
        public delegate void AddListItem(string strEndPoint);
        int second = 0;

        public Test()
        {
            InitializeComponent();
        }

        private void Test_Load(object sender, EventArgs e)
        {
            timer1.Interval = 1000;
            timer1.Stop();
        }

        public void AddListItemMethod(String myString)
        {
            if (this.cboClient.InvokeRequired)
            {
                AddListItem myDelegate = new AddListItem(AddListItemMethod);
                this.Invoke(myDelegate, new object[] { myString });
            }
            else
            {
                cboClient.Items.Add(myString);
            }
        }


        Dictionary<string, ConnectionClient> dictConn = new Dictionary<string, ConnectionClient>();

        bool isWatch = true;

        #region 1.被线程调用 监听连接端口
        /// <summary>
        /// 被线程调用 监听连接端口
        /// </summary>
        void StartWatch()
        {
            while (isWatch)
            {
                //threadWatch.SetApartmentState(ApartmentState.STA);
                //监听 客户端 连接请求，但是，Accept会阻断当前线程
                Socket sokMsg = newsock.Accept();//监听到请求，立即创建负责与该客户端套接字通信的套接字
                ConnectionClient connection = new ConnectionClient(sokMsg, ShowMsg, RemoveClientConnection);

                //将负责与当前连接请求客户端 通信的套接字所在的连接通信类 对象 装入集合
                dictConn.Add(sokMsg.RemoteEndPoint.ToString(), connection);

                this.AddListItemMethod(sokMsg.RemoteEndPoint.ToString()); //将 通信套接字的 客户端IP端口保存在下拉框里
                this.ShowMsg("接收连接成功");

                //启动一个新线程，负责监听该客户端发来的数据
                //Thread threadConnection = new Thread(ReciveMsg);
                //threadConnection.IsBackground = true;
                //threadConnection.Start(sokMsg);
            }
        }
        #endregion

        #region 发送消息 到指定的客户端 -btnSend_Click
        //发送消息 到所有的客户端
        private void SendMsgToClient()
        {
            //byte[] arrMsg = System.Text.Encoding.UTF8.GetBytes(txtInput.Text.Trim());
            //从下拉框中 获得 要哪个客户端发送数据

            //给所有客户端发送信息
            if (second == 60)
            {
                foreach (string key in dictConn.Keys)
                {
                    dictConn[key].Send(tbCheck.Text.Trim());
                }
                second = 0;
            }
            else
            {
                foreach (string key in dictConn.Keys)
                {
                    dictConn[key].Send(tbMsg.Text.Trim());
                }
                second++;
            }

        }
        #endregion

        #region 2 移除与指定客户端的连接
        /// <summary>
        /// 移除与指定客户端的连接
        /// </summary>
        /// <param name="key">指定客户端的IP和端口</param>
        public void RemoveClientConnection(string key)
        {
            if (dictConn.ContainsKey(key))
            {
                //dictConn.Remove(key);
                //cboClient.Items.Remove(key);
            }
        }
        #endregion

        #region 向文本框显示消息
        /// <summary>
        /// 向文本框显示消息
        /// </summary>
        /// <param name="msgStr">消息</param>
        public void ShowMsg(string msgStr)
        {
            if (this.txtShow.InvokeRequired)
            {
                ConnectionClient.DGShowMsg msgDelegate = new ConnectionClient.DGShowMsg(ShowMsg);
                this.Invoke(msgDelegate, new object[] { msgStr });
            }
            else
            {
                txtShow.AppendText(msgStr + "\r\n");
            }
        }
        #endregion

        #region timer
        private void timer1_Tick(object sender, EventArgs e)
        {
            SendMsgToClient();
        }
        #endregion

        private void btnClose_Click(object sender, EventArgs e)
        {
            //SendMsgToClient();
            tbMsg.ReadOnly = true;
            tbCheck.ReadOnly = true;
            timer1.Start();
            btnClose.Enabled = false;
            btnEdit.Enabled = true;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            tbMsg.ReadOnly = false;
            btnClose.Enabled = true;
            btnEdit.Enabled = false;
            tbCheck.ReadOnly = false;
        }

        /// <summary>
        /// 启动服务器按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSendMsg_Click(object sender, EventArgs e)
        {
            isWatch = true;
            newsock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //创建 ip对象
            IPAddress address = IPAddress.Parse("127.0.0.1");
            IPEndPoint ipep = new IPEndPoint(address, 5117);//本机预使用的IP和端口
            newsock.Bind(ipep);//绑定
            newsock.Listen(10);//监听

            threadWatch = new Thread(StartWatch);
            threadWatch.IsBackground = true;
            threadWatch.Start();

        }

        private void btnCloseConn_Click(object sender, EventArgs e)
        {
            isWatch = false;
            newsock.Dispose();
            newsock = null;
            threadWatch.Abort();
        }
    }
}




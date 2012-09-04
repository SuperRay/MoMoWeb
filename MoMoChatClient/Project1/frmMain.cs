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
    public partial class frmMain : Form
    {
        Person currPerson;
        ImageList largeimageList = new ImageList();
        ImageList smallimageList = new ImageList();
        ChatControl chatcontrol = new ChatControl();
        private Proxy_Singleton ProxySingleton = Proxy_Singleton.GetInstance();
        public delegate void ListViewDoubleClickHandler(object sender,
        MouseEventArgs e);

        public frmMain()
        {
            InitializeComponent();
            this.Closed += delegate { Window_Closed(); };
        }

        public frmMain(Person currentPerson)
        {
            InitializeComponent();
            largeimageList.ImageSize = new Size(40, 40);
            smallimageList.ImageSize = new Size(20, 20);
            lstChatters.Items.Clear();
            currPerson = currentPerson;
            //当前用户加入聊天
            ProxySingleton.Connect(currPerson);
            ProxySingleton.ProxyEvent +=
            new Proxy_Singleton.ProxyEventHandler(ProxySingleton_ProxyEvent);
            //ChatControl窗体订阅ProxyCallBackEvent事件
            ProxySingleton.ProxyCallBackEvent +=
            new Proxy_Singleton.ProxyCallBackEventHandler(
            this.chatcontrol.ProxySingleton_ProxyCallBackEvent);
            //MainWindow窗体订阅ProxyCallBackEvent事件
            ProxySingleton.ProxyCallBackEvent +=
            new Proxy_Singleton.ProxyCallBackEventHandler(
            this.ProxySingleton_ProxyCallBackEvent);
            this.lblcurrPerson.Text += currPerson.Name.ToString();
            this.Closed += delegate { Window_Closed(); };
        }

        private void Window_Closed()
        {
            //用户退出聊天
            ProxySingleton.ExitChatSession();
            Application.Exit();
        }

        private delegate void ProxySingleton_ProxyEvent_Delegate(object sender,
ProxyEventArgs e);
        //该方法作为ProxyEvent事件绑定的委托所引用的方法
        //通过ProxyEventArgs参数获得所有参与聊天的用户的信息并显示他们的名称
        private void ProxySingleton_ProxyEvent(object sender, ProxyEventArgs e)
        {
            //如果Windows 窗体中的控件被绑定到特定的线程，则不具备线程安全性。
            //因此，如果从另一个线程调用控件的方法，
            //那么必须使用控件的一个 Invoke 方法来将调用封送到适当的线程。
            //InvokeRequired属性可用于确定是否必须调用 Invoke 方法。 
            if (this.InvokeRequired)
            {
                this.Invoke(new
                ProxySingleton_ProxyEvent_Delegate(ProxySingleton_ProxyEvent),
                sender, e);
                return;
            }
            lstChatters.View = View.LargeIcon;
            ImageList imageListLarge = new ImageList();
            ImageList imageListSmall = new ImageList();
            imageListSmall.ImageSize = new Size(20, 20);
            imageListLarge.ImageSize = new Size(40, 40);
            int i = 0;
            foreach (Person person in e.list)
            {
                ListViewItem item1 = new ListViewItem(person.Name, i++);
                //利用Tag属性来保存聊天者对象
                item1.Tag = person as object;
                lstChatters.Items.AddRange(new ListViewItem[] { item1 });
                //向ImageList中添加相应图像
                imageListSmall.Images.Add(Bitmap.FromFile(person.ImageURL));
                imageListLarge.Images.Add(Bitmap.FromFile(person.ImageURL));
            }
            lstChatters.SmallImageList = imageListSmall;
            lstChatters.LargeImageList = imageListLarge;
        }
        // 该方法作为ProxyCallBackEvent事件绑定的委托所引用的方法，
        // 负责调用客户端的回调方法与服务通信
        private void ProxySingleton_ProxyCallBackEvent(object sender,
        ProxyCallBackEventArgs e)
        {
            switch (e.callbackType)
            {
                case CallBackType.Receive:
                    Receive(e.person, e.message);
                    break;
                case CallBackType.ReceiveWhisper:
                    ReceiveWhisper(e.person, e.message);
                    break;
                case CallBackType.UserEnter:
                    UserEnter(e.person);
                    break;
                case CallBackType.UserLeave:
                    UserLeave(e.person);
                    break;
            }
        }
        // 当接收到广播消息时，显示聊天窗体并显示消息内容
        private void Receive(Person sender, string message)
        {
            showChatWindow(sender);
        }
        // 当接收到指定消息时，显示聊天窗体并显示消息内容
        private void ReceiveWhisper(Person sender, string message)
        {
            showChatWindow(sender);
        }
        // 当有新用户加入时，将其添加至字典对象chatters中，
        // 并在ListView中添加相应的用户头像。
        private void UserEnter(Person person)
        {
            smallimageList = lstChatters.SmallImageList;
            largeimageList = lstChatters.LargeImageList;
            ListViewItem item1 =
            new ListViewItem(person.Name, smallimageList.Images.Count);
            item1.Tag = person as object;
            lstChatters.Items.AddRange(new ListViewItem[] { item1 });
            smallimageList.Images.Add(Bitmap.FromFile(person.ImageURL));
            largeimageList.Images.Add(Bitmap.FromFile(person.ImageURL));
            lstChatters.SmallImageList = smallimageList;
            lstChatters.LargeImageList = largeimageList;
        }
        // 当有聊天用户离开时，将其从字典对象chatters中移除。
        private void UserLeave(Person person)
        {
            ListViewItem item = lstChatters.FindItemWithText(person.Name, false, 0);
            lstChatters.Items.Remove(item);
        }
        // 显示聊天窗口
        private void showChatWindow(Person otherPerson)
        {
            this.chatcontrol.CurrentPerson = currPerson;
            this.chatcontrol.OtherPerson = otherPerson;
            this.chatcontrol.Show();
        }
        // 当选择相应的用户头像时，弹出与该用户聊天的窗体
        private void lstChatters_MouseClick(object sender, MouseEventArgs e)
        {
            Person otherPerson = new Person();
            //判断是否选中了相应的聊天者头像
            if (!Object.Equals(((ListView)sender).GetItemAt(e.X, e.Y), null))
            {
                otherPerson.Name = lstChatters.SelectedItems[0].Text;
                otherPerson.ImageURL =
                (lstChatters.SelectedItems[0].Tag as Person).ImageURL;
                showChatWindow(otherPerson);
            }
        }
    }
}

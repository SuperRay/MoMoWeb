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
    public partial class ChatControl : Form
    {
        Person currentPerson;
        Person otherPerson;
        private Proxy_Singleton ProxySingleton = Proxy_Singleton.GetInstance();
        public ChatControl()
        {
            InitializeComponent();
        }
        // 将聊天者输入的信息添加至RichTextBox中作为聊天记录
        public void AppendText(string text)
        {
            this.rtxtRecord.AppendText(text);
            this.Show();
        }
        public void AppendChatRecords(string msg)
        {
            this.rtxtRecord.AppendText(msg);
            this.Show();
        }
        // 返回或设置当前聊天者对象的属性
        public Person CurrentPerson
        {
            get { return currentPerson; }
            set
            {
                currentPerson = value;
            }
        }
        // 返回或设置当前聊天中另一方对象的属性并设置Label的Text属性
        public Person OtherPerson
        {
            get { return otherPerson; }
            set
            {
                otherPerson = value;
                if (otherPerson != null)
                {
                    this.lblChatTo2.Text = otherPerson.Name;
                    this.Text = "Chatting with " + otherPerson.Name + " ...";
                }
            }
        }
        // 当ProxyCallBackEvent事件触发，该方法将被调用
        // 它将通过检查ProxyCallBackEventArgs的参数值来判断收到的消息类型，
        // 然后调用相应的方法
        public void ProxySingleton_ProxyCallBackEvent(object sender,
        ProxyCallBackEventArgs e)
        {
            switch (e.callbackType)
            {
                case CallBackType.Receive:
                    Receive(e.person.Name, e.message);
                    break;
                case CallBackType.ReceiveWhisper:
                    ReceiveWhisper(e.person.Name, e.message);
                    break;
                case CallBackType.UserEnter:
                    break;
                case CallBackType.UserLeave:
                    break;
            }
        }
        // 该方法将把收到的广播来的消息添加到RichTextBox中作为聊天记录
        public void Receive(string senderName, string message)
        {
            AppendText(senderName + ": " + message + Environment.NewLine);
            this.Show();
        }
        // 该方法将把收到的对方发来的消息添加到RichTextBox中作为聊天记录
        public void ReceiveWhisper(string senderName, string message)
        {
            AppendText(senderName + " Say: " + message + Environment.NewLine);
        }
        // 调用ProxySingleton类型的对象的SayAndClear()方法，
        //并将发送消息框清空
        private void SayAndClear(string to, string msg, bool pvt)
        {
            if (msg != "")
            {
                try
                {
                    ProxySingleton.SayAndClear(to, msg, pvt);
                    txtMessage.Text = "";
                }
                catch
                {
                    AbortProxyAndUpdateUI();
                    AppendText("Disconnected at "
                    + DateTime.Now.ToString() + Environment.NewLine);
                    Error("Error: Connection to chat server lost!");
                }
            }
        }
        // 调用ProxySingleton类型的对象的AbortProxy()方法并提示错误消息
        private void AbortProxyAndUpdateUI()
        {
            ProxySingleton.AbortProxy();
            MessageBox.Show("An error occurred, Disconnecting");
        }
        // 调用ProxySingleton类型的对象的ExitChatSession()方法并提示错误消息
        private void Error(string errMessage)
        {
            ProxySingleton.ExitChatSession();
            MessageBox.Show(errMessage, "Connection error",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        // 调用ProxySingleton类型的对象的SayAndClear()方法来发送消息，
        // 同时在RichTextBox中添加自己发送的消息内容作为聊天历史记录
        private void btnSend_Click(object sender, EventArgs e)
        {
            if (txtMessage.Text == "")
                return;
            Person to = otherPerson;
            if (to != null)
            {
                string receiverName = to.Name;
                AppendText("Say to " + receiverName + ": "
                + txtMessage.Text + Environment.NewLine);
                SayAndClear(receiverName, txtMessage.Text, true);
                txtMessage.Focus();
            }
        }
        // 重写OnClosed方法，将对象引用置为空
        protected override void OnClosed(EventArgs e)
        {
            this.CurrentPerson = null;
            this.OtherPerson = null;
            this.Visible = false;
        }
        // 隐藏聊天窗体，并将对象引用置为空
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.CurrentPerson = null;
            this.OtherPerson = null;
            this.Visible = false;
        }
        // 隐藏聊天窗体，并将对象引用置为空
        private void lblExit_Click(object sender, EventArgs e)
        {
            this.CurrentPerson = null;
            this.OtherPerson = null;
            this.Visible = false;
        }
    }
}

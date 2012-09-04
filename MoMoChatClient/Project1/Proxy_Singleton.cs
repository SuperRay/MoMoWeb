using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Windows.Forms;
using MoMoChatClient.ServiceReferenceTB;

namespace MoMoChatClient
{
    public sealed class Proxy_Singleton : IChatCallback
    {
        private static Proxy_Singleton singleton = null;
        private static readonly object singletonLock = new object();
        private ChatClient proxy;
        private Person myPerson;

        private delegate void HandleDelegate(Person[] list);
        private delegate void HandleErrorDelegate();
        // ProxyEventHandler与ProxyCallBackEventHandler事件用于窗体订阅。
        public delegate void ProxyEventHandler(object sender, ProxyEventArgs e);
        public delegate void ProxyCallBackEventHandler(object sender,
        ProxyCallBackEventArgs e);
        public event ProxyEventHandler ProxyEvent;
        public event ProxyCallBackEventHandler ProxyCallBackEvent;

        private Proxy_Singleton()
        {
        }

        // 接收发来的广播消息
        public void Receive(Person sender, string message)
        {
            Receive(sender, message, CallBackType.Receive);
        }

        // 接收发来的消息
        public void ReceiveWhisper(Person sender, string message)
        {
            Receive(sender, message, CallBackType.ReceiveWhisper);
        }

        // 根据callbacktype参数的值来封装消息，并触发ProxyCallBackEvent事件
        private void Receive(Person sender, string message, CallBackType callbackType)
        {
            ProxyCallBackEventArgs e = new ProxyCallBackEventArgs();
            e.message = message;
            e.callbackType = callbackType;
            e.person = sender;
            OnProxyCallBackEvent(e);
        }

        // 新的聊天者加入
        public void UserEnter(Person person)
        {
            UserEnterLeave(person, CallBackType.UserEnter);
        }

        // 聊天者离开
        public void UserLeave(Person person)
        {
            UserEnterLeave(person, CallBackType.UserLeave);
        }

        // 被UserEnter()以及UserLeave()调用，
        // 根据callbackType参数的值来封装消息，并触发ProxyCallBackEvent事件
        private void UserEnterLeave(Person person, CallBackType callbackType)
        {
            ProxyCallBackEventArgs e = new ProxyCallBackEventArgs();
            e.person = person;
            e.callbackType = callbackType;
            OnProxyCallBackEvent(e);
        }

        // 异步的Join操作，首先调用BeginJoin，该操作完成后将调用OnEndJoin。
        public void Connect(Person p)
        {
            InstanceContext site = new InstanceContext(this);
            proxy = new ChatClient(site);
            IAsyncResult iar =
            proxy.BeginJoin(p, new AsyncCallback(OnEndJoin), null);
        }

        // 作为异步调用中BeginInvoke的回调方法，
        // 获取当前在线用户的列表
        private void OnEndJoin(IAsyncResult iar)
        {
            try
            {
                Person[] list = proxy.EndJoin(iar);
                HandleEndJoin(list);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            }
        }

        // 如果在线者列表不为空，则触发ProxyEvent事件
        private void HandleEndJoin(Person[] list)
        {
            if (list == null)
            {
                MessageBox.Show("Error: List is empty", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
                ExitChatSession();
            }
            else
            {
                ProxyEventArgs e = new ProxyEventArgs();
                e.list = list;
                OnProxyEvent(e);
            }
        }

        // 触发ProxyCallBackEvent事件
        protected void OnProxyCallBackEvent(ProxyCallBackEventArgs e)
        {
            if (ProxyCallBackEvent != null)
            {
                ProxyCallBackEvent(this, e);
            }
        }

        // 触发ProxyEvent事件
        protected void OnProxyEvent(ProxyEventArgs e)
        {
            if (ProxyEvent != null)
            {
                ProxyEvent(this, e);
            }
        }

        // 单例模式，返回Proxy_Singleton类型的唯一对象
        public static Proxy_Singleton GetInstance()
        {
            lock (singletonLock)
            {
                if (singleton == null)
                {
                    singleton = new Proxy_Singleton();
                }
                return singleton;
            }
        }

        //通过布尔值 pvt来决定是调用客户端代理的广播方法还是发送消息给
        //指定目标的方法
        public void SayAndClear(string to, string msg, bool pvt)
        {
            if (!pvt)
                proxy.Say(msg);
            else
                proxy.Whisper(to, msg);
        }

        // 先调用客户端代理对象的Leave方法
        // 最后调用AbortProxy()来释放客户端代理对象
        public void ExitChatSession()
        {
            try
            {
                proxy.Leave();
            }
            catch { }
            finally
            {
                AbortProxy();
            }
        }

        // 调用客户端代理的Abort与Close方法
        public void AbortProxy()
        {
            if (proxy != null)
            {
                proxy.Abort();
                proxy.Close();
                proxy = null;
            }
        }
    }
}

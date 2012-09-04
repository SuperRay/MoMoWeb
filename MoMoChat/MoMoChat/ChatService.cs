using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace ChatLib
{
    #region 说明
    /*ChatService继承了IChat，不再需要像服务契约IChat一样添加ServiceContract属性。另外设置了其ServiceBehavior属性的InstanceContextMode参数来决定实例化方式（PerSession方式或者PerCall方式）。PerSession表明服务对象的生命周期存活于一个会话期间，在同一个会话期间对于服务的不同操作的调用都会施加到同一个客户端代理类型的对象上；PerCall则表示服务对象是在方法被调用时创建，结束后即被销毁。ServiceBehavior.ConcurrencyMode 参数的设置则用于控制具体服务对象的并发行为，其包括三种行为：
Single: 为默认方式。服务实例是单线程的，不接受重入调用(reentrant calls)。也就是说对于同一个服务实例的多个调用必须排队，直到上一次调用完成后才能继续。
    Reentrant: 和 Single 一样，也是单线程的，但能接受重入调用，至于针对同一服务对象的多个调用仍然需要排队。因为在 Single 模式下，当方法调用另外一个服务时，方法会阻塞，直到所调用的服务完成。如果方法不能重入，那么调用方会因无法接受所调用服务的返回消息，无法解除阻塞状态而陷入死锁。Reentrant 模式解决了这种不足。
    Multiple: 允许多个客户端同时调用服务方法。不再有锁的问题，不再提供同步保障。因此使用该模式时，必须自行提供多线程同步机制（比如在本例中给static类型的syncObj对象加锁）来保证数据成员的读写安全
    */
    #endregion
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession,
                        ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class ChatService : IChat
    {
        //用于保障多线程同步而设置的对象
        private static Object syncObj = new Object();
        //客户端的回调接口对象
        IChatCallback callback = null;
        //用于广播事件的委托
        public delegate void ChatEventHandler(object sender, ChatEventArgs e);
        public static event ChatEventHandler ChatEvent;
        private ChatEventHandler myEventHandler = null;
        //利用字典对象chatters来保存聊天者对象以及绑定的对应事件委托
        static Dictionary<Person, ChatEventHandler> chatters =
        new Dictionary<Person, ChatEventHandler>();
        //当前聊天者对象
        private Person person;

        // 判断具有相应名字的聊天者是否存在于字典对象中
        private bool checkIfPersonExists(string name)
        {
            foreach (Person p in chatters.Keys)
            {
                if (p.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        // 在字典对象中搜索判断其中是否包含了相应名字的聊天者，
        // 如果有则返回其对应的委托ChatEventHandler；否则返回空
        private ChatEventHandler getPersonHandler(string name)
        {
            foreach (Person p in chatters.Keys)
            {
                //不区分大小写
                if (p.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    ChatEventHandler chatTo = null;
                    chatters.TryGetValue(p, out chatTo);
                    return chatTo;
                }
            }
            return null;
        }

        // 在字典对象中搜索判断其中是否包含了相应名字的聊天者，
        // 如果有则返回聊天者对象；否则返回空
        private Person getPerson(string name)
        {
            foreach (Person p in chatters.Keys)
            {
                if (p.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return p;
                }
            }
            return null;
        }

        // 加入聊天，如果字典对象chatters中没有同名的聊天者
        public Person[] Join(Person person)
        {
            bool userAdded = false;
            //创建新的ChatEventHandler类型委托,其指向MyEventHandler()方法
            myEventHandler = new ChatEventHandler(MyEventHandler);
            //执行关键区域，判断是否存在同名聊天者，
            //如果不存在则向字典中添加该对象并将MyEventHandler委托作为其value,
            //以待后面来触发
            lock (syncObj)
            {
                if (!checkIfPersonExists(person.Name) && person != null)
                {
                    this.person = person;
                    chatters.Add(person, MyEventHandler);
                    userAdded = true;
                }
            }
            //如果新的聊天者添加成功，获得一个回调的实例来创建一个消息，
            //并将其广播给其他的聊天者
            //读取字典chatters的所有聊天者对象并返回一个包含了所有对象的列表
            if (userAdded)
            {
                callback = OperationContext.Current.GetCallbackChannel<IChatCallback>();
                ChatEventArgs e = new ChatEventArgs();
                e.msgType = MessageType.UserEnter;
                e.person = this.person;
                //广播有新聊天者加入的消息
                BroadcastMessage(e);
                //将新加入聊天者对象的委托加到全局的多播委托上
                ChatEvent += myEventHandler;
                Person[] list = new Person[chatters.Count];
                //执行关键区域，将字典chatters的所有聊天者对象
                //拷贝至一个聊天者列表上，用于方法返回
                lock (syncObj)
                {
                    chatters.Keys.CopyTo(list, 0);
                }
                return list;
            }
            else
            {
                return null;
            }
        }

        // 广播当前聊天者输入的消息给所有聊天者
        public void Say(string msg)
        {
            ChatEventArgs e = new ChatEventArgs();
            e.msgType = MessageType.Receive;
            e.person = this.person;
            e.message = msg;
            BroadcastMessage(e);
        }

        //在字典对象chatters中查找具有相应名称的聊天者对象，
        //并异步地触发其对应的ChatEventHandle委托
        public void Whisper(string to, string msg)
        {
            ChatEventArgs e = new ChatEventArgs();
            e.msgType = MessageType.ReceiveWhisper;
            e.person = this.person;
            e.message = msg;
            try
            {
                ChatEventHandler chatterTo;
                //执行关键区域，获取具有相应名称的聊天者对象在chatters中所对应的委托
                lock (syncObj)
                {
                    chatterTo = getPersonHandler(to);
                    if (chatterTo == null)
                    {
                        throw new KeyNotFoundException("The person whos name is " + to + " could not be found");
                    }
                }
                //异步执行委托
                chatterTo.BeginInvoke(this, e, new AsyncCallback(EndAsync), null);
            }
            catch (KeyNotFoundException)
            {
            }
        }
        /// 当聊天者离开时，从字典chatters中移除包含该对象的项。
        /// 并从全局的多播委托上移除该对象对应的委托
        public void Leave()
        {
            if (this.person == null)
                return;
            //获得该聊天者对象所对应的ChatEventHandler委托
            ChatEventHandler chatterToRemove = getPersonHandler(this.person.Name);
            //执行关键区域，从从字典chatters中移除包含该聊天者对象的项。
            lock (syncObj)
            {
                chatters.Remove(this.person);
            }
            //从全局的多播委托上移除该对象对应的委托
            ChatEvent -= chatterToRemove;
            ChatEventArgs e = new ChatEventArgs();
            e.msgType = MessageType.UserLeave;
            e.person = this.person;
            this.person = null;
            //将消息广播给其他聊天者
            BroadcastMessage(e);
        }
        // 当chatters中的聊天者对象所对应的ChatEventHandler委托被触发时，
        // MyEventHandler方法将执行。
        // 该方法通过检查传递过来的ChatEventArgs参数类型来执行相应的客户端
        //的回调接口中的方法。
        private void MyEventHandler(object sender, ChatEventArgs e)
        {
            try
            {
                switch (e.msgType)
                {
                    case MessageType.Receive:
                        callback.Receive(e.person, e.message);
                        break;
                    case MessageType.ReceiveWhisper:
                        callback.ReceiveWhisper(e.person, e.message);
                        break;
                    case MessageType.UserEnter:
                        callback.UserEnter(e.person);
                        break;
                    case MessageType.UserLeave:
                        callback.UserLeave(e.person);
                        break;
                }
            }
            catch
            {
                Leave();
            }
        }
        // 异步地触发字典chatters中所有聊天者对象所对应的ChatEventHandler委托。
        // BeginInvoke 方法可启动异步调用。第一个参数是一个 AsyncCallback 委托，
        //该委托引用在异步调用完成时要调用的方法。
        // 第二个参数是一个用户定义的对象，该对象可向回调方法传递信息。
        //BeginInvoke 立即返回，不等待异步调用完成。
        //BeginInvoke 会返回 IAsyncResult，这个结果可用于监视异步调用进度。
        private void  BroadcastMessage(ChatEventArgs e)
        {
            ChatEventHandler temp = ChatEvent;
            if (temp != null)
            {
                foreach (ChatEventHandler handler in temp.GetInvocationList())
                {
                    handler.BeginInvoke(this, e, new AsyncCallback(EndAsync), null);
                }
            }
        }
        // EndInvoke 方法检索异步调用的结果。
        //调用 BeginInvoke 后可随时调用 EndInvoke 方法；
        //如果异步调用尚未完成，EndInvoke 将一直阻止调用线程，
        //直到异步调用完成后才允许调用线程执行。
        private void EndAsync(IAsyncResult ar)
        {
            ChatEventHandler d = null;
            try
            {
                //System.Runtime.Remoting.Messaging.AsyncResult封装了异步委托上
                //的异步操作的结果。
                //AsyncResult.AsyncDelegate 属性可用于获取在其上调用
                //异步调用的委托对象。 
                System.Runtime.Remoting.Messaging.AsyncResult asres =
                (System.Runtime.Remoting.Messaging.AsyncResult)ar;
                d = ((ChatEventHandler)asres.AsyncDelegate);
                d.EndInvoke(ar);
            }
            catch
            {
                ChatEvent -= d;
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoMoChatClient.ServiceReferenceYB;

namespace MoMoChatClient
{
    class CommonVar
    {
    }

    //定义了消息类型的枚举
    public enum CallBackType { Receive, ReceiveWhisper, UserEnter, UserLeave };
    //代理事件参数
    public class ProxyEventArgs : EventArgs
    {
        //当前的在线用户列表
        public Person[] list;
    }
    // 客户端回调方法所绑定的事件的参数定义
    public class ProxyCallBackEventArgs : EventArgs
    {
        //回调的类型
        public CallBackType callbackType;
        //收到的消息内容
        public string message = "";
        //发送消息的人
        public Person person = null;
    }
}

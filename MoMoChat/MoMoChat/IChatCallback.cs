using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace ChatLib
{
    public interface IChatCallback
    {
        [OperationContract(IsOneWay = true)]
        void Receive(Person sender, string message);

        [OperationContract(IsOneWay = true)]
        void ReceiveWhisper(Person sender, string message);

        [OperationContract(IsOneWay = true)]
        void UserEnter(Person person);

        [OperationContract(IsOneWay = true)]
        void UserLeave(Person person);

    }

    // 定义说明消息类型的枚举类型MessageType
    public enum MessageType { Receive, UserEnter, UserLeave, ReceiveWhisper };

    // ChatEventArgs继承至EventArgs，作为传递的消息参数，
    //其包括消息的类型msgType、消息发送者person以及消息的主体内容message
    public class ChatEventArgs : EventArgs
    {
        public MessageType msgType;
        public Person person;
        public string message;
    }

}

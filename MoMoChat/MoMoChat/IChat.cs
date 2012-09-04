using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace ChatLib
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IChatCallback))]
    public interface IChat
    {
        /// <summary>
        /// 向所有用户广播消息
        /// </summary>
        /// <param name="msg">消息内容</param>
        [OperationContract(IsOneWay = true, IsInitiating = false, IsTerminating = true)]
        void Say(string msg);

        /// <summary>
        /// 向指定的用户发消息
        /// </summary>
        /// <param name="to">发送对象</param>
        /// <param name="msg">消息内容</param>
        [OperationContract(IsOneWay = true, IsInitiating = false, IsTerminating = false)]
        void Whisper(string to, string msg);

        /// <summary>
        /// 用户加入聊天组
        /// </summary>
        /// <param name="name">加入的人员信息</param>
        /// <returns>返回当前人员名单</returns>
        [OperationContract(IsOneWay = false, IsInitiating = true, IsTerminating = false)]
        Person[] Join(Person name);

        /// <summary>
        /// 离开聊天
        /// </summary>
        [OperationContract(IsOneWay = true, IsInitiating = false, IsTerminating = true)]
        void Leave();

    }
}

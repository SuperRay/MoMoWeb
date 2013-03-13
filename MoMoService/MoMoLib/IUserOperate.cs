using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.Drawing;

namespace MoMoLib
{
    [ServiceContract]
    public interface IUserOperate
    {
        [OperationContract]
        string UserLogin(UserInfo user);

        [OperationContract]
        string UserRegist(UserInfo userInfo);

        [OperationContract]
        bool UpdateUserInfo(UserInfo UserInfo);

        [OperationContract]
        UserInfo GetUserInfo(string loginName);

        [OperationContract]
        string ListCodeQuestion();
    }
}

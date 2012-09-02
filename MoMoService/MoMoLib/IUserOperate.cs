using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace MoMoLib
{
    [ServiceContract]
    public interface IUserOperate
    {
        [OperationContract]
        bool UserLogin(string _username, string _pwd);

        //[OperationContract]
        //bool UserRegist(string _username, string _pwd);

    }
}

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
        bool UserLogin(UserInfo user);
        //bool UserLogin(string x, string y);

        //[OperationContract]
        //bool UserRegist(string _username, string _pwd);

    }
    #region 用户类
    //[DataContract]
    //public class UserInfo
    //{
    //    private string _loginName;
    //    private string _userName;

    //    [DataMember(Name = "UserName", IsRequired = false, Order = 0)]
    //    public string UserName
    //    {
    //        get { return _userName; }
    //        set { _userName = value; }
    //    }
        //private string _englishName;

        //[DataMember(Name = "EnglishName", IsRequired = false, Order = 1)]
        //public string EnglishName
        //{
        //    get { return _englishName; }
        //    set { _englishName = value; }
        //}
        //private string _password;

        //[DataMember(Name = "Password", IsRequired = false, Order = 2)]
        //public string Password
        //{
        //    get { return _password; }
        //    set { _password = value; }
        //}

        //private Image _photo;
        //[DataMember(Name = "Photo", IsRequired = false, Order = 3)]
        //public Image Photo
        //{
        //    get { return _photo; }
        //    set { _photo = value; }
        //}
        //private int _phone;
        //[DataMember(Name = "Phone", IsRequired = false, Order = 4)]
        //public int Phone
        //{
        //    get { return _phone; }
        //    set { _phone = value; }
        //}

        //[DataMember(Name = "LoginName", IsRequired = false, Order = 5)]
        //public string LoginName
        //{
        //    get { return _loginName; }
        //    set { _loginName = value; }
        //}

        //private string _mobilPhone;
        //[DataMember(Name = "MobilPhone", IsRequired = false, Order = 6)]
        //public string MobilPhone
        //{
        //    get { return _mobilPhone; }
        //    set { _mobilPhone = value; }
        //}

        //private int _questionID;
        //[DataMember(Name = "QuestionID", IsRequired = false, Order = 7)]
        //public int QuestionID
        //{
        //    get { return _questionID; }
        //    set { _questionID = value; }
        //}
        //private int _mail;
        //[DataMember(Name = "Mail", IsRequired = false, Order = 8)]
        //public int Mail
        //{
        //    get { return _mail; }
        //    set { _mail = value; }
        //}

        //private string _questionAnswer;
        //[DataMember(Name = "QuestionAnswer", IsRequired = false, Order = 9)]
        //public string QuestionAnswer
        //{
        //    get { return _questionAnswer; }
        //    set { _questionAnswer = value; }
        //}

        //private string _job;
        //[DataMember(Name = "Job", IsRequired = false, Order = 10)]
        //public string Job
        //{
        //    get { return _job; }
        //    set { _job = value; }
        //}

        //private string _company;
        //[DataMember(Name = "Company", IsRequired = false, Order = 11)]
        //public string Company
        //{
        //    get { return _company; }
        //    set { _company = value; }
        //}

        //private string _idNumber;
        //[DataMember(Name = "IdNumber", IsRequired = false, Order = 12)]
        //public string IdNumber
        //{
        //    get { return _idNumber; }
        //    set { _idNumber = value; }
        //}

        //private string _graduteSchool;
        //[DataMember(Name = "GraduteSchool", IsRequired = false, Order = 13)]
        //public string GraduteSchool
        //{
        //    get { return _graduteSchool; }
        //    set { _graduteSchool = value; }
        //}

        //private string _qq;
        //[DataMember(Name = "QQ", IsRequired = false, Order = 14)]
        //public string QQ
        //{
        //    get { return _qq; }
        //    set { _qq = value; }
        //}

        //private string _msn;
        //[DataMember(Name = "Msn", IsRequired = false, Order = 15)]
        //public string Msn
        //{
        //    get { return _msn; }
        //    set { _msn = value; }
        //}

        //private string _weibo;
        //[DataMember(Name = "Weibo", IsRequired = false, Order = 16)]
        //public string Weibo
        //{
        //    get { return _weibo; }
        //    set { _weibo = value; }
        //}
    //}
    #endregion
}

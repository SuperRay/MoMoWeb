using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace MoMoLib
{
    [DataContract]
    public class UserInfo
    {
        private string _loginName;
        private string _userName;
        private DateTime _birthday;
        private string _sexy;

        [DataMember]
        public string Sexy
        {
            get { return _sexy; }
            set { _sexy = value; }
        }

        [DataMember]
        public DateTime Birthday
        {
            get { return _birthday; }
            set { _birthday = value; }
        }

        [DataMember]
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }
        private string _englishName;

        [DataMember]
        public string EnglishName
        {
            get { return _englishName; }
            set { _englishName = value; }
        }
        private string _password;

        [DataMember]
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        //private Bitmap _photo;
        //[DataMember]
        //public Bitmap Photo
        //{
        //    get { return _photo; }
        //    set { _photo = value; }
        //}

        private int _phone;
        [DataMember]
        public int Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }

        [DataMember]
        public string LoginName
        {
            get { return _loginName; }
            set { _loginName = value; }
        }

        private string _mobilPhone;
        [DataMember]
        public string MobilPhone
        {
            get { return _mobilPhone; }
            set { _mobilPhone = value; }
        }

        private int _questionID;
        [DataMember]
        public int QuestionID
        {
            get { return _questionID; }
            set { _questionID = value; }
        }
        private int _mail;
        [DataMember]
        public int Mail
        {
            get { return _mail; }
            set { _mail = value; }
        }

        private string _questionAnswer;
        [DataMember]
        public string QuestionAnswer
        {
            get { return _questionAnswer; }
            set { _questionAnswer = value; }
        }

        private string _job;
        [DataMember]
        public string Job
        {
            get { return _job; }
            set { _job = value; }
        }

        private string _company;
        [DataMember]
        public string Company
        {
            get { return _company; }
            set { _company = value; }
        }

        private string _idNumber;
        [DataMember]
        public string IdNumber
        {
            get { return _idNumber; }
            set { _idNumber = value; }
        }

        private string _graduteSchool;
        [DataMember]
        public string GraduteSchool
        {
            get { return _graduteSchool; }
            set { _graduteSchool = value; }
        }

        private string _qq;
        [DataMember]
        public string QQ
        {
            get { return _qq; }
            set { _qq = value; }
        }

        private string _msn;
        [DataMember]
        public string Msn
        {
            get { return _msn; }
            set { _msn = value; }
        }

        private string _weibo;
        [DataMember]
        public string Weibo
        {
            get { return _weibo; }
            set { _weibo = value; }
        }
    }
}

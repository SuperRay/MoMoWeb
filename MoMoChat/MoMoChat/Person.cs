using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace ChatLib
{
    [DataContract]
    public class Person
    {
        private string _imageURL;
        private string _name;

        public Person(string imageURL, string name)
        {
            this._imageURL = imageURL;
            this._name = name;
        }

        [DataMember]
        public string ImageURL
        {
            get { return _imageURL; }
            set { _imageURL = value; }
        }

        [DataMember]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
    }
}

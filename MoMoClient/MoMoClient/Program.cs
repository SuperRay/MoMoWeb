using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoMoClient.ServiceReference1;

namespace MoMoClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcom to MoMoWeb");

            using (UserOperateClient myClient = new UserOperateClient())
            {
                UserInfo user = new UserInfo();
                Console.Write("Your Name: ");
                user.LoginName = Console.ReadLine();
                Console.Write("Password:");
                user.Password = Console.ReadLine();
                string answer = myClient.UserRegist(user);
                Console.WriteLine("Client says:{0}", answer);
            }
            Console.ReadLine();
        }
    }
}

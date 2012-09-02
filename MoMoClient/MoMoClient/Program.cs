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
                
                Console.Write("Your word: ");
                string word = Console.ReadLine();
                bool answer = myClient.UserLogin("", "");
                Console.WriteLine("Client says:{0}", answer);
            }
            Console.ReadLine();
        }
    }
}

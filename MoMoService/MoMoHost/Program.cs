using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using MoMoLib;

namespace MoMoHost
{
    class Program
    {
        static void Main(string[] args)
        {
            ////承载运行基本的基址
            //Uri baseAddress = new Uri("http://localhost:8020/MoMoWeb/services");

            ////创建一个新的实例以承载服务 必须指定实现服务协定和基址的类型
            //ServiceHost selfHost = new ServiceHost(typeof(UserOperate), baseAddress);

            ////添加公开服务的终结点 必须指定终结点公开的协议，绑定和终结点的地址
            //selfHost.AddServiceEndpoint(typeof(IUserOperate), new WSHttpBinding(), "UserOperate");

            ////启用元数据交换 添加服务元数据行为
            //ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
            //smb.HttpGetEnabled = true;
            //selfHost.Description.Behaviors.Add(smb);

            ////打开ServiceHost 并等待传入消息，用户按Enter键时，关闭ServiceHost
            //selfHost.Open();

            //Console.WriteLine("The service is ready.");
            //Console.WriteLine("Press <Enter> to terminate service");
            //Console.WriteLine();
            //Console.ReadLine();

            //selfHost.Close();

            using (ServiceHost serviceHost = new ServiceHost(typeof(UserOperate)))
            {
                serviceHost.Open();
                DisplayHostInfo(serviceHost);
                Console.WriteLine("The service is ready.");
                Console.WriteLine("Press <Enter> to terminate service");
                Console.WriteLine();
                Console.ReadLine();
                serviceHost.Close();

            }

        }
        static void DisplayHostInfo(ServiceHost host)
        {
            Console.WriteLine();
            Console.WriteLine("******Host Info*******");

            foreach (System.ServiceModel.Description.ServiceEndpoint se in host.Description.Endpoints)
            {
                Console.WriteLine("Address:{0}", se.Address);
                Console.WriteLine("Binding:{0}", se.Binding.Name);
                Console.WriteLine("Contract:{0}", se.Contract.Name);
                Console.WriteLine();
            }
            Console.WriteLine("**********************************");
        }
    }
}

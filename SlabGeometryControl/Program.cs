using System;
using System.Diagnostics.Contracts;
using System.IO;
using Alvasoft.Server;
using Alvasoft.Wcf.NetConfiguration;
using log4net.Config;

namespace Alvasoft
{
    class Program
    {
        static void Main(string[] args)
        {            
            var configLogingFileName = "Settings\\Logging.xml";
            XmlConfigurator.Configure(new FileInfo(configLogingFileName));

            var netConfiguration = new NetConfigurationImpl {
                ServerHost = "192.168.1.66",
                ServerPort = 9876
            };


            using (var controller = new GCSService(netConfiguration)) {
                controller.OpenService();

                Console.WriteLine("Сервис запущен. Нажмите Enter для остановки.");
                Console.ReadLine();

                controller.CloseService();
            }            
        }
    }
}

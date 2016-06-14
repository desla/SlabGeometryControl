using System;
using System.Diagnostics.Contracts;
using System.IO;
using Alvasoft.Server;
using Alvasoft.Wcf.NetConfiguration;
using log4net.Config;
using System.ServiceProcess;
using System.Windows.Forms;

namespace Alvasoft
{
    class Program : ServiceBase {

        private static GCSService controller;
        private static NetConfigurationImpl netConfiguration = new NetConfigurationImpl {
            ServerHost = "localhost",
            ServerPort = 9876
        };

        static void Main(string[] args)
        {
            var appPath = Application.StartupPath + "/";
            var configLogingFileName = appPath + "Settings/Logging.xml";
            XmlConfigurator.Configure(new FileInfo(configLogingFileName));

            if (args.Length > 0 && args[0].ToLower().Equals("console")) {                
                using (controller = new GCSService(netConfiguration)) {
                    controller.OpenService();
                    Console.WriteLine("Сервис запущен. Нажмите Enter для остановки.");
                    Console.ReadLine();
                    controller.CloseService();
                }
            } else {
                ServiceBase.Run(new Program());
            }
        }        

        protected override void OnStart(string[] args) {
            controller = new GCSService(netConfiguration);
            controller.OpenService();
        }

        protected override void OnStop() {
            controller.CloseService();
            controller.Dispose(); 
        }
    }
}

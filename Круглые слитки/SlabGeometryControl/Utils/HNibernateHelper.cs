using System;
using System.Windows.Forms;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace Alvasoft.Utils
{
    public class NHibernateHelper
    {
        private static string configurationFileName;
        private static ISessionFactory _sessionFactory;

        private static string GetConfigurationFileName() {
            if (configurationFileName == null) {
                var appPath = Application.StartupPath + "/";
                configurationFileName = appPath + "Settings/hibernate.cfg.xml";
            }

            return configurationFileName;
        }

        private static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null) {
                    var configuration = new Configuration();
                    configuration.Configure(GetConfigurationFileName());
                    configuration.AddAssembly(typeof(Program).Assembly);
                    //new SchemaExport(configuration).Execute(false, true, false);
                    _sessionFactory = configuration.BuildSessionFactory();                    
                }
                return _sessionFactory;
            }
        }

        public static ISession OpenSession()
        {
            if (!SessionFactory.IsClosed) {
                return SessionFactory.OpenSession();
            }            

            throw new ArgumentException("Соединение закрыто.");
        }

        public static void CloseConnection()
        {
            if (!SessionFactory.IsClosed) {
                _sessionFactory.Close();
            }            
        }
    }
}

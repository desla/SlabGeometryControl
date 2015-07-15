using System;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace Alvasoft.Utils
{
    public class NHibernateHelper
    {
        private const string CONFIGURATION_FILE_NAME = "Settings/hibernate.cfg.xml";
        private static ISessionFactory _sessionFactory;

        private static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null) {
                    var configuration = new Configuration();
                    configuration.Configure(CONFIGURATION_FILE_NAME);
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

namespace Alvasoft.Wcf.Client.Exceptions
{
    using System;

    public class NetConfigurationException : Exception
    {
        public NetConfigurationException() : base("Конфигурация подключения не полная.")
        {            
        }
    }
}

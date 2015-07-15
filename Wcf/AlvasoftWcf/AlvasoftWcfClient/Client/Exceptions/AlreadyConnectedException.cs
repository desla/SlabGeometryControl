namespace Alvasoft.Wcf.Client.Exceptions
{
    using System;

    public class AlreadyConnectedException : Exception
    {
        public AlreadyConnectedException() : base("Попытка выполнить повторное подключение.")
        {
        }
    }
}

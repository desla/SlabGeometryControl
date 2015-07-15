namespace Alvasoft.Wcf.Client.Exceptions
{
    using System;

    public class AlreadyDisconnectedException : Exception
    {
        public AlreadyDisconnectedException() : base("Попытка выполнить повторное отключение.")
        {
        }
    }
}

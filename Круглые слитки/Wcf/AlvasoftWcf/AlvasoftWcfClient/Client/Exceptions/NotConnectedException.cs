namespace Alvasoft.Wcf.Client.Exceptions
{
    using System;

    public class NotConnectedException : Exception
    {
        public NotConnectedException() :base("Подключение отсутствует.")
        {
        }
    }
}

namespace Alvasoft.Wcf.Client
{
    using Alvasoft.Wcf.NetConfiguration;

    /// <summary>
    /// Интерфейс WCF клиента.
    /// </summary>
    public interface IClient
    {
        /// <summary>
        /// Сетевая конфигурация клиента.
        /// </summary>
        INetConfiguration NetConfig { get; set; }        

        /// <summary>
        /// Возвращает состояние подключения.
        /// </summary>
        bool IsConnected { get; }        

        /// <summary>
        /// Выполняет подключение клиента к серверу.
        /// </summary>
        void Connect();

        /// <summary>
        /// Выполняет отключение клиента от сервера.
        /// </summary>
        void Disconnect();
    }
}

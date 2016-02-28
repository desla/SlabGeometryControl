namespace Alvasoft.Wcf.NetConfiguration
{
    /// <summary>
    /// Интерфейс конфигурации сети.    
    /// </summary>
    public interface INetConfiguration
    {
        /// <summary>
        /// Хост или Ip адрес сервера.
        /// </summary>
        string ServerHost { get; }

        /// <summary>
        /// Порт сервера.
        /// </summary>
        int ServerPort { get; }
    }
}

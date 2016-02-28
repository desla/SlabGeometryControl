namespace Alvasoft.Wcf.NetConfiguration
{
    /// <summary>
    /// Реализация конфигурации сети.
    /// </summary>
    public class NetConfigurationImpl : INetConfiguration
    {
        /// <summary>
        /// Хост или Ip адрес сервера.
        /// </summary>
        public string ServerHost { get; set; }

        /// <summary>
        /// Порт сервера.
        /// </summary>
        public int ServerPort { get; set; }
    }
}

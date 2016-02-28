namespace Alvasoft.Wcf.Client
{
    /// <summary>
    /// Интерфейс для реагирования на события клиента.
    /// </summary>
    public interface IClientActionsListener
    {
        /// <summary>
        /// Оповещает о подключении клиента.
        /// </summary>
        /// <param name="aClient">Клиент.</param>
        /// <param name="aSessionId">Идентификатор выданной сессии.</param>
        void OnConnected(IClient aClient, long aSessionId);

        /// <summary>
        /// Оповещает об отключении клиента.
        /// </summary>
        /// <param name="aClient">Клиент.</param>
        /// <param name="aSessionId">Идентификатор сессии.</param>
        void OnDisconnected(IClient aClient, long aSessionId);
    }
}

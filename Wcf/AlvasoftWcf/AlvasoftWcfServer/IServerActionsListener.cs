namespace Alvasoft.Wcf.Server
{
    /// <summary>
    /// Интерфейс для реагирования на события сервера.
    /// </summary>
    public interface IServerActionsListener
    {
        /// <summary>
        /// Оповещает о подключении нового клиента.
        /// </summary>
        /// <param name="aSessionId">Идентификатор сессии клиента.</param>
        void OnConnected(long aSessionId);

        /// <summary>
        /// Оповещает об отключении клиента.
        /// </summary>
        /// <param name="aSessionId">Идентификатор сессии клиента.</param>
        void OnDisconnected(long aSessionId);

        /// <summary>
        /// Разрыв подключения.
        /// </summary>
        /// <param name="aSessionId">Идентификатор сессии.</param>
        void OnConnectionFault(long aSessionId);
    }
}

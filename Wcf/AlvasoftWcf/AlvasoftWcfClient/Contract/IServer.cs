namespace Alvasoft.Wcf.Contract
{
    using System.ServiceModel;

    /// <summary>
    /// Итерфейс сервера.
    /// </summary>
    [ServiceContract]
    public interface IServer
    {
        /// <summary>
        /// Выполняет логическое подключение клиента.
        /// </summary>
        /// <returns>Идентификатор сессии клиента.</returns>
        [OperationContract]
        long ConnectClient();

        /// <summary>
        /// Выполняет логического отключение клиента.
        /// </summary>
        /// <param name="aSessionId">Идентификатор сессии.</param>
        [OperationContract]
        void DisconnectClient(long aSessionId);
    }
}

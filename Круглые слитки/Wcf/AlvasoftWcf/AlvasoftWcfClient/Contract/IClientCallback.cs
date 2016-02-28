namespace Alvasoft.Wcf.Contract
{
    using System.ServiceModel;

    /// <summary>
    /// Интерфейс обратного вызова клиента. 
    /// Служит для тестирования подключения клиента.
    /// </summary>
    public interface IClientCallback
    {
        /// <summary>
        /// Возвращает идентификатор выданной сессии.
        /// </summary>
        /// <returns>Идентификатор сессии.</returns>
        [OperationContract]
        long GetSessionId();
    }
}

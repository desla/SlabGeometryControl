namespace Alvasoft.Wcf.Client
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using Alvasoft.Wcf.Client.Exceptions;
    using Alvasoft.Wcf.NetConfiguration;
    using Alvasoft.Wcf.Contract;

    /// <summary>
    /// Базовый класс клиента. Выполняет подключение и отключение.
    /// </summary>
    /// <typeparam name="TConcretServer"></typeparam>
    public abstract class WcfClientBase<TConcretServer>
        : IClient, IClientCallback
        where TConcretServer : IServer
    {
        /// <summary>
        /// Имя сервиса по-умолчанию.
        /// </summary>
        private const string DEFAULT_SERVICE_NAME = "AlvasoftServiceDefault";

        /// <summary>
        /// Файбрика инстансов.
        /// </summary>
        private DuplexChannelFactory<TConcretServer> channelFactory = null;

        /// <summary>
        /// Идентификатор сессии.
        /// </summary>
        private long sessionId = long.MinValue;

        /// <summary>
        /// Экземпляр конкретного сервера.
        /// </summary>
        protected TConcretServer ServerInstance { get; private set; }        

        /// <summary>
        /// Конфигурация подключения.
        /// </summary>
        public INetConfiguration NetConfig { get; set; }

        /// <summary>
        /// Имя сервиса.
        /// </summary>
        public string ServiceName { get; protected set; }

        /// <summary>
        /// Биндинг сервиса.
        /// </summary>
        public Binding ServiceBinding { get; protected set; }

        /// <summary>
        /// Слушатель событий подключения / отключения.
        /// </summary>
        public IClientActionsListener ConnectionActionsListener { get; set; }        

        /// <summary>
        /// Состояние подключения.
        /// </summary>
        public bool IsConnected {
            get { return IsNetConnected() && GetSessionId() != long.MinValue; }
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="aConfig">Конфигурация.</param>
        protected WcfClientBase(INetConfiguration aConfig)
        {
            if (aConfig == null) {
                throw new ArgumentNullException("aConfig");
            }

            if (string.IsNullOrEmpty(aConfig.ServerHost)) {
                throw new ArgumentException("aConfig.ServerHost");
            }

            if (aConfig.ServerPort < 0 || aConfig.ServerPort > 65535) {
                throw new ArgumentException("aConfig.ServerPort");
            }

            NetConfig = new NetConfigurationImpl {
                ServerHost = aConfig.ServerHost,
                ServerPort = aConfig.ServerPort
            };

            ServiceName = DEFAULT_SERVICE_NAME;
            ServiceBinding = new NetTcpBinding(SecurityMode.None);            
            (ServiceBinding as NetTcpBinding).MaxReceivedMessageSize = 9999999;
        }

        /// <summary>
        /// Выполняет подключение к серверу.
        /// Исключение: AlreadyConnectedException.
        /// </summary>
        public void Connect()
        {            
            if (!IsConnected) {
                var connectionString = MakeConnectionString();                                
                LogInfo("Подключение к " + connectionString + "...");
                
                ServerInstance = ConnectToServerInstance(connectionString);
                sessionId = ServerInstance.ConnectClient();

                if (ConnectionActionsListener != null) {
                    ConnectionActionsListener.OnConnected(this, GetSessionId());
                }

                LogInfo("Подключение выполнено.");
            }
            else {                
                throw new AlreadyConnectedException();
            }
        }        

        /// <summary>
        /// Выполняет отключение.
        /// </summary>
        public void Disconnect()
        {
            if (IsConnected) {
                LogInfo("Отключение от " + ServiceName + "...");
                try {
                    ServerInstance.DisconnectClient(GetSessionId());
                    channelFactory.Close();
                }
                catch (Exception ex) {
                    LogError("Ошибка при отключении: " + ex.Message);
                }
                finally {
                    channelFactory = null;
                    ServerInstance = default(TConcretServer);
                    sessionId = long.MinValue;
                    if (ConnectionActionsListener != null) {
                        ConnectionActionsListener.OnDisconnected(this, GetSessionId());
                    }
                }

                LogInfo("Отключение выполнено.");
            }
            else {
                throw new AlreadyDisconnectedException();
            }
        }

        /// <summary>
        /// Возвращает идентфикатор сессии.
        /// </summary>
        /// <returns>Идентификатор сессии.</returns>
        public long GetSessionId()
        {
            return sessionId;
        }

        /// <summary>
        /// Проверяет соединение и бросает исключение, если соединение отсутствует.
        /// </summary>
        protected void CheckConnection()
        {            
            if (!IsConnected) {
                throw new NotConnectedException();
            }
        }

        /// <summary>
        /// Логирует информационное сообщение.
        /// </summary>
        /// <param name="aInfoMessage">Информационное сообщение.</param>
        protected abstract void LogInfo(string aInfoMessage);

        /// <summary>
        /// Логирует ошибку.
        /// </summary>
        /// <param name="aErrorMessage">Текст ошибки.</param>
        protected abstract void LogError(string aErrorMessage);

        /// <summary>
        /// Логирует информацию.
        /// </summary>
        /// <param name="aDebugMessage">Дебаг-информация.</param>
        protected abstract void LogDebug(string aDebugMessage);

        /// <summary>
        /// Создает строку подключения.
        /// </summary>
        /// <returns>Строка подключения.</returns>
        private string MakeConnectionString()
        {
            var sourceString = "{0}://{1}:{2}/{3}";
            try {
                var connectionString = string.Format(sourceString,
                                                    ServiceBinding.Scheme,
                                                    NetConfig.ServerHost,
                                                    NetConfig.ServerPort,
                                                    ServiceName);
                return connectionString;
            }
            catch (NullReferenceException) {
                throw new NetConfigurationException();
            }
        }

        /// <summary>
        /// Выполняет физическое подключение к серверу.
        /// </summary>
        /// <param name="aConnectionString">Строка подключения.</param>
        /// <returns>Экземпляр сервера.</returns>
        private TConcretServer ConnectToServerInstance(string aConnectionString)
        {
            var address = new Uri(aConnectionString);
            var endpoint = new EndpointAddress(address);
            channelFactory = new DuplexChannelFactory<TConcretServer>(this, ServiceBinding, endpoint);

            return channelFactory.CreateChannel();
        }

        /// <summary>
        /// Возвращает состояние физического подключения.
        /// </summary>
        /// <returns>True - если подключение есть, false - иначе.</returns>
        private bool IsNetConnected()
        {            
            return channelFactory != null && channelFactory.State == CommunicationState.Opened;
        }
    }
}

namespace Alvasoft.Wcf.Server
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using Alvasoft.Wcf.NetConfiguration;
    using Alvasoft.Wcf.Contract;

    /// <summary>
    /// Класс сервера без Callback'а.
    /// </summary>    
    public abstract class WcfServerBase : WcfServerBase<IClientCallback>
    {
        protected WcfServerBase(INetConfiguration aConfig)
            : base(aConfig)
        {
        }
    }

    /// <summary>
    /// Базовый класс сервера.
    /// </summary>
    [ServiceBehavior
        (InstanceContextMode = InstanceContextMode.Single, 
         ConcurrencyMode = ConcurrencyMode.Multiple)]
    public abstract class WcfServerBase<TClientCallback> 
        : IServer
        where TClientCallback : IClientCallback
    {
        /// <summary>
        /// Имя сервиса по-умолчанию.
        /// </summary>
        private const string DEFAULT_SERVICE_NAME = "AlvasoftServiceDefault";

        /// <summary>
        /// Содержит идентификаторы сессий клиентов и каналы обратной связи.
        /// </summary>
        private Dictionary<long, TClientCallback> sessions = new Dictionary<long, TClientCallback>();
        private object sessionsLock = new object();

        /// <summary>
        /// Идентификатор сессии, который выдается клиентам при подключении.
        /// </summary>
        private long LastSessionId { get; set; }
        private object sessionGeneratorLock = new object();

        /// <summary>
        /// Текущий открытый канал связи с клиентами.
        /// </summary>
        private ServiceHost CurrentService { get; set; }

        /// <summary>
        /// Имя сервиса.
        /// </summary>
        public string ServiceName { get; protected set; }

        /// <summary>
        /// Биндинг сервиса.
        /// </summary>
        public Binding ServiceBinding { get; protected set; }

        /// <summary>
        /// Слушатель событий подключений/отключений клиентов к серверу.
        /// </summary>
        public IServerActionsListener ConnectionsActionListener { get; set; }

        /// <summary>
        /// Конфигурация подключения.
        /// </summary>
        public INetConfiguration NetConfig { get; protected set; }

        /// <summary>
        /// Тип бизнес-контракта.
        /// </summary>
        public Type ContractType { get; set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="aConfig">Конфигурация.</param>
        protected WcfServerBase(INetConfiguration aConfig)
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
        }

        /// <summary>
        /// Выполняет логическое подключение нового клиента.
        /// </summary>
        /// <returns>Идентификатор сессии клиента.</returns>
        public long ConnectClient()
        {
            LogInfo("Подключение нового клиента...");

            var sessionId = GenerateSessionId();
            lock (sessionsLock) {
                sessions[sessionId] = OperationContext.Current.GetCallbackChannel<TClientCallback>();                
            }
            OperationContext.Current.Channel.Closed += ChannelClosedAction;

            if (ConnectionsActionListener != null) {
                ConnectionsActionListener.OnConnected(sessionId);
            }

            LogInfo("Новый клиент подключе. Сессия: " + sessionId);

            return sessionId;            
        }        

        /// <summary>
        /// Выполняет логическое отключение клиента.
        /// </summary>
        /// <param name="aSessionId">Сессия клиента.</param>
        public void DisconnectClient(long aSessionId)
        {
            LogInfo("Отключение клиента. Сессия: " + aSessionId + "...");
            if (IsContainSession(aSessionId)) {
                lock (sessionsLock) {
                    if (IsContainSession(aSessionId)) {
                        sessions.Remove(aSessionId);
                    }                    
                }                

                if (ConnectionsActionListener != null) {
                    ConnectionsActionListener.OnDisconnected(aSessionId);
                }

                LogInfo("Отключение клиента выполнено. Сессия: " + aSessionId);
            }
            else {
                throw new FaultException("Клиент с такой сессией не подключен : " + aSessionId);
            }
        }        

        /// <summary>
        /// Запускает сервис.
        /// </summary>
        public void OpenService()
        {
            LogInfo("Запуск сервиса...");
            if (!IsListening()) {
                var connectionString = MakeConnectionString();
                var address = new Uri(connectionString);
                CurrentService = new ServiceHost(this);                
                CurrentService.AddServiceEndpoint(ContractType, ServiceBinding, address);
                CurrentService.Open();
                LogInfo("Сервис запущен.");
            }
            else {
                throw new FaultException("Повторный запуск сервиса.");
            }
        }

        /// <summary>
        /// Останавливает сервис.
        /// </summary>
        public void CloseService()
        {
            LogInfo("Остановка сервиса...");
            if (IsListening()) {                
                CurrentService.Close();
                CurrentService = null;
                sessions.Clear();                    
                LogInfo("Сервис остановлен.");                
            }
            else {
                throw new FaultException("Сервис не запущен.");
            }            
        }

        /// <summary>
        /// Возвращает список идентификаторов сессий.
        /// </summary>
        /// <returns>Список идентификаторов сессий.</returns>
        public long[] GetSessionsIds()
        {
            return sessions.Keys.ToArray();
        }

        /// <summary>
        /// Возвращает канал связи с клиентом.
        /// </summary>
        /// <param name="aSessionId">Идентификатор сессии.</param>
        /// <returns>Канал связи с клиентом.</returns>
        public TClientCallback GetClient(long aSessionId)
        {
            if (IsConnectedSession(aSessionId)) {                
                return sessions[aSessionId];
            }

            return default(TClientCallback);
        }

        /// <summary>
        /// Выполняет логирование сообщения.
        /// </summary>
        /// <param name="aInfoMessage"></param>
        protected abstract void LogInfo(string aInfoMessage);

        /// <summary>
        /// Возвращает состояние логического подключения клиента.
        /// </summary>
        /// <param name="aSessionId">Идентификатор сессии клиента.</param>
        /// <returns>True - если клиента подключен, false - иначе.</returns>
        protected bool IsContainSession(long aSessionId)
        {            
            return sessions.ContainsKey(aSessionId);
        }

        /// <summary>
        /// Возвращает состояние физического и логического подключения клиента.
        /// </summary>
        /// <param name="aSessionId">Идентификатор сессии клиента.</param>
        /// <returns>True - если клиент подключен, false - иначе.</returns>
        protected bool IsConnectedSession(long aSessionId)
        {
            try {
                return IsContainSession(aSessionId) && sessions[aSessionId].GetSessionId() == aSessionId;
            }
            catch {
                return false;
            }
        }

        /// <summary>
        /// Генерирует идентификатор сессии.
        /// </summary>
        /// <returns>Идентификатор сессии.</returns>
        private long GenerateSessionId()
        {
            lock (sessionGeneratorLock) {
                return ++LastSessionId;
            }            
        }

        /// <summary>
        /// Вызывается при обрыве связи
        /// </summary>
        /// <param name="aSessionId">Идентификатор сессии.</param>
        private void ConnectionFault(long aSessionId)
        {
            LogInfo("Обрыв связи с клиентом. Сессия: " + aSessionId + "...");
            if (IsContainSession(aSessionId)) {
                lock (sessionsLock) {
                    if (IsContainSession(aSessionId)) {
                        sessions.Remove(aSessionId);
                    }                    
                }                

                if (ConnectionsActionListener != null) {
                    ConnectionsActionListener.OnConnectionFault(aSessionId);
                }                
            }
        }

        /// <summary>
        /// Срабатывает при физической потери связи с каналом клиента.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChannelClosedAction(object sender, EventArgs e)
        {
            var closedSessions = new List<long>();
            lock (sessionsLock) {
                foreach (var sessionInfo in sessions) {
                    try {
                        sessionInfo.Value.GetSessionId();
                    }
                    catch {
                        closedSessions.Add(sessionInfo.Key);
                    }
                }
            }            

            foreach (var closedSession in closedSessions) {
                ConnectionFault(closedSession);
            }
        }

        /// <summary>
        /// Создает строку подключения.
        /// </summary>
        /// <returns>Строка подключения.</returns>
        private string MakeConnectionString()
        {
            var sourceString = "{0}://{1}:{2}/{3}";
            var connectionString = string.Format(sourceString,
                                                ServiceBinding.Scheme,
                                                NetConfig.ServerHost,
                                                NetConfig.ServerPort,
                                                ServiceName);
            return connectionString;            
        }

        /// <summary>
        /// Возвращает состояние сервиса.
        /// </summary>
        /// <returns>True - если сервис запущен, false - иначе.</returns>
        private bool IsListening()
        {
            return CurrentService != null && CurrentService.State == CommunicationState.Opened;
        }
    }
}

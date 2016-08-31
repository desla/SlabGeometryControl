namespace Alvasoft.SlabGeometryControl
{
    using System.ServiceModel;
    using Alvasoft.Wcf.Contract;

    /// <summary>
    /// Интерфейс для работы с системой контроля геометрии слитков.
    /// </summary>
    [ServiceContract(CallbackContract = typeof(IClientCallback))]
    public interface ISlabGeometryControlClient01 : IServer
    {        
        #region Api главного экрана, информация о слитках.

        /// <summary>
        /// Возвращает состояние подключения к контроллеру.
        /// </summary>
        /// <param name="aSessionId">Идентификатор сессии клиента.</param>
        /// <returns>Состояние подключения к контроллеру.</returns>
        [OperationContract]
        ControllerConnectionState GetConnectionState(long aSessionId);

        /// <summary>
        /// Показывает состояние работы системы: идет сканирование или нет.
        /// </summary>
        /// <param name="aSessionId">Идентификатор сессии клиента.</param>
        /// <returns>Состояние системы: SCANNING, если идет сканирование, WAITING - иначе.</returns>
        [OperationContract]
        SGCSystemState GetSGCSystemState(long aSessionId);

        /// <summary>
        /// Возвращает параметра слитка по его ID.
        /// </summary>
        /// <param name="aSessionId">Идентификатор сессии клиента.</param>
        /// <param name="aSlabId">ID слитка.</param>
        /// <returns>Параметры слитка.</returns>
        [OperationContract]
        DimentionResult[] GetDimentionResultsBySlabId(long aSessionId, int aSlabId);        

        /// <summary>
        /// Возвращает все измерения.
        /// </summary>
        /// <param name="aSessionId">Идентификатор сессии клиента.</param>
        /// <returns>Список измерений.</returns>
        [OperationContract]
        Dimention[] GetDimentions(long aSessionId);

        /// <summary>
        /// Возвращает список всех типа-размеров.
        /// </summary>
        /// <param name="aSessionId">Идентификатор сессии клиента.</param>
        /// <returns>Список типа-размерров.</returns>
        [OperationContract]
        StandartSize[] GetStandartSizes(long aSessionId);

        /// <summary>
        /// Возвращает список всех ограничений.
        /// </summary>
        /// <param name="aSessionId">Идентификатор сессии клиента.</param>
        /// <returns>Список ограничений.</returns>
        [OperationContract]
        Regulation[] GetRegulations(long aSessionId);

        /// <summary>
        /// Возвращает описание слитка по его номеру.
        /// </summary>
        /// <param name="aSessionId">Идентификатор сессии клиента.</param>
        /// <param name="aNumber">Номер слитка.</param>
        /// <returns>Описание слитка.</returns>
        [OperationContract]
        SlabInfo GetSlabInfoByNumber(long aSessionId, string aNumber);

        /// <summary>
        /// Возвращает список описаний слитков по интервалу времени сканирования.
        /// </summary>
        /// <param name="aSessionId">Идентификатор сессии клиента.</param>
        /// <param name="aFromTime">Время С...</param>
        /// <param name="aToTime">Время ...До</param>
        /// <returns>Список описаний слитков.</returns>
        [OperationContract]
        SlabInfo[] GetSlabInfosByTimeInterval(long aSessionId, long aFromTime, long aToTime);

        #endregion

        #region Api информация о датчиках.

        /// <summary>
        /// Возвращает количество активных датчиков в системе.
        /// </summary>
        /// <param name="aSessionId">Идентификатор сессии клиента.</param>
        /// <returns>Количество активных датчиков.</returns>
        [OperationContract]
        int GetSensorsCount(long aSessionId);

        /// <summary>
        /// Возвращает описание датчиков в системе.
        /// </summary>
        /// <param name="aSessionId">Идентификатор сессии клиента.</param>
        /// <returns>Список описаний датчиков.</returns>
        [OperationContract]
        SensorInfo[] GetSensorInfos(long aSessionId);

        /// <summary>
        /// Возвращает текущие показания датчика по его идентификатору.
        /// </summary>
        /// <param name="aSessionId">Идентификатор сессии клиента.</param>
        /// <param name="aSensorId">Идентификатор датчика.</param>
        /// <returns>Текущие показания датчика.</returns>
        [OperationContract]
        SensorValue GetSensorValueBySensorId(long aSessionId, int aSensorId);

        #endregion

        #region Api Данные об отсканированных слитках.

        /// <summary>
        /// Возвращает все точки поверхности слитка по идентификатору слитка.
        /// </summary>
        /// <param name="aSessionId">Идентификатор сессии клиента.</param>
        /// <param name="aSlabId">Идентификатор слитка.</param>
        /// <param name="aIsUseFilters">Показывает использовать или нет фильтры.</param>
        /// <returns>Все точки поверхности слитка.</returns>
        [OperationContract]
        SlabModel3D GetSlabModel3DBySlabId(long aSessionId, int aSlabId, bool aIsUseFilters);

        /// <summary>
        /// Возвращает показания датчика при замере слитка.
        /// </summary>
        /// <param name="aSessionId">Идентификатор сессии клиента.</param>
        /// <param name="aSlabId">Идентификатор слитка.</param>
        /// <param name="aSensorId">Идентификатор датчика.</param>
        /// <returns>Замеренные датчиком показания.</returns>
        [OperationContract]
        SensorValue[] GetSensorValuesBySlabId(long aSessionId, int aSlabId, int aSensorId);

        /// <summary>
        /// Возвращает строковое описание пересчитанных параметров.
        /// </summary>
        /// <param name="aSessionId">Идентификатор сессии.</param>
        /// <param name="aSlabId">Идентификатор слитка.</param>
        /// <returns>Строковое описание пересчитанных параметров.</returns>
        [OperationContract]
        string GetRecalculatedValuesString(long aSessionId, int aSlabId);

        #endregion

        #region Api для внесения изменений в конфигурацию.

        /// <summary>
        /// Добавляет типа-размер.
        /// </summary>
        /// <param name="aSessionId">Идентификатор сессии клиента.</param>
        /// <param name="aStandartSize">Описание нового типа-размера.</param>
        /// <returns>Идентификатор нового типа-размера.</returns>
        [OperationContract]
        int AddStandartSize(long aSessionId, StandartSize aStandartSize);

        /// <summary>
        /// Удаляет типа-размер по идентификатору.
        /// </summary>
        /// <param name="aSessionId">Идентификатор сессии клиента.</param>
        /// <param name="aStandartSizeId">Идентификатор типа-размера.</param>
        /// <returns>True - если удаление успешно, false - иначе.</returns>
        [OperationContract]
        bool RemoveStandartSize(long aSessionId, int aStandartSizeId);

        /// <summary>
        /// Вносит изменения в описание типа-размера.
        /// </summary>
        /// <param name="aSessionId">Идентификатор сессии клиента.</param>
        /// <param name="aStandartSize">Типа-размер.</param>
        [OperationContract]
        void EditStandartSize(long aSessionId, StandartSize aStandartSize);

        /// <summary>
        /// Добавляет новое правило для параметров слитка.
        /// </summary>
        /// <param name="aSessionId">Идентификатор сессии клиента.</param>
        /// <param name="aRegulation">Правило для параметров слитка.</param>
        /// <returns>Идентификатор нового правила.</returns>
        [OperationContract]
        int AddRegulation(long aSessionId, Regulation aRegulation);

        /// <summary>
        /// Удаляет правило для параметров слитка.
        /// </summary>
        /// <param name="aSessionId">Идентификатор сессии клиента.</param>
        /// <param name="aRegulationId">Идентификатор правила.</param>
        /// <returns>True - если удаление успешно, false - иначе.</returns>
        [OperationContract]
        bool RemoveRegulation(long aSessionId, int aRegulationId);

        /// <summary>
        /// Вносит изменения в правило.
        /// </summary>
        /// <param name="aSessionId">Идентификатор сессии клиента.</param>
        /// <param name="aRegulation">Правило.</param>
        [OperationContract]
        void EditRegulation(long aSessionId, Regulation aRegulation);

        /// <summary>
        /// Добавляет описание нового датчика.
        /// </summary>
        /// <param name="aSessionId">Идентификатор сессии клиента.</param>
        /// <param name="aSensorInfo">Описание датчика.</param>
        /// <returns>Идентификатор нового описания датчика.</returns>
        [OperationContract]
        int AddSensorInfo(long aSessionId, SensorInfo aSensorInfo);

        /// <summary>
        /// Удаляет описание датчика по его идентификатору.
        /// </summary>
        /// <param name="aSessionId">Идентификатор сессии клиента.</param>
        /// <param name="aSensorInfoId">Идентификатор датчика.</param>
        /// <returns>True - если удаление успешно, false - иначе.</returns>
        [OperationContract]
        bool RemoveSensorInfo(long aSessionId, int aSensorInfoId);

        /// <summary>
        /// Вносит изменения в описание датчика.
        /// </summary>
        /// <param name="aSessionId">Идентификатор сессии клиента.</param>
        /// <param name="aSensorInfo">Описание датчика.</param>
        [OperationContract]
        void EditSensorInfo(long aSessionId, SensorInfo aSensorInfo);

        /// <summary>
        /// Изменяет статус датчика на активный.
        /// </summary>
        /// <param name="aSessionId">Идентификатор сессии клиента.</param>
        /// <param name="aSensorId">Идентификатор датчика.</param>
        /// <returns>True - если удалось активировать датчик, false - иначе.</returns>
        [OperationContract]
        bool ActivateSensor(long aSessionId, int aSensorId);

        /// <summary>
        /// Добавляет к датчику ОРС-тег.
        /// </summary>
        /// <param name="aSessionId">Идентификатор сессии клиента.</param>
        /// <param name="aSensorId">Идентификатор датчика.</param>
        /// <param name="aTagValue">Значение тега.</param>
        /// <param name="aTagName">Имя тега.</param>
        [OperationContract]
        void AddOpcSensorTag(long aSessionId, int aSensorId, string aTagValue, string aTagName);

        /// <summary>
        /// Устанавливает калибровочное значение для датчика.
        /// </summary>
        /// <param name="aSessionId">Идентификатор сессии клиента.</param>
        /// <param name="aSensorId">Идентификатор датчика.</param>
        /// <param name="aCalibratedValue">Калибровочное значение.</param>
        [OperationContract]
        void SetCalibratedValue(long aSessionId, int aSensorId, double aCalibratedValue);

        #endregion
    }
}

namespace Alvasoft.SlabGeometryControl
{
    using System.ServiceModel;
    using Alvasoft.Wcf.Contract;

    /// <summary>
    /// Интерфейс для работы с системой контроля геометрии слитков.
    /// </summary>
    [ServiceContract]
    public interface ISlabGeometryControlClient01_copy : IServer
    {        
        #region Api главного экрана, информация о слитках.

        /// <summary>
        /// Возвращает состояние подключения к контроллеру.
        /// </summary>
        /// <returns>Состояние подключения к контроллеру.</returns>
        [OperationContract]
        ControllerConnectionState GetConnectionState();

        /// <summary>
        /// Показывает состояние работы системы: идет сканирование или нет.
        /// </summary>
        /// <returns>Состояние системы: SCANNING, если идет сканирование, WAITING - иначе.</returns>
        [OperationContract]
        SGCSystemState GetSGCSystemState();        

        /// <summary>
        /// Возвращает параметра слитка по его ID.
        /// </summary>
        /// <param name="aSlabId">ID слитка.</param>
        /// <returns>Параметры слитка.</returns>
        [OperationContract]
        DimentionResult[] GetDimentionResultsBySlabId(int aSlabId);        

        /// <summary>
        /// Возвращает все измерения.
        /// </summary>
        /// <returns>Список измерений.</returns>
        [OperationContract]
        Dimention[] GetDimentions();

        /// <summary>
        /// Возвращает список всех типа-размеров.
        /// </summary>
        /// <returns>Список типа-размерров.</returns>
        [OperationContract]
        StandartSize[] GetStandartSizes();

        /// <summary>
        /// Возвращает список всех ограничений.
        /// </summary>
        /// <returns>Список ограничений.</returns>
        [OperationContract]
        Regulation[] GetRegulations();

        /// <summary>
        /// Возвращает описание слитка по его номеру.
        /// </summary>
        /// <param name="aNumber">Номер слитка.</param>
        /// <returns>Описание слитка.</returns>
        [OperationContract]
        SlabInfo GetSlabInfoByNumber(string aNumber);

        /// <summary>
        /// Возвращает список описаний слитков по интервалу времени сканирования.
        /// </summary>
        /// <param name="aFromTime">Время С...</param>
        /// <param name="aToTime">Время ...До</param>
        /// <returns>Список описаний слитков.</returns>
        [OperationContract]
        SlabInfo[] GetSlabInfosByTimeInterval(long aFromTime, long aToTime);

        #endregion

        #region Api информация о датчиках.

        /// <summary>
        /// Возвращает количество активных датчиков в системе.
        /// </summary>
        /// <returns>Количество активных датчиков.</returns>
        [OperationContract]
        int GetSensorsCount();

        /// <summary>
        /// Возвращает описание датчиков в системе.
        /// </summary>
        /// <returns>Список описаний датчиков.</returns>
        [OperationContract]
        SensorInfo[] GetSensorInfos();

        /// <summary>
        /// Возвращает текущие показания датчика по его идентификатору.
        /// </summary>
        /// <param name="aSensorId">Идентификатор датчика.</param>
        /// <returns>Текущие показания датчика.</returns>
        [OperationContract]
        SensorValue GetSensorValueBySensorId(int aSensorId);

        #endregion

        #region Api Данные об отсканированных слитках.

        /// <summary>
        /// Возвращает все точки поверхности слитка по идентификатору слитка.
        /// </summary>
        /// <param name="aSlabId">Идентификатор слитка.</param>
        /// <returns>Все точки поверхности слитка.</returns>
        [OperationContract]
        SlabPoint[] GetSlabPointsBySlabId(int aSlabId);

        /// <summary>
        /// Возвращает показания датчика при замере слитка.
        /// </summary>
        /// <param name="aSlabId">Идентификатор слитка.</param>
        /// <param name="aSensorId">Идентификатор датчика.</param>
        /// <returns>Замеренные датчиком показания.</returns>
        [OperationContract]
        SensorValue[] GetSensorValuesBySlabId(int aSlabId, int aSensorId);       

        #endregion

        #region Api для внесения изменений в конфигурацию.

        /// <summary>
        /// Добавляет типа-размер.
        /// </summary>
        /// <param name="aStandartSize">Описание нового типа-размера.</param>
        /// <returns>Идентификатор нового типа-размера.</returns>
        [OperationContract]
        int AddStandartSize(StandartSize aStandartSize);

        /// <summary>
        /// Удаляет типа-размер по идентификатору.
        /// </summary>
        /// <param name="aStandartSizeId">Идентификатор типа-размера.</param>
        /// <returns>True - если удаление успешно, false - иначе.</returns>
        [OperationContract]
        bool RemoveStandartSize(int aStandartSizeId);

        /// <summary>
        /// Вносит изменения в описание типа-размера.
        /// </summary>
        /// <param name="aStandartSize">Типа-размер.</param>
        [OperationContract]
        void EditStandartSize(StandartSize aStandartSize);

        /// <summary>
        /// Добавляет новое правило для параметров слитка.
        /// </summary>
        /// <param name="aRegulation">Правило для параметров слитка.</param>
        /// <returns>Идентификатор нового правила.</returns>
        [OperationContract]
        int AddRegulation(Regulation aRegulation);

        /// <summary>
        /// Удаляет правило для параметров слитка.
        /// </summary>
        /// <param name="aRegulationId">Идентификатор правила.</param>
        /// <returns>True - если удаление успешно, false - иначе.</returns>
        [OperationContract]
        bool RemoveRegulation(int aRegulationId);

        /// <summary>
        /// Вносит изменения в правило.
        /// </summary>
        /// <param name="aRegulation">Правило.</param>
        [OperationContract]
        void EditRegulation(Regulation aRegulation);

        /// <summary>
        /// Добавляет описание нового датчика.
        /// </summary>
        /// <param name="aSensorInfo">Описание датчика.</param>
        /// <returns>Идентификатор нового описания датчика.</returns>
        [OperationContract]
        int AddSensorInfo(SensorInfo aSensorInfo);

        /// <summary>
        /// Удаляет описание датчика по его идентификатору.
        /// </summary>
        /// <param name="aSensorInfoId">Идентификатор датчика.</param>
        /// <returns>True - если удаление успешно, false - иначе.</returns>
        [OperationContract]
        bool RemoveSensorInfo(int aSensorInfoId);

        /// <summary>
        /// Вносит изменения в описание датчика.
        /// </summary>
        /// <param name="aSensorInfo">Описание датчика.</param>
        [OperationContract]
        void EditSensorInfo(SensorInfo aSensorInfo);

        /// <summary>
        /// Изменяет статус датчика на активный.
        /// </summary>
        /// <param name="aSensorId">Идентификатор датчика.</param>
        /// <returns>True - если удалось активировать датчик, false - иначе.</returns>
        [OperationContract]
        bool ActivateSensor(int aSensorId);

        /// <summary>
        /// Добавляет к датчику ОРС-тег.
        /// </summary>
        /// <param name="aSensorId">Идентификатор датчика.</param>
        /// <param name="aTagValue">Значение тега.</param>
        /// <param name="aTagName">Имя тега.</param>
        [OperationContract]
        void AddOpcSensorTag(int aSensorId, string aTagValue, string aTagName);

        #endregion
    }
}

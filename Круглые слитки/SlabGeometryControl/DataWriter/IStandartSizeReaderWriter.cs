namespace Alvasoft.DataWriter
{
    /// <summary>
    /// Интерфейс для доступа к типа-размерам.
    /// </summary>
    public interface IStandartSizeReaderWriter
    {        
        /// <summary>
        /// Возвращает список описаний типа-размеров.
        /// </summary>
        /// <returns>Список типа-размеров.</returns>
        IStandartSize[] GetStandartSizes();

        /// <summary>
        /// Добавляет новые типа-размер.
        /// </summary>
        /// <param name="aStandartSize">Типа-размер.</param>
        int AddStandartSize(IStandartSize aStandartSize);

        /// <summary>
        /// Удаляет типа-размер.
        /// </summary>
        /// <param name="aId">Идентификатор типа-размера.</param>
        void RemoveStandartSize(int aId);

        /// <summary>
        /// Вносит измерения в типа-размер.
        /// </summary>
        /// <param name="aStandartSize">Описание типа-размера.</param>
        void EditStandartSize(IStandartSize aStandartSize);
    }
}

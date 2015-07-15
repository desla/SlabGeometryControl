namespace Alvasoft.DimentionConfiguration
{
    /// <summary>
    /// Конфигурация измерений (параметров слитка).
    /// </summary>
    public interface IDimentionConfiguration
    {
        /// <summary>
        /// Возвращает описание измерения по идентификатору.
        /// </summary>
        /// <param name="aId">Идентификатор.</param>
        /// <returns>Описание измерения.</returns>
        IDimentionInfo GetDimentionInfoById(int aId);

        /// <summary>
        /// Возвращает описание измерения по имени.
        /// </summary>
        /// <param name="aName">Имя.</param>
        /// <returns>Описание измерения.</returns>
        IDimentionInfo GetDimentionInfoByName(string aName);

        /// <summary>
        /// Возвращает измерение по индексу.
        /// </summary>
        /// <param name="aIndex">Индекс.</param>
        /// <returns>Описание измерения.</returns>
        IDimentionInfo GetDimentionInfoByIndex(int aIndex);

        /// <summary>
        /// Возвращает количество измерений.
        /// </summary>
        /// <returns>Количество измерений.</returns>
        int GetDimentionInfosCount();
    }
}

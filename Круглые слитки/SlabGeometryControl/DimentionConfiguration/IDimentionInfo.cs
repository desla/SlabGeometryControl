namespace Alvasoft.DimentionConfiguration
{
    /// <summary>
    /// Описание измерения.
    /// </summary>
    public interface IDimentionInfo
    {
        /// <summary>
        /// Возвращает идентификатор измерения.
        /// </summary>
        /// <returns>Идентификатор.</returns>
        int GetId();

        /// <summary>
        /// Возвращает имя измерения.
        /// </summary>
        /// <returns>Имя измерения.</returns>
        string GetName();

        /// <summary>
        /// Возвращает описание измерения.
        /// </summary>
        /// <returns>Описание измерения.</returns>
        string GetDescription();
    }
}

namespace Alvasoft.DimentionValueContainer
{
    /// <summary>
    /// Накопитель параметров слитка.
    /// </summary>
    public interface IDimentionValueContainer
    {
        /// <summary>
        /// Возвращает сохраненные параметра слитка.
        /// </summary>
        /// <returns>Параметра слитка.</returns>
        IDimentionValue[] GetDimentionValues();

        /// <summary>
        /// Добавляет значение параметра слитка.
        /// </summary>
        /// <param name="aDimentionValue">Значение параметра слитка.</param>
        void AddDimentionValue(IDimentionValue aDimentionValue);

        /// <summary>
        /// Очищает все сохраненные значения параметров слитка.
        /// </summary>
        void Clear();

        /// <summary>
        /// Возвращает признак отсутствия элементов в накопителе.
        /// </summary>
        /// <returns>True, если данных нет, false - иначе.</returns>
        bool IsEmpty();
    }
}

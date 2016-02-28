using Alvasoft.DimentionValueContainer;

namespace Alvasoft.DataWriter
{
    /// <summary>
    /// Писатель значение параметра слитка.
    /// </summary>
    public interface IDimentionValueReaderWriter
    {
        /// <summary>
        /// Записывает значение параметра слитка.
        /// </summary>
        /// <param name="aValues">Значение параметра слитка.</param>
        /// <param name="aSlabId">Идентификатор слитка.</param>
        void WriteDimentionValues(int aSlabId, IDimentionValue[] aValues);

        /// <summary>
        /// Возвращает значение параметра слитка.
        /// </summary>
        /// <param name="aSlabId">Идентификатор слитка.</param>
        /// <param name="aDimentionId">Идентификатор измерения.</param>
        /// <returns>Параметра слитка.</returns>
        IDimentionValue ReadDimentionValue(int aSlabId, int aDimentionId);
    }
}

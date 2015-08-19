using Alvasoft.SlabBuilder;
using Alvasoft.SlabBuilder.Impl;

namespace Alvasoft.DimentionCalculator
{
    /// <summary>
    /// Алгоритм расчета конкретного параметра слитка.
    /// </summary>
    public interface IDimentionAlgorithm
    {
        /// <summary>
        /// Возвразает имя расчитываемого параметра слитка.
        /// </summary>
        /// <returns>Имя параметра.</returns>
        string GetName();        

        /// <summary>
        /// Рассчитывает значение параметра слитка.
        /// </summary>
        /// <param name="aSlabModel">Модель слитка.</param>
        /// <returns>Значение параметра.</returns>
        double CalculateValue(SlabModelImpl aSlabModel);
    }
}

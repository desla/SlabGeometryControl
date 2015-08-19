using Alvasoft.DimentionConfiguration;
using Alvasoft.DimentionValueContainer;
using Alvasoft.SlabBuilder;
using Alvasoft.SlabBuilder.Impl;

namespace Alvasoft.DimentionCalculator
{
    /// <summary>
    /// Модуль расчета параметров слитка.
    /// </summary>
    public interface IDimentionCalculator
    {
        /// <summary>
        /// Устанавливает конфигурацию измерений.
        /// </summary>
        /// <param name="aConfiguration">Конфигурация измерений.</param>
        void SetDimentionConfiguration(IDimentionConfiguration aConfiguration);

        /// <summary>
        /// Устанавливает накопитель параметров слитка.
        /// </summary>
        /// <param name="aContainer">Накопитель параметров слитка.</param>
        void SetDimentionValueContainer(IDimentionValueContainer aContainer);

        /// <summary>
        /// Расчитывает параметра слитка и сохраняет результаты в накопитель параметров слитка.
        /// </summary>
        /// <param name="aSlabModel">Модель слитка.</param>
        void CalculateDimentions(ISlabModel aSlabModel);
    }
}

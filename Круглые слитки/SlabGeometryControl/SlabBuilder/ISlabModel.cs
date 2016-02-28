using Alvasoft.SlabGeometryControl;
using Alvasoft.Utils.Mathematic3D;

namespace Alvasoft.SlabBuilder
{
    /// <summary>
    /// Модель слитка.
    /// </summary>
    public interface ISlabModel
    {
        /// <summary>
        /// Возвращает минимальное значение по оси OZ (глубина слитка).
        /// </summary>
        /// <returns></returns>
        double GetLengthLimit();

        /// <summary>
        /// Возвращает точку центра слитка по оси OZ.
        /// </summary>
        /// <param name="aZ">Глубина слитка.</param>
        /// <returns></returns>
        Point3D GetCenterPoint(double aZ);

        /// <summary>
        /// Возвращает диаметр слитка в указанной позиции слитка.
        /// </summary>
        /// <param name="aZ">Глубина слитка.</param>
        /// <returns>Диаметр.</returns>
        double GetDiameter(double aZ);

        /// <summary>
        /// Для передачи данных по сети.
        /// </summary>
        /// <returns></returns>
        SlabModel3D ToSlabModel();
    }
}

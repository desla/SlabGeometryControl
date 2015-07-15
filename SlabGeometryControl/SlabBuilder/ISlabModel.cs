using Alvasoft.Utils.Mathematic3D;

namespace Alvasoft.SlabBuilder
{
    /// <summary>
    /// Модель слитка.
    /// </summary>
    public interface ISlabModel
    {
        /// <summary>
        /// Возвращает максимальное значение по оси OY.
        /// </summary>
        /// <returns></returns>
        double GetTopLimit();
        /// <summary>
        /// Возвращает минимальное значение по оси OY.
        /// </summary>
        /// <returns></returns>
        double GetBottomLimit();
        /// <summary>
        /// Возвращает минимальное значение по оси OX.
        /// </summary>
        /// <returns></returns>
        double GetLeftLimit();
        /// <summary>
        /// Возвращает максимальное значениие по оси OX.
        /// </summary>
        /// <returns></returns>
        double GetRightLimit();
        /// <summary>
        /// Возвращает минимальное значение по оси OZ (глубина слитка).
        /// </summary>
        /// <returns></returns>
        double GetLengthLimit();

        Point3D GetTopSidePoint(double aX, double aZ);
        Point3D GetBottomSidePoint(double aX, double aZ);
        Point3D GetLeftSidePoint(double aY, double aZ);
        Point3D GetRightSidePoint(double aY, double aZ);
    }
}

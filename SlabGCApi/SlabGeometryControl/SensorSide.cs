using System.Runtime.Serialization;

namespace Alvasoft.SlabGeometryControl
{
    /// <summary>
    /// Сторона датчика.
    /// </summary>
    [DataContract]
    public enum SensorSide
    {
        /// <summary>
        /// Верхняя стоорна.
        /// </summary>
        [EnumMember]
        TOP,
        /// <summary>
        /// Нижняя сторона.
        /// </summary>
        [EnumMember]
        BOTTOM,
        /// <summary>
        /// Левая сторона.
        /// </summary>
        [EnumMember]
        LEFT,
        /// <summary>
        /// Правая сторона.
        /// </summary>
        [EnumMember]
        RIGHT
    }
}

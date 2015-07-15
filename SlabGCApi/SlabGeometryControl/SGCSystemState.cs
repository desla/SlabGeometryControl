using System.Runtime.Serialization;

namespace Alvasoft.SlabGeometryControl
{
    /// <summary>
    /// Состояние работы системы: идет сканирование или нет.
    /// </summary>
    [DataContract]
    public enum SGCSystemState
    {
        /// <summary>
        /// Идет процесс сканирования.
        /// </summary>
        [EnumMember]
        SCANNING,
        /// <summary>
        /// В ожидании сканирования.
        /// </summary>
        [EnumMember]
        WAITING
    }
}

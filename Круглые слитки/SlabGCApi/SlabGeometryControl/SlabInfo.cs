using System.Runtime.Serialization;

namespace Alvasoft.SlabGeometryControl
{
    /// <summary>
    /// Информация о слитке.
    /// </summary>
    [DataContract]
    public class SlabInfo
    {
        /// <summary>
        /// Идентификатор слитка.
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Номер слитка.
        /// </summary>
        [DataMember]
        public string Number { get; set; }

        /// <summary>
        /// Идентификатор типа-размера.
        /// </summary>
        [DataMember]
        public int StandartSizeId { get; set; }

        /// <summary>
        /// Время начала сканирования.
        /// </summary>
        [DataMember]
        public long StartScanTime { get; set; }

        /// <summary>
        /// Время окончания сканирования.
        /// </summary>
        [DataMember]
        public long EndScanTime { get; set; }
    }
}

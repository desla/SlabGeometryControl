using System.Runtime.Serialization;

namespace Alvasoft.SlabGeometryControl
{
    /// <summary>
    /// Ограничения для слитков.
    /// </summary>
    [DataContract]                
    public class Regulation
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Идентификатор измерения.
        /// </summary>
        [DataMember]
        public int DimentionId { get; set; }

        /// <summary>
        /// Идентификатор типа-размера.
        /// </summary>
        [DataMember]
        public int StandartSizeId { get; set; }

        /// <summary>
        /// Максимальное значение параметра слитка.
        /// </summary>
        [DataMember]
        public double MaxValue { get; set; }

        /// <summary>
        /// Минимальное значение параметра.
        /// </summary>
        [DataMember]
        public double MinValue { get; set; }
    }
}

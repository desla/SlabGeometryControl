using System.Runtime.Serialization;

namespace Alvasoft.SlabGeometryControl
{
    /// <summary>
    /// Параметр слитка.
    /// </summary>
    [DataContract]
    public class DimentionResult
    {
        /// <summary>
        /// Идентификатор измерения.
        /// </summary>
        [DataMember]
        public int DimentionId { get; set; }

        /// <summary>
        /// Идентификатор слитка.
        /// </summary>
        [DataMember]
        public int SlabId { get; set; }

        /// <summary>
        /// Значение параметра.
        /// </summary>
        [DataMember]
        public double Value { get; set; }
    }
}

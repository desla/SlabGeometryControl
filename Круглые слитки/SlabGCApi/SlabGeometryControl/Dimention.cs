using System.Runtime.Serialization;

namespace Alvasoft.SlabGeometryControl
{
    /// <summary>
    /// Параметра слитка.
    /// </summary>
    [DataContract]
    public class Dimention
    {
        /// <summary>
        /// Идентификатор измерения.
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Имя измерения.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Описание.
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}

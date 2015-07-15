using System.Runtime.Serialization;

namespace Alvasoft.SlabGeometryControl
{
    /// <summary>
    /// Типо-размер.
    /// </summary>
    [DataContract]        
    public class StandartSize
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        [DataMember]        
        public int Id { get; set; }

        /// <summary>
        /// Ширина.
        /// </summary>
        [DataMember]
        public double Width { get; set; }

        /// <summary>
        /// Высота.
        /// </summary>
        [DataMember]
        public double Height { get; set; }

        /// <summary>
        /// Длина слитка.
        /// </summary>
        [DataMember]
        public double Length { get; set; }

        public override string ToString()
        {
            return (int)Width + "x" + (int)Height + "x" + (int)Length;
        }
    }
}

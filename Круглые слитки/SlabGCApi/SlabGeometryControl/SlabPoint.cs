using System.Runtime.Serialization;

namespace Alvasoft.SlabGeometryControl
{
    /// <summary>
    /// Точка поверхности слитка в пространстве.
    /// </summary>
    [DataContract]                
    public class SlabPoint
    {
        /// <summary>
        /// Положение по оси OX.
        /// </summary>
        [DataMember]
        public double X { get; set; }

        /// <summary>
        /// Положение по оси OY.
        /// </summary>
        [DataMember]
        public double Y { get; set; }

        /// <summary>
        /// Положение по оси OZ.
        /// </summary>
        [DataMember]
        public double Z { get; set; }
    }
}

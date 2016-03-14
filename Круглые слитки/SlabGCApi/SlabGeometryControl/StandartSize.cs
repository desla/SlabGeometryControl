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
        /// Диаметр.
        /// </summary>
        [DataMember]
        public double Diameter { get; set; }        

        public override string ToString()
        {
            return "D " + (int)Diameter;
        }
    }
}

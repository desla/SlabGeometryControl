using System.Runtime.Serialization;

namespace Alvasoft.SlabGeometryControl
{
    [DataContract]
    public class SlabModel3D
    {
        [DataMember]
        public SlabPoint[] CenterLine { get; set; }

        [DataMember]
        public double[] Diameters { get; set; }      
        
        [DataMember]
        public SlabPoint[][] SensorsLines { get; set; }   
    }
}

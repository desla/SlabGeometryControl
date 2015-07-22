using System.Runtime.Serialization;

namespace Alvasoft.SlabGeometryControl
{
    [DataContract]
    public class SlabModel3D
    {
        [DataMember]
        public SlabPoint[][] TopLines { get; set; }

        [DataMember]
        public SlabPoint[][] BottomLines { get; set; }

        [DataMember]
        public SlabPoint[][] LeftLines { get; set; }

        [DataMember]
        public SlabPoint[][] RightLines { get; set; }
    }
}

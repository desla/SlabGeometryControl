using System.Collections.Generic;

namespace Alvasoft.DataProviderConfiguration.XmlImpl
{
    public class OpcSensorInfoImpl : 
        IOpcSensorInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string EnableTag { get; set; }
        public string CurrentValueTag { get; set; }
        public string ValuesListTag { get; set; }        
    }
}

using System.Collections.Generic;

namespace Alvasoft.DataProviderConfiguration.XmlImpl
{
    public class OpcSensorInfoImpl : 
        IOpcSensorInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int GetId()
        {
            return Id;
        }

        public string GetSensorName()
        {
            return Name;
        }
    }
}

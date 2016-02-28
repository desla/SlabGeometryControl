using System;
using Alvasoft.DataEnums;

namespace Alvasoft.SensorConfiguration.XmlImpl
{
    public class SensorInfoImpl :
        ISensorInfo
    {
        private int id;
        private string name;
        private SensorType sType;
        private SensorSide side;
        private double shift;

        public SensorInfoImpl(int aId, string aName, SensorType aSensorType, SensorSide aSide, double aShift)
        {
            if (string.IsNullOrEmpty(aName)) {
                throw new ArgumentNullException("aName");
            }

            id = aId;
            name = aName;
            sType = aSensorType;
            side = aSide;
            shift = aShift;
        }

        public int GetId()
        {
            return id;
        }

        public string GetName()
        {
            return name;
        }

        public SensorType GetSensorType()
        {
            return sType;
        }

        public SensorSide GetSensorSide()
        {
            return side;
        }

        public double GetShift()
        {
            return shift;
        }
    }
}

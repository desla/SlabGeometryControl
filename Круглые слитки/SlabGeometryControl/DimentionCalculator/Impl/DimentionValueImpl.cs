using Alvasoft.DimentionValueContainer;

namespace Alvasoft.DimentionCalculator.Impl
{
    public class DimentionValueImpl : 
        IDimentionValue
    {
        private int dimentionId;
        private double value;
        private int slabId;

        public DimentionValueImpl(int aDimentionId, double aValue)
        {
            dimentionId = aDimentionId;
            value = aValue;
            slabId = -1;
        }

        public int GetDimentionId()
        {
            return dimentionId;
        }

        public double GetValue()
        {
            return value;
        }

        public int GetSlabId()
        {
            return slabId;
        }
    }
}

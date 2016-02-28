namespace Alvasoft.DataWriter
{
    public class RegulationImpl : IRegulation
    {
        public int Id { get; set; }
        public int DimentionId { get; set; }
        public int StandartSizeId { get; set; }
        public double MaxValue { get; set; }
        public double MinValue { get; set; }

        public int GetId()
        {
            return Id;
        }

        public int GetDimentionId()
        {
            return DimentionId;
        }

        public int GetStandartSizeId()
        {
            return StandartSizeId;
        }

        public double GetMaxValue()
        {
            return MaxValue;
        }

        public double GetMinValue()
        {
            return MinValue;
        }
    }
}

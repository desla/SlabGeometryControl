namespace Alvasoft.DataWriter
{
    public class SlabInfoImpl : ISlabInfo
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public int StandartSizeId { get; set; }
        public long StartScanTime { get; set; }
        public long EndScanTime { get; set; }

        public int GetId()
        {
            return Id;
        }

        public string GetNumber()
        {
            return Number;
        }

        public int GetStandartSizeId()
        {
            return StandartSizeId;
        }

        public long GetStartScanTime()
        {
            return StartScanTime;
        }

        public long GetEndScanTime()
        {
            return EndScanTime;
        }
    }
}

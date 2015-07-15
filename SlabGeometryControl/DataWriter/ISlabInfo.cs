namespace Alvasoft.DataWriter
{
    public interface ISlabInfo
    {
        int GetId();

        string GetNumber();

        int GetStandartSizeId();

        long GetStartScanTime();

        long GetEndScanTime();
    }
}

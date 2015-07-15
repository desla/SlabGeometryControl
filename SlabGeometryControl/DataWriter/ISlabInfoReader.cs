namespace Alvasoft.DataWriter
{
    public interface ISlabInfoReader
    {
        ISlabInfo GetSlabInfo(string aSlabNumber);

        ISlabInfo GetSlabInfo(int aSlabId);

        ISlabInfo[] GetSlabInfoByTimeInterval(long aFrom, long aTo);
    }
}

namespace Alvasoft.DimentionConfiguration
{
    public class DimentionInfoImpl : 
        IDimentionInfo
    {
        private int id;
        private string name;
        private string description;

        public DimentionInfoImpl(int aId, string aName, string aDescription)
        {
            id = aId;
            name = aName;
            description = aDescription;
        }

        public int GetId()
        {
            return id;
        }

        public string GetName()
        {
            return name;
        }

        public string GetDescription()
        {
            return description;
        }
    }
}

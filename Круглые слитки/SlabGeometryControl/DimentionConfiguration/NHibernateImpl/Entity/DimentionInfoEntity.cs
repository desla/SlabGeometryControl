namespace Alvasoft.DimentionConfiguration.NHibernateImpl.Entity
{
    public class DimentionInfoEntity
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        
        public DimentionInfoEntity()
        {
            Id = -1;
            Name = "";
            Description = "";            
        }

        public virtual IDimentionInfo ToDimentionInfo()
        {
            return new DimentionInfoImpl(Id, Name, Description);
        }
    }
}

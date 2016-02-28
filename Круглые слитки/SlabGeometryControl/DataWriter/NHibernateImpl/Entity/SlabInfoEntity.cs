namespace Alvasoft.DataWriter.NHibernateImpl.Entity
{
    public class SlabInfoEntity
    {
        public virtual int Id { get; set; }
        public virtual string Number { get; set; }
        public virtual int StandartSizeId { get; set; }        
        public virtual long StartScanTime { get; set; }
        public virtual long EndScanTime { get; set; }        

        public SlabInfoEntity()
        {
        }
    }
}

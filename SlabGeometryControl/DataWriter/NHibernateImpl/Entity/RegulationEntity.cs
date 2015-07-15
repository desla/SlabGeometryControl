namespace Alvasoft.DataWriter.NHibernateImpl.Entity
{
    public class RegulationEntity
    {
        public virtual int Id { get; set; }
        public virtual int DimentionId { get; set; }
        public virtual int StandartSizeId { get; set; }
        public virtual double MaxValue { get; set; }
        public virtual double MinValue { get; set; }
    }
}

namespace Alvasoft.DataWriter.NHibernateImpl.Entity
{
    public class StandartSizeEntity
    {
        public virtual int Id { get; set; }
        public virtual double Width { get; set; }
        public virtual double Height { get; set; }
        public virtual double Length { get; set; }

        public StandartSizeEntity()
        {
        }
    }
}

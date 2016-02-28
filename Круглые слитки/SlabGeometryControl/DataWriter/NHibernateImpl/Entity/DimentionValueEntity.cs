using Alvasoft.DimentionValueContainer;

namespace Alvasoft.DataWriter.NHibernateImpl.Entity
{
    public class DimentionValueEntity
    {
        public virtual int Id { get; set; }
        public virtual int SlabId { get; set; }
        public virtual int DimentionId { get; set; }
        public virtual double Value { get; set; }

        public DimentionValueEntity(IDimentionValue aValue)
        {
            SlabId = -1;
            DimentionId = aValue.GetDimentionId();
            Value = aValue.GetValue();
        }

        public DimentionValueEntity()
        {
        }
    }
}

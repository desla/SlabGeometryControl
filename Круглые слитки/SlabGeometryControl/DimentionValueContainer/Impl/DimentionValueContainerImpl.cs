using System.Collections.Generic;

namespace Alvasoft.DimentionValueContainer.Impl
{
    public class DimentionValueContainerImpl
        : IDimentionValueContainer
    {
        private List<IDimentionValue> container = new List<IDimentionValue>();
        private object accessLock = new object();

        public IDimentionValue[] GetDimentionValues()
        {
            lock (accessLock) {
                return container.ToArray();
            }            
        }

        public void AddDimentionValue(IDimentionValue aDimentionValue)
        {
            lock (accessLock) {
                container.Add(aDimentionValue);
            }
        }

        public void Clear()
        {
            lock (accessLock) {
                container.Clear();
            }
        }

        public bool IsEmpty()
        {
            lock (accessLock) {
                return container.Count == 0;
            }
        }
    }
}

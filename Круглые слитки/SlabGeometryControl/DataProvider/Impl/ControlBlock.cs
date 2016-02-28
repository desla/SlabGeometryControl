using Alvasoft.Utils.Activity;
using OpcTagAccessProvider;

namespace Alvasoft.DataProvider.Impl
{
    public class ControlBlock : InitializableImpl
    {
        public OpcValueImpl MaxSize { get; set; }

        public OpcValueImpl StartIndex { get; set; }

        public OpcValueImpl EndIndex { get; set; }

        public OpcValueImpl[] Times { get; set; }

        public OpcValueImpl TimeSyncActivator { get; set; }

        public OpcValueImpl TimeForSync { get; set; }

        public OpcValueImpl ResetToZeroItem { get; set; }

        public int DataBlocksCount { get; set; }

        public int DataBlockSize { get; set; }

        protected override void DoInitialize()
        {
            MaxSize.Activate();
            StartIndex.Activate();
            EndIndex.Activate();            
            TimeSyncActivator.Activate();
            TimeForSync.Activate();
            ResetToZeroItem.Activate();
            foreach (var timeBlock in Times) {
                timeBlock.Activate();
            }
        }

        protected override void DoUninitialize()
        {
            MaxSize.Deactivate();
            StartIndex.Deactivate();
            EndIndex.Deactivate();            
            TimeSyncActivator.Deactivate();
            TimeForSync.Deactivate();
            ResetToZeroItem.Deactivate();
            foreach (var timeBlock in Times) {
                timeBlock.Deactivate();
            }
        }
    }
}

using Alvasoft.Utils.Activity;
using OpcTagAccessProvider;

namespace Alvasoft.DataProvider.Impl
{
    public class ControlBlock : InitializableImpl
    {
        public OpcValueImpl MaxSize { get; set; }

        public OpcValueImpl StartIndex { get; set; }

        public OpcValueImpl EndIndex { get; set; }

        public OpcValueImpl Times { get; set; }

        public OpcValueImpl TimeSyncActivator { get; set; }

        public OpcValueImpl TimeForSync { get; set; }

        protected override void DoInitialize()
        {
            MaxSize.Activate();
            StartIndex.Activate();
            EndIndex.Activate();
            Times.Activate();
            TimeSyncActivator.Activate();
            TimeForSync.Activate();
        }

        protected override void DoUninitialize()
        {
            MaxSize.Deactivate();
            StartIndex.Deactivate();
            EndIndex.Deactivate();
            Times.Deactivate();
            TimeSyncActivator.Deactivate();
            TimeForSync.Deactivate();
        }
    }
}

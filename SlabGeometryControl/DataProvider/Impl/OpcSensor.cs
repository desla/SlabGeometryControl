using Alvasoft.Utils.Activity;
using OpcTagAccessProvider;

namespace Alvasoft.DataProvider.Impl
{
    public class OpcSensor : InitializableImpl
    {        
        public OpcValueImpl Enable { get; set; }

        public OpcValueImpl CurrentValue { get; set; }

        public OpcValueImpl ValuesList { get; set; }

        protected override void DoInitialize()
        {
            Enable.Activate();
            CurrentValue.Activate();
            ValuesList.Activate();
        }

        protected override void DoUninitialize()
        {
            Enable.Deactivate();
            CurrentValue.Deactivate();
            ValuesList.Deactivate();
        }
    }
}

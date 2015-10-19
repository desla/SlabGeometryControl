using Alvasoft.Utils.Activity;
using OpcTagAccessProvider;

namespace Alvasoft.DataProvider.Impl
{
    using System;

    public class OpcSensor : InitializableImpl
    {
        private const int VALUES_COUT = 2000;

        public OpcValueImpl Enable { get; set; }

        public OpcValueImpl CurrentValue { get; set; }

        public OpcValueImpl ValuesList { get; set; }

        public void ResetValues()
        {
            try {
                ValuesList.WriteValue(new double[VALUES_COUT]);
            }
            catch {
                Console.WriteLine("Ошибка при обнулении значений.");
            }
        }

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

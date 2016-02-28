using Alvasoft.Utils.Activity;
using OpcTagAccessProvider;

namespace Alvasoft.DataProvider.Impl
{
    using System;

    public class OpcSensor : InitializableImpl
    {
        public int dataBlockSize = 100;

        public OpcValueImpl Enable { get; set; }

        public OpcValueImpl CurrentValue { get; set; }

        public OpcValueImpl[] DataBlocks { get; set; }

        public void ResetValues()
        {
            try {
                foreach (var dataBlock in DataBlocks) {
                    dataBlock.WriteValue(new double[dataBlockSize]);
                }
            }
            catch {
                Console.WriteLine("Ошибка при обнулении значений.");
            }
        }

        protected override void DoInitialize()
        {
            Enable.Activate();
            CurrentValue.Activate();
            foreach (var dataBlock in DataBlocks) {
                dataBlock.Activate();
            }
        }

        protected override void DoUninitialize()
        {
            Enable.Deactivate();
            CurrentValue.Deactivate();
            foreach (var dataBlock in DataBlocks) {
                dataBlock.Deactivate();
            }
        }
    }
}

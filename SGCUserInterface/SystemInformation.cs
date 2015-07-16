using Alvasoft.SlabGeometryControl;

namespace SGCUserInterface
{
    public class SystemInformation
    {
        public ControllerConnectionState ControllerConnectionState { get; set; }

        public int SensorsCount { get; set; }

        public SGCSystemState SGCState { get; set; }
    }
}

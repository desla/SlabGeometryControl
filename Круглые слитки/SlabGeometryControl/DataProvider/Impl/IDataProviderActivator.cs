using OPCAutomation;

namespace Alvasoft.DataProvider.Impl
{
    public interface IDataProviderActivator
    {
        void SetOpcServer(OPCServer aOpcServer);

        void SetActivationTagName(string aTagName);

        void SetActivatorListener(IActivatorListener aActivatorListener);

        bool GetCurrentValue();
    }
}

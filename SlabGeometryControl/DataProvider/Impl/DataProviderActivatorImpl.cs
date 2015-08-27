using System;
using Alvasoft.Utils.Activity;
using OpcTagAccessProvider;
using OPCAutomation;

namespace Alvasoft.DataProvider.Impl
{
    public class DataProviderActivatorImpl 
        :InitializableImpl,
        IDataProviderActivator, 
        IOpcValueListener
    {
        private OPCServer server;
        private string tagName;
        private IActivatorListener listener;
        private OpcValueImpl activationTag;

        public void SetOpcServer(OPCServer aOpcServer)
        {
            server = aOpcServer;
        }

        public void SetActivationTagName(string aTagName)
        {
            tagName = aTagName;
        }

        public void SetActivatorListener(IActivatorListener aActivatorListener)
        {
            listener = aActivatorListener;
        }

        protected override void DoInitialize()
        {
            if (server == null || server.ServerState != (int) OPCServerState.OPCRunning) {
                throw new ArgumentException("OPC Server не подключен.");
            }

            if (string.IsNullOrEmpty(tagName)) {
                throw new ArgumentException("Имя тега активации не задано.");
            }

            if (listener == null) {
                throw new ArgumentException("Слушатель активатора для DataProvider не задан.");
            }

            activationTag = new OpcValueImpl(server, tagName);
            activationTag.IsListenValueChanging = true;
            activationTag.SubscribeToValueChange(this);
            activationTag.Activate();
        }

        protected override void DoUninitialize()
        {
            if (activationTag != null && activationTag.IsActive) {
                activationTag.Deactivate();
            }            
        }

        public void OnValueChanged(IOpcValue aOpcValue, OpcValueChangedEventArgs aEventArgs)
        {
            if (listener != null) {
                listener.OnActivationTagValueChanged((bool)aEventArgs.Value);
            }
        }

        public bool GetCurrentValue()
        {
            return Convert.ToBoolean(activationTag.ReadCurrentValue());
        }        
    }
}

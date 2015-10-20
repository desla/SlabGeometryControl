namespace Alvasoft.DataProviderConfiguration.XmlImpl
{
    public class OpcControlBlockImpl : IOpcControlBlock
    {
        public string ActivationTag { get; set; }
        public string DataMaxSizeTag { get; set; }
        public string StartIndexTag { get; set; }
        public string EndIndexTag { get; set; }
        public string TimesTag { get; set; }
        public string DateTimeSyncActivatorTag { get; set; }
        public string DateTimeForSyncTag { get; set; }
        public string ResetToZeroTag { get; set; }
    }
}

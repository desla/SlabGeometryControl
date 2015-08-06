namespace Alvasoft.DataProviderConfiguration
{
    /// <summary>
    /// Блок контрольной информации для OPC.
    /// </summary>
    public interface IOpcControlBlock
    {
        string ActivationTag { get; }

        string DataMaxSizeTag { get; }

        string StartIndexTag { get; }

        string EndIndexTag { get; }

        string TimesTag { get; }

        string DateTimeSyncActivatorTag { get; }

        string DateTimeForSyncTag { get; }
    }
}

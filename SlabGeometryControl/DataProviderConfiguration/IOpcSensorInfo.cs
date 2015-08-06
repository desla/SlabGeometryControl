namespace Alvasoft.DataProviderConfiguration
{
    /// <summary>
    /// Описание орс-датчика. Будет дополняться
    /// новыми тегами орс.
    /// </summary>
    public interface IOpcSensorInfo
    {
        int Id { get; }

        string Name { get; }

        string EnableTag { get; }

        string CurrentValueTag { get; }

        string ValuesListTag { get; }
    }
}

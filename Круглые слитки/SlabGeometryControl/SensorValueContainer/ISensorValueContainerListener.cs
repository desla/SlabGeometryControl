namespace Alvasoft.SensorValueContainer
{
    /// <summary>
    /// Подписчик на добавление данных в накопитель.
    /// </summary>
    public interface ISensorValueContainerListener
    {
        /// <summary>
        /// Возникает при добавлении данных в накопитель.
        /// </summary>
        /// <param name="aContainer">Накопитель.</param>
        void OnDataReceived(ISensorValueContainer aContainer);
    }
}

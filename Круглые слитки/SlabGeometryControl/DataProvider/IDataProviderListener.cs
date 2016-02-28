namespace Alvasoft.DataProvider
{
    /// <summary>
    /// Подписчик на изменение состояния у DataProvider'а.
    /// </summary>
    public interface IDataProviderListener
    {
        /// <summary>
        /// Возникает при изменении состояния у DataProvider'а.
        /// </summary>
        /// <param name="aDataProvider">DataProvider.</param>
        void OnStateChanged(IDataProvider aDataProvider);
    }
}

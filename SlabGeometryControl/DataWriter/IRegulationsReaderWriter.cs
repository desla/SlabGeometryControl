namespace Alvasoft.DataWriter
{
    /// <summary>
    /// Обеспечивает доступ к правилам параметров слитка.
    /// </summary>
    public interface IRegulationsReaderWriter
    {        
        /// <summary>
        /// Возвращает правила параметров слитка.
        /// </summary>
        /// <returns>Список параметров силтка.</returns>
        IRegulation[] GetRegulations();

        /// <summary>
        /// Добавляет новое правило.
        /// </summary>
        /// <param name="aRegulation">Правило.</param>
        /// <returns>Идентификатор нового правила.</returns>
        int CreateRegulation(IRegulation aRegulation);

        /// <summary>
        /// Удаляет правило.
        /// </summary>
        /// <param name="aId">Идентификатор.</param>
        void RemoveRegulation(int aId);

        /// <summary>
        /// Изменяет правило.
        /// </summary>
        /// <param name="aRegulation">Правило.</param>
        void EditRegulation(IRegulation aRegulation);
    }
}

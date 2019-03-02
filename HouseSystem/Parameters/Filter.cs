namespace HouseSystem.Parameters
{
    /// <summary>
    /// Параметр фильтрации в GET запросах
    /// </summary>
    public class Filter
    {
        /// <summary>
        /// Поле по которому фильтруется максимальный элемент
        /// </summary>
        public string Max { get; set; } = "";

        /// <summary>
        /// Показывает что фильтр пустой
        /// </summary>
        /// <returns></returns>
        public virtual bool IsEmpty()
        {
            return string.IsNullOrEmpty(Max);
        }
    }
}
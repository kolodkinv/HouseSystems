using System;

namespace Monitoring.Exceptions
{
    /// <summary>
    /// Исключение возникающее когда объект мониторинга не найден
    /// </summary>
    public class NotFoundException: Exception
    {
        public NotFoundException(string message)
            : base(message)
        { }
    }
}
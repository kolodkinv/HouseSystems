using System;

namespace Monitoring.Exceptions
{
    /// <summary>
    /// Исключение возникающее при регистрации объектов с дублирующимися ключами
    /// </summary>
    public class DuplicateException : Exception
    {
        public DuplicateException(string message)
            : base(message)
        { }
    }
}
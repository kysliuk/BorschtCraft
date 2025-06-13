using System;

namespace BorschtCraft
{
    public class CannotDecorateException : Exception
    {
        public CannotDecorateException() { }
        public CannotDecorateException(string message) : base(message) { }
        public CannotDecorateException(string message, Exception inner) : base(message, inner) { }
    }
}

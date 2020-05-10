using System;

namespace MarvelApp.Exceptions
{
    public class NotFoundException : Exception 
    {
        public NotFoundException() : base("Item não encontrado.")
        {
        }

        public NotFoundException(string message) : base(message)
        {
        }
    }
}

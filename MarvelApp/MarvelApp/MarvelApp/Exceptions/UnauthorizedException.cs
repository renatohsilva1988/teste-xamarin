using System;
using System.Collections.Generic;
using System.Text;

namespace MarvelApp.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException() : base("Não autorizado.")
        {
        }

        public UnauthorizedException(string message) : base(message)
        {
        }

        public UnauthorizedException(string message, Exception ex) : base(message, ex)
        {
        }
    }
}

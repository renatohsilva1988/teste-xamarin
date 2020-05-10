using System;

namespace MarvelApp.Exceptions
{
    public class TokenNotFoundException : Exception
    {
        public TokenNotFoundException() : base("Token não foi encontrado.")
        {
        }
    }
}

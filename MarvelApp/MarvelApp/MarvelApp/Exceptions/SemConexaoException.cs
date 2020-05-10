using System;

namespace MarvelApp.Exceptions
{
    public class SemConexaoException : Exception
    {
        public SemConexaoException() : base("Você não possui Internet.")
        {
        }
    }
}

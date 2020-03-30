using System;

namespace Buildings.Domain.Exceptions
{
    public class BuildingException : Exception
    {
        public BuildingException(string message = null) : base(message) {}
    }
}
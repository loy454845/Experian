using System;
namespace ExperianTechTest.Exceptions
{
    public class FileInvalidException : Exception
    {
        public FileInvalidException() : base("Uploaded file is invalid.")
        {
        }
    }
}


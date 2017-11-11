using System;
using System.Runtime.Serialization;

namespace MUTDOD.Common.ModuleBase.Storage.Exceptions
{
    [Serializable]
    public class DatabaseAlreadyExistException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public DatabaseAlreadyExistException()
        {
        }

        public DatabaseAlreadyExistException(string message) : base(message)
        {
        }

        public DatabaseAlreadyExistException(string message, Exception inner) : base(message, inner)
        {
        }

        protected DatabaseAlreadyExistException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
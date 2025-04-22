using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Business.Validations
{
    public class MarkTestException : Exception
    {
        public MarkTestException()
        {
        }

        public MarkTestException(string message) : base(message)
        {
        }

        public MarkTestException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MarkTestException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}

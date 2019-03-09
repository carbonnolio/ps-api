using System;
using System.Net;

namespace PetStore.Api.Exceptions
{
    public class CustomHttpException : Exception
    {
        public readonly HttpStatusCode StatusCode;

        public CustomHttpException(HttpStatusCode statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}

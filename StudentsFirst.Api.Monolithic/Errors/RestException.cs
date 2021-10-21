using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace StudentsFirst.Api.Monolithic.Errors
{
    public class RestException : Exception
    {
        public RestException(HttpStatusCode code, ProblemDetails details)
        {
            Code = code;
            Details = details;
        }

        public HttpStatusCode Code { get; }
        public ProblemDetails Details { get; }
    }
}

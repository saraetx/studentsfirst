using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace StudentsFirst.Api.Monolithic.Errors
{
    public class NotFoundRestException : RestException
    {
        public NotFoundRestException(string? entityName = null)
            : base(HttpStatusCode.NotFound, new ProblemDetails()
            {
                Title = "Not found.",
                Detail = entityName == null ? "Not found." : $"The specified {entityName} was not found"
            }) { }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using StudentsFirst.Api.Monolithic.Errors;

namespace StudentsFirst.Api.Monolithic.Infrastructure.Errors
{
    public class ValidationPipelineBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public async Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next
        )
        {
            IDictionary<string, string[]> errors = _validators
                .Select(v => v.Validate(request))
                .Where(v => !v.IsValid)
                .SelectMany(v => v.Errors)
                .ToDictionary(e => e.PropertyName, e => new string[] { e.ErrorMessage });
            
            if (errors.Count > 0)
            {
                throw new RestException(HttpStatusCode.BadRequest, new ValidationProblemDetails(errors));
            }

            return await next();
        }
    }
}

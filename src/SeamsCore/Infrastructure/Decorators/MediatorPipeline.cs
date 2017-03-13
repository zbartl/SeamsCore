using MediatR;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SeamsCore.Infrastructure.Decorators
{
    public class MediatorPipeline<TRequest, TResponse>
      : IAsyncRequestHandler<TRequest, TResponse>
      where TRequest : IRequest<TResponse>
    {
        private readonly IAsyncRequestHandler<TRequest, TResponse> _inner;
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public MediatorPipeline(IAsyncRequestHandler<TRequest, TResponse> inner,
            IEnumerable<IValidator<TRequest>> validators)
        {
            _inner = inner;
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest message)
        {
            var failures = _validators
                .Select(v => v.Validate(message))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .ToList();
            if (failures.Any())
            {
                throw new ValidationException(failures);
            }

            return await _inner.Handle(message);
        }
    }
}

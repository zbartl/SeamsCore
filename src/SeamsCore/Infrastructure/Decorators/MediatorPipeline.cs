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
      : IRequestHandler<TRequest, TResponse>
      where TRequest : IRequest<TResponse>
    {
        private readonly IRequestHandler<TRequest, TResponse> _inner;
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public MediatorPipeline(IRequestHandler<TRequest, TResponse> inner,
            IEnumerable<IValidator<TRequest>> validators)
        {
            _inner = inner;
            _validators = validators;
        }

        public TResponse Handle(TRequest message)
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

            return _inner.Handle(message);
    }
    }
}

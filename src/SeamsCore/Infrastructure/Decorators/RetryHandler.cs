using MediatR;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Polly;
using Microsoft.Extensions.Logging;

namespace SeamsCore.Infrastructure.Decorators
{
    public class RetryHandler<TRequest, TResponse>
      : IAsyncRequestHandler<TRequest, TResponse>
      where TRequest : IAsyncRequest<TResponse>
    {
        private readonly IAsyncRequestHandler<TRequest, TResponse> _inner;
        private readonly ILogger<RetryHandler<TRequest, TResponse>> _logger;

        public RetryHandler(IAsyncRequestHandler<TRequest, TResponse> inner,
            ILogger<RetryHandler<TRequest, TResponse>> logger)
        {
            _inner = inner;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest message)
        {
            TResponse response = default(TResponse);
            var divideByZeroPolicy =
                Policy
                    .Handle<DivideByZeroException>()
                    .RetryAsync(3, (exception, retryCount) =>
                    {
                        _logger.LogError("Tried to divide by zero {0} times!", retryCount);
                    });

            await divideByZeroPolicy.ExecuteAsync(async () =>
             {
                 response = await _inner.Handle(message);
             });

            return response;
        }
    }
}

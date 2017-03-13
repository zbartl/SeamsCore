using AutoMapper;
using MediatR;
using SeamsCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SeamsCore.Features.Home
{
    public class Test
    {
        public class Query : IRequest<Result>
        {
            public int Divisor { get; set; }
        }

        public class Result
        {
            public string Data { get; set; }
        }
        
        public class Handler : IAsyncRequestHandler<Query, Result>
        {
            public async Task<Result> Handle(Query message)
            {
                var someDivision = (10 / message.Divisor).ToString();
                var result = new Result { Data = someDivision };
                return result;
            }
        }
    }
}

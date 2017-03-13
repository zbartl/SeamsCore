using AutoMapper;
using MediatR;
using SeamsCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using SeamsCore.Domain;

namespace SeamsCore.Features.Account
{
    public class Logoff
    {
        public class Command : IRequest
        {
        }

        public class Handler : IAsyncRequestHandler<Command>
        {
            private readonly UserManager<User> _userManager;
            private readonly SignInManager<User> _signInManager;

            public Handler(UserManager<User> userManager, SignInManager<User> signInManager)
            {
                _userManager = userManager;
                _signInManager = signInManager;
            }

            public async Task Handle(Command message)
            {
                await _signInManager.SignOutAsync();
            }
        }
    }
}

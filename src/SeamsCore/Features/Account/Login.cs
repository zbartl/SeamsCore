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
    public class Login
    {
        public class Command : IAsyncRequest<SignInResult>
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public class Handler : IAsyncRequestHandler<Command, SignInResult>
        {
            private readonly SignInManager<User> _signInManager;

            public Handler(SignInManager<User> signInManager)
            {
                _signInManager = signInManager;
            }

            public async Task<SignInResult> Handle(Command message)
            {
                return await _signInManager.PasswordSignInAsync(message.Email, message.Password, message.RememberMe, lockoutOnFailure: false);
            }
        }
    }
}

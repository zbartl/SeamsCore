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
    public class ForgotPassword
    {
        public class Command : IAsyncRequest
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public class Handler : AsyncRequestHandler<Command>
        {
            private readonly UserManager<User> _userManager;

            public Handler(UserManager<User> userManager)
            {
                _userManager = userManager;
            }

            protected override async Task HandleCore(Command message)
            {
                var user = await _userManager.FindByNameAsync(message.Email);
                if (user != null && (await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
                    // Send an email with this link
                }
            }
        }
    }
}

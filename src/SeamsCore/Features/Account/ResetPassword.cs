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
    public class ResetPassword
    {
        public class Command : IAsyncRequest<IdentityResult>
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            public string Code { get; set; }
        }

        public class Handler : IAsyncRequestHandler<Command, IdentityResult>
        {
            private readonly UserManager<User> _userManager;

            public Handler(UserManager<User> userManager)
            {
                _userManager = userManager;
            }

            public async Task<IdentityResult> Handle(Command message)
            {
                var user = await _userManager.FindByNameAsync(message.Email);
                if (user == null)
                {
                    return null;
                }
                var result = await _userManager.ResetPasswordAsync(user, message.Code, message.Password);
                return result;
            }
        }
    }
}

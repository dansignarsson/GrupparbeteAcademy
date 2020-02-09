using GroupJoinMVC.Models.Data;
using GroupJoinMVC.Models.Entities;
using GroupJoinMVC.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroupJoinMVC.Models
{
    public class AccountService
    {
        UserManager<MyIdentityUser> userManager;
        SignInManager<MyIdentityUser> signInManager;
        readonly GroupJoinDBContext context;

        public AccountService(
            UserManager<MyIdentityUser> userManager,
            SignInManager<MyIdentityUser> signInManager,
            GroupJoinDBContext context
            )
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context;
        }


        public async Task<IdentityResult> TryRegisterAsync(AccountRegisterVM viewModel)
        {
            // Try to create a new user
            return await userManager.CreateAsync(
                new MyIdentityUser { UserName = viewModel.Username,
                                     FirstName = viewModel.FirstName,
                                     Email = viewModel.Username},
                viewModel.Password);
        }

        public async Task<SignInResult> TryLoginAsync(AccountLoginVM viewModel)
        {
            // Try to sign user
            return await signInManager.PasswordSignInAsync(
                viewModel.Username,
                viewModel.Password,
                isPersistent: false,
                lockoutOnFailure: false);
        }

        public async Task TryLogoutAsync()
        {
            await signInManager.SignOutAsync();
        }
        public AspNetUsers[] GetUserIdAndFNameFromDB(AccountLoginVM id)
        {
            return context.AspNetUsers
                   .Where(p => p.UserName.Contains(id.Username))
                   .Select(p => new AspNetUsers
                   {
                       Id = p.Id,
                       FirstName = p.FirstName                       
                   }).ToArray();        
        }

        public bool CheckAuthToken(string userId)
        {
            return context.AspNetUsers
                .Any(p => p.Id == userId);
        }

        internal AccountGetUserEmailVM GetUserEmail(UserId data)
        {
            return context.AspNetUsers
                 .Where(u => u.Id == data.UserID)
                 .Select(u => new AccountGetUserEmailVM
                 {
                     Email = u.Email
                 }).FirstOrDefault();
        }
    }
}

using GymManagementBLL.Services.Interface;
using GymManagementBLL.ViewModels.AccountViewModels;
using GymManangementDAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public ApplicationUser? ValidateUser(LoginViewModel loginViewModel)
        {
            var User = _userManager.FindByEmailAsync(loginViewModel.Email).Result;
            if (User is null)
                return null;

            var isPasswordValid = _userManager.CheckPasswordAsync(User, loginViewModel.Password).Result;
            return isPasswordValid ? User : null;


        }
    }
}

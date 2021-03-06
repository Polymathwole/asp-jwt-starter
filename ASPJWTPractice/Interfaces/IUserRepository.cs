﻿using System.Collections.Generic;
using System.Threading.Tasks;
using ASPJWTPractice.Db;
using ASPJWTPractice.Identity;
using ASPJWTPractice.Request;
using Microsoft.AspNetCore.Identity;

namespace ASPJWTPractice.Interfaces
{
    public interface IUserRepository: IRepository<User>
    {
        Task<IdentityResult> CreateNewUser(SignupUser user);
        Task<AppUser> FindIdentityUser(string username);
        List<AppUser> GetAllUsers();
        Task<IdentityResult> DeleteUser(string username);
        Task<IdentityResult> UpdateUser(AppUser user);
        Task<bool> CheckPassword(AppUser user, string password);
        User FindUser(AppUser appUser);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ASPJWTPractice.Identity;
using ASPJWTPractice.Db;
using ASPJWTPractice.Request;
using ASPJWTPractice.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ASPJWTPractice.Repositories
{
    public class UserRepository : EFRepository<User>, IUserRepository
    {
        private UserManager<AppUser> userManager;

        public UserRepository(UserManager<AppUser> um, AppDbContext dbContext) : base(dbContext)
        {
            userManager = um;
        }

        public async Task<IdentityResult> CreateNewUser(SignupUser newUser)
        {
            //userManager.PasswordHasher = new PasswordHasher();
            AppUser user = new AppUser
            {
                UserName = newUser.UserName.Trim(),
                Email = newUser.Email.Trim(),
                PhoneNumber = newUser.PhoneNumber.Trim(),
                /*FirstName = newUser.FirstName.Trim(),
                LastName = newUser.LastName.Trim(),
                DoB = newUser.DoB,
                Sex = newUser.Sex.ToUpper().Trim(),
                DateCreated = DateTime.Now*/
            };
            IdentityResult identityResult = await userManager.CreateAsync(user, newUser.Password);

            if (identityResult.Succeeded)
            {
                var signupuser = new User(newUser.FirstName.Trim(), newUser.LastName.Trim(), newUser.UserName.Trim(), newUser.DoB, newUser.Sex.ToUpper().Trim(), user.Id);
                _appDbContext.Users.Add(signupuser);
                await _appDbContext.SaveChangesAsync();
            }
            return identityResult;
        }

        public async Task<AppUser> FindIdentityUser(string username) => await userManager.FindByNameAsync(username);

        public User FindUser(AppUser appUser)
        {
            User user = _appDbContext.Users.SingleOrDefault(u => u.UserName == appUser.UserName);
            return user;
        }

        public async Task<IdentityResult> DeleteUser(string username)
        {
            IdentityResult result = null;
            AppUser user = await userManager.FindByNameAsync(username);

            if (user != null)
            {
                result = await userManager.DeleteAsync(user);
            }

            return result;
        }

        public List<AppUser> GetAllUsers() => userManager.Users.ToList();

        public async Task<User> GetByIdentId(string id)
        {
            return await _appDbContext.Users.FirstOrDefaultAsync(t => t.IdentityId == id);
        }

        public async Task<IdentityResult> UpdateUser(AppUser user) => await userManager.UpdateAsync(user);

        public async Task<bool> CheckPassword(AppUser user, string password)
        {
            return await userManager.CheckPasswordAsync(user, password);
        }
        
        public async Task<bool> HasValidRefreshToken(User user, string refreshToken)
        {
            //User usr = _appDbContext.Users.SingleOrDefault(us => us.UserName == user.UserName);
            User usr = user;
            await _appDbContext.Entry(usr).Collection(b => b.RefreshTokens).LoadAsync();
            return usr.RefreshTokens.Any(rt => rt.Token == refreshToken && rt.Active);
        }

        public async Task AddRefreshToken(User user, string token, int userId, string remoteIpAddress, double minutesToExpire = 15)
        {
            User usr = user;
            await _appDbContext.Entry(usr).Collection(b => b.RefreshTokens).LoadAsync();
            usr.RefreshTokens.Add(new RefreshToken(token, DateTime.Now.AddMinutes(minutesToExpire), userId, remoteIpAddress));
        }

        public void RemoveRefreshToken(User user, string refreshToken)
        {
            User usr = user;
            usr.RefreshTokens.Remove(usr.RefreshTokens.First(t => t.Token == refreshToken));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPJWTPractice.Db
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Sex { get; set; }
        public string IdentityId { get; set; }
        public DateTime DoB { get; set; }
        public string UserName { get; private set; }
        public bool isDeleted { get; set; }
        public bool isOnline { get; set; }
        private readonly List<RefreshToken> _refreshTokens = new List<RefreshToken>();
        public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();

        public User(string firstName, string lastName, string userName, DateTime dob, string sex, string identity)
        {
            FirstName = firstName;
            LastName = lastName;
            UserName = userName;
            DoB = dob;
            Sex = sex;
            isDeleted = false;
            IdentityId = identity;
        }
        public User()
        {

        }
        public bool HasValidRefreshToken(string refreshToken)
        {
            return _refreshTokens.Any(rt => rt.Token == refreshToken && rt.Active);
        }

        public void AddRefreshToken(string token, int userId, string remoteIpAddress, double minutesToExpire = 15)
        {
            _refreshTokens.Add(new RefreshToken(token, DateTime.Now.AddMinutes(minutesToExpire), userId, remoteIpAddress));
        }

        public void RemoveRefreshToken(string refreshToken)
        {
            _refreshTokens.Remove(_refreshTokens.First(t => t.Token == refreshToken));
        }
    }
}

using ASPJWTPractice.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace ASPJWTPractice.Controllers
{
    public class TokenFactory : ITokenFactory
    {
        public string GenerateToken(int size = 32)
        {
            byte[] tokenBytes = new byte[size];

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(tokenBytes);
                string toekn = Convert.ToBase64String(tokenBytes);
                return toekn;
            }
        }
    }
}

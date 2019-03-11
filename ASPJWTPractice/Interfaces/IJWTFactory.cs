using ASPJWTPractice.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPJWTPractice.Interfaces
{
    public interface IJWTFactory
    {
        Task<AccessToken> GenerateEncodedToken(string id, string userName);
    }
}

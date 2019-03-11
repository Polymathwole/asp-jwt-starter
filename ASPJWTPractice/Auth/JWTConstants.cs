using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPJWTPractice.Controllers
{
    public class JWTConstants
    {
        public static class JwtClaimIdentifiers
        {
            public const string ROL = "rol", ID = "id";
        }

        public static class JwtClaims
        {
            public const string APIACCESS = "api_access";
        }
    }
}

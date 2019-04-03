using ASPJWTPractice.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPJWTPractice.Response
{
    public class RefreshTokenResponse
    {
        public AccessToken AccessToken { get; }
        public string RefreshToken { get; }
        public IEnumerable<Error> Errors { get; }

        public RefreshTokenResponse(AccessToken newToken, string newRefresh)
        {
            AccessToken = newToken;
            RefreshToken = newRefresh;
        }

        public RefreshTokenResponse(IEnumerable<Error> errs)
        {
            Errors = errs;
        }
    }
}

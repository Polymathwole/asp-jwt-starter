using ASPJWTPractice.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPJWTPractice.Response
{
    public class LoginResponse
    {
        public AccessToken AccessToken { get; }
        public string RefreshToken { get; }
        public IEnumerable<Error> Errors { get; }

        public LoginResponse(AccessToken token, string refresh)
        {
            AccessToken = token;
            RefreshToken = refresh;
        }

        public LoginResponse(IEnumerable<Error> errs)
        {
            Errors = errs;
        }
    }
}

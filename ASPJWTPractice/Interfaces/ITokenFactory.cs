using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPJWTPractice.Interfaces
{
    public interface ITokenFactory
    {
        string GenerateToken(int size = 32);
    }
}

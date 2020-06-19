using System.Collections.Generic;
using System.Security.Claims;

namespace Livraria.Domain.Contracts.Identity
{
    public interface IUser
    {
        string Name { get; }
        string AuthenticationType { get; }
        bool IsAuthenticated();
        IEnumerable<Claim> GetClaimsIdentity();
        IEnumerable<string> GetUserAuthenticatedId();
    }
}

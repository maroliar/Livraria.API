using Livraria.Domain.Contracts.Identity;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Livraria.Infra.Data.Identity
{
    public class AspNetUser : IUser
    {
        private readonly IHttpContextAccessor _accessor;

        public AspNetUser(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public string Name => _accessor.HttpContext.User.Identity.Name;
        public string AuthenticationType => _accessor.HttpContext.User.Identity.AuthenticationType;

        public bool IsAuthenticated()
        {
            return _accessor.HttpContext.User.Identity.IsAuthenticated;
        }

        public IEnumerable<Claim> GetClaimsIdentity()
        {
            return _accessor.HttpContext.User.Claims;
        }

        public IEnumerable<string> GetUserAuthenticatedId()
        {
            return _accessor.HttpContext.User.Claims
                        .Where(c => c.Type == ClaimTypes.Sid)
                        .Select(c => c.Value)
                        .ToList();
        }

    }
}

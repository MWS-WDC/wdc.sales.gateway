using Wdc.Sales.Users.Api.Models;

namespace Wdc.Sales.Users.Api.Abstracts
{
    public interface ITokenService
    {
        Task<string> CreateToken(ApplicationUser user);
    }
}

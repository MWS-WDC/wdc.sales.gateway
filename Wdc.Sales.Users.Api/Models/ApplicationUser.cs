using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Wdc.Sales.Users.Api.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required, MaxLength(50)]
        public required string FirstName { get; init; }

        [Required, MaxLength(50)]
        public required string LastName { get; init; }
    }
}

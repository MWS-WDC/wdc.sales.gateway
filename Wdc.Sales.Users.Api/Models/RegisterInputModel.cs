using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Wdc.Sales.Users.Api.Models
{
    public class RegisterInputModel
    {
        [Required, MaxLength(50)]
        public required string FirstName { get; init; }

        [Required, MaxLength(50)]
        public required string LastName { get; init; }

        [Required, MaxLength(100)]
        public required string Username { get; init; }

        [Required, EmailAddress]
        public required string Email { get; init; }

        [Required, PasswordPropertyText(true), StringLength(256)]
        public required string Password { get; init; }
    }
}
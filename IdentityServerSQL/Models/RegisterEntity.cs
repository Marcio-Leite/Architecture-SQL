using System.ComponentModel.DataAnnotations;

namespace IdentityServerSQL.Models
{
    public class RegisterEntity
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "O {0} precisa ter ao menos {2} e no máximo {1} caracteres.",
            MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare(nameof(Password), ErrorMessage = "A senha e a confirmação não batem.")]
        public string ConfirmPassword { get; set; }

        [Required] [Display(Name = "Name")] public string Name { get; set; }

        [Required]
        [Display(Name = "LastName")]
        public string LastName { get; set; }


        public class LoginEntity
        {
            [Required] [EmailAddress] public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }
    }
}
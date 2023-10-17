using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.Dtos
{
    public class RegisterDto
    {
        [Required]
        public string DisplayName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        [RegularExpression("(?=^.{6,10}(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()-+}{&quot;:;'?/&gt;.&lt;,])(?!.*\\s)).*$",
          ErrorMessage = "password must have 1 uppercase , 1lowercase , 1 number, 1 non alphanumeric and at least 6 characters")]
        public string Password { get; set; }

    }
}

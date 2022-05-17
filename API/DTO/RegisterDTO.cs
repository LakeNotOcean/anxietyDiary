using System.ComponentModel.DataAnnotations;

namespace Api.DTO
{
    public class RegisterDTO
    {
        [Required]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"(\p{IsBasicLatin}|^[0-9]+){3,10}", ErrorMessage = "UserName is invalid")]
        public string UserName { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{5,60}$", ErrorMessage = "Password must be complex")]
        public string Password { get; set; }

        [RegularExpression(@"(^\p{IsCyrillic}+$)|(^\p{IsBasicLatin}+$)", ErrorMessage = "Invalid name format")]
        public string FirstName { get; set; }
        [RegularExpression(@"(^\p{IsCyrillic}+$)|(^\p{IsBasicLatin}+$)", ErrorMessage = "Invalid name format")]
        public string SecondName { get; set; }
    }
}
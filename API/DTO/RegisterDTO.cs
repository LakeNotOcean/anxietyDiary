using System.ComponentModel.DataAnnotations;

namespace Api.DTO
{
    public class RegisterDTO
    {
        [Required]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^([A-Za-z0-9]){3,10}$", ErrorMessage = "Неверное имя пользователя")]
        public string UserName { get; set; }
        [Required]
        [RegularExpression(@"^([\x00-\x7F]){5,60}$", ErrorMessage = "Некорректный формат пароля")]
        public string Password { get; set; }

        [RegularExpression(@"(^\p{IsCyrillic}+$)|(^\p{IsBasicLatin}+$)", ErrorMessage = "Некорректный формат имени")]
        public string FirstName { get; set; }
        [RegularExpression(@"(^\p{IsCyrillic}+$)|(^\p{IsBasicLatin}+$)", ErrorMessage = "Неккоретный формат имени")]
        public string SecondName { get; set; }
    }
}
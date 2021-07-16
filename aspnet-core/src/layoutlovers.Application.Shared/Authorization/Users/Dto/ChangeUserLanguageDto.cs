using System.ComponentModel.DataAnnotations;

namespace layoutlovers.Authorization.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}

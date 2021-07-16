using System.ComponentModel.DataAnnotations;

namespace layoutlovers.Localization.Dto
{
    public class CreateOrUpdateLanguageInput
    {
        [Required]
        public ApplicationLanguageEditDto Language { get; set; }
    }
}
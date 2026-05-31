using System.ComponentModel.DataAnnotations;

namespace Project_Tracker_C_.Dtos
{
    public class UpdateTitleDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;
    }
}

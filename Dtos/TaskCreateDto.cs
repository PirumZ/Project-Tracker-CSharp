using System.ComponentModel.DataAnnotations;

namespace Project_Tracker_C_.Dtos
{
    public class TaskCreateDto
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
    }
}

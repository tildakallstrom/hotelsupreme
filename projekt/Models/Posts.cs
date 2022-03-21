using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace projekt.Models
{
    public class Posts
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Text { get; set; }
        [Display(Name = "Image")]
        public string? ImageName { get; set; }
        [NotMapped]
        [Display(Name = "Image")]
        public IFormFile? ImageFile { get; set; }
        public string? Author { get; set; }
    }
}
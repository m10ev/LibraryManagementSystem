using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class Author
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(20)]
        public string LastName { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }
        [Required]
        [MaxLength(200)]
        public string Biography { get; set; }

        public string? ImageUrl { get; set; }

        public ICollection<Book>? Books { get; set; }
    }
}

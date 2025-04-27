using Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(55)]
        public string Title { get; set; }
        [Required]
        public Genre Genre { get; set; }
        [StringLength(13, MinimumLength = 10, ErrorMessage = "ISBN must be between 10 and 13 characters.")]
        public string ISBN { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime PublicationDate { get; set; }
        [Required]
        public int AuthorID { get; set; }

        [ForeignKey("AuthorID")]
        public Author? Author { get; set; }

        public ICollection<BorrowedBook>? BorrowedBooks { get; set; }
    }
}

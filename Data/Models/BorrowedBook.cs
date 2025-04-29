using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class BorrowedBook
    {
        [Key, Column(Order = 0)]
        public int BookID { get; set; }
        [Key, Column(Order = 1)]
        public int MemberID { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime BorrowDate { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DueDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ReturnDate { get; set; }

        [ForeignKey("BookID")]
        public Book? Book { get; set; }

        [ForeignKey("MemberID")]
        public Member? Member { get; set; }
    }
}

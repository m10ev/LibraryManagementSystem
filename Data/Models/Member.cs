using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class Member
    {
        [Key]
        public int Id{ get; set; }
        [Required]
        [MaxLength(20)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(20)]
        public string LastName { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime MembershipExpireDate { get; set; }
        [Required]
        [MaxLength(16)]
        public string PhoneNumber { get; set; }

        public ICollection<BorrowedBook>? BorrowedBooks { get; set; }
    }
}

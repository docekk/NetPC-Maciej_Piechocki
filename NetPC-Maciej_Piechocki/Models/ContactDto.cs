using System.ComponentModel.DataAnnotations;

namespace NetPC_Maciej_Piechocki.Models
{
    public class ContactDto
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
		[Required]
		public string PhoneNumber { get; set; } = string.Empty;
		[Required]
		public string Email { get; set; } = string.Empty;
		[Required]
		public string Password {  get; set; } = string.Empty;
	}
}

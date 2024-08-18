using System.ComponentModel.DataAnnotations;

namespace mini_crm.Models
{
    public class LegalPerson
    {
        [Key]
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string INN { get; set; }
        public string OGRN { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string Phone { get; set; }
    }
}

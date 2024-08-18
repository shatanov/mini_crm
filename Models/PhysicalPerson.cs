using System.ComponentModel.DataAnnotations;

namespace mini_crm.Models
{
    public class PhysicalPerson
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string PlaceOfWork { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}

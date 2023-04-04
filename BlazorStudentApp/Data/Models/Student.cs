using System.ComponentModel.DataAnnotations;

namespace BlazorStudentApp.Data.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required]
        [StringLength(16, ErrorMessage = "Name too long (16 character limit).")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Address { get; set; }
    }
}

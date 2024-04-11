using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyIKEA.Models
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeId { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [DisplayName("Employee Name")]
        [StringLength(65, ErrorMessage ="Name can only have max 65 characters")]
        public string EmployeeName { get; set; }

        public IList<DepartmentList>? DepartmentList { get; set; }
    }
}

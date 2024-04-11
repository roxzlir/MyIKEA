using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyIKEA.Models
{
    public class DepartmentList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DepartmentListId { get; set; }
        [Required]
        [ForeignKey("Employee")]
        public int FkEmployeeId { get; set; }
        [Required]
        [ForeignKey("Department")]
        public int FkDepartmentId { get; set; }
        public Employee? Employee { get; set; }
        public Department? Department { get; set; }
    }
}

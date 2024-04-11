using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyIKEA.Models
{
    public class Department
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DepartmentId { get; set; }
        [Required(ErrorMessage ="Name is required")]
        [DisplayName("Department Name")]
        [StringLength(35, ErrorMessage = "Department name can only have max 35 characters")]
        public string DepartmentName { get; set; }
        public IList<DepartmentList>? DepartmentList { get; set; }
    }
}

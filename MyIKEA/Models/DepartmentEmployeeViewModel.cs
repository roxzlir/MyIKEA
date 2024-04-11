namespace MyIKEA.Models
{
    public class DepartmentEmployeeViewModel
    {
        public string selectedDepartmentName { get; set; }
        public IEnumerable<EmployeeWithDepartment> Employees { get; set;}
        public IEnumerable<Department> Departments { get; set; }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyIKEA.Data;
using MyIKEA.Models;
using MyIKEA.Utility;

namespace MyIKEA.Controllers
{
    [Authorize(Roles = $"{SD.Role_Admin},{SD.Role_Employee}")] //** Här lägger vi för att spärra allt innehåll för alla utom rollen/rollerna vi valt
    public class DepartmentEmployeesController : Controller
    {
        string selectedDepartmentName = null;

        //Det är här vi lägger kontext för att få kontakt med vår databas
        private readonly ApplicationDbContext _context;

        public DepartmentEmployeesController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int? departmentId)
        {
            //Här lagarat vi all data som finns i databasen för Departments till var departments
            var departments = await _context.Departments.ToListAsync();

            //Här skapar vi en var query som tar in precis alla properties som finns i våra databaser från alla våra tabeller.
            var query = from dl in _context.DepartmentsList
                        join e in _context.Employees on dl.FkEmployeeId equals e.EmployeeId
                        join d in _context.Departments on dl.FkDepartmentId equals d.DepartmentId
                        select new {dl, e ,d};

            //skapa ett filter med departmentId om det finns
            //Här kör vi så om vi skulle få ett departmentId med skickat in till vår Index så vill vi sorterar om vår query till att gå efter det värdet
            if (departmentId.HasValue)
            {
                query = query.Where(x => x.d.DepartmentId == departmentId.Value);
            }


            //Här tar vi alltså och samlar all data som finns i vår query (datan från databaserna) och sparar ner det mot EmployeeWithDepartment och 
            //lagrar det i var employees. Så det som nu finns kopplat i var employees är alltså både EmployeeName och DepartmentName i list format.
            var employees = await query.Select(x => new EmployeeWithDepartment
            {
                EmployeeName = x.e.EmployeeName,
                DepartmentName = x.d.DepartmentName,
            }).ToListAsync();


            //Detta skapar vi för att kunna styra mer vad som syns OM man inte valt något departmentId än så när man kommer till första sidan för Index så vill vi inte displaya alla namn etc. 
            if (departmentId.HasValue)
            {
                var selectedDepartment = await _context.Departments.Where(d => d.DepartmentId == departmentId.Value).FirstOrDefaultAsync();
                if (selectedDepartment != null)
                {
                    selectedDepartmentName = selectedDepartment.DepartmentName;
                }
            }


            //För att kunna använda både ENDAST allt som finns i vå Department klass så skapade vi en DepartmentEmployeeViewModel. I den har vi 2 st Enumerable
            //listor, en heter Departments och en heter Employees. Så väljer vi här att lagra båda dem i var viewModel. Så denna viewModel innehåller nu:
            //Departments = Allt som finns i databasen för Departments
            //Employees = Alla EmployeeName + DepartmentName
            var viewModel = new DepartmentEmployeeViewModel
            {
                Departments = departments,
                Employees = employees,
                selectedDepartmentName = selectedDepartmentName
            };
            //och det är alltså den vi returnerar till vår View så vi kan jobba där med alla dem olika objekten.

            
            

            return View(viewModel);
        }
    }
}

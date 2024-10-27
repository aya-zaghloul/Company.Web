using AutoMapper;
using Company.Data.Models;
using Company.Service.Dto.Department.Dto;
using Company.Service.Dto.Employee.Dto;
using Company.Service.Interface;
using Company.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Company.Web.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IDepartmentService _departmentService;
        private readonly IMapper _mapper;

        public EmployeeController(IEmployeeService employeeService, IDepartmentService departmentService, IMapper mapper)
        {
            _employeeService = employeeService;
            _departmentService = departmentService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index(string searchInp)
        {
            if (string.IsNullOrEmpty(searchInp))
            {
                var emp = _employeeService.GetAll();
                //ViewBag.Message = "This Message From View Bag";
                //ViewData["TxtMessage"] = "This Message From View Data";
                ////TempData["TxtMessage2"] = "This Message From Temp Data";
                //TempData.Keep("TxtMessage2");
                return View(emp);
            }
            else
            {
                var emp = _employeeService.GetEmployeeByName(searchInp);
                return View(emp);

                
            }
            
            
        }

        [HttpGet]
        public IActionResult Create()
        {
            //TempData["TxtMessage2"] = "This Message From Temp Data";
            var department = _departmentService.GetAll();
            //ViewBag.Departments = department ?? new List<Department>();

            var departmentDtos = _mapper.Map<IEnumerable<DepartmentDto>>(department) ?? new List<DepartmentDto>();

            ViewBag.Departments = departmentDtos;

            //ViewBag.Departments = _departmentService.GetAll();

            return View();
        }
        //var department = _departmentService.GetAll();
        //ViewBag.Departments = department ?? new List<Department>();

        [HttpPost]
        public IActionResult Create(EmployeeDto employee)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _employeeService.Add(employee);
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("DepartmentError", "ValidationErrors");
                return View(employee);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("DepartmentError", ex.Message);
                return View(employee);
            }
        }
    }
}

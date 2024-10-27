using AutoMapper;
using Company.Data.Models;
using Company.Repository.Interface;
using Company.Service.Dto.Department.Dto;
using Company.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Company.Web.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;
        private readonly IMapper _mapper;

        public DepartmentController(IDepartmentService departmentService, IMapper mapper)
        {
            _departmentService = departmentService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var department = _departmentService.GetAll();
            return View(department);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

            [HttpPost]
        public IActionResult Create(DepartmentDto department)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _departmentService.Add(department);
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("DepartmentError", "ValidationErrors");
                return View(department);
            }
            catch (Exception ex) 
            {
                ModelState.AddModelError("DepartmentError", ex.Message);
                return View(department);
            }
        }

        public IActionResult Details(int? id, string viewname= "Details")
        {
            var department = _departmentService.GetById(id);
            if (department == null)
            {
                return NotFound();
            }
            return View(viewname, department);
            
        }

        //[HttpGet]
        //public IActionResult Update(int? id)
        //{

        //    return Details(id, "Update");
        //}

        //[HttpPost]
        //public IActionResult Update(int? id, DepartmentDto department)
        //{
        //    if (department.Id != id.Value)
        //    {
        //        return RedirectToAction("NotFoundPage", null, "Home");
        //    }
        //    _departmentService.Update(department);
        //    return RedirectToAction(nameof(Index));
        //}


        public IActionResult Delete(int? id)
        {
            var department = _departmentService.GetById(id);
            if (department is null)
            {
                return RedirectToAction("NotFoundPage", null, "Home");
            }
            //department.IsDeleted = true;
            //_departmentService.Update(department);//soft delete
             _departmentService.Delete(department); // hard delete
            return RedirectToAction(nameof(Index));
        }
    }
}

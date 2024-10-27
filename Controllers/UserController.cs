using Company.Data.Models;
using Company.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Company.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UserController> _logger;

        public UserController(UserManager<ApplicationUser> userManager, ILogger<UserController> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async  Task<IActionResult> Index(string searchInp)
        {
            List<ApplicationUser> users;
            if (string.IsNullOrEmpty(searchInp))

                users =await _userManager.Users.ToListAsync();
           
            else
            
               users = await _userManager.Users.
                   Where(users => users.NormalizedEmail.Trim().Contains(searchInp.Trim().ToUpper())).
                   ToListAsync();
               return View(users);
        }

        public async Task<IActionResult> Details(string? id, string viewname = "Details")
        {
            var Users = await _userManager.FindByIdAsync(id);
            if (Users == null)
            {
                return NotFound();
            }
            if (viewname == "Update")
            {
                var userModel = new UserUpdateViewModel
                { 
                    Id = Users.Id,
                    UserName = Users.UserName
                };
            }

            return View(viewname,Users);

        }

        [HttpGet]
        public async Task<IActionResult> Update(string? id)
        {

            return await Details(id, "Update");
        }

        [HttpPost]
        public async Task<IActionResult> Update(string? id, UserUpdateViewModel model)
        {
            if(id != model.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByIdAsync(id);
                    if (user is null)
                    {
                        return NotFound();
                    }
                    user.UserName = model.UserName;
                    user.NormalizedUserName = model.UserName.ToUpper();

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User Updated Successfully");
                        return RedirectToAction(nameof(Index));
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex.Message);
                }
            }
            
            return View(model);
        }

        public async Task<IActionResult> Delete(string? id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if(user is null)
                {
                    return NotFound();
                }
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                foreach(var item in result.Errors)
                {
                    //_logger.LogInformation(item.ErrorMessage);
                    _logger.LogError(item.Description);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            
            return RedirectToAction(nameof(Index));
        }
    }
}

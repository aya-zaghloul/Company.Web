using System.ComponentModel.DataAnnotations;

namespace Company.Web.Models
{
    public class ResetPasswordViewModel
    {
        //[RegularExpression(@"^(?.*[A-Z])(?.*[a-z])(?.*\d)(?.*[\W_].{8,}$)", ErrorMessage = "Password Must be At Least 8 Characters")]
        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is Required")]
        [Compare(nameof(Password), ErrorMessage = "Confirm Password Doesn't Match Password ")]
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Company.Web.Models
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage = "First Name is Required")]
        public string FirstName{ get; set; }

        [Required(ErrorMessage = "Last Name is Required")]
        public string LastName{ get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage ="Invalid Email Format")]
        public string Email{ get; set; }

        //[RegularExpression(@"^(?.*[A-Z])(?.*[a-z])(?.*\d)(?.*[\W_].{8,}$)", ErrorMessage = "Password Must be At Least 8 Characters")]
        [Required(ErrorMessage = "Password is Required")]
        public string Password{ get; set; }

        [Required(ErrorMessage = "Confirm Password is Required")]
        [Compare(nameof(Password), ErrorMessage = "Confirm Password Doesn't Match Password ")]
        public string ConfirmPassword{ get; set; }

        [Required(ErrorMessage = "IsActive is Required")]
        public bool IsActive { get; set; }
    }
}

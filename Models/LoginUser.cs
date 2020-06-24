using System.ComponentModel.DataAnnotations;

namespace ActivityCenter.Models
{
    public class LoginUser
    {
        [Required (ErrorMessage="Email is required.")]
        [EmailAddress (ErrorMessage="Please enter a valid Email address.")]
        public string LoginEmail {get;set;}

        [Required (ErrorMessage="Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be 8 characters or longer.")]
        [DataType("Password")]
        public string LoginPassword {get;set;}
    }
}
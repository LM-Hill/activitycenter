using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActivityCenter.Models
{
    public class User
    {
        [Key]
        public int UserId {get;set;}

        [Required]
        [MinLength(1,ErrorMessage="First name is required.")]
        // [StrongName]
        public string FirstName {get;set;}

        [Required]
        [MinLength(1,ErrorMessage="Last name is required.")]
        // [StrongName]
        public string LastName {get;set;}

        [Required]
        [EmailAddress]
        public string Email {get;set;}

        [Required]
        [MinLength(8,ErrorMessage="A password is required.")]
        [DataType("Password")]
        // [StrongPassword]
        public string Password {get;set;}

        [Required]
        [Compare("Password")]
        [DataType("Password")]
        [NotMapped]
        public string Confirm {get;set;}

        public DateTime CreatedAt {get;set;} = DateTime.Now;
        
        public DateTime UpdatedAt {get;set;} = DateTime.Now;

        public List<Jubilee> MyEvents {get;set;}
        public List<Outing> MyOutings {get;set;}
    }
}
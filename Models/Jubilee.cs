using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;   

namespace ActivityCenter.Models
{
    public class Jubilee
    {
        [Key]
        public int JubileeId {get;set;}

        [Required(ErrorMessage="Title is required.")]
        [MinLength(2, ErrorMessage="Title must be at least 2 characters.")]
        public string Title {get;set;}

        [Required(ErrorMessage="Date/Time is required.")]
        public DateTime JubileeTime {get;set;}

        public int Duration {get;set;}

        [Required]
        [MinLength(10,ErrorMessage="Description must be at least 10 characters.")]
        public string Description {get;set;}

        public int UserId {get;set;}
        public User Coordinator {get;set;}
        public List<Outing> Teammates {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;

    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace NewBeltExam.Models
{
    public class User
    {
        [Key]
        public int UserId {get;set;}
        [Required(ErrorMessage="First Name required")]
        public string FirstName {get;set;}
        [Required(ErrorMessage="Last Name required")]
        public string LastName {get;set;}
        [Required(ErrorMessage="Email is required")]
        [EmailAddress(ErrorMessage="Invalid Email")]
        public string Email {get;set;}
        [Required(ErrorMessage="Password is required")]
        [MinLength(8,ErrorMessage="Invalid Password")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", 
        ErrorMessage = "Password must have at least one capital letter and one special character")]
        public string Password {get;set;}
        [NotMapped]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string Confirm {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
        public List<Party> PlannedPartys {get;set;}
        public List<Join> AttendingPartys {get;set;}
    }
}
using System;
using System.ComponentModel.DataAnnotations;


namespace NewBeltExam.Models
{
    public class LoginUser
    {
        [Required(ErrorMessage="Email is Required")]
        [EmailAddress(ErrorMessage="Invalid Email")]
        public string LoginEmail {get;set;}
        [Required(ErrorMessage="Password is Required")]
        [MinLength(8,ErrorMessage="Password must be 8 characters or longer!")]
        public string LoginPassword {get;set;}
    }
}
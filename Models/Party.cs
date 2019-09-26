using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace NewBeltExam.Models
{
    public class Party
    {
        [Key]
        public int PartyId {get;set;}

        [Required(ErrorMessage="Activity is Required")]
        public string Title {get;set;}

        [Required(ErrorMessage="StartTime is Required")]
        public DateTime StartTime {get;set;}

        [Required(ErrorMessage="Length of Activity is Required")]
        public int Duration {get;set;}

        [Required(ErrorMessage="Description of Activity is Required")]
        public string Description {get;set;}

        public int PlannerId {get;set;}
        public User Planner {get;set;}
        public List<Join> AttendingUsers {get;set;}
        
    }
}
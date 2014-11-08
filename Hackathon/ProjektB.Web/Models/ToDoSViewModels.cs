using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.EnterpriseServices;
using System.Linq;
using System.Web;

namespace ProjektB.Web.Models
{
    public class ToDoViewModel
    {
        public int ItemNumber { get; set; }
        [Required]
        [Display(Name="Item Value")]
        public string ItemValue { get; set; }
    }
    
    public class ToDosViewModel
    {
        [Required]
        public List<ToDoViewModel> Items { get; set; }
    }
}
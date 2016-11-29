using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DMS.Models
{
    public class Role
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Роль")]
        public string Name { get; set; }
    }
}
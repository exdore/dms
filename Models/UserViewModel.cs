using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DMS.Models
{
    public class UserViewModel
    {
        public IEnumerable<User> Users { get; set; }
        public SelectList Departments { get; set; }
        public SelectList Roles { get; set; }
    }
}
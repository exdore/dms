using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DMS.Models
{
    public class UserIndexViewModel
    {
        public UserViewModel Users { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}
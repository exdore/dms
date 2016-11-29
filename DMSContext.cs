using DMS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DMS
{
    public class DMSContext: DbContext
    {
        public DMSContext()
            : base("DMSContext")
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Class> Classes { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Document> Documents { get; set; }
    }
}
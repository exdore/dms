using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DMS.Models
{
    public class DocumentViewModel
    {
        public IEnumerable<Document> Documents { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExecutionDate { get; set; }
        public SelectList Classes { get; set; }
        public SelectList Executors { get; set; }
    }
}
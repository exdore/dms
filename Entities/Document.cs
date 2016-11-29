using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DMS.Models
{
    public class Document
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Номер документа")]
        public string Number { get; set; }
        [Display(Name = "Тип документа")]
        public int ClassId { get; set; }
        [Display(Name = "Тип документа")]
        public Class Class { get; set; }
        [Display(Name = "Исполнитель")]
        public int? ExecutorId { get; set; }
        [Display(Name = "Исполнитель")]
        public User Executor { get; set; }
        [Display(Name = "Создатель")]
        public int? UserId { get; set; }
        [Display(Name = "Создатель")]
        public User User { get; set; }
        [Required]
        [Display(Name = "Дата создания")]
        public DateTime? CreationTime { get; set; }
        public DateTime? ExecutionTime { get; set; }
    }
}
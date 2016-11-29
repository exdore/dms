using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DMS.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "ФИО")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Логин")]
        public string Login { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
        [Display(Name = "Отдел")]
        public int DepartmentId { get; set; }
        [Display(Name = "Отдел")]
        public Department Department { get; set; }
        [Display(Name = "Роль")]
        public int RoleId { get; set; }
        [Display(Name = "Роль")]
        public Role Role { get; set; }
        [Required]
        [Display(Name = "Табельный номер")]
        public string PersonnelNumber { get; set; }
        [Display(Name = "Является руководителем отдела")]
        public bool IsManager { get; set; }
    }
}
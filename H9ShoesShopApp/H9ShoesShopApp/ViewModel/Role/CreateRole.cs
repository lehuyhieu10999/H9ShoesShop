
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace H9ShoesShopApp.ViewModel
{
    public class CreateRole
    {
        [Required]
        [Display(Name ="Tên quyền ")]
        public string RoleName { get; set; }
    }
}

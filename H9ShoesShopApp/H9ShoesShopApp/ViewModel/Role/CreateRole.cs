using System.ComponentModel.DataAnnotations;

namespace H9ShoesShopApp.ViewModel
{
	public class CreateRole
    {
        [Required]
        [Display(Name ="Tên quyền ")]
        public string RoleName { get; set; }
    }
}

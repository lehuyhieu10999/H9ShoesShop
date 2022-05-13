using System.ComponentModel.DataAnnotations;

namespace H9ShoesShopApp.ViewModel
{
	public class EditRole
    {
        public string RoleId { get; set; }
        [Required]
        [Display(Name = "Tên quyền")]
        public string RoleName { get; set; }
    }
}

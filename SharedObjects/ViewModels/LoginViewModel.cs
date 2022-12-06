using System;
using System.ComponentModel.DataAnnotations;

namespace SharedObjects.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Không để trống")]
        [Display(Name = "Nhập username hoặc email của bạn")]
        public string UserNameOrEmail { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }
        [Display(Name = "Nhớ thông tin đăng nhập?")]
        public bool RememberMe { get; set; }
    }
}

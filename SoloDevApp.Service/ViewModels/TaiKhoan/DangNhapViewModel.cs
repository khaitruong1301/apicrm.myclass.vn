using System.ComponentModel.DataAnnotations;

namespace SoloDevApp.Service.ViewModels
{
    public class DangNhapViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập email!")]
        [EmailAddress(ErrorMessage ="Email không đúng định dạng!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu!")]
        [MinLength(6, ErrorMessage = "Mật khẩu ít nhất {1} ký tự!")]
        public string MatKhau { get; set; }
    }
}
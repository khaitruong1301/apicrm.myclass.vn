using System.ComponentModel.DataAnnotations;

namespace SoloDevApp.Service.ViewModels
{
    public class DangKyViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string MatKhau { get; set; }

        [Compare("MatKhau")]
        public string NhapLaiMatKhau { get; set; }

        [Required]
        public string HoTen { get; set; }

        [Required]
        public string BiDanh { get; set; }

        [Required]
        [RegularExpression("^[0-9]+$")]
        public string SoDT { get; set; }
        public string Avatar { get; set; }
        public string MaNhomQuyen { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace SoloDevApp.Service.ViewModels
{
    public class DangNhapFacebookViewModel
    {
        public string Email { get; set; }

        [Required]
        public string FacebookId { get; set; }

        [Required]
        [EmailAddress]
        public string FacebookEmail { get; set; }
    }
}
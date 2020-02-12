using System.ComponentModel.DataAnnotations;

namespace SoloDevApp.Service.ViewModels
{
    public class FacebookViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [EmailAddress]
        public string FacebookEmail { get; set; }

        [Required]
        public string FacebookId { get; set; }

        [Required]
        public string Avatar { get; set; }
    }
}
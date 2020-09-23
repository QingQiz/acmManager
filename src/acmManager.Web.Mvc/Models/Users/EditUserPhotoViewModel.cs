using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace acmManager.Web.Models.Users
{
    public class EditUserPhotoViewModel
    {
        [Required]
        public IFormFile Photo { get; set; }
    }
}
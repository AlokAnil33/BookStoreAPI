using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace BookStore.API.DataModels
{
    public class SigninModel
    {
        [Required, EmailAddress]
        public string EMail { get; set; }
        [Required]
        public string Password { get; set; }
    }
}

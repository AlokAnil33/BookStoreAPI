using Microsoft.AspNetCore.Identity;

namespace BookStore.API.DataModels
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName {  get; set; }
        public string LastName { get; set; }
    }
}

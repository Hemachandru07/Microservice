using Microsoft.AspNetCore.Identity;

namespace MS.Services.AuthAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}

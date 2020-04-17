using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaveOnCloud.Web.Models
{
    public class ApplicationUser : IdentityUser
    {
        [NotMapped]
        public string Roles { get; set; }
    }
}

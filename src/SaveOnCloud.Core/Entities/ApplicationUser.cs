using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaveOnCloud.Core.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [NotMapped]
        public string Roles { get; set; }
    }
}

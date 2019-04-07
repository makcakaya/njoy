using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Njoy.Data
{
    public sealed class AppUser : IdentityUser<int>
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        // This Id is equal to the Id in the AspNetUser table and it's manually set.
        public override int Id { get; set; }
    }
}
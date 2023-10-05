using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GoogleAuth.MVC.Models
{
    public class GoogleAuthContext:IdentityDbContext
    {
        public GoogleAuthContext(DbContextOptions<GoogleAuthContext> options):base(options) { }
    }
}

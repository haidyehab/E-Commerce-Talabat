using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity
{
    public class AppicationIdentityDbContext :IdentityDbContext<AppUser>
    {
        public AppicationIdentityDbContext(DbContextOptions<AppicationIdentityDbContext> options):base(options)
        {
            
        }
    }
}

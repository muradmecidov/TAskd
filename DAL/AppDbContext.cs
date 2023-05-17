using Microsoft.EntityFrameworkCore;
using WebFrontToBack.Models;

namespace WebFrontToBack.DAL;

public class AppDbContext:DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
    {     
        
    }

    public DbSet<Slider> Sliders { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<ServiceImage> ServiceImages { get; set; }
    public DbSet<TeamMember> TeamMembers { get; set; }
    public DbSet<RecentWork> RecentWorks { get; set; }

}

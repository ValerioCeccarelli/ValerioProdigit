using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ValerioProdigit.Api.Models;

namespace ValerioProdigit.Api.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
{
    public DbSet<Reservation> Reservations { get; set; } = null!;
    public DbSet<Classroom> Classrooms { get; set; } = null!;
    public DbSet<Building> Buildings { get; set; } = null!;
    public DbSet<Row> Rows { get; set; } = null!;
    public DbSet<Lesson> Lessons { get; set; } = null!;

    public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
    {
		
    }
	
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Building>().HasAlternateKey(b => b.Code);
        modelBuilder.Entity<Classroom>().HasAlternateKey(c => new {c.Code, c.BuildingId});

        modelBuilder.Entity<Lesson>()
	        .HasAlternateKey(l => new {l.ClassroomId, l.TeacherId, l.StartHour, l.FinishHour});
        
        //for the Lesson model there is no check on the database level for the overlapping of the lessons
        //the check is done only in the application layer
        //this should be enough because the event of create a lesson is not a frequent operation
        
        //no duplicate users
        modelBuilder.Entity<Reservation>()
	        .HasAlternateKey(r => new {r.LessonId, r.StudentId});
        //no two user for the same seat
        modelBuilder.Entity<Reservation>()
	        .HasAlternateKey(r => new {r.LessonId, r.Seat, r.Row});
    }
}
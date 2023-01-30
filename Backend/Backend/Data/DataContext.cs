using Backend.Models;
using Microsoft.EntityFrameworkCore;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace Backend.Data;

public class DataContext : DbContext
{
    public DbSet<Admin> Admins { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<College> Colleges { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Recruiter> Recruiters { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<SendMessage> SendMessages { get; set; }
    public DbSet<DeliveredMessage> DeliveredMessages { get; set; }
    public DbSet<RecruiterCompany> RecruiterCompanies { get; set; }
    public DbSet<StudentCourse> StudentCourses { get; set; }
    
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RecruiterCompany>()
            .HasKey(pc => new { pc.RecruiterId, pc.CompanyId });
        modelBuilder.Entity<RecruiterCompany>()
            .HasOne(p => p.Recruiter)
            .WithMany(pc => pc.RecruiterCompanies)
            .HasForeignKey(c => c.RecruiterId);
        modelBuilder.Entity<RecruiterCompany>()
            .HasOne(p => p.Company)
            .WithMany(pc => pc.RecruiterCompanies)
            .HasForeignKey(c => c.CompanyId);
        
        modelBuilder.Entity<StudentCourse>()
            .HasKey(po => new { po.StudentId, po.CourseId });
        modelBuilder.Entity<StudentCourse>()
            .HasOne(p => p.Student)
            .WithMany(po => po.StudentCourses)
            .HasForeignKey(c => c.StudentId);
        modelBuilder.Entity<StudentCourse>()
            .HasOne(p => p.Course)
            .WithMany(po => po.StudentCourses)
            .HasForeignKey(o => o.CourseId);
    }
}
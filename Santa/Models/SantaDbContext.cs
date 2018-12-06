using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

public class SantaDbContext : DbContext
{
    public SantaDbContext(DbContextOptions options) : base(options)
    {
    }

    public SantaDbContext()
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Group>().HasOne(g => g.Owner).WithMany(p => p.OwnedGroups).IsRequired();
        modelBuilder.Entity<Group>().HasMany(g => g.Members).WithOne(m => m.Group).IsRequired();
        modelBuilder.Entity<Person>().HasMany(p => p.Groups).WithOne(m => m.Person).IsRequired();

        modelBuilder.Entity<Membership>().HasKey(m => new {m.GroupId, m.PersonId});
        // modelBuilder.Entity<Membership>().HasOne(m => m.Group).WithMany(g => g.Members).IsRequired();
        modelBuilder.Entity<Membership>().HasOne(m => m.Person).WithMany(p => p.Groups).IsRequired();
        modelBuilder.Entity<Membership>().HasOne(m => m.Giftee).WithMany().IsRequired(false);
    }

    public DbSet<Person> Persons { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Membership> Mappings { get; set; }
}
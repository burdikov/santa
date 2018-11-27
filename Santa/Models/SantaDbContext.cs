using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

public class SantaDbContext : DbContext
{
    public SantaDbContext(DbContextOptions options) : base(options)
    {
    }

    protected SantaDbContext()
    {
    }
}
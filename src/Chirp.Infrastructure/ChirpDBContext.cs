using Microsoft.EntityFrameworkCore;

public class ChirpDBContext : DbContext
{
	public DbSet<Author> Authors { get; set; }
	public DbSet<Cheep> Cheeps { get; set; }
	public ChirpDBContext(DbContextOptions<ChirpDBContext> options) : base(options)
	{

	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Author>()
			.HasMany(a => a.Followers)
			.WithMany(a => a.Following);

		modelBuilder.Entity<Author>()
			.HasMany(a => a.Cheeps)
			.WithOne(c => c.Author)
			.HasForeignKey(c => c.AuthorId)
			.OnDelete(DeleteBehavior.Restrict);
		
		modelBuilder.Entity<Author>()
			.HasMany(a => a.LikedCheeps)
			.WithMany(c => c.LikedBy)
			.UsingEntity(m => m.ToTable("AuthorLikedCheeps"));

		modelBuilder.Entity<Cheep>().ToTable("Cheeps");

		modelBuilder.Entity<Cheep>().Property(c => c.Text).HasMaxLength(160);

		modelBuilder.Entity<Author>().HasIndex(a => a.Name).IsUnique();
		modelBuilder.Entity<Author>().HasIndex(a => a.Email).IsUnique();
	}
}
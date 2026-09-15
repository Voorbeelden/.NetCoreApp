using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Calenderapp.MVC.Models;

// This is a trimmed version of the real DbContext, kept here to show the general
// setup: ASP.NET Identity combined with the app's own entities in one context, plus
// EF Core migrations to evolve the schema. Several DbSets (Kalender, Planning,
// Vakantie, Module) are part of the scheduling engine that isn't included in this
// portfolio excerpt — see the README for why.
//
// The original file also seeded a few Identity accounts (admin/directeur/secretariaat)
// via HasData() for local development. That block — including the password hash and
// security stamp EF generates for a seeded user — has been removed entirely here.
// Publishing a password hash, even for a throwaway dev account, is bad practice: it's
// crackable offline, and it's simply not something that belongs in a public repo.
public partial class CalenderAppContext(DbContextOptions<CalenderAppContext> options)
	: IdentityDbContext<IdentityUser, IdentityRole, string>(options)
{
	public virtual DbSet<Docent> Docenten { get; set; }
	public virtual DbSet<Lokaal> Lokalen { get; set; }
	public virtual DbSet<Opleiding> Opleidingen { get; set; }
	public virtual DbSet<Settings> Settings { get; set; }
	public virtual DbSet<School> School { get; set; }

	// Not included in this excerpt: Planningen, Kalenders, Modules, Vakanties.

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		=> optionsBuilder.UseSqlServer("Name=ConnectionStrings:LocalConnection");

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		// Role seeding (admin / directeur / secretariaat / docent) stays here since it
		// carries no secrets — just role names.
		modelBuilder.Entity<IdentityRole>().HasData(
			new IdentityRole { Id = "1", Name = "admin", NormalizedName = "ADMIN" },
			new IdentityRole { Id = "2", Name = "directeur", NormalizedName = "DIRECTEUR" },
			new IdentityRole { Id = "3", Name = "secretariaat", NormalizedName = "SECRETARIAAT" },
			new IdentityRole { Id = "4", Name = "docent", NormalizedName = "DOCENT" }
		);

		// User seeding removed — see the note above. In practice, seed a local admin
		// account through `dotnet user-secrets` / an environment-specific script that
		// never gets committed, not through a migration.

		base.OnModelCreating(modelBuilder);
	}
}

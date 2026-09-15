using Microsoft.EntityFrameworkCore;
using Calenderapp.MVC.Models;

namespace Calenderapp.MVC.Services;

/// <summary>
/// One place for the "set the audit fields, save, and expose active vs. soft-deleted
/// rows" logic that used to be duplicated across every entity's controller. Registered
/// per entity type in DI (see Program.cs) — e.g. AuditableEntityService&lt;Docent&gt;
/// and AuditableEntityService&lt;Lokaal&gt; are two separate, independently scoped
/// instances, both backed by the same generic implementation.
///
/// This is deliberately a thin data-access layer, not a full business-logic service —
/// entity-specific validation and form parsing still live in the controller, since
/// that's genuinely different per entity. What moves here is only the part that was
/// identical everywhere.
/// </summary>
public class AuditableEntityService<TEntity>(CalenderAppContext context)
	where TEntity : class, IAuditableEntity, new()
{
	private readonly CalenderAppContext _context = context;
	private readonly DbSet<TEntity> _set = context.Set<TEntity>();

	public IQueryable<TEntity> QueryActive() => _set.Where(e => e.VerwijderdDoor == null);

	public IQueryable<TEntity> QueryDeleted() => _set.Where(e => e.VerwijderdDoor != null);

	public async Task<TEntity?> GetByIdAsync(int id) => await _set.FindAsync(id);

	public async Task CreateAsync(TEntity entity, string user)
	{
		entity.AanmaakDatum = DateTime.Now;
		entity.AangemaaktDoor = user;
		_set.Add(entity);
		await _context.SaveChangesAsync();
	}

	public async Task UpdateAsync(TEntity entity, string user)
	{
		entity.UpdateDatum = DateTime.Now;
		entity.UpdatedDoor = user;
		_context.Update(entity);
		await _context.SaveChangesAsync();
	}

	/// <summary>Marks a row as deleted without removing it — the default "Delete" action everywhere in the app.</summary>
	public async Task SoftDeleteAsync(TEntity entity, string user)
	{
		entity.VerwijderDatum = DateTime.Now;
		entity.VerwijderdDoor = user;
		await _context.SaveChangesAsync();
	}

	/// <summary>Undoes a soft delete — the "Reset" action on the DeleteIndex screen.</summary>
	public async Task RestoreAsync(TEntity entity)
	{
		entity.VerwijderDatum = null;
		entity.VerwijderdDoor = null;
		await _context.SaveChangesAsync();
	}

	/// <summary>Actually removes the row — only reachable from DeleteIndex, admin-only in every controller that uses it.</summary>
	public async Task HardDeleteAsync(TEntity entity)
	{
		_set.Remove(entity);
		await _context.SaveChangesAsync();
	}
}

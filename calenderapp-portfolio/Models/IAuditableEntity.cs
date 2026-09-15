namespace Calenderapp.MVC.Models;

/// <summary>
/// Every entity in the app carries the same soft-delete + audit fields
/// (AanmaakDatum/AangemaaktDoor, UpdateDatum/UpdatedDoor, VerwijderDatum/VerwijderdDoor),
/// but originally each entity declared them independently with no shared contract —
/// which meant the CRUD logic that sets/reads them couldn't be written generically
/// and ended up copy-pasted per controller instead. Extracting this interface is what
/// makes <see cref="AuditableEntityService{TEntity}"/> and
/// <see cref="EntityCrudController{TEntity}"/> possible.
/// </summary>
public interface IAuditableEntity
{
	DateTime AanmaakDatum { get; set; }
	string AangemaaktDoor { get; set; }
	DateTime? UpdateDatum { get; set; }
	string? UpdatedDoor { get; set; }
	DateTime? VerwijderDatum { get; set; }
	string? VerwijderdDoor { get; set; }
}

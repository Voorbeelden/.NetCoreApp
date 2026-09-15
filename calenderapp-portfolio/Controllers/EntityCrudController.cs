using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Calenderapp.MVC.Models;
using Calenderapp.MVC.Models.ViewModels;
using Calenderapp.MVC.Services;
using static Calenderapp.MVC.Models.ViewModels.SRDViewModel;

namespace Calenderapp.MVC.Controllers;

/// <summary>
/// Shared base for the entity-CRUD controllers (Docent, Lokaal, School, Opleiding).
/// Originally each of these was its own ~370-line controller with an almost identical
/// Index/DeleteIndex/Details/GetIndex/GetView — same shape, different entity. That
/// duplication is what this base class removes: everything that's genuinely the same
/// per entity type lives here, and each concrete controller only supplies the small
/// amount that actually differs (which properties to show on the detail screen, which
/// Settings field controls its page size, and the Create/Edit form parsing, since form
/// fields are inherently entity-specific).
///
/// This intentionally does NOT try to also generate Create/Edit — those still read raw
/// IFormCollection values per entity, and forcing that into a generic shape would trade
/// one kind of duplication for a worse kind of indirection. Generalizing the read side
/// (list, sort, search, paginate, detail) was the part with a clear, safe abstraction;
/// the write side stays explicit.
/// </summary>
public abstract class EntityCrudController<TEntity>(
	AuditableEntityService<TEntity> service,
	ISearchEngine searchEngine,
	CalenderAppContext context) : Controller
	where TEntity : class, IAuditableEntity, new()
{
	protected readonly AuditableEntityService<TEntity> Service = service;
	protected readonly ISearchEngine SearchEngine = searchEngine;
	protected readonly CalenderAppContext Context = context;

	/// <summary>Used as the SRDViewModel "Controller" field and in the navigation title, e.g. "Docent".</summary>
	protected abstract string EntityName { get; }

	/// <summary>Which Settings field controls this entity's page size (e.g. settings => settings.PaginationDocent).</summary>
	protected abstract int GetPaginationSize(Settings settings);

	/// <summary>The label/name shown at the top of the detail/delete-confirmation screen.</summary>
	protected abstract string GetDisplayName(TEntity entity);

	/// <summary>The rows shown in the detail/delete-confirmation table.</summary>
	protected abstract List<AttributeInfo> BuildAttributes(TEntity entity);

	public virtual async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, string searchCategory, int? pageNumber)
	{
		ViewBag.Message = TempData["Message"];
		return await GetIndex(sortOrder, currentFilter, searchString, searchCategory, pageNumber, isDeleted: false);
	}

	public virtual async Task<IActionResult> DeleteIndex(string sortOrder, string currentFilter, string searchString, string searchCategory, int? pageNumber)
	{
		ViewBag.Message = TempData["Message"];
		return await GetIndex(sortOrder, currentFilter, searchString, searchCategory, pageNumber, isDeleted: true);
	}

	public virtual async Task<IActionResult> Details(int? id)
		=> await GetView(id, action: "", titelValue: "Details", returnAction: nameof(Index), navigatie: $"Details {EntityName}");

	private async Task<IActionResult> GetIndex(string sortOrder, string currentFilter, string searchString, string searchCategory, int? pageNumber, bool isDeleted)
	{
		try
		{
			var searchTerm = searchString ?? currentFilter;

			var query = isDeleted ? Service.QueryDeleted() : Service.QueryActive();
			query = SearchEngine.ApplySorting(query, sortOrder);
			query = SearchEngine.ApplySearch(query, searchTerm, searchCategory);

			var settings = await Context.Settings.FirstOrDefaultAsync(s => s.SettingsId == 1);
			var pageSize = settings is null ? 0 : GetPaginationSize(settings);

			return View(isDeleted ? nameof(DeleteIndex) : nameof(Index),
				await PaginatedList<TEntity>.CreateAsync(query, pageNumber ?? 1, pageSize));
		}
		catch (Exception ex)
		{
			ViewBag.Message = ex.Message;
			return View(isDeleted ? nameof(DeleteIndex) : nameof(Index));
		}
	}

	protected async Task<IActionResult> GetView(int? id, string action, string titelValue, string returnAction, string navigatie)
	{
		try
		{
			if (id is null) return NotFound();

			var entity = await Service.GetByIdAsync(id.Value);
			if (entity is null) return NotFound();

			var viewModel = new SRDViewModel
			{
				Id = id.Value,
				Navigatie = navigatie,
				Benaming = GetDisplayName(entity),
				Controller = EntityName,
				Action = action,
				Attributen = BuildAttributes(entity),
				TitelValue = titelValue,
				ReturnAction = returnAction,
			};

			return View("~/Views/Shared/Layout/_SRD.cshtml", viewModel);
		}
		catch (Exception ex)
		{
			ViewBag.Message = ex.Message;
			return RedirectToAction(action is "Reset" or "Delete" ? nameof(DeleteIndex) : nameof(Index));
		}
	}

	// SoftDelete / SoftDeleteConfirmed / Reset / ResetConfirmed / Delete / DeleteConfirmed follow the
	// same GetView(...)-then-Service.SoftDeleteAsync/RestoreAsync/HardDeleteAsync shape in every
	// concrete controller. They're kept in the concrete controllers rather than generalized further
	// here, since [Authorize(Roles = "admin")] is applied per-action and reads better spelled out
	// than hidden behind another layer of abstraction.
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Calenderapp.MVC.Models;
using Calenderapp.MVC.Services;

namespace Calenderapp.MVC.Controllers;

[Authorize(Roles = "admin, directeur, secretariaat")]
public class BeschikbaarLokaalController(
	CalenderAppContext context,
	ISearchEngine searchEngine,
	RoomAvailabilityService availabilityService) : Controller
{
	private readonly CalenderAppContext _context = context;
	private readonly ISearchEngine _searchEngine = searchEngine;
	private readonly RoomAvailabilityService _availabilityService = availabilityService;

	public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, string searchCategory, int? pageNumber)
	{
		ViewData["CurrentSort"] = sortOrder;
		ViewData["KlasNummer"] = sortOrder == "KlasNummer" ? "KlasNummer_desc" : "KlasNummer";
		ViewData["Verdieping"] = sortOrder == "Verdieping" ? "Verdieping_desc" : "Verdieping";
		ViewData["Zitplaatsen"] = sortOrder == "Zitplaatsen" ? "Zitplaatsen_desc" : "Zitplaatsen";
		ViewData["LocatieCampus"] = sortOrder == "LocatieCampus" ? "LocatieCampus_desc" : "LocatieCampus";
		ViewData["LokaalType"] = sortOrder == "LokaalType" ? "LokaalType_desc" : "LokaalType";
		ViewData["IsBeschikbaar"] = sortOrder == "IsBeschikbaar" ? "IsBeschikbaar_desc" : "IsBeschikbaar";
		ViewData["CurrentFilter"] = searchString;

		try
		{
			var searchTerm = searchString ?? currentFilter;
			if (searchString != null) pageNumber = 1;

			var lokalen = _context.Lokalen.Where(l => l.VerwijderdDoor == null).AsQueryable();
			lokalen = _searchEngine.ApplySorting(lokalen, sortOrder);
			lokalen = _searchEngine.ApplySearch(lokalen, searchTerm, searchCategory);

			// The rule itself ("occupied right now") lives in RoomAvailabilityService —
			// this controller just applies the result to the rows it's about to show.
			var lokaalIds = lokalen.Select(l => l.LokaalId).ToList();
			var occupiedRoomIds = await _availabilityService.GetOccupiedRoomIdsRightNowAsync(lokaalIds);

			foreach (var lokaal in lokalen)
			{
				if (occupiedRoomIds.Contains(lokaal.LokaalId))
				{
					lokaal.IsBeschikbaar = false;
				}
			}

			var settings = await _context.Settings.FirstOrDefaultAsync(s => s.SettingsId == 1);
			var pageSize = settings?.PaginationLokaal ?? 0;

			return View(await PaginatedList<Lokaal>.CreateAsync(lokalen, pageNumber ?? 1, pageSize));
		}
		catch (Exception ex)
		{
			ViewBag.Message = ex.Message;
			return View();
		}
	}
}

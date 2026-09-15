using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Calenderapp.MVC.Models;
using Calenderapp.MVC.Models.ViewModels;
using Calenderapp.MVC.Services;
using static Calenderapp.MVC.Models.ViewModels.SRDViewModel;

namespace Calenderapp.MVC.Controllers;

[Authorize(Roles = "admin, directeur")]
public class LokaalController(AuditableEntityService<Lokaal> service, ISearchEngine searchEngine, CalenderAppContext context)
	: EntityCrudController<Lokaal>(service, searchEngine, context)
{
	protected override string EntityName => "Lokaal";
	protected override int GetPaginationSize(Settings settings) => settings.PaginationLokaal;
	protected override string GetDisplayName(Lokaal entity) => entity.KlasLokaal;

	protected override List<AttributeInfo> BuildAttributes(Lokaal entity) =>
	[
		new() { Weergave = "Verdieping", Waarde = entity.Verdieping.ToString() },
		new() { Weergave = "KlasNummer", Waarde = entity.KlasNummer },
		new() { Weergave = "Zitplaatsen", Waarde = entity.Zitplaatsen.ToString() },
		new() { Weergave = "Locatie Campus", Waarde = entity.LocatieCampus ?? "" },
		new() { Weergave = "Lokaal Type", Waarde = entity.LokaalType ?? "" },
		new() { Weergave = "Omschrijving", Waarde = entity.Omschrijving ?? "" },
	];

	public IActionResult Create() => View();

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(IFormCollection form)
	{
		var lokaal = new Lokaal
		{
			Verdieping = Convert.ToInt32(form["Verdieping"]),
			Zitplaatsen = Convert.ToInt32(form["Zitplaatsen"]),
			KlasNummer = Convert.ToString(form["KlasNummer"]),
			LocatieCampus = Convert.ToString(form["LocatieCampus"]),
			LokaalType = Convert.ToString(form["LokaalType"]),
			Omschrijving = Convert.ToString(form["Omschrijving"]),
			IsBeschikbaar = true,
		};

		if (!ModelState.IsValid)
		{
			ViewBag.Message = "Jouw gegevens zijn niet correct ingevuld";
			return View(lokaal);
		}

		await Service.CreateAsync(lokaal, User.Identity!.Name ?? "user not logged in");
		return RedirectToAction(nameof(Index));
	}

	public async Task<IActionResult> Edit(int? id)
	{
		if (id is null) return NotFound();
		var lokaal = await Service.GetByIdAsync(id.Value);
		return lokaal is null ? NotFound() : View(lokaal);
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Edit(int id, IFormCollection form)
	{
		var lokaal = await Service.GetByIdAsync(id);
		if (lokaal is null) return NotFound();

		lokaal.Verdieping = Convert.ToInt32(form["Verdieping"]);
		lokaal.Zitplaatsen = Convert.ToInt32(form["Zitplaatsen"]);
		lokaal.KlasNummer = Convert.ToString(form["KlasNummer"]);
		lokaal.LocatieCampus = Convert.ToString(form["LocatieCampus"]);
		lokaal.LokaalType = Convert.ToString(form["LokaalType"]);
		lokaal.Omschrijving = Convert.ToString(form["Omschrijving"]);

		if (!ModelState.IsValid)
		{
			ViewBag.Message = "Jouw gegevens zijn niet correct ingevuld";
			return View(lokaal);
		}

		await Service.UpdateAsync(lokaal, User.Identity!.Name ?? "user not logged in");
		return RedirectToAction(nameof(Index));
	}

	public async Task<IActionResult> SoftDelete(int? id)
		=> await GetView(id, "Delete", "Verwijderen", nameof(Index), "Lokaal Verwijderen");

	[HttpPost, ActionName("SoftDelete")]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> SoftDeleteConfirmed(int id)
	{
		var lokaal = await Service.GetByIdAsync(id);
		if (lokaal is not null)
		{
			await Service.SoftDeleteAsync(lokaal, User.Identity!.Name ?? "user not logged in");
		}
		return RedirectToAction(nameof(Index));
	}

	[Authorize(Roles = "admin")]
	public async Task<IActionResult> Reset(int? id)
		=> await GetView(id, "Reset", "Herstellen", nameof(DeleteIndex), "Lokaal Herstellen");

	[Authorize(Roles = "admin")]
	[HttpPost, ActionName("Reset")]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> ResetConfirmed(int id)
	{
		var lokaal = await Service.GetByIdAsync(id);
		if (lokaal is not null)
		{
			await Service.RestoreAsync(lokaal);
		}
		return RedirectToAction(nameof(DeleteIndex));
	}

	[Authorize(Roles = "admin")]
	public async Task<IActionResult> Delete(int? id)
		=> await GetView(id, "Delete", "Definitief verwijderen", nameof(DeleteIndex), "Lokaal Definitief Verwijderen");

	[Authorize(Roles = "admin")]
	[HttpPost, ActionName("Delete")]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> DeleteConfirmed(int id)
	{
		var lokaal = await Service.GetByIdAsync(id);
		if (lokaal is not null)
		{
			await Service.HardDeleteAsync(lokaal);
		}
		return RedirectToAction(nameof(DeleteIndex));
	}
}

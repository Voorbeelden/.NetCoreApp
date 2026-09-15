using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Calenderapp.MVC.Models;
using Calenderapp.MVC.Models.ViewModels;
using Calenderapp.MVC.Services;
using static Calenderapp.MVC.Models.ViewModels.SRDViewModel;

namespace Calenderapp.MVC.Controllers;

[Authorize(Roles = "admin, directeur")]
public class OpleidingController(AuditableEntityService<Opleiding> service, ISearchEngine searchEngine, CalenderAppContext context)
	: EntityCrudController<Opleiding>(service, searchEngine, context)
{
	protected override string EntityName => "Opleiding";
	protected override int GetPaginationSize(Settings settings) => settings.PaginationOpleiding;
	protected override string GetDisplayName(Opleiding entity) => entity.OpleidingNaam;

	protected override List<AttributeInfo> BuildAttributes(Opleiding entity) =>
	[
		new() { Weergave = "OpleidingNaam", Waarde = entity.OpleidingNaam },
		new() { Weergave = "Afkorting", Waarde = entity.Afkorting },
		new() { Weergave = "Aantal Dagen", Waarde = entity.AantalDagen.ToString() },
	];

	public IActionResult Create() => View();

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(IFormCollection form)
	{
		var opleiding = new Opleiding
		{
			OpleidingNaam = Convert.ToString(form["OpleidingNaam"]),
			Afkorting = Convert.ToString(form["Afkorting"]),
			AantalDagen = Convert.ToInt32(form["AantalDagen"]),
		};

		if (!ModelState.IsValid)
		{
			ViewBag.Message = "Jouw gegevens zijn niet correct ingevuld";
			return View(opleiding);
		}

		await Service.CreateAsync(opleiding, User.Identity!.Name ?? "user not logged in");
		return RedirectToAction(nameof(Index));
	}

	public async Task<IActionResult> Edit(int? id)
	{
		if (id is null) return NotFound();
		var opleiding = await Service.GetByIdAsync(id.Value);
		return opleiding is null ? NotFound() : View(opleiding);
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Edit(int id, IFormCollection form)
	{
		var opleiding = await Service.GetByIdAsync(id);
		if (opleiding is null) return NotFound();

		opleiding.OpleidingNaam = Convert.ToString(form["OpleidingNaam"]);
		opleiding.Afkorting = Convert.ToString(form["Afkorting"]);
		opleiding.AantalDagen = Convert.ToInt32(form["AantalDagen"]);

		if (!ModelState.IsValid)
		{
			ViewBag.Message = "Jouw gegevens zijn niet correct ingevuld";
			return View(opleiding);
		}

		await Service.UpdateAsync(opleiding, User.Identity!.Name ?? "user not logged in");
		return RedirectToAction(nameof(Index));
	}

	public async Task<IActionResult> SoftDelete(int? id)
		=> await GetView(id, "Delete", "Verwijderen", nameof(Index), "Opleiding Verwijderen");

	[HttpPost, ActionName("SoftDelete")]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> SoftDeleteConfirmed(int id)
	{
		var opleiding = await Service.GetByIdAsync(id);
		if (opleiding is not null)
		{
			await Service.SoftDeleteAsync(opleiding, User.Identity!.Name ?? "user not logged in");
		}
		return RedirectToAction(nameof(Index));
	}

	[Authorize(Roles = "admin")]
	public async Task<IActionResult> Reset(int? id)
		=> await GetView(id, "Reset", "Herstellen", nameof(DeleteIndex), "Opleiding Herstellen");

	[Authorize(Roles = "admin")]
	[HttpPost, ActionName("Reset")]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> ResetConfirmed(int id)
	{
		var opleiding = await Service.GetByIdAsync(id);
		if (opleiding is not null)
		{
			await Service.RestoreAsync(opleiding);
		}
		return RedirectToAction(nameof(DeleteIndex));
	}

	[Authorize(Roles = "admin")]
	public async Task<IActionResult> Delete(int? id)
		=> await GetView(id, "Delete", "Definitief verwijderen", nameof(DeleteIndex), "Opleiding Definitief Verwijderen");

	[Authorize(Roles = "admin")]
	[HttpPost, ActionName("Delete")]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> DeleteConfirmed(int id)
	{
		var opleiding = await Service.GetByIdAsync(id);
		if (opleiding is not null)
		{
			await Service.HardDeleteAsync(opleiding);
		}
		return RedirectToAction(nameof(DeleteIndex));
	}
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Calenderapp.MVC.Models;
using Calenderapp.MVC.Models.ViewModels;
using Calenderapp.MVC.Services;
using static Calenderapp.MVC.Models.ViewModels.SRDViewModel;

namespace Calenderapp.MVC.Controllers;

[Authorize(Roles = "admin, directeur")]
public class DocentController(AuditableEntityService<Docent> service, ISearchEngine searchEngine, CalenderAppContext context)
	: EntityCrudController<Docent>(service, searchEngine, context)
{
	protected override string EntityName => "Docent";
	protected override int GetPaginationSize(Settings settings) => settings.PaginationDocent;
	protected override string GetDisplayName(Docent entity) => entity.VolledigeNaam;

	protected override List<AttributeInfo> BuildAttributes(Docent entity) =>
	[
		new() { Weergave = "Voornaam", Waarde = entity.Voornaam },
		new() { Weergave = "Achternaam", Waarde = entity.Achternaam },
		new() { Weergave = "Email", Waarde = entity.Email },
		new() { Weergave = "Afkorting", Waarde = entity.Afkorting },
		new() { Weergave = "TypeContract", Waarde = entity.TypeContract },
	];

	public IActionResult Create() => View();

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(IFormCollection form)
	{
		var docent = new Docent
		{
			Voornaam = Convert.ToString(form["Voornaam"]),
			Achternaam = Convert.ToString(form["Achternaam"]),
			Email = Convert.ToString(form["Email"]),
			Afkorting = Convert.ToString(form["Afkorting"]),
			TypeContract = Convert.ToString(form["TypeContract"]),
		};

		if (!ModelState.IsValid)
		{
			ViewBag.Message = "Jouw gegevens zijn niet correct ingevuld";
			return View(docent);
		}

		try
		{
			await Service.CreateAsync(docent, User.Identity!.Name ?? "user not logged in");
			return RedirectToAction(nameof(Index));
		}
		catch (Exception)
		{
			// Most likely cause: Email has a unique constraint on it.
			ViewBag.Message = "Email-adres hoort uniek te zijn";
			return View(docent);
		}
	}

	public async Task<IActionResult> Edit(int? id)
	{
		if (id is null) return NotFound();
		var docent = await Service.GetByIdAsync(id.Value);
		return docent is null ? NotFound() : View(docent);
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Edit(int id, IFormCollection form)
	{
		var docent = await Service.GetByIdAsync(id);
		if (docent is null) return NotFound();

		docent.Voornaam = Convert.ToString(form["Voornaam"]);
		docent.Achternaam = Convert.ToString(form["Achternaam"]);
		docent.Email = Convert.ToString(form["Email"]);
		docent.Afkorting = Convert.ToString(form["Afkorting"]);
		docent.TypeContract = Convert.ToString(form["TypeContract"]);

		if (!ModelState.IsValid)
		{
			ViewBag.Message = "Jouw gegevens zijn niet correct ingevuld";
			return View(docent);
		}

		await Service.UpdateAsync(docent, User.Identity!.Name ?? "user not logged in");
		return RedirectToAction(nameof(Index));
	}

	public async Task<IActionResult> SoftDelete(int? id)
		=> await GetView(id, "Delete", "Verwijderen", nameof(Index), "Docent Verwijderen");

	[HttpPost, ActionName("SoftDelete")]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> SoftDeleteConfirmed(int id)
	{
		var docent = await Service.GetByIdAsync(id);
		if (docent is not null)
		{
			await Service.SoftDeleteAsync(docent, User.Identity!.Name ?? "user not logged in");
		}
		return RedirectToAction(nameof(Index));
	}

	[Authorize(Roles = "admin")]
	public async Task<IActionResult> Reset(int? id)
		=> await GetView(id, "Reset", "Herstellen", nameof(DeleteIndex), "Docent Herstellen");

	[Authorize(Roles = "admin")]
	[HttpPost, ActionName("Reset")]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> ResetConfirmed(int id)
	{
		var docent = await Service.GetByIdAsync(id);
		if (docent is not null)
		{
			await Service.RestoreAsync(docent);
		}
		return RedirectToAction(nameof(DeleteIndex));
	}

	[Authorize(Roles = "admin")]
	public async Task<IActionResult> Delete(int? id)
		=> await GetView(id, "Delete", "Definitief verwijderen", nameof(DeleteIndex), "Docent Definitief Verwijderen");

	[Authorize(Roles = "admin")]
	[HttpPost, ActionName("Delete")]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> DeleteConfirmed(int id)
	{
		var docent = await Service.GetByIdAsync(id);
		if (docent is not null)
		{
			await Service.HardDeleteAsync(docent);
		}
		return RedirectToAction(nameof(DeleteIndex));
	}
}

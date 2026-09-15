using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Calenderapp.MVC.Models;
using Calenderapp.MVC.Models.ViewModels;
using Calenderapp.MVC.Services;
using static Calenderapp.MVC.Models.ViewModels.SRDViewModel;

namespace Calenderapp.MVC.Controllers;

[Authorize(Roles = "admin, directeur")]
public class SchoolController(AuditableEntityService<School> service, ISearchEngine searchEngine, CalenderAppContext context)
	: EntityCrudController<School>(service, searchEngine, context)
{
	protected override string EntityName => "School";
	protected override int GetPaginationSize(Settings settings) => settings.PaginationSchool;
	protected override string GetDisplayName(School entity) => entity.Afkorting;

	protected override List<AttributeInfo> BuildAttributes(School entity) =>
	[
		new() { Weergave = "School", Waarde = entity.SchoolNaam },
		new() { Weergave = "Te Behalen Percentage", Waarde = entity.TeBehalenPercentage.ToString() },
		new() { Weergave = "Email", Waarde = entity.Email },
		new() { Weergave = "Afkorting", Waarde = entity.Afkorting },
	];

	public IActionResult Create() => View();

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(IFormCollection form)
	{
		var school = new School
		{
			SchoolNaam = Convert.ToString(form["SchoolNaam"]),
			TeBehalenPercentage = Convert.ToInt32(form["TeBehalenPercentage"]),
			Afkorting = Convert.ToString(form["Afkorting"]),
			Email = Convert.ToString(form["Email"]),
		};

		if (!ModelState.IsValid)
		{
			ViewBag.Message = "Jouw gegevens zijn niet correct ingevuld";
			return View(school);
		}

		await Service.CreateAsync(school, User.Identity!.Name ?? "user not logged in");
		return RedirectToAction(nameof(Index));
	}

	public async Task<IActionResult> Edit(int? id)
	{
		if (id is null) return NotFound();
		var school = await Service.GetByIdAsync(id.Value);
		return school is null ? NotFound() : View(school);
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Edit(int id, IFormCollection form)
	{
		var school = await Service.GetByIdAsync(id);
		if (school is null) return NotFound();

		school.SchoolNaam = Convert.ToString(form["SchoolNaam"]);
		school.TeBehalenPercentage = Convert.ToInt32(form["TeBehalenPercentage"]);
		school.Afkorting = Convert.ToString(form["Afkorting"]);
		school.Email = Convert.ToString(form["Email"]);

		if (!ModelState.IsValid)
		{
			ViewBag.Message = "Jouw gegevens zijn niet correct ingevuld";
			return View(school);
		}

		await Service.UpdateAsync(school, User.Identity!.Name ?? "user not logged in");
		return RedirectToAction(nameof(Index));
	}

	public async Task<IActionResult> SoftDelete(int? id)
		=> await GetView(id, "Delete", "Verwijderen", nameof(Index), "School Verwijderen");

	[HttpPost, ActionName("SoftDelete")]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> SoftDeleteConfirmed(int id)
	{
		var school = await Service.GetByIdAsync(id);
		if (school is not null)
		{
			await Service.SoftDeleteAsync(school, User.Identity!.Name ?? "user not logged in");
		}
		return RedirectToAction(nameof(Index));
	}

	[Authorize(Roles = "admin")]
	public async Task<IActionResult> Reset(int? id)
		=> await GetView(id, "Reset", "Herstellen", nameof(DeleteIndex), "School Herstellen");

	[Authorize(Roles = "admin")]
	[HttpPost, ActionName("Reset")]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> ResetConfirmed(int id)
	{
		var school = await Service.GetByIdAsync(id);
		if (school is not null)
		{
			await Service.RestoreAsync(school);
		}
		return RedirectToAction(nameof(DeleteIndex));
	}

	[Authorize(Roles = "admin")]
	public async Task<IActionResult> Delete(int? id)
		=> await GetView(id, "Delete", "Definitief verwijderen", nameof(DeleteIndex), "School Definitief Verwijderen");

	[Authorize(Roles = "admin")]
	[HttpPost, ActionName("Delete")]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> DeleteConfirmed(int id)
	{
		var school = await Service.GetByIdAsync(id);
		if (school is not null)
		{
			await Service.HardDeleteAsync(school);
		}
		return RedirectToAction(nameof(DeleteIndex));
	}
}

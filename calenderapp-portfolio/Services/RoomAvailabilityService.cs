using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Calenderapp.MVC.Models;

namespace Calenderapp.MVC.Services;

/// <summary>
/// The "is this classroom free right now" rule used to live inline in
/// BeschikbaarLokaalController.Index() — useful logic, but impossible to unit test
/// without spinning up a controller and a database context together. Pulling it out
/// here means the rule itself (what "occupied right now" means) can be tested against
/// an in-memory set of Kalender rows, independent of MVC.
/// </summary>
public class RoomAvailabilityService(CalenderAppContext context)
{
	private readonly CalenderAppContext _context = context;

	/// <summary>
	/// Returns the ids, among <paramref name="roomIds"/>, of rooms that have a class in
	/// progress right now (today's weekday, current school year, current time falls
	/// inside the lesson's start/end).
	/// </summary>
	public async Task<HashSet<int>> GetOccupiedRoomIdsRightNowAsync(IEnumerable<int> roomIds)
	{
		var roomIdList = roomIds.ToList();
		var now = DateTime.Now;
		var currentTime = TimeOnly.FromDateTime(now);
		var culture = new CultureInfo("nl-NL");
		var todayName = culture.TextInfo.ToTitleCase(culture.DateTimeFormat.GetDayName(now.DayOfWeek));
		var schoolYear = GetSchoolYear(now);

		var lessons = await _context.Kalenders
			.Where(k => roomIdList.Contains(k.LokaalId))
			.Include(k => k.Planning)
			.ToListAsync();

		return lessons
			.Where(k => k.LesDag == todayName
				&& k.Planning.StartDatum <= now
				&& k.Planning.EindDatum >= now
				&& k.Schooljaar == schoolYear
				&& k.StartLes <= currentTime && k.EindLes >= currentTime)
			.Select(k => k.LokaalId)
			.ToHashSet();
	}

	/// <summary>School years run September–June; "2025-2026" from September through June, flips over at the summer break.</summary>
	public static string GetSchoolYear(DateTime date) => date.Month switch
	{
		>= 9 and <= 12 => $"{date.Year}-{date.Year + 1}",
		>= 1 and <= 6 => $"{date.Year - 1}-{date.Year}",
		_ => "Geen geldige datum voor een schooljaar",
	};
}

namespace Calenderapp.MVC;

/// <summary>
/// Extracted so controllers (and EntityCrudController&lt;T&gt; in particular) depend on
/// this abstraction rather than the concrete SearchEngine — makes it possible to supply
/// a fake/stub implementation in a controller unit test without needing a real
/// expression-tree evaluator or a database behind it.
/// </summary>
public interface ISearchEngine
{
	IQueryable<T> ApplySorting<T>(IQueryable<T> query, string sortOrder) where T : class;
	IQueryable<T> ApplySearch<T>(IQueryable<T> query, string searchString, string searchCategory) where T : class;
}

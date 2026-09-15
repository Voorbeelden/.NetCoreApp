namespace Calenderapp.MVC.Models.ViewModels
{
    public class SRDViewModel
    {
        public int Id { get; set; }
        public required string Benaming { get; set; }
        public required string Navigatie { get; set; }
        public required string TitelValue { get; set; }
        public required string ReturnAction { get; set; }
        public required string Controller { get; set; }
        public required string Action { get; set; }
        public required List<AttributeInfo> Attributen { get; set; }

        public class AttributeInfo
        {
            public required string Weergave { get; set; }
            public required string Waarde { get; set; }
        }
    }
}

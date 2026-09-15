using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Calenderapp.MVC.Models
{
    
    public partial class Settings
    {     
        public int SettingsId { get; set; }
        [Display(Name = "Docent")]
        [Required(ErrorMessage = "Docent is verplicht!")]
        public int PaginationDocent { get;set;}
        [Display(Name = "Lokaal")]
        [Required(ErrorMessage = "Lokaal is verplicht!")]
        public int PaginationLokaal { get; set; }
        [Display(Name = "Module")]
        [Required(ErrorMessage = "Module is verplicht!")]
        public int PaginationModules { get; set; }
        [Display(Name = "School")]
        [Required(ErrorMessage = "School is verplicht!")]
        public int PaginationSchool { get; set; }
        [Display(Name = "Opleiding")]
        [Required(ErrorMessage = "Opleiding is verplicht!")]
        public int PaginationOpleiding { get; set; }
        [Display(Name = "Planning")]
        [Required(ErrorMessage = "Planning is verplicht!")]
        public int PaginationPlanning { get; set; }
        [Display(Name = "Vakantie")]
        [Required(ErrorMessage = "Vakantie is verplicht!")]
        public int PaginationVakantie { get; set; }
        [Display(Name = "Deeltijds")]
        [Required(ErrorMessage = "Deeltijds is verplicht!")]
        public int ContractDeeltijds { get; set; }
        [Display(Name = "Voltijds")]
        [Required(ErrorMessage = "Voltijds is verplicht!")]
        public int ContractVoltijds { get; set; }
    }
}

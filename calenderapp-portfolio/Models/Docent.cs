using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Calenderapp.MVC.Models;

public partial class Docent : IAuditableEntity
{
    public int DocentId { get; set; }
    [Required(ErrorMessage = "Voornaam is verplicht!")]
    [MaxLength(150, ErrorMessage = "Voornaam mag niet meer dan 150 tekens bevatten!")] 
    public string Voornaam { get; set; } = null!;
    [Required(ErrorMessage = "Achternaam is verplicht!")]
    [MaxLength(150, ErrorMessage = "Achternaam mag niet meer dan 150 tekens bevatten!")]
    public string Achternaam { get; set; } = null!; 
    [Display(Name = "Email adres")]
    [Required(ErrorMessage = "Email is verplicht!")]
    [MaxLength(150, ErrorMessage = "Email mag niet meer dan 150 tekens bevatten!")]
    [EmailAddress]
    public string Email { get; set; } = null!;
    [Required(ErrorMessage = "Afkorting is verplicht!")]
    [MaxLength(10, ErrorMessage = "Afkorting mag niet meer dan 10 tekens bevatten!")] 
    public string Afkorting { get; set; } = null!;
    [Required(ErrorMessage = "Type Contract is verplicht!")]
    public string TypeContract { get; set; } = null!;
    public DateTime AanmaakDatum { get; set; }
    public string AangemaaktDoor { get; set; } = null!;
    public DateTime? UpdateDatum { get; set; }
    public string? UpdatedDoor { get; set; }
    public DateTime? VerwijderDatum { get; set; }
    public string? VerwijderdDoor { get; set; }
    [NotMapped]
    public string VolledigeNaam
    {
        get
        {
            return Voornaam + " " + Achternaam;
        }
    }
    [JsonIgnore]
    public virtual ICollection<Kalender> Kalender { get; set; } = [];
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Calenderapp.MVC.Models;

public partial class Opleiding : IAuditableEntity
{
    public int OpleidingId { get; set; }
    [Display(Name = "Opleiding")]
    [Required(ErrorMessage = "Opleiding is verplicht!")]
    [MaxLength(50, ErrorMessage = "Opleiding mag niet meer dan 50 tekens bevatten!")]
    public string OpleidingNaam { get; set; } = null!;
    [Required(ErrorMessage = "Afkorting is verplicht!")]
    [MaxLength(10, ErrorMessage = "Afkorting mag niet meer dan 10 tekens bevatten!")]
    public string Afkorting { get; set; } = null!;
    [Display(Name = "Aantal Dagen/week")]
    [Required(ErrorMessage = "Aantal Dagen per week is verplicht!")]
    public int AantalDagen { get; set; }
    public DateTime AanmaakDatum { get; set; }
    public string AangemaaktDoor { get; set; } = null!;
    public DateTime? UpdateDatum { get; set; }
    public string? UpdatedDoor { get; set; }
    public DateTime? VerwijderDatum { get; set; }
    public string? VerwijderdDoor { get; set; }

    [Display(Name = "Planning")]
    [JsonIgnore]
    public virtual ICollection<Planning> Planningen { get; set; } = [];
}
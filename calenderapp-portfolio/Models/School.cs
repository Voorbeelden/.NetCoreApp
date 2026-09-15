using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Calenderapp.MVC.Models;

public partial class School : IAuditableEntity
{
    public int SchoolId { get; set; }
    [Required(ErrorMessage = "School is verplicht!")]
    [MaxLength(50, ErrorMessage = "School mag niet meer dan 50 tekens bevatten!")]
    public string SchoolNaam { get; set; } = null!;
   [Required(ErrorMessage = "Te Behalen Percentage is verplicht!")]
    public int TeBehalenPercentage { get; set; }
    [Required(ErrorMessage = "Afkorting is verplicht!")]
    [MaxLength(10, ErrorMessage = "Afkorting mag niet meer dan 10 tekens bevatten!")]
    public string Afkorting { get; set; } = null!;
    [Display(Name = "Email Planning Verantwoordelijke")]
    [Required(ErrorMessage = "Email is verplicht!")]
    [MaxLength(150, ErrorMessage = "Email mag niet meer dan 150 tekens bevatten!")]
    public string Email { get; set; } = null!;
    public DateTime AanmaakDatum { get; set; }
    public string AangemaaktDoor { get; set; } = null!;
    public DateTime? UpdateDatum { get; set; }
    public string? UpdatedDoor { get; set; }
    public DateTime? VerwijderDatum { get; set; }
    public string? VerwijderdDoor { get; set; }
}

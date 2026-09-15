using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Calenderapp.MVC.Models;
public partial class Lokaal : IAuditableEntity
{
    public int LokaalId { get; set; }
    [Display(Name = "Lokaal")]
    [Required(ErrorMessage = "Klaslokaal is verplicht!")]
    public string KlasNummer { get; set; } = null!;
    [Display(Name = "Verdieping")]
    [Required(ErrorMessage = "Verdieping is verplicht!")]
    public int Verdieping { get; set; }
    [Display(Name = "Max aantal")]
    [Required(ErrorMessage = "Max aantal is verplicht!")]
    public int Zitplaatsen { get; set; }
    public string? Omschrijving { get; set; }
    public string? LokaalType { get; set; }
    public string? LocatieCampus { get; set; }
    public DateTime AanmaakDatum { get; set; }
    public string AangemaaktDoor { get; set; } = null!;
    public DateTime? UpdateDatum { get; set; }
    public string? UpdatedDoor { get; set; }
    public DateTime? VerwijderDatum { get; set; }
    public string? VerwijderdDoor { get; set; }   
    public bool? IsBeschikbaar { get; set; }
    [NotMapped]
    public string KlasLokaal
    {
        get
        {
            return Verdieping + " . " + KlasNummer + " ( " + Zitplaatsen + " )";
        }
    }
    [Display(Name = "Planningen")]
    [JsonIgnore]
    public virtual ICollection<Kalender> Kalender { get; set; } = [];
}

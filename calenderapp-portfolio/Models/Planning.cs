using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Calenderapp.MVC.Models;

public partial class Planning
{
    public int PlanningId { get; set; }
    [Display(Name = "Opleiding")]
    [Required(ErrorMessage = "Opleiding is verplicht!")]
    [ForeignKey("FK_OpleidingPlanning")]
    public int OpleidingId { get; set; }
    [Display(Name = "Start Datum Opleiding")]
    [Required(ErrorMessage = "Start Datum Opleiding is verplicht!")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = false)]
    public DateTime StartDatum { get; set; }
    [Display(Name = "Eind Datum Opleiding")]
    [Required(ErrorMessage = "Eind Datum Opleiding is verplicht!")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd-MM-yy}", ApplyFormatInEditMode = false)]
    public DateTime EindDatum { get; set; }
    [Display(Name = "Module")]
    [Required(ErrorMessage = "Module is verplicht!")]
    [ForeignKey("FK_ModulePlanning")]
    public int ModuleId { get; set; }
    public string TypeSchool { get; set; } = null!;
    public string? Omschrijving { get; set; }
    [Required(ErrorMessage = "Code is verplicht!")]
    public string Code { get; set; } = null!;
    public string? DoorlopendeVakantie { get; set; }
    public DateTime AanmaakDatum { get; set; }
    public string AangemaaktDoor { get; set; } = null!;
    public DateTime? UpdateDatum { get; set; }
    public string? UpdatedDoor { get; set; }
    public DateTime? VerwijderDatum { get; set; }
    public string? VerwijderdDoor { get; set; }

    public virtual Module Module { get; set; } = null!;

    public virtual Opleiding Opleiding { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<Kalender> Kalender { get; set; } = [];

}

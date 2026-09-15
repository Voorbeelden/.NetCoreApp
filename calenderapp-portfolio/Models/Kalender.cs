using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Calenderapp.MVC.Models
{
    public partial class Kalender
    {
        [Key]
        public int KalenderId { get; set; }
        public int PlanningId { get; set; }
        [Required(ErrorMessage = "Docent is verplicht!")]
        [ForeignKey("FK_DocentKalender")]
        public int DocentId { get; set; }
        [Required(ErrorMessage = "Lokaal is verplicht!")]
        [ForeignKey("FK_LokaalKalender")]
        public int LokaalId { get; set; }
        [Required(ErrorMessage = "Lesdag(en) is verplicht!")]
        public string LesDag { get; set; } = null!;
        public string Schooljaar { get; set; } = null!;
        [Required(ErrorMessage = "Start lesuur is verplicht!")]
       // [JsonConverter(typeof(TimeOnlyConverter))]
        public TimeOnly StartLes { get; set; }
        [Required(ErrorMessage = "Eind lesuur is verplicht!")]
       // [JsonConverter(typeof(TimeOnlyConverter))]
        public TimeOnly EindLes { get; set; }
        public int? AfwezigeDocentId { get; set; }
        public int? VervangendeKalenderId { get; set; }
        public bool? IsAfwezig { get; set; }
        public bool? IsBetalend { get; set; }
        public bool? IsVrijwillig { get; set; }
        public DateTime? StartDatumAfwezig { set; get; }
        public DateTime? EindDatumAfwezig { set; get; }
        public DateTime AanmaakDatum { get; set; }
        public string AangemaaktDoor { get; set; } = null!;
        public DateTime? UpdateDatum { get; set; }
        public string? UpdatedDoor { get; set; }
        public DateTime? VerwijderDatum { get; set; }
        public string? VerwijderdDoor { get; set; }
        public virtual Planning Planning { get; set; } = null!;
        public virtual Docent Docent { get; set; } = null!;
        public virtual Lokaal Lokaal { get; set; } = null!;
    }
}

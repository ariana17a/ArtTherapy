using System.ComponentModel.DataAnnotations;

namespace ArtTherapy_MVC.Models
{
    public class PlanUtilizareViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nume plan")]
        public string Nume { get; set; }

        [Display(Name = "Descriere")]
        public string Descriere { get; set; }

        [Display(Name = "Limită lucrări")]
        public int? LimitaLucrari { get; set; }

        [Display(Name = "Emoții extinse")]
        public bool EmotiiExtinse { get; set; }

        [Display(Name = "Feedback deblocabil")]
        public bool FeedbackDeblocabil { get; set; }

        [Display(Name = "Sesiuni terapie")]
        public bool SesiuniTerapie { get; set; }

        [Display(Name = "Chat securizat")]
        public bool ChatSecurizat { get; set; }
    }
}

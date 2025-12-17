using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ArtTherapy_MVC.Models
{
    public class FeedbackViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Text feedback")]
        public string Text { get; set; }

        [Required]
        [Display(Name = "Utilizator")]
        public int UtilizatorId { get; set; }

        [Required]
        [Display(Name = "Lucrare artistică")]
        public int LucrareArtisticaId { get; set; }

        // pentru afișare în listă
        public string NumeUtilizator { get; set; }
        public string TitluLucrare { get; set; }

        // pentru dropdown-uri
        public List<SelectListItem> Utilizatori { get; set; }
        public List<SelectListItem> LucrariArtistice { get; set; }
    }
}
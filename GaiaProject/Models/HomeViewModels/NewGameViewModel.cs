using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GaiaProject.Models.HomeViewModels
{
    public class NewGameViewModel
    {
        [StringLength(10,MinimumLength =3)]
        [Required]
        [Display(Name = "GameName")]
        public string Name { set; get; }
        [Required]
        [EmailAddress]
        [Display(Name = "Player1")]
        public string Player1 { set; get; }
        [Required]
        [EmailAddress]
        [Display(Name = "Player2")]
        public string Player2 { set; get; }
        [EmailAddress]
        [Display(Name = "Player3")]
        public string Player3 { set; get; }
        [EmailAddress]
        [Display(Name = "Player4")]
        public string Player4 { set; get; }
    }
}

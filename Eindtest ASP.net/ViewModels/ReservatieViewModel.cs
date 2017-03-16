using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Eindtest_ASP.net.Models;
using System.ComponentModel.DataAnnotations;

namespace Eindtest_ASP.net.ViewModels
{
    public class ReservatieViewModel
    {
        public List<Film> Films { get; set; }
        public Klant Klant { get; set; }
        [DisplayFormat(DataFormatString = "{0:€ #,##0.00}")]
        public decimal TotaalPrijs { get; set; }

        public ReservatieViewModel(List<Film> Films, Klant Klant, decimal TotaalPrijs)
        {
            this.Films = Films;
            this.Klant = Klant;
            this.TotaalPrijs = TotaalPrijs;

        }
    }
}
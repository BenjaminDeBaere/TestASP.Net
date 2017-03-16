using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Eindtest_ASP.net.Models
{
    public class FilmProperties
    {
        [DisplayFormat(DataFormatString ="{0:€ #,##0.00}")]
        public decimal Prijs { get; set; }
    }
}
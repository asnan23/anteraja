using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnterAjaTest.Shared
{
    public class MasterPriceVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Origin is required field")]
        [Display(Name = "Origin")]
        public string origin_code { get; set; }

        [Required(ErrorMessage = "Destination is required field")]
        [Display(Name = "Destination")]
        public string destination_code { get; set; }

        [Required(ErrorMessage = "Price is required field")]
        [Range(1, double.MaxValue, ErrorMessage = "Value for the Price can't be lower than 1")]
        [Display(Name = "Price")]
        public double price { get; set; }

        [Required(ErrorMessage = "Product is required field")]
        [Display(Name = "Product")]
        public string product { get; set; }
    }
}

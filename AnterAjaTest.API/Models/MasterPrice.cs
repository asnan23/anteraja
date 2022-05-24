using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AnterAjaTest.API.Models
{
    [Table("master_price", Schema = "dbo")]
    public class MasterPrice
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string origin_code { get; set; }

        [Required]
        public string destination_code { get; set; }

        [Required]
        public double price { get; set; }

        [Required]
        public string product { get; set; }
    }
}

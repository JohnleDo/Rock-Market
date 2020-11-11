using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Rock_Market.Models
{
    public class StoreViewModel
    {
        [Key]
        [Required]
        public string Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public float Price { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}

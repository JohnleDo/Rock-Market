using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Rock_Market.Models
{
    public class ImageModel
    {
        [Key]
        [Required]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [DisplayName("Image Group")]
        public string ImageGroup { get; set; }

        [Required]
        public byte[] Image { get; set; }

        [Required]
        [DisplayName("User ID")]
        public string UserID { get; set; }
    }
}

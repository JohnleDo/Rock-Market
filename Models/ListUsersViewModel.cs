using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Rock_Market.Models
{
    public class ListUsersViewModel
    {
        public string Id { get; set; }

        [Required]
        public string Email { get; set; }
        public IList<string> Roles { get; set; }
    }
}

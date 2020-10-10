using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Rock_Market.Models
{
    public class ListUserAndRoleViewModel
    {
        public ListUserAndRoleViewModel()
        {
            Roles = new List<string>();
            Roless = new List<IdentityRole>();
            Users = new List<ListUsersViewModel>();
        }

        public string Id { get; set; }

        [Required]
        public string Email { get; set; }
        public IList<string> Roles { get; set; }

        public IList<IdentityRole> Roless { get; set; }
        public IList<ListUsersViewModel> Users { get; set; }
    }
}

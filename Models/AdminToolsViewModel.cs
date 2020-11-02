using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Account.Internal;
using Rock_Market.Areas.Identity.Pages.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Rock_Market.Models
{
    public class AdminToolsViewModel
    {
        public AdminToolsViewModel()
        {
            Roles = new List<IdentityRole>();
            Users = new List<ListUsersViewModel>();
        }
        public IList<IdentityRole> Roles { get; set; }
        public IList<ListUsersViewModel> Users { get; set; }
        public CreateRoleViewModel createRole { get; set; }
        public EditUserViewModel editUser { get; set; }
        public CreateUserViewModel createUser { get; set; }
    }
}

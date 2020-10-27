using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rock_Market.Models
{
    public class CreateRoleViewModel
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
    }
}

using System;
using Microsoft.AspNetCore.Identity;

namespace SharedObjects.Models
{
    public class AppRole : IdentityRole
    {
        public string Description { get; set; }
    }
}

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace SharedObjects.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Avatar { get; set; }
        public DateTime? BirthDay { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? LastLogin { get; set; }
        public List<Order> Orders { get; set; }
        public List<News> News { get; set; }
        public List<Page> Pages { get; set; }
    }
}

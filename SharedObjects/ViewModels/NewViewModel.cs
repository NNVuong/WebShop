using System;
using Microsoft.AspNetCore.Http;

namespace SharedObjects.ViewModels
{
    public class NewViewModel
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Contents { get; set; }
        public IFormFile Thumb { get; set; }
        public string Alias { get; set; }
        public string UserId { get; set; }
        public bool IsNewFeed { get; set; }
    }
}

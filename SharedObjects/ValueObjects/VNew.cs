using System;
using System.ComponentModel.DataAnnotations;

namespace SharedObjects.ValueObjects
{
    public class VNew
    {
        [Key]
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Contents { get; set; }
        public string Thumb { get; set; }
        public string Alias { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UserId { get; set; }
        public bool IsHot { get; set; }
        public bool IsNewFeed { get; set; }
        public string UserName { get; set; }
    }
}

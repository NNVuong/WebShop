using System;
using System.ComponentModel.DataAnnotations;

namespace SharedObjects.ValueObjects
{
    public class VTransactStatus
    {
        [Key]
        public int TransactStatusId { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Assignment4.Entities
{
    //[Index(nameof(name), IsUnique = true)]
    public class Tag
    {
        [Key]
        public int tagId { get; set; }

        [Required]
        [StringLength(50)]
        public string name { get; set; }

        public ICollection<Task> tasks { get; set; }
    }
}

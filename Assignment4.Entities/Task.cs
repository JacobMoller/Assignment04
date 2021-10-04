using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Assignment4.Core;


namespace Assignment4.Entities
{
    public class Task
    {
        [Key]
        public int taskId { get; set; }

        [Required]
        [StringLength(100)]
        public string title { get; set; }

        public User assignedTo { get; set; }
        public string description { get; set; }

        [Required]
        public State state { get; set; }

        public ICollection<Tag> tags { get; set; }
    }
}

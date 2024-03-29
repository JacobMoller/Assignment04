using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Assignment4.Entities
{
    public class User
    {
        [Key]
        public int id { get; set; }

        [StringLength(100)]
        public string name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string email { get; set; }

        public List<Task> tasks { get; set; }

    }
}

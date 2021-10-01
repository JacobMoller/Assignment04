using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Assignment4.Entities
{
    //[Index(nameof(email), IsUnique = true)]
    public class User
    {
        [Key]
        public int userId { get; set; }

        [StringLength(100)]
        public string name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string email { get; set; }

        public List<Task> tasks { get; set; }

    }
}

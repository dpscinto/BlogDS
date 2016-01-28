using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BlogDS.Models
{
    public class ContactMessage
    {
        [Required]
        public string c_name { get; set; }
        [Required]
        public string c_email { get; set; }
        [Required]
        public string c_message { get; set; }
    }
}
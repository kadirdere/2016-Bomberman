namespace BotManagerAPI.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Email")]
    public partial class Email
    {
        public int EmailId { get; set; }

        [Required]
        [StringLength(100)]
        public string DestinationEmail { get; set; }

        [Required]
        [StringLength(1000)]
        public string EmailContent { get; set; }

        public byte IsSent { get; set; }

        public int Retries { get; set; }

        [Required]
        [StringLength(100)]
        public string Subject { get; set; }
    }
}

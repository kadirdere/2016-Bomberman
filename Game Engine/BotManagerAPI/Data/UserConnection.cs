namespace BotManagerAPI.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserConnection")]
    public partial class UserConnection
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(255)]
        public string userId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(255)]
        public string providerId { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(255)]
        public string providerUserId { get; set; }

        public int rank { get; set; }

        [StringLength(255)]
        public string displayName { get; set; }

        [StringLength(512)]
        public string profileUrl { get; set; }

        [StringLength(512)]
        public string imageUrl { get; set; }

        [Required]
        [StringLength(255)]
        public string accessToken { get; set; }

        [StringLength(255)]
        public string secret { get; set; }

        [StringLength(255)]
        public string refreshToken { get; set; }

        public long? expireTime { get; set; }
    }
}

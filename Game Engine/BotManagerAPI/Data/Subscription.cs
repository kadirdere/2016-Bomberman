namespace BotManagerAPI.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Subscription")]
    public partial class Subscription
    {
        public int SubscriptionId { get; set; }

        public int? Age { get; set; }

        [Required]
        [StringLength(250)]
        public string EmailAddress { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        public byte? PreviousContestant { get; set; }

        public DateTime? SubscriptionDate { get; set; }

        [StringLength(255)]
        public string UniqueCode { get; set; }

        public int? ChallengeTypeId { get; set; }

        public int? ExperienceLevelId { get; set; }

        public int? MarketingSourceId { get; set; }

        public virtual ChallengeType ChallengeType { get; set; }

        public virtual ExperienceLevel ExperienceLevel { get; set; }

        public virtual MarketingSource MarketingSource { get; set; }
    }
}

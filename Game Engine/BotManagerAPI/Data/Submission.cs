namespace BotManagerAPI.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Submission")]
    public partial class Submission
    {
        public int SubmissionId { get; set; }

        public DateTime? BuildCompleteTimestamp { get; set; }

        [StringLength(2000)]
        public string BuildLogPath { get; set; }

        public bool BuildOk { get; set; }

        public bool BuildStarted { get; set; }

        public bool Complete { get; set; }

        public DateTime? MatchCompleteTimestamp { get; set; }

        [StringLength(2000)]
        public string MatchDataPath { get; set; }

        public bool MatchStarted { get; set; }

        [StringLength(2000)]
        public string SubmissionPath { get; set; }

        public DateTime UploadTimestamp { get; set; }

        public int AppUser { get; set; }

        public bool IsPreferred { get; set; }

        public virtual AppUser AppUser1 { get; set; }
    }
}

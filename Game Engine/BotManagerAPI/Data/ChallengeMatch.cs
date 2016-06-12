using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BotManagerAPI.Data
{
    [Table("ChallengeMatch")]
    public class ChallengeMatch
    {
        public int ChallengeMatchId { get; set; }

        public int ChallengerOneId { get; set; }

        public int ChallengerTwoId { get; set; }

        public DateTime MatchStartTimestamp { get; set; }

        public DateTime? MatchEndTimestamp { get; set; }

        [StringLength(100)]
        public String MatchLogDir { get; set; }

        public int? WinnerId { get; set; }

        public virtual AppUser ChallengerOne { get; set; }

        public virtual AppUser ChallengerTwo { get; set; }

        public virtual AppUser Winner { get; set; }
    }
}
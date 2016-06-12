namespace BotManagerAPI.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Setting")]
    public partial class Setting
    {
        public int SettingId { get; set; }

        [Required]
        [StringLength(50)]
        public string SettingName { get; set; }

        [Required]
        [StringLength(50)]
        public string SettingValue { get; set; }
    }
}

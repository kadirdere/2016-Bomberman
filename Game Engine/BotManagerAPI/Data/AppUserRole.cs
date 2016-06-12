namespace BotManagerAPI.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AppUserRole")]
    public partial class AppUserRole
    {
        public int AppUserRoleId { get; set; }

        public int AppUser { get; set; }

        public int Role { get; set; }

        public virtual AppUser AppUser1 { get; set; }

        public virtual Role Role1 { get; set; }
    }
}

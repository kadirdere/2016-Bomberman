namespace BotManagerAPI.Data
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class PlayerDashboardDB : DbContext
    {
        public PlayerDashboardDB()
            : base("name=PlayerDashboardDB")
        {
        }

        public virtual DbSet<AppUser> AppUsers { get; set; }
        public virtual DbSet<AppUserDetail> AppUserDetails { get; set; }
        public virtual DbSet<AppUserRole> AppUserRoles { get; set; }
        public virtual DbSet<ChallengeType> ChallengeTypes { get; set; }
        public virtual DbSet<Email> Emails { get; set; }
        public virtual DbSet<ExperienceLevel> ExperienceLevels { get; set; }
        public virtual DbSet<MarketingSource> MarketingSources { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Setting> Settings { get; set; }
        public virtual DbSet<Submission> Submissions { get; set; }
        public virtual DbSet<Subscription> Subscriptions { get; set; }
        public virtual DbSet<UserConnection> UserConnections { get; set; }
        public virtual DbSet<ChallengeMatch> ChallengeMatches { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<AppUser>()
                .Property(e => e.Username)
                .IsUnicode(false);

            modelBuilder.Entity<AppUser>()
                .HasMany(e => e.AppUserRoles)
                .WithRequired(e => e.AppUser1)
                .HasForeignKey(e => e.AppUser)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AppUser>()
                .HasMany(e => e.Submissions)
                .WithRequired(e => e.AppUser1)
                .HasForeignKey(e => e.AppUser)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AppUser>()
                .HasMany(e => e.PlayerOneMatches)
                .WithRequired(e => e.ChallengerOne)
                .HasForeignKey(e => e.ChallengerOneId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AppUser>()
                .HasMany(e => e.PlayerTwoMatches)
                .WithRequired(e => e.ChallengerTwo)
                .HasForeignKey(e => e.ChallengerTwoId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AppUser>()
                .HasMany(e => e.WinningMatches)
                .WithRequired(e => e.Winner)
                .HasForeignKey(e => e.WinnerId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AppUserDetail>()
                .Property(e => e.EmailAddress)
                .IsUnicode(false);

            modelBuilder.Entity<AppUserDetail>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<AppUserDetail>()
                .Property(e => e.FoundChallenge)
                .IsUnicode(false);

            modelBuilder.Entity<AppUserDetail>()
                .Property(e => e.IdentityNumber)
                .IsUnicode(false);

            modelBuilder.Entity<AppUserDetail>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<AppUserDetail>()
                .Property(e => e.Occupation)
                .IsUnicode(false);

            modelBuilder.Entity<AppUserDetail>()
                .HasMany(e => e.AppUsers)
                .WithOptional(e => e.AppUserDetail)
                .HasForeignKey(e => e.AppUserDetails);

            modelBuilder.Entity<ChallengeType>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Email>()
                .Property(e => e.DestinationEmail)
                .IsUnicode(false);

            modelBuilder.Entity<Email>()
                .Property(e => e.EmailContent)
                .IsUnicode(false);

            modelBuilder.Entity<Email>()
                .Property(e => e.Subject)
                .IsUnicode(false);

            modelBuilder.Entity<ExperienceLevel>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<MarketingSource>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Role>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Role>()
                .HasMany(e => e.AppUserRoles)
                .WithRequired(e => e.Role1)
                .HasForeignKey(e => e.Role)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Setting>()
                .Property(e => e.SettingName)
                .IsUnicode(false);

            modelBuilder.Entity<Setting>()
                .Property(e => e.SettingValue)
                .IsUnicode(false);

            modelBuilder.Entity<Submission>()
                .Property(e => e.BuildLogPath)
                .IsUnicode(false);

            modelBuilder.Entity<Submission>()
                .Property(e => e.MatchDataPath)
                .IsUnicode(false);

            modelBuilder.Entity<Submission>()
                .Property(e => e.SubmissionPath)
                .IsUnicode(false);

            modelBuilder.Entity<Subscription>()
                .Property(e => e.EmailAddress)
                .IsUnicode(false);

            modelBuilder.Entity<Subscription>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<Subscription>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<Subscription>()
                .Property(e => e.UniqueCode)
                .IsUnicode(false);

            modelBuilder.Entity<UserConnection>()
                .Property(e => e.userId)
                .IsUnicode(false);

            modelBuilder.Entity<UserConnection>()
                .Property(e => e.providerId)
                .IsUnicode(false);

            modelBuilder.Entity<UserConnection>()
                .Property(e => e.providerUserId)
                .IsUnicode(false);

            modelBuilder.Entity<UserConnection>()
                .Property(e => e.displayName)
                .IsUnicode(false);

            modelBuilder.Entity<UserConnection>()
                .Property(e => e.profileUrl)
                .IsUnicode(false);

            modelBuilder.Entity<UserConnection>()
                .Property(e => e.imageUrl)
                .IsUnicode(false);

            modelBuilder.Entity<UserConnection>()
                .Property(e => e.accessToken)
                .IsUnicode(false);

            modelBuilder.Entity<UserConnection>()
                .Property(e => e.secret)
                .IsUnicode(false);

            modelBuilder.Entity<UserConnection>()
                .Property(e => e.refreshToken)
                .IsUnicode(false);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using Bomberman;
using BotManagerAPI.Data;
using GameEngine.Loggers;
using ICSharpCode.SharpZipLib.Zip;

namespace BotManagerAPI.GameEngine
{
    public class MatchRunnerJob
    {
        public ILogger Logger = new InMemoryLogger();

        public void ExecuteJob(int playerOneId, int playerTwoId)
        {
            using (var db = new PlayerDashboardDB())
            {
                try
                {
                    var player1 = db.Submissions.OrderByDescending(x => x.UploadTimestamp)
                        .FirstOrDefault(x => x.AppUser == playerOneId && x.IsPreferred && x.Complete && x.BuildOk);
                    var player2 = db.Submissions.OrderByDescending(x => x.UploadTimestamp)
                        .FirstOrDefault(x => x.AppUser == playerTwoId && x.IsPreferred && x.Complete && x.BuildOk);

                    if (player1 == null || player2 == null)
                    {
                        return;
                    }

                    var match = new ChallengeMatch
                    {
                        ChallengerOneId = player1.AppUser,
                        ChallengerTwoId = player2.AppUser,
                        MatchStartTimestamp = DateTime.Now
                    };

                    db.ChallengeMatches.Add(match);
                    db.SaveChanges();

                    var matchLogDir = ConfigurationManager.AppSettings.Get("compMatchLogDirectory");
                    var gameWorkDir = Path.Combine(matchLogDir, match.ChallengeMatchId.ToString());

                    if (Directory.Exists(gameWorkDir))
                    {
                        Directory.Delete(gameWorkDir, true);
                    }

                    var paths = new String[2]
                    {
                        player1.SubmissionPath,
                        player2.SubmissionPath
                    };
                    var options = new Options { BotFolders = paths, Log = gameWorkDir };
                    var game = new BombermanGame();
                    game.StartNewGame(options);

                    match.MatchEndTimestamp = DateTime.Now;
                    match.WinnerId = game.Engine.Winner.PlayerEntity.Key == 'A' ? playerOneId : playerTwoId;

                    var zipFilePath = Path.Combine(matchLogDir, String.Format("match-{0}.zip", match.ChallengeMatchId));

                    if (File.Exists(zipFilePath))
                    {
                        File.Delete(zipFilePath);
                    }

                    using (var zipFile = ZipFile.Create(zipFilePath))
                    {
                        zipFile.UseZip64 = UseZip64.Off;
                        zipFile.BeginUpdate();

                        AddFolderToZip(zipFile, gameWorkDir, gameWorkDir);

                        zipFile.CommitUpdate();
                        zipFile.Close();
                    }

                    Directory.Delete(gameWorkDir, true);

                    match.MatchLogDir = zipFilePath;
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    Console.WriteLine("Failed to run match");
                    throw;
                }

            }
        }

        public void AddFolderToZip(ZipFile f, string root, string folder)
        {
            var relative = folder.Substring(root.Length);
            if (relative.Length > 0)
            {
                f.AddDirectory(relative);
            }
            foreach (var file in Directory.GetFiles(folder))
            {
                relative = file.Substring(root.Length);
                f.Add(file, relative);
            }
            foreach (var subFolder in Directory.GetDirectories(folder))
            {
                AddFolderToZip(f, root, subFolder);
            }
        }
    }
}
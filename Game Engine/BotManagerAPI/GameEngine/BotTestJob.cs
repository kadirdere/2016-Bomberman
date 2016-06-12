using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Bomberman;
using BotManagerAPI.Data;
using GameEngine.Loggers;
using ICSharpCode.SharpZipLib.Zip;
using TestHarness.TestHarnesses.Bot;
using TestHarness.Util;

namespace BotManagerAPI.GameEngine
{
    public class BotTestJob
    {
        public ILogger Logger = new InMemoryLogger();

        public void ExecuteJob(int submissionId)
        {
            using (var db = new PlayerDashboardDB())
            {
                try
                {
                    var submission = db.Submissions.Single(x => x.SubmissionId == submissionId);

                    if (!submission.BuildOk)
                    {
                        return;
                    }

                    submission.MatchStarted = true;
                    db.SaveChanges();

                    var referenceBotDir = ConfigurationManager.AppSettings.Get("referenceBotDirectory");
                    var matchLogDir = ConfigurationManager.AppSettings.Get("testMatchLogDirectory");
                    var gameWorkDir = Path.Combine(matchLogDir, submissionId.ToString());

                    if (Directory.Exists(gameWorkDir))
                    {
                        Directory.Delete(gameWorkDir, true);
                    }

                    var paths = new String[2]
                    {
                        referenceBotDir,
                        submission.SubmissionPath
                    };
                    var options = new Options { BotFolders = paths, Log = gameWorkDir };
                    var game = new BombermanGame();
                    game.StartNewGame(options);

                    submission.Complete = true;
                    submission.MatchCompleteTimestamp = DateTime.Now;

                    var zipFilePath = Path.Combine(matchLogDir, String.Format("match-{0}.zip", submissionId));

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

                    submission.MatchDataPath = zipFilePath;
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    Console.WriteLine("Failed to compile submission " + submissionId);
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
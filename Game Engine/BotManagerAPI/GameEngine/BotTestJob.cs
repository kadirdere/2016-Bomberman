using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Bomberman;
using BotManagerAPI.Common;
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

                    var referenceBotDir = DirectorySettings.ReferenceBotDirectory;
                    var matchLogDir = DirectorySettings.TestMatchLogDirectory;
                    var rootMatchDir = Path.Combine(matchLogDir, submissionId.ToString());

                    var botsDir = Path.Combine(rootMatchDir, "Bots");
                    var gameWorkDir = Path.Combine(rootMatchDir, "Game");

                    if (Directory.Exists(gameWorkDir))
                    {
                        Directory.Delete(gameWorkDir, true);
                    }

                    Directory.CreateDirectory(botsDir);
                    Directory.CreateDirectory(gameWorkDir);

                    var botDir = Path.Combine(botsDir, submissionId.ToString());
                    DirectorySettings.CopyDirectory(DirectorySettings.GetBotSourcePath(submissionId), botDir, true);

                    var paths = new []
                    {
                        referenceBotDir,
                        botDir
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

                    Directory.Delete(rootMatchDir, true);

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
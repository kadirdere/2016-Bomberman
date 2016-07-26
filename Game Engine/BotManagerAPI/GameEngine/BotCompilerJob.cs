using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using BotManagerAPI.Common;
using BotManagerAPI.Data;
using GameEngine.Loggers;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using TestHarness.TestHarnesses.Bot;
using TestHarness.Util;

namespace BotManagerAPI.GameEngine
{
    public class BotCompilerJob
    {
        public ILogger Logger = new InMemoryLogger();

        public void ExecuteJob(int submissionId)
        {
            using (var db = new PlayerDashboardDB())
            {
                var submission = db.Submissions.Single(x => x.SubmissionId == submissionId);
                try
                {
                    submission.BuildStarted = true;
                    submission.Complete = false;
                    submission.BuildOk = false;
                    submission.MatchStarted = false;
                    submission.BuildLogPath = null;
                    submission.MatchDataPath = null;
                    submission.BuildLogPath = null;
                    submission.BuildCompleteTimestamp = null;
                    submission.MatchCompleteTimestamp = null;
                    db.SaveChanges();
                    Logger.LogInfo("Extracting bot");
                    if (!submission.SubmissionPath.EndsWith(".zip"))
                    {
                        Logger.LogException("Bot is not zipped!");
                        db.SaveChanges();
                        throw new ArgumentException("Invalid zip file");
                    }

                    try
                    {
                        ExtractBot(submission);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogException("Failed to extract bot files", ex);
                        throw;
                    }

                    var sourcePath = DirectorySettings.GetBotSourcePath(submissionId);

                    Logger.LogInfo("Compiling bot");
                    try
                    {
                        var botCompiler = new BotCompiler(BotMetaReader.ReadBotMeta(sourcePath),
                            sourcePath, Logger);
                        submission.BuildOk = botCompiler.Compile();
                    }
                    catch (Exception ex)
                    {
                        Logger.LogException("Failed to compile bot", ex);
                    }
                }
                catch (DbEntityValidationException e)
                {
                    Console.WriteLine("Failed to compile submission " + submissionId);
                    throw;
                }
                catch (Exception e)
                {
                    Logger.LogException(e);
                }

                submission.BuildLogPath = WriteLogFile(submissionId);
                submission.BuildCompleteTimestamp = DateTime.Now;
                db.SaveChanges();
            }
        }

        private string WriteLogFile(int submissionId)
        {
            var compileLogDir = DirectorySettings.CompileLogDirectory;
            Directory.CreateDirectory(compileLogDir);

            var logDir = Path.Combine(compileLogDir, (submissionId + "-compile.log"));
            File.WriteAllText(logDir, Logger.ReadAll(), Encoding.UTF8);

            return logDir;
        }

        private void ExtractBot(Submission submission)
        {
            var path = Path.Combine(DirectorySettings.BotSourceDirectory, submission.SubmissionId.ToString());

            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }

            Directory.CreateDirectory(path);

            var zf = new ZipFile(submission.SubmissionPath);
            try
            {
                ExtractZipToFolder(zf, path);
            }
            finally
            {
                zf.IsStreamOwner = true; // Makes close also shut the underlying stream
                zf.Close(); // Ensure we release resources
            }
        }

        public void ExtractZipToFolder(ZipFile f, string folder)
        {
            foreach (ZipEntry zipEntry in f)
            {
                if (!zipEntry.IsFile)
                {
                    continue;           // Ignore directories
                }
                var entryFileName = zipEntry.Name;
                Logger.LogInfo("Extracting file " + entryFileName);

                var buffer = new byte[4096];     // 4K is optimum
                var zipStream = f.GetInputStream(zipEntry);

                var fullZipToPath = Path.Combine(folder, entryFileName);
                var directoryName = Path.GetDirectoryName(fullZipToPath);
                if (!string.IsNullOrEmpty(directoryName))
                    Directory.CreateDirectory(directoryName);

                using (var streamWriter = File.Create(fullZipToPath))
                {
                    StreamUtils.Copy(zipStream, streamWriter, buffer);
                }
            }
        }
    }
}
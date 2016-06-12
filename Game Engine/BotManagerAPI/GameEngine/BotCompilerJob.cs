﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using BotManagerAPI.Data;
using GameEngine.Loggers;
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
                try
                {
                    var submission = db.Submissions.Single(x => x.SubmissionId == submissionId);

                    submission.BuildStarted = true;
                    db.SaveChanges();

                    var botCompiler = new BotCompiler(BotMetaReader.ReadBotMeta(submission.SubmissionPath),
                        submission.SubmissionPath, Logger);
                    submission.BuildOk = botCompiler.Compile();
                    submission.BuildCompleteTimestamp = DateTime.Now;

                    var compileLogDir = ConfigurationManager.AppSettings.Get("compileLogDirectory");
                    Directory.CreateDirectory(compileLogDir);

                    var logDir = Path.Combine(compileLogDir, (submissionId + "-compile.log"));
                    File.WriteAllText(logDir, Logger.ReadAll(), Encoding.UTF8);

                    submission.BuildLogPath = logDir;
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    Console.WriteLine("Failed to compile submission " + submissionId);
                    throw;
                }

            }
        }
    }
}
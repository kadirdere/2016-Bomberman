using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using TestHarness.Properties;

namespace BotManagerAPI.Common
{
    public static class DirectorySettings
    {
        public static string CompileLogDirectory
        {
            get { return ConfigurationManager.AppSettings.Get("compileLogDirectory"); }
        }

        public static string ReferenceBotDirectory
        {
            get { return ConfigurationManager.AppSettings.Get("referenceBotDirectory"); }
        }

        public static string TestMatchLogDirectory
        {
            get { return ConfigurationManager.AppSettings.Get("testMatchLogDirectory"); }
        }

        public static string MatchDirectory
        {
            get { return ConfigurationManager.AppSettings.Get("compMatchLogDirectory"); }
        }

        public static string BotSourceDirectory 
        {
            get { return ConfigurationManager.AppSettings.Get("botSourceDirectory"); }
        }

        public static string GetBotSourcePath(int submissionId)
        {
            var rootPath = Path.Combine(DirectorySettings.BotSourceDirectory, submissionId.ToString());
            const string botMetaFileName = "bot.json";

            foreach (var path in Directory.EnumerateFiles(rootPath, "*.json", SearchOption.AllDirectories))
            {
                if (path.EndsWith(botMetaFileName))
                    return Path.GetDirectoryName(path);
            }

            throw new FileNotFoundException("Bot meta could not be found in directory " + rootPath);
        }

        public static void CopyDirectory(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            var dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            var dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            var files = dir.GetFiles();
            foreach (var file in files)
            {
                var temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (!copySubDirs) return;
            foreach (var subdir in dirs)
            {
                var temppath = Path.Combine(destDirName, subdir.Name);
                CopyDirectory(subdir.FullName, temppath, copySubDirs);
            }
        }
    }
}
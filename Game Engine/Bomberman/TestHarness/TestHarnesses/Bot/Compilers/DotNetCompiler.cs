using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Domain.Meta;
using GameEngine.Loggers;
using TestHarness.Properties;
using TestHarness.Util;

namespace TestHarness.TestHarnesses.Bot.Compilers
{
    public class DotNetCompiler : ICompiler
    {
        private readonly BotMeta _botMeta;
        private readonly string _botDir;
        private readonly ILogger _compileLogger;
        private bool hasErrors;

        public DotNetCompiler(BotMeta botMeta, string botDir, ILogger compileLogger)
        {
            _botMeta = botMeta;
            _botDir = botDir;
            _compileLogger = compileLogger;
        }

        public bool HasPackageManager()
        {
            var path = Path.Combine(_botDir, "nuget.exe");
            var exists = File.Exists(path);

            _compileLogger.LogInfo("Checking if bot " + _botMeta.NickName + " has a nuget package manager exe at location " + _botDir);

            return exists;
        }

        public bool RunPackageManager()
        {
            hasErrors = false;
            if (!HasPackageManager()) return true;

            _compileLogger.LogInfo("Nuget Package manager Found, running restore");
            using (var handler = new ProcessHandler(_botDir, Path.Combine(_botDir, "nuget.exe"), "restore", _compileLogger))
            {
                handler.ProcessToRun.ErrorDataReceived += ProcessErrorRecieved;
                handler.ProcessToRun.OutputDataReceived += ProcessDataRecieved;
                    
                return handler.RunProcess() == 0 && !hasErrors;
            }
        }

        public bool RunCompiler()
        {
            hasErrors = false;
            _compileLogger.LogInfo("Compiling bot " + _botMeta.NickName + " in location " + _botMeta.ProjectLocation + " using .Net");
            using (var handler = new ProcessHandler(Path.Combine(_botDir, _botMeta.ProjectLocation??""), Settings.Default.PathToMSBuild, "/t:rebuild /p:Configuration=Release", _compileLogger))
            {
                handler.ProcessToRun.ErrorDataReceived += ProcessErrorRecieved;
                handler.ProcessToRun.OutputDataReceived += ProcessDataRecieved;

                return handler.RunProcess() == 0 && !hasErrors;
            }
        }

        void ProcessDataRecieved(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            _compileLogger.LogInfo(e.Data);
        }

        void ProcessErrorRecieved(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            if (!String.IsNullOrEmpty(e.Data))
            {
                hasErrors = true;
                _compileLogger.LogException(e.Data);
            }
        }
    }
}

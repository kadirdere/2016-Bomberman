using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Domain.Meta;
using GameEngine.Loggers;
using TestHarness.Properties;
using TestHarness.Util;

namespace TestHarness.TestHarnesses.Bot.Compilers
{
    public class JavaScriptCompiler : ICompiler
    {
        private readonly BotMeta _botMeta;
        private readonly string _botDir;
        private readonly ILogger _compileLogger;
        private bool _failed;

        public JavaScriptCompiler(BotMeta botMeta, string botDir, ILogger compileLogger)
        {
            _botMeta = botMeta;
            _botDir = botDir;
            _compileLogger = compileLogger;
        }

        public bool HasPackageManager()
        {
            var path = Path.Combine(_botDir, "package.json");
            var exists = File.Exists(path);

            _compileLogger.LogInfo("Checking if bot " + _botMeta.NickName + " has a package.json file " + _botDir);

            return exists;
        }

        public bool RunPackageManager()
        {
            _failed = false;
            if (!HasPackageManager()) return true;

            _compileLogger.LogInfo("Found package.json, doing install");
            using (var handler = new ProcessHandler(_botDir, Settings.Default.PathToNpm, "install", _compileLogger))
            {
                handler.ProcessToRun.ErrorDataReceived += ProcessErrorRecieved;
                handler.ProcessToRun.OutputDataReceived += ProcessDataRecieved;

                return handler.RunProcess() == 0 && !_failed;
            }
        }

        public bool RunCompiler()
        {
            _compileLogger.LogInfo("Compiling bot " + _botMeta.NickName + " using JavaScript");
            return true;
        }

        void ProcessDataRecieved(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            if (!String.IsNullOrEmpty(e.Data))
            {
                _failed = _failed || HasNpmWarning(e.Data);
                _compileLogger.LogInfo(e.Data);
            }
        }

        void ProcessErrorRecieved(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            if (!String.IsNullOrEmpty(e.Data))
            {
                _failed = _failed || HasNpmWarning(e.Data);
                _compileLogger.LogException(e.Data);
            }
        }

        private static bool HasNpmWarning(String data)
        {
            return data.Contains("npm ERR!") || data.Contains("npm WARN EJSONPARS");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TestHarness.TestHarnesses.Bot;
using TestHarness.Util;
using Domain;
using GameEngine.Loggers;
using System.Threading.Tasks;
using Bomberman;

namespace BotManagerAPI.Controllers
{
    public class BotController : ApiController
    {
        public ILogger Logger = new ConsoleLogger();

        [Route("CompileBot")]
        [HttpGet]
        public IHttpActionResult CompileBot(int botId)
        {
            String path = "C:\\Development\\entelect-100k.bot-runner\\Sample Bots\\C#";
            BotCompiler botCompiler = new BotCompiler(BotMetaReader.ReadBotMeta(path), path, Logger);
            bool result = botCompiler.Compile();
            return Ok(result);
        }

        [Route("ExecuteBot")]
        [HttpGet]
        public IHttpActionResult ExecuteBot(int botId)
        {
            String[] paths = new String[2] {
                "C:\\Development\\entelect-100k.bot-runner\\Sample Bots\\C#",
                "C:\\Development\\entelect-100k.bot-runner\\Sample Bots\\C#"
            };
            String workDir = "C:\\Development\\entelect-100k.bot-runner\\Sample Bots\\C#\\output";
            var options = new Options();
            options.BotFolders = paths;
            options.Log = workDir;
            options.GameSeed = 2;
            BombermanGame BombermanGame = new BombermanGame();
            BombermanGame.StartNewGame(options);
            return Ok(true);
        }

        [Route("ExecuteBots")]
        [HttpGet]
        public IHttpActionResult ExecuteBots(int botOneId, int botTwoId)
        {
            String[] paths = new String[2] {
                "C:\\Development\\entelect-100k.bot-runner\\Sample Bots\\C#",
                "C:\\Development\\entelect-100k.bot-runner\\Sample Bots\\C#"
            };
            String workDir = "C:\\Development\\entelect-100k.bot-runner\\Sample Bots\\C#\\output";
            var options = new Options();
            options.BotFolders = paths;
            options.Log = workDir;
            options.GameSeed = 2;
            BombermanGame BombermanGame = new BombermanGame();
            BombermanGame.StartNewGame(options);
            return Ok(true);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BotManagerAPI.GameEngine;
using Hangfire;
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
            var id = BackgroundJob.Enqueue(() => new BotCompilerJob().ExecuteJob(botId));
            BackgroundJob.ContinueWith(id, () => new BotTestJob().ExecuteJob(botId));


            return Ok();
        }

        [Route("ExecuteBot")]
        [HttpGet]
        public IHttpActionResult ExecuteBot(int botId)
        {
            BackgroundJob.Enqueue(() => new BotTestJob().ExecuteJob(botId));
            
            return Ok();
        }

        [Route("ExecuteBots")]
        [HttpGet]
        public IHttpActionResult ExecuteBots(int playerOneId, int playerTwoId)
        {
            BackgroundJob.Enqueue(() => new MatchRunnerJob().ExecuteJob(playerOneId, playerTwoId));
            
            return Ok();
        }
    }
}

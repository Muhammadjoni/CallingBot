using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CallingBotSample.Bots;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph.Communications.Common;
using Microsoft.Graph.Communications.Common.Telemetry;
using Microsoft.Graph.Communications.Core.Serialization;

namespace CallingBotSample.Controllers
{
  public class JoinCallController : Controller
  {
    private readonly  IGraphLogger GraphLogger;
    private CallingBot bot;

    public JoinCallController(CallingBot bot)
    {
      this.GraphLogger = GraphLogger.CreateShim(nameof(JoinCallController));

      this.bot = bot;
    }

//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


    [HttpPost]
    [Route("joincall")]

    public async Task<IActionResult> JoinCallAsync([FromBody] JoinCallRequestData joinCallBody)
    {
      Validator.NotNull(joinCallBody, nameof(joinCallBody));

      try
      {
        var call = await this.GraphLogger.JoinCallAsync(joinCallBody).ConfigureAwait(false);

        var callUriTemplate = new UriBuilder(this.bot.BotInstanceUri);
        callUriTemplate.Path = HttpRouteConstants.CallRoutePrefix.Replace("{callLegId}", call.Id);
        callUriTemplate.Query = this.bot.BotInstanceUri.Query.Trim('?');

        var callUri = callUriTemplate.Uri.AbsoluteUri;
        var values = new Dictionary<string, string>
                {
                    { "legId", call.Id },
                    { "scenarioId", call.ScenarioId.ToString() },
                    { "call", callUri },
                    { "logs", callUri.Replace("/calls/", "/logs/") },
                };

        var serializer = new CommsSerializer(pretty: true);
        var json = serializer.SerializeObject(values);
        return this.Ok(json);
      }
      catch (Exception e)
      {
        return this.Exception(e);
      }
    }

  }
}

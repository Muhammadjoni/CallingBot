using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// using System.Linq;
using System.Threading.Tasks;
using CallingBotSample.Bots;
using CallingBotSample.DB;
using CallingBotSample.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CallingBotSample.Controllers
{
  [ApiController]
  [Route("callback")]
  public class CallbackController : Controller
  {
    private readonly CallingBot bot;
    private readonly ApplicationDbContext context;

    public CallbackController(CallingBot bot, ApplicationDbContext context)
    {
      this.bot = bot;
      this.context = context;
    }

    [HttpPost, HttpGet]
    public async Task HandleCallbackRequestAsync()
    {
      await this.bot.ProcessNotificationAsync(this.Request, this.Response).ConfigureAwait(false);
    }


/*
    [HttpGet("participants")]
    public IEnumerable<String> GetAllParticipants()
    {
      List<String> participantIds  = context.ParticipantDetails.Select(x => x.UserId).Distinct().ToList();
      List<String> participants = new List<string>();

          foreach (var u in participants)
          {
            participants.Add(context.ParticipantDetails.Where(d => d.UserId.Equals(u)).Select(x => x.UserId).Single());
          }

      return participants;

      // var participants = (args.ResourceData as ICollection<object>).Select(x => x as Participant);
      // List<Participant> currentParticipants = new List<Participant>();
      // currentParticipants = participants.ToList();

      // return currentParticipants;
    }

    [HttpGet("participants/{id}")]
    public async Task GetParticipantById(string id)
    {
      try
        {
          var Ids = context.ParticipantDetails.Where(d => d.UserId.Equals(id)).ToList();

          Response.StatusCode = 200;
          Response.ContentType = "application/json";
          await Response.Body.WriteAsync(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(Ids)));
        }
        catch (Exception e)
        {
          Response.StatusCode = 400;
          Response.ContentType = "application/json";
          await Response.Body.WriteAsync(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(e)));
        }
    }
*/
    [HttpGet("state")]
    public async Task GetCallState()
    {

    }


  }
}

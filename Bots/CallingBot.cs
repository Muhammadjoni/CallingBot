// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using CallingBotSample.Interfaces;
using CallingBotSample.Utility;
using CallingMeetingBot.Extenstions;
using Microsoft.AspNetCore.Http;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Teams;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Microsoft.Graph.Communications.Client.Authentication;
using Microsoft.Graph.Communications.Common.Telemetry;
using Microsoft.Graph.Communications.Core.Notifications;
using Microsoft.Graph.Communications.Core.Serialization;
using System;
using System.IO;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Linq;

namespace CallingBotSample.Bots
{
    public class CallingBot : ActivityHandler
    {
        private readonly IConfiguration configuration;
        public IGraphLogger GraphLogger { get; }

        private IRequestAuthenticationProvider AuthenticationProvider { get; }

        private INotificationProcessor NotificationProcessor { get; }
        private CommsSerializer Serializer { get; }
        private readonly BotOptions options;

        private readonly ICard card;
        private readonly IGraph graph;
        private readonly IGraphServiceClient graphServiceClient;




        public CallingBot(BotOptions options, IConfiguration configuration, ICard card, IGraph graph, IGraphServiceClient graphServiceClient, IGraphLogger graphLogger)
        {
            this.options = options;
            this.configuration = configuration;
            this.card = card;
            this.graph = graph;
            this.graphServiceClient = graphServiceClient;
            this.GraphLogger = graphLogger;

            var name = this.GetType().Assembly.GetName().Name;
            this.AuthenticationProvider = new AuthenticationProvider(name, options.AppId, options.AppSecret, graphLogger);

            this.Serializer = new CommsSerializer();
            this.NotificationProcessor = new NotificationProcessor(Serializer);
            this.NotificationProcessor.OnNotificationReceived += this.NotificationProcessor_OnNotificationReceived;
        }


        public async Task ProcessNotificationAsync(
            HttpRequest request,
            HttpResponse response)
        {
            try
            {
                var httpRequest = request.CreateRequestMessage();
                var results = await this.AuthenticationProvider.ValidateInboundRequestAsync(httpRequest).ConfigureAwait(false);
                if (results.IsValid)
                {
                    var httpResponse = await this.NotificationProcessor.ProcessNotificationAsync(httpRequest).ConfigureAwait(false);
                    await httpResponse.CreateHttpResponseAsync(response).ConfigureAwait(false);



                    // var options = new JsonSerializerOptions { WriteIndented = true };
                    // string jsonString = JsonConvert.SerializeObject(httpRequest);

                    // Console.WriteLine(jsonString);

                    // var fileName = "HttpResponse.json";
                    // var jsonString = JsonSerializer.Serialize(httpResponse);
                    // File.WriteAllText(fileName, jsonString);

                    // Console.WriteLine(File.ReadAllText(fileName));

                }
                else
                {
                    var httpResponse = httpRequest.CreateResponse(HttpStatusCode.Forbidden);
                    await httpResponse.CreateHttpResponseAsync(response).ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await response.WriteAsync(e.ToString()).ConfigureAwait(false);
            }
        }

    // var participants = (args.ResourceData as ICollection<object>).Select(x => x as Participant);
    // List<Participant> currentParticipants = new List<Participant>();
    // currentParticipants = participants.ToList();

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var credentials = new MicrosoftAppCredentials(this.configuration[Common.Constants.MicrosoftAppIdConfigurationSettingsKey], this.configuration[Common.Constants.MicrosoftAppPasswordConfigurationSettingsKey]);
            ConversationReference conversationReference = null;
            foreach (var member in membersAdded)
            {

                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    var proactiveMessage = MessageFactory.Attachment(this.card.GetWelcomeCardAttachment());
                    proactiveMessage.TeamsNotifyUser();
                    var conversationParameters = new ConversationParameters
                    {
                        IsGroup = false,
                        Bot = turnContext.Activity.Recipient,
                        Members = new ChannelAccount[] { member },
                        TenantId = turnContext.Activity.Conversation.TenantId
                    };
                    await ((BotFrameworkAdapter)turnContext.Adapter).CreateConversationAsync(
                        turnContext.Activity.TeamsGetChannelId(),
                        turnContext.Activity.ServiceUrl,
                        credentials,
                        conversationParameters,
                        async (t1, c1) =>
                        {
                            conversationReference = t1.Activity.GetConversationReference();
                            await ((BotFrameworkAdapter)turnContext.Adapter).ContinueConversationAsync(
                                configuration[Common.Constants.MicrosoftAppIdConfigurationSettingsKey],
                                conversationReference,
                                async (t2, c2) =>
                                {
                                    await t2.SendActivityAsync(proactiveMessage, c2);
                                },
                                cancellationToken);
                        },
                        cancellationToken);
                }
            }
        }


        private async Task SendReponse(ITurnContext<IMessageActivity> turnContext, string input, CancellationToken cancellationToken)
        {
            switch (input)
            {
                case "joinscheduledmeeting":
                    // var onlineMeeting = await graph.CreateOnlineMeetingAsync();
                    // if (onlineMeeting != null)
                    // {
                        var statefullCall = await graph.JoinScheduledMeeting(this.configuration[Common.Constants.ThreadIdConfigurationSettingsKey], this.configuration[Common.Constants.UserIdConfigurationSettingsKey]);
                        if (statefullCall != null)
                        {
                            // var JoinWebUrl = "https://teams.microsoft.com/l/meetup-join/19%3ameeting_NDI3NTk5MTYtZDg2NC00ZjY2LTlhZjctY2YxNjZmNjc5NjYy%40thread.v2/0?context=%7b%22Tid%22%3a%22603439c3-58ad-4a91-8ed3-b53e9a8677b3%22%2c%22Oid%22%3a%222bef8a9f-25bf-4959-9851-ec0ca99f023a%22%7d";
                            await turnContext.SendActivityAsync($"[Click here to Join the meeting]({this.configuration[Common.Constants.MeetingUrlConfigurationSettingsKey]})");
                        }
                    // }
                    break;
                // case "createcall":
                //     var call = await graph.CreateCallAsync();
                //     if (call != null)
                //     {
                //         await turnContext.SendActivityAsync("Placed a call Successfully.");
                //     }
                //     break;
                // case "transfercall":
                //     var sourceCallResponse = await graph.CreateCallAsync();
                //     if (sourceCallResponse != null)
                //     {
                //         await turnContext.SendActivityAsync("Transferring the call!");
                //         await graph.TransferCallAsync(sourceCallResponse.Id);
                //     }
                //     break;
                // case "inviteparticipant":
                //     var meeting = await graph.CreateOnlineMeetingAsync();
                //     if (meeting != null)
                //     {
                //         var statefullCall = await graph.JoinScheduledMeeting(meeting.JoinWebUrl);
                //         if (statefullCall != null)
                        // {
                //             graph.InviteParticipant(statefullCall.Id);
                //             await turnContext.SendActivityAsync("Invited participant successfuly");
                //         }
                //     }
                //     break;
                default:
                    await turnContext.SendActivityAsync("Welcome to bot");
                    break;
            }
        }

        private void NotificationProcessor_OnNotificationReceived(NotificationEventArgs args)
        {
            _ = NotificationProcessor_OnNotificationReceivedAsync(args).ForgetAndLogExceptionAsync(
              this.GraphLogger,
              $"Error processing notification {args.Notification.ResourceUrl} with scenario {args.ScenarioId}");
        }

        private async Task NotificationProcessor_OnNotificationReceivedAsync(NotificationEventArgs args)
        {
            this.GraphLogger.CorrelationId = args.ScenarioId;
            if (args.ResourceData is Call call)
            {
                if (args.ChangeType == ChangeType.Created && call.State == CallState.Incoming)
                {
                    await this.BotAnswerIncomingCallAsync(call.Id, args.TenantId, args.ScenarioId).ConfigureAwait(false);
                }

                var participants = (args.ResourceData as ICollection<object>).Select(x => x as Participant);
                List<Participant> currentParticipants = new List<Participant>();
                currentParticipants = participants.ToList();
                Console.WriteLine(participants);
            }
        }

        // private async Task TraceCallback(NotificationEventArgs args, ITurnContext<IMessageActivity> turnContext )
        // {
        //   var activity = turnContext.Activity;

        //     if (args.RequestId == this.configuration[Common.Constants.MuteMicConfigurationSettingsKey])
        //     {
        //       await activity.
        //     }
        // }

        private async Task BotAnswerIncomingCallAsync(string callId, string tenantId, Guid scenarioId)
        {
            Task answerTask = Task.Run(async () =>
                                await this.graphServiceClient.Communications.Calls[callId].Answer(
                                    callbackUri: new Uri(options.BotBaseUrl, "callback").ToString(),
                                    mediaConfig: new ServiceHostedMediaConfig
                                    {
                                        PreFetchMedia = new List<MediaInfo>()
                                        {
                                            new MediaInfo()
                                            {
                                                Uri = new Uri(options.BotBaseUrl, "audio/speech.wav").ToString(),
                                                ResourceId = Guid.NewGuid().ToString(),
                                            }
                                        }
                                    },
                                    acceptedModalities: new List<Modality> { Modality.Audio }).Request().PostAsync()
                                 );

            await answerTask.ContinueWith(async (antecedent) =>
            {

                if (antecedent.Status == System.Threading.Tasks.TaskStatus.RanToCompletion)
                {
                    await Task.Delay(5000);
                    await graphServiceClient.Communications.Calls[callId].PlayPrompt(
                       prompts: new List<Microsoft.Graph.Prompt>()
                       {
                           new MediaPrompt
                           {
                               MediaInfo = new MediaInfo
                               {
                                   Uri = new Uri(options.BotBaseUrl, "audio/speech.wav").ToString(),
                                   ResourceId = Guid.NewGuid().ToString(),
                               }
                           }
                       })
                       .Request()
                       .PostAsync();
                }
            }
          );
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(turnContext.Activity.Text))
            {
                dynamic value = turnContext.Activity.Value;
                if (value != null)
                {
                    string type = value["type"];
                    type = string.IsNullOrEmpty(type) ? "." : type.ToLower();
                    await SendReponse(turnContext, type, cancellationToken);
                }
            }
            else
            {
                await SendReponse(turnContext, turnContext.Activity.Text.Trim().ToLower(), cancellationToken);
            }
        }
    }
}

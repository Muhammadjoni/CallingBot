// <copyright file="JoinInfo.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CallingBotSample.Helpers
{
    using System;
    using System.IO;
    using System.Net;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using System.Text.RegularExpressions;
    using Microsoft.Graph;

    /// <summary>
    /// Gets the join information.
    /// </summary>
    public class JoinInfo
    {
        /// <summary>
        /// Parse Join URL into its components.
        /// </summary>
        /// <param name="joinURL">Join URL from Team's meeting body.</param>
        /// <returns>Parsed data.</returns>
        // public static (ChatInfo, MeetingInfo) ParseJoinURL(string joinURL)
        // {
            // var joinURL = "https://teams.microsoft.com/l/meetup-join/19%3ameeting_OTU1N2NkYTEtMjg2ZS00NjkyLTgzNzMtMzI4M2M2MDA3ZmQy%40thread.v2/0?context=%7b%22Tid%22%3a%22a264f13b-3b8d-4bee-a4a6-06546dadfe7c%22%2c%22Oid%22%3a%224f2a04eb-0e7d-42a5-97dc-a58c55de060e%22%7d";
            // var decodedURL = WebUtility.UrlDecode(joinURL);

    //// URL being needs to be in this format.
    //// https://teams.microsoft.com/l/meetup-join/19:cd9ce3da56624fe69c9d7cd026f9126d@thread.skype/1509579179399?context={"Tid":"72f988bf-86f1-41af-91ab-2d7cd011db47","Oid":"550fae72-d251-43ec-868c-373732c2704f","MessageId":"1536978844957"}
    //// https://teams.microsoft.com/l/meetup-join/19:meeting_MDQzYmJlMDctMWJiZS00OGExLTlmYjUtZTczNzVhZGM1OTQx@thread.v2/0?context={"Tid":"c80f38d3-c04c-49bf-a48b-9d99278d4ac6","Oid":"782f076f-f6f9-4bff-9673-ea1997283e9c"}
    /////    https://teams.microsoft.com/l/meetup-join/19%3ameeting_MjExYmVmNTQtYTYzYy00Mzc5LThiNzQtMmE5YmRlYWZkODc0%40thread.v2/0?context=%7b%22Tid%22%3a%22603439c3-58ad-4a91-8ed3-b53e9a8677b3%22%2c%22Oid%22%3a%222bef8a9f-25bf-4959-9851-ec0ca99f023a%22%7d
    // https://teams.microsoft.com/l/meetup-join/19%3Ameeting_YzZkOGQ2MzQtNTYwNi00ZjY4LThiMjgtOTBjNDQxODY5Yzk0%40thread.v2/0?context=%7B%22Tid%22%3A%22603439c3-58ad-4a91-8ed3-b53e9a8677b3%22%2C%22Oid%22%3A%222bef8a9f-25bf-4959-9851-ec0ca99f023a%22%2C%22MessageId%22%3A%220%22%7D

          //   var regex = new Regex("https://teams\\.microsoft\\.com.*/(?<thread>[^/]+)/(?<message>[^/]+)\\?context=(?<context>{.*})");
          //   var match = regex.Match(decodedURL);
          //   if (!match.Success)
          //   {
          //     throw new ArgumentException($"Join URL cannot be parsed: {joinURL}.", nameof(joinURL));
          //   }

          //   using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(match.Groups["context"].Value)))
          //   {
          //     var ctxt = (Context)new DataContractJsonSerializer(typeof(Context)).ReadObject(stream);
          //     var chatInfo = new ChatInfo
          //     {
          //       ThreadId = match.Groups["thread"].Value,
          //       MessageId = match.Groups["message"].Value,
          //       ReplyChainMessageId = ctxt.MessageId,
          //     };

          //     var meetingInfo = new OrganizerMeetingInfo
          //     {
          //       Organizer = new IdentitySet
          //       {
          //         User = new Identity { Id = ctxt.Oid },
          //       },
          //     };
          //     meetingInfo.Organizer.User.SetTenantId(ctxt.Tid);

          //     return (chatInfo, meetingInfo);
          //   }
          // }

      // var regex = new Regex("https://teams\\.microsoft\\.com.*/(?<thread>[^/]+)/(?<message>[^/]+)\\?context=(?<context>{.*})");
      //       var match = regex.Match(decodedURL);
      //       if (!match.Success)
      //       {
      //           throw new ArgumentException($"Join URL cannot be parsed: {joinURL}.", nameof(joinURL));
      //       }

      //       using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(match.Groups["context"].Value)))
      //       {
      //           var ctxt = (Context)new DataContractJsonSerializer(typeof(Context)).ReadObject(stream);
      //           var chatInfo = new ChatInfo
      //           {
      //               ThreadId = "19:meeting_NWI5NTM2YmQtOGNhNi00YmZmLWIzMjYtN2JhNTgyZTU3ZjEw@thread.v2",
      //               MessageId = "0",
      //               ReplyChainMessageId = null
      //           };

      //           var meetingInfo = new OrganizerMeetingInfo
      //           {
      //               Organizer = new IdentitySet
      //               {
      //                   User = new Identity { Id = "4f2a04eb-0e7d-42a5-97dc-a58c55de060e"},
      //               },
      //           };
      //           meetingInfo.Organizer.User.SetTenantId("a264f13b-3b8d-4bee-a4a6-06546dadfe7c");

      //           return (chatInfo, meetingInfo);
      //       }
      //   }

        // /// <summary>
        // /// Join URL context.
        // /// </summary>
        // [DataContract]
        private class Context
        {
            /// <summary>
            /// Gets or sets the Tenant Id.
            /// </summary>
            [DataMember]
            public string Tid { get; set; }

            /// <summary>
            /// Gets or sets the AAD object id of the user.
            /// </summary>
            [DataMember]
            public string Oid { get; set; }

            /// <summary>
            /// Gets or sets the chat message id.
            /// </summary>
            [DataMember]
            public string MessageId { get; set; }
        }
    }
}

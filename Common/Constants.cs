using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CallingBotSample.Common
{
    public class Constants
    {

        /// <summary>
        /// Azure Client Id.
        /// </summary>
        public const string ClientIdConfigurationSettingsKey = "AzureAd:ClientId";

        /// <summary>
        /// Azure Tenant Id.
        /// </summary>
        public const string TenantIdConfigurationSettingsKey = "TenantId";
        public const string ThreadIdConfigurationSettingsKey = "threadId";
        /// <summary>
        /// Azure ClientSecret .
        /// </summary>
        public const string MeetingUrlConfigurationSettingsKey = "MeetingURL";
        public const string ClientSecretConfigurationSettingsKey = "AzureAd:ClientSecret";

        public const string UserIdConfigurationSettingsKey = "UserId";

        public const string MicrosoftAppPasswordConfigurationSettingsKey = "MicrosoftAppPassword";

        public const string MicrosoftAppIdConfigurationSettingsKey = "MicrosoftAppId";

        public const string BotBaseUrlConfigurationSettingsKey = "BotBaseUrl";
    }
}

/* ===================================================
 * 
 * This file is part of the DM Jukebox project.
 * Copyright (c) 2016 Joe Clapis. All Rights Reserved.
 * 
 * =================================================== */

using Newtonsoft.Json;

namespace DMJukebox.Discord
{
    /// <summary>
    /// This describes some information about a user.
    /// </summary>
    /// <remarks>
    /// For more information, please see the documentation at
    /// https://discordapp.com/developers/docs/topics/gateway.
    /// </remarks>
    [JsonObject]
    internal class UserInfo
    {
        /// <summary>
        /// True if the email for this account has been verified, false if
        /// it has not (bot accounts are auto-verified even though they don't
        /// have an email address).
        /// </summary>
        [JsonProperty("verified")]
        public bool IsVerified { get; set; }

        /// <summary>
        /// The user's username.
        /// </summary>
        [JsonProperty("username")]
        public string Username { get; set; }

        /// <summary>
        /// True if the account has two-factor authentication enabled,
        /// false if it doesn't.
        /// </summary>
        [JsonProperty("mfa_enabled")]
        public bool IsTwoFactorAuthEnabled { get; set; }

        /// <summary>
        /// The unique ID for the user
        /// </summary>
        [JsonProperty("id")]
        public string ID { get; set; }

        /// <summary>
        /// The user's email address (this is null for bots)
        /// </summary>
        [JsonProperty("email")]
        public string EmailAddress { get; set; }

        /// <summary>
        /// This is the four-digit discriminator tag added to the username
        /// to uniquely identify this user among others with the same username
        /// </summary>
        [JsonProperty("discriminator")]
        public string Discriminator { get; set; }

        /// <summary>
        /// True if this user is an OAuth2 application bot, false if it's
        /// just a normal user.
        /// </summary>
        [JsonProperty("bot")]
        public bool IsBot { get; set; }

        /// <summary>
        /// The hash for the user's avatar.
        /// </summary>
        [JsonProperty("avatar")]
        public string AvatarHash { get; set; }
    }
}

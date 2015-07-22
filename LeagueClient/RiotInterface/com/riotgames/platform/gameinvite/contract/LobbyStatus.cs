﻿using System;
using RtmpSharp.IO;

namespace LeagueClient.RiotInterface.Riot.Platform
{
    [Serializable]
    [SerializedName("com.riotgames.platform.gameinvite.contract.LobbyStatus")]
    public class LobbyStatus
    {
        [SerializedName("chatKey")]
        public String ChatKey { get; set; }

        [SerializedName("gameMetaData")]
        public String GameData { get; set; }

        [SerializedName("owner")]
        public Player Owner { get; set; }

        [SerializedName("members")]
        public Member[] PlayerIds { get; set; }

        [SerializedName("invitees")]
        public Invitee[] InvitedPlayers { get; set; }

        [SerializedName("invitationId")]
        public String InvitationID { get; set; }
    }
}

﻿using System;
using RtmpSharp.IO;

namespace LeagueClient.RiotInterface.Riot.Platform
{
    [Serializable]
    [SerializedName("com.riotgames.platform.reroll.pojo.StoreAccountBalanceNotification")]
    public class StoreAccountBalanceNotification
    {
        [SerializedName("rp")]
        public Double Rp { get; set; }

        [SerializedName("ip")]
        public Double Ip { get; set; }
    }
}

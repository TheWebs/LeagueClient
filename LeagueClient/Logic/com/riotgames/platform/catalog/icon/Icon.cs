﻿using System;
using RtmpSharp.IO;

namespace LeagueClient.Logic.Riot.Platform
{
    [Serializable]
    [SerializedName("com.riotgames.platform.catalog.icon.Icon")]
    public class Icon
    {
        [SerializedName("purchaseDate")]
        public DateTime PurchaseDate { get; set; }

        [SerializedName("iconId")]
        public Int32 IconId { get; set; }

        [SerializedName("summonerId")]
        public Double SummonerId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using RtmpSharp.IO;

namespace LeagueClient.Logic.Riot.Platform {

  [Serializable]
  [SerializedName("com.riotgames.platform.messaging.ClientLoginKickNotification")]
  public class ClientLoginKickNotification : RiotException {
    [SerializedName("sessionToken")]
    public String sessionToken { get; set; }
  }
}

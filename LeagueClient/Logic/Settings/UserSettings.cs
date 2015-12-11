﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using MFroehlich.Parsing;
using MFroehlich.Parsing.DynamicJSON;
using System.Xml.Serialization;

namespace LeagueClient.Logic.Settings {
  public class UserSettings : ISettings {
    public string Username { get; set; }
    public string Password { get; set; }
    public int ProfileIcon { get; set; }
    public string SummonerName { get; set; }

    public string StatusMessage { get; set; }
  }
}
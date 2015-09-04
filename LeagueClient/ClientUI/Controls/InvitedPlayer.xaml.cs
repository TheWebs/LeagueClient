﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LeagueClient.Logic.Riot.Platform;

namespace LeagueClient.ClientUI.Controls {
  /// <summary>
  /// Interaction logic for InvitedPlayer.xaml
  /// </summary>
  public partial class InvitedPlayer : UserControl {
    public InvitedPlayer(Invitee player) {
      InitializeComponent();

      NameText.Content = player.SummonerName;
      switch (player.InviteeState) {
        case "PENDING": StateText.Content = "Pending"; break;
        case "ACCEPTED": StateText.Content = "Accepted"; break;
        case "QUIT": StateText.Content = "Quit"; break;
        default: break;
      }
    }
  }
}
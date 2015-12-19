﻿using LeagueClient.Logic;
using MFroehlich.League.Assets;
using MFroehlich.League.DataDragon;
using MFroehlich.League.RiotAPI;
using System;
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

namespace LeagueClient.ClientUI.Controls {
  /// <summary>
  /// Interaction logic for MatchDetails.xaml
  /// </summary>
  public partial class MatchDetails : UserControl {
    private Action back;
    public MatchDetails(RiotACS.Game game, RiotACS.Delta delta, Action back = null) {
      InitializeComponent();

      this.back = back;
      if (back == null) BackButton.Visibility = Visibility.Collapsed;

      var blue = new List<object>();
      var red = new List<object>();
      foreach (var player in game.Participants) {
        var champ = LeagueData.GetChampData(player.ChampionId);
        var spell1 = LeagueData.GetSpellData(player.Spell1Id);
        var spell2 = LeagueData.GetSpellData(player.Spell2Id);

        var items = new[] { player.Stats.Item0, player.Stats.Item1, player.Stats.Item2, player.Stats.Item3, player.Stats.Item4, player.Stats.Item5, player.Stats.Item6 };
        var datas = new ItemDto[7];
        for (int i = 0; i < items.Length; i++) {
          if (items[i] == 0) continue;
          var data = LeagueData.ItemData.Value.data[items[i].ToString()];
          datas[i] = data;
        }

        var item = new {
          ChampImage = LeagueData.GetChampIconImage(champ),
          Spell1Image = LeagueData.GetSpellImage(spell1),
          Spell2Image = LeagueData.GetSpellImage(spell2),
          Name = champ.name,
          Score = $"{player.Stats.Kills} / {player.Stats.Deaths} / {player.Stats.Assists}",
          Item0Image = datas[0] == null ? null : LeagueData.GetItemImage(datas[0]),
          Item1Image = datas[1] == null ? null : LeagueData.GetItemImage(datas[1]),
          Item2Image = datas[2] == null ? null : LeagueData.GetItemImage(datas[2]),
          Item3Image = datas[3] == null ? null : LeagueData.GetItemImage(datas[3]),
          Item4Image = datas[4] == null ? null : LeagueData.GetItemImage(datas[4]),
          Item5Image = datas[5] == null ? null : LeagueData.GetItemImage(datas[5]),
          Item6Image = datas[6] == null ? null : LeagueData.GetItemImage(datas[6]),
          CS = player.Stats.TotalMinionsKilled,
          Gold = (player.Stats.GoldEarned / 1000.0).ToString("#.#k")
        };

        if (player.TeamId == 100) blue.Add(item);
        else red.Add(item);
      }
      BlueTeam.ItemsSource = blue;
      RedTeam.ItemsSource = red;
    }

    private void BackButton_Click(object sender, RoutedEventArgs e) {
      back();
    }

    private void OverviewButton_Click(object sender, RoutedEventArgs e) {

    }

    private void GraphsButton_Click(object sender, RoutedEventArgs e) {

    }
  }
}

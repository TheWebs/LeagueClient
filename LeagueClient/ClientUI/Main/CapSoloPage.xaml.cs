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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LeagueClient.ClientUI.Controls;
using LeagueClient.Logic.Riot;
using MFroehlich.League.Assets;
using MFroehlich.League.DataDragon;
using static LeagueClient.Logic.Strings;

namespace LeagueClient.ClientUI.Main {
  /// <summary>
  /// Interaction logic for TeambuilderSoloPage.xaml
  /// </summary>
  public partial class CapSoloPage : Page, IClientSubPage {
    private bool spell1;

    public event EventHandler Close;

    public CapSoloPage() {
      InitializeComponent();
      ChampSelector.Champions = Client.AvailableChampions;
      SpellSelector.Spells = (from spell in LeagueData.SpellData.Value.data.Values
                              where spell.modes.Contains("CLASSIC")
                              select spell);

      Player.Editable = true;
      Player.PlayerUpdate += PlayerUpdate;
      Player.ChampClicked += Champion_Click;
      Player.Spell1Clicked += Spell1_Click;
      Player.Spell2Clicked += Spell2_Click;
      Player.MasteryClicked += Player_MasteryClicked;
    }

    private void PlayerUpdate(object sender, EventArgs e) {
      GameMap.Players.Clear();
      GameMap.Players.Add(Player.State);
      if (Player.CanBeReady()) EnterQueueButt.BeginStoryboard(App.FadeIn);
      else EnterQueueButt.BeginStoryboard(App.FadeOut);
    }

    private void Player_MasteryClicked(object src, EventArgs args) {
      ChampPopup.BeginStoryboard(App.FadeIn);
      MasteryEditor.Reset();
      ChampSelector.Visibility = System.Windows.Visibility.Collapsed;
      MasteryEditor.Visibility = System.Windows.Visibility.Visible;
    }

    private void Spell1_Click(object src, EventArgs args) {
      spell1 = true;
      SpellPopup.BeginStoryboard(App.FadeIn);
    }

    private void Spell2_Click(object src, EventArgs args) {
      spell1 = false;
      SpellPopup.BeginStoryboard(App.FadeIn);
    }

    private void Champion_Click(object src, EventArgs args) {
      ChampSelector.UpdateChampList();
      ChampPopup.BeginStoryboard(App.FadeIn);
      ChampSelector.Visibility = System.Windows.Visibility.Visible;
      MasteryEditor.Visibility = System.Windows.Visibility.Collapsed;
    }

    private void ChampionPopup_Close(object sender, RoutedEventArgs e) {
      ChampPopup.BeginStoryboard(App.FadeOut);
      MasteryEditor.Save().Wait();
      Player.UpdateMasteries();
    }

    private void ChampSelector_SkinSelected(object sender, ChampionDto.SkinDto e) {
      Player.State.Champion = ChampSelector.SelectedChampion;
      Player.Skin = e;
      ChampionPopup_Close(sender, null);
    }

    private void Spell_Select(object sender, SpellDto spell) {
      if (spell1) {
        Player.State.Spell1 = spell;
      } else {
        Player.State.Spell2 = spell;
      }
      SpellPopup.BeginStoryboard(App.FadeOut);
    }

    private void EnterQueue(object sender, RoutedEventArgs e) {
      var id = RiotCalls.CapService.CreateSoloQuery(Player.State);
      Client.AddDelegate(id, response => {
        if (response.status.Equals("OK"))
          Dispatcher.Invoke(() => Client.QueueManager.ShowQueuer(new CapSoloQueuer(Player)));
      });
      if (Close != null) Close(this, new EventArgs());
    }

    public bool CanPlay() {
      return false;
    }

    public Page GetPage() {
      return this;
    }
  }
}
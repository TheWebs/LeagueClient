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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LeagueClient.Logic;
using LeagueClient.Logic.Queueing;
using LeagueClient.Logic.Riot;
using LeagueClient.Logic.Riot.Platform;
using RtmpSharp.Messaging;

namespace LeagueClient.ClientUI.Main {
  /// <summary>
  /// Interaction logic for CustomCreatePage.xaml
  /// </summary>
  public partial class CustomCreatePage : Page, IClientSubPage {
    private const int BorderSize = 3;
    private const int MarginSize = 10;
    private GameMap selected;
    private Border current;

    public CustomCreatePage() {
      InitializeComponent();
      current = SummonersRift;
      SummonersRift.Tag = selected = GameMap.SummonersRift;
      CrystalScar.Tag = GameMap.TheCrystalScar;
      TwistedTreeline.Tag = GameMap.TheTwistedTreeline;
      HowlingAbyss.Tag = GameMap.HowlingAbyss;
      foreach(var border in new[] { SummonersRift, CrystalScar, TwistedTreeline, HowlingAbyss }) {
        border.MouseEnter += Border_MouseEnter;
        border.MouseLeave += Border_MouseLeave;
        border.MouseUp += Border_MouseUp;
      }
      Spectators.ItemsSource = SpectatorState.Values.Values;
      Spectators.SelectedItem = SpectatorState.ALL;
      GameType.ItemsSource = new[] { GameConfig.Blind, GameConfig.Draft, GameConfig.AllRandom };
      GameType.SelectedItem = GameConfig.Blind;
      GameName.Text = Client.LoginPacket.AllSummonerData.Summoner.Name + "'s Game";
      GameName.CaretIndex = GameName.Text.Length;
      Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Input, (Func<bool>) GameName.Focus);
      MapSelected();
    }

    private void MapSelected() {
      current.Effect = new DropShadowEffect { BlurRadius = 15, Color = App.FocusColor, ShadowDepth = 0 };
      MapLabel.Content = selected.DisplayName;
      int old = (int) (TeamSize.SelectedItem ?? 0);
      var size = new int[selected.TotalPlayers / 2];
      for (int i = 0; i < size.Length; i++) size[i] = size.Length - i;
      TeamSize.ItemsSource = size;
      TeamSize.SelectedIndex = 0;
    }

    private async void CreateGame(object sender, RoutedEventArgs e) {
      ErrorLabel.Visibility = Visibility.Collapsed;
      var config = new PracticeGameConfig();
      config.GameName = GameName.Text;
      config.GamePassword = GamePass.Text;
      config.MaxNumPlayers = ((int) TeamSize.SelectedItem) * 2;
      config.GameTypeConfig = (GameType.SelectedItem as GameConfig).Key;
      config.AllowSpectators = (Spectators.SelectedItem as SpectatorState).Key;
      config.GameMap = selected;
      switch (selected.MapId) {
        case 8: config.GameMode = "ODIN"; break;
        case 10:
        case 11: config.GameMode = "CLASSIC"; break;
        case 12:
        case 14: config.GameMode = "ARAM"; break;
      }
      GameDTO game = null;
      try {
        game = await RiotCalls.GameService.CreatePracticeGame(config);
      } catch { }

      if (game?.Name == null) {
        ErrorLabel.Visibility = Visibility.Visible;
        return;
      } else {
        Client.QueueManager.ShowPage(new CustomLobbyPage(game));
      }
    }

    #region Border UI Listeners
    private Dictionary<UIElement, Animation> Animations = new Dictionary<UIElement, Animation>();

    public event EventHandler Close;

    private async void Border_MouseEnter(object sender, MouseEventArgs e) {
      var border = sender as Border;

      if (Animations.ContainsKey(border))
        Animations[border].Abort();

      var anim = new Animation(200, BorderSize, d => {
        border.Margin = new Thickness(MarginSize - d, MarginSize - d, -d, -d);
        border.BorderThickness = new Thickness(d);
      });
      await (Animations[border] = anim).Start();
    }

    private async void Border_MouseLeave(object sender, MouseEventArgs e) {
      var border = sender as Border;

      if (Animations.ContainsKey(border))
        Animations[border].Abort();

      var anim = new Animation(200, BorderSize, d => {
        border.Margin = new Thickness(MarginSize - BorderSize + d, MarginSize - BorderSize + d, -BorderSize + d, -BorderSize + d);
        border.BorderThickness = new Thickness(BorderSize - d);
      });
      await (Animations[border] = anim).Start();
    }

    private void Border_MouseUp(object sender, MouseButtonEventArgs e) {
      var border = sender as Border;

      if (Animations.ContainsKey(border))
        Animations[border].Abort();

      current.Effect = null;
      current = border;
      selected = border.Tag as GameMap;
      MapSelected();
    }
    #endregion

    public Page Page => this;
    public bool CanPlay => false;

    public IQueuer HandleClose() => null;
    public void ForceClose() { }
    public bool HandleMessage(MessageReceivedEventArgs args) => false;
  }

  public class Animation {
    private int sleep;
    private int count;
    private double step;
    private volatile bool done;

    private Task task;
    private Action<double> Changed;

    public Animation(int duration, int count, Action<double> action, double step = .5) {
      sleep = (int) ((duration / count) * step);
      this.count = count;
      this.step = step;
      Changed = action;
    }

    public void Abort() {
      App.Current?.Dispatcher?.Invoke(Changed, count);
      done = true;
    }

    public async Task Start() => await (task = Task.Run((Action) ThreadLoop));

    private void ThreadLoop() {
      for (double d = 1; d <= count; d += step) {
        if (done) return;
        App.Current?.Dispatcher?.Invoke(Changed, d);
        System.Threading.Thread.Sleep(sleep);
      }
    }
  }
}
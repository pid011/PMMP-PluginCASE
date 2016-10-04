using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace CASE
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private int apiTextBoxCount = 0;

        private string authorName;
        private string pluginName;
        private string pluginVersion;
        private List<string> supportAPI;

        private Constructure constructure;

        private CreateAlert createAlertWindow;

        public MainWindow()
        {
            InitializeComponent();

            DoubleAnimation heightani = new DoubleAnimation(329.572, TimeSpan.FromSeconds(0.6));
            DoubleAnimation fadeani = new DoubleAnimation(1, TimeSpan.FromSeconds(1.4));
            main_item_grid.BeginAnimation(HeightProperty, heightani);
            main_item_grid.BeginAnimation(OpacityProperty, fadeani);
        }

        private void Title_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => this.DragMove();

        private void CloseButton_Clicked(object sender, RoutedEventArgs e) => this.Close();

        private void MinimizeButton_Clicked(object sender, RoutedEventArgs e) => this.WindowState = WindowState.Minimized;

        private void apiAddButton_Clicked(object sender, RoutedEventArgs e)
        {
            apiTextBoxCount++;
            if (apiTextBoxCount > 1)
            {
                apiAddButton.IsEnabled = false;
                apiButton_cancelMessage.Visibility = Visibility.Visible;
            }

            Grid apiGrid = new Grid();
            apiGrid.Height = 40;

            TextBox apiTextbox = new TextBox();
            apiTextbox.Name = $"textbox_api{apiTextBoxCount}";
            Thickness apiTextBoxMargin = apiTextbox.Margin;
            apiTextBoxMargin.Left = 207.5;
            apiTextBoxMargin.Top = 2.5;
            apiTextBoxMargin.Bottom = 1.422;
            apiTextbox.Margin = apiTextBoxMargin;
            apiTextbox.TextWrapping = TextWrapping.Wrap;
            apiTextbox.HorizontalAlignment = HorizontalAlignment.Left;
            apiTextbox.Width = 185;
            Thickness apiTextBoxThickness = apiTextbox.BorderThickness;
            apiTextBoxThickness.Top = 2;
            apiTextBoxThickness.Left = 2;
            apiTextBoxThickness.Bottom = 2;
            apiTextBoxThickness.Right = 2;
            apiTextbox.BorderThickness = apiTextBoxThickness;
            apiTextbox.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 150, 136));
            apiTextbox.Foreground = new SolidColorBrush(Color.FromRgb(33, 33, 33));
            apiTextbox.FontSize = 18.677;
            apiTextbox.SelectionBrush = new SolidColorBrush(Color.FromRgb(178, 223, 219));
            apiTextbox.MaxLines = 1;
            apiTextbox.SelectionBrush = new SolidColorBrush(Color.FromRgb(255, 235, 59));

            apiGrid.Children.Add(apiTextbox);
            stackpanel_api.Children.Add(apiGrid);
        }

        private void CreateButton_Clicked(object sender, RoutedEventArgs e)
        {
            new List<Label>()
            {
                name_emptyAlert, pluginName_emptyAlert, pluginVersion_emptyAlert, api_emptyAlert,
                name_noMatch, pluginName_noMatch, api_noMatch
            }
            .ForEach(x => x.Visibility = Visibility.Hidden);

            bool isFaild = false;

            #region 텍스트 상태 체크

            string authorName = this.textbox_name.Text;
            if (string.IsNullOrWhiteSpace(authorName))
            {
                isFaild = true;
                this.name_emptyAlert.Visibility = Visibility.Visible;
            }
            else if (!Regex_CheckName(authorName))
            {
                isFaild = true;
                this.name_noMatch.Visibility = Visibility.Visible;
            }
            string pluginName = this.textbox_pluginName.Text;
            if (string.IsNullOrWhiteSpace(pluginName))
            {
                isFaild = true;
                this.pluginName_emptyAlert.Visibility = Visibility.Visible;
            }
            else if (!Regex_CheckName(pluginName))
            {
                isFaild = true;
                this.pluginName_noMatch.Visibility = Visibility.Visible;
            }

            string pluginVersion = this.textbox_pluginVersion.Text;
            if (string.IsNullOrWhiteSpace(pluginVersion))
            {
                isFaild = true;
                this.pluginVersion_emptyAlert.Visibility = Visibility.Visible;
            }

            var stp = stackpanel_api.Children;
            var supportAPI = new List<string>();
            supportAPI.Add(((stp[0] as Grid).Children[1] as TextBox).Text);

            if (stp.Count > 1)
            {
                UIElement[] ue = new UIElement[3];
                stp.CopyTo(ue, 0);
                var uelist = ue.ToList();

                uelist.Remove(uelist[0]);
                uelist.RemoveAll(x => x == null);
                foreach (var item in uelist)
                {
                    supportAPI.Add(((item as Grid).Children[0] as TextBox).Text);
                }
            }
            supportAPI.RemoveAll(x => string.IsNullOrWhiteSpace(x));
            if (supportAPI.Count == 0)
            {
                isFaild = true;
                this.api_emptyAlert.Visibility = Visibility.Visible;
            }
            else
            {
                if (supportAPI.Any(x => !Regex_CheckAPI(x)))
                {
                    isFaild = true;
                    this.api_noMatch.Visibility = Visibility.Visible;
                }
            }

            #endregion 텍스트 상태 체크

            if (!isFaild)
            {
                this.authorName = authorName;
                this.pluginName = pluginName;
                this.pluginVersion = pluginVersion;
                this.supportAPI = supportAPI;

                this.createButton.IsEnabled = false;
                this.createButton.Content = "생성중...";
                this.Dispatcher.Invoke((ThreadStart) (() => { }), DispatcherPriority.ApplicationIdle);

                CreatePluginCase();
            }
            else
            {
                this.authorName = null;
                this.pluginName = null;
                this.pluginVersion = null;
                this.supportAPI = null;
            }
        }

        private bool Regex_CheckName(string text)
        {
            foreach (var ch in text)
            {
                if (!Regex.IsMatch(ch.ToString(), @"([a-zA-Z])"))
                {
                    return false;
                }
            }
            return true;
        }

        private bool Regex_CheckAPI(string text)
        {
            if (!Regex.IsMatch(text, @"^([0-9]+).([0-9]+).([0-9]+)$"))
            {
                return false;
            }
            return true;
        }

        private void CreatePluginCase()
        {
            this.constructure = new Constructure(
                new ConstructureInfo(this.authorName, this.pluginName, this.pluginVersion, this.supportAPI));
            this.constructure.CreateSuccessedEvent += Constructure_CreateSuccessedEvent;

            createAlertWindow = new CreateAlert(constructure);
            createAlertWindow.Owner = this;
            createAlertWindow.UserChooseEvent += CreateAlertWindow_UserChooseEvent;
            createAlertWindow.Show();
        }

        private void Constructure_CreateSuccessedEvent(object sender, EventArgs e)
        {
            MessageBox.Show($"[생성완료] 생성경로: {(sender as Constructure).MainDirectory}", "생성완료");
        }

        private void CreateAlertWindow_UserChooseEvent(object sender, EventArgs e)
        {
            var window = sender as CreateAlert;

            if (window.IsYes)
            {
                constructure.Start();
            }
            this.createButton.IsEnabled = true;
            this.createButton.Content = "생성";
        }
    }
}
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
using System.Windows.Shapes;

namespace CASE
{
    /// <summary>
    /// CreateAlert.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CreateAlert : Window
    {
        public bool IsYes { get; private set; }

        public event EventHandler UserChooseEvent;

        private Constructure constructure;

        public CreateAlert(Constructure cst)
        {
            InitializeComponent();
            this.constructure = cst;

            this.tb_author.Text = constructure.Info.Author;
            this.tb_pluginName.Text = constructure.Info.PluginName;
            this.tb_pluginVersion.Text = constructure.Info.PluginVersion;

            TextBlock[] list = new TextBlock[] { tb_api0, tb_api1, tb_api2 };
            for (int i = 0; i < constructure.Info.API.Count; i++)
            {
                list[i].Text = constructure.Info.API[i];
            }
        }

        private void Title_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => this.DragMove();

        private void YesButton_Clicked(object sender, RoutedEventArgs e)
        {
            this.IsYes = true;
            UserChooseEvent(this, null);
            this.Close();
        }

        private void NoButton_Clicked(object sender, RoutedEventArgs e)
        {
            this.IsYes = false;
            UserChooseEvent(this, null);
            this.Close();
        }
    }
}

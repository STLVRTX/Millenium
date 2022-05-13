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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Millenium
{
    public partial class Menu : UserControl, IComponentConnector
    {
        private MainWindow mainWindow;

        public Menu(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
        }

        private void ButtonPlay_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.CurrWindow = MainWindow.ActWindow.Menu_ModeSelect;
        }

        private void ButtonHighscores_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.CurrWindow = MainWindow.ActWindow.Highscores;
        }

        private void ButtonEnd_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
            return;
        }
    }
}

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

namespace Millenium
{
    /// <summary>
    /// Interaction logic for Menu_ModeSelect.xaml
    /// </summary>
    public partial class Menu_ModeSelect : UserControl
    {
        private MainWindow mainWindow;

        public Menu_ModeSelect(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
        }

        private void Button1Player_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.CurrWindow = MainWindow.ActWindow.Menu_1Player;
        }

        private void Button2Player_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.CurrWindow = MainWindow.ActWindow.Menu_2Player;
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.CurrWindow = MainWindow.ActWindow.Menu;
        }
    }
}

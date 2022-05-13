using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    public partial class Menu_2Player : UserControl
    {
        private MainWindow mainWindow;
        private static readonly Regex _regex = new("^[a-z0-9A-Z ]+$");

        public Menu_2Player(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
        }

        private void TextPlayer_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            bool regexSucc = _regex.IsMatch(e.Text);

            if (!regexSucc)
                e.Handled = true;
        }

        private void Button2Player_Click(object sender, RoutedEventArgs e)
        {
            if(TextPlayer1.Text != "" && TextPlayer2.Text != "")
            {
                mainWindow.Player1 = TextPlayer1.Text;
                mainWindow.Player2 = TextPlayer2.Text;
                mainWindow.CurrWindow = MainWindow.ActWindow.Silver;
            }
            else
            {
                MessageBox.Show("Please fill in your name!");
                return;
            }
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.CurrWindow = MainWindow.ActWindow.Menu_ModeSelect;
        }
    }
}

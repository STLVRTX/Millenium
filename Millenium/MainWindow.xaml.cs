using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
using System.Windows.Threading;

namespace Millenium
{
    public partial class MainWindow : Window
    {
        private ActWindow currWindow;
        internal string Player1 { get; set; }
        internal string Player2 { get; set; }

        public ActWindow CurrWindow
        {
            get { return currWindow; }
            set
            {
                switch (value)
                {
                    case ActWindow.Menu:
                        //Setze Background auf Menü-Background
                        BackgroundGame.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/Images/Millenium_Menu.png"));
                        //Lade die Menu-UserControl
                        MainContent.Content = new Menu(this);
                        break;

                    case ActWindow.Silver:
                        BackgroundGame.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/Images/EN_en/Millenium_Silver_en.png"));
                        MainContent.Content = new Silver(this, Player1, Player2);
                        break;

                    case ActWindow.Menu_ModeSelect:
                        BackgroundGame.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/Images/Millenium_Menu_ModeSelect.png"));
                        MainContent.Content = new Menu_ModeSelect(this);
                        break;

                    case ActWindow.Menu_2Player:
                        BackgroundGame.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/Images/EN_en/Millenium_Menu_2Player_en.png"));
                        MainContent.Content = new Menu_2Player(this);
                        break;

                    case ActWindow.Menu_1Player:
                        BackgroundGame.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/Images/EN_en/Millenium_Menu_1Player_en.png"));
                        MainContent.Content = new Menu_1Player(this);
                        break;

                    case ActWindow.Highscores:
                        BackgroundGame.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/Images/EN_en/Millenium_Highscores_en.png"));
                        MainContent.Content = new Highscores(this);
                        break;
                }
                currWindow = value;
            }
        }

        public MainWindow()
        {          
            InitializeComponent();
            CurrWindow = ActWindow.Menu;
            Player1 = "";
            Player2 = "";
            Highscores.ReadHighscores();
        }
        #region Enum ActWindow
        public enum ActWindow
        {
            Menu, Silver, Menu_ModeSelect, Menu_2Player, Menu_1Player, Highscores
        }
        #endregion
    }
}

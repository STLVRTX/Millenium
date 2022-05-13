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
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

namespace Millenium
{
    public partial class Highscores : UserControl
    {
        private MainWindow mainWindow;
        public static List<UserScore> highscores = new List<UserScore>();
        private TextBlock[] rankings;
        private TextBlock[] names;
        private TextBlock[] scores;

        public Highscores(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            rankings = new TextBlock[] { Ranking0, Ranking1, Ranking2, Ranking3, Ranking4, Ranking5, Ranking6, Ranking7, Ranking8, Ranking9};
            names = new TextBlock[] { Name0, Name1, Name2, Name3, Name4, Name5, Name6, Name7, Name8, Name9 };
            scores = new TextBlock[] { Score0, Score1, Score2, Score3, Score4, Score5, Score6, Score7, Score8, Score9 };
            SetHighscores();
        }

        private void ButtonMenu_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.CurrWindow = MainWindow.ActWindow.Menu;
        }

        public class UserScore
        {
            public int Ranking { get; set; }
            public string Name { get; set; }
            public int Score { get; set; }

            public UserScore(int score, string name)
            {
                Name = name;
                Score = score;
            }
        }

        public static void ReadHighscores()
        {
            string projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
            if (File.Exists(projectDirectory))
            {
                StreamReader r = new StreamReader(projectDirectory + "/highscores.json");
                string jsonString = r.ReadToEnd();
                r.Close();

                if (jsonString != "")
                    highscores = JsonSerializer.Deserialize<List<UserScore>>(jsonString);
            }
        }
        
        private void SetHighscores()
        {
            List<UserScore> SortedList = highscores.OrderBy(o => o.Score).ToList();

            for(int i = 0; i < SortedList.Count; i++)
            {
                if (i == 10)
                    break;
                SortedList[i].Ranking = i + 1;
                rankings[i].Text = SortedList[i].Ranking.ToString();
                names[i].Text = SortedList[i].Name.ToString();
                scores[i].Text = SortedList[i].Score.ToString();
            }

            string projectDirectory = AppDomain.CurrentDomain.BaseDirectory;

            string json = JsonSerializer.Serialize(SortedList);
            File.WriteAllText(projectDirectory + "/highscores.json", json);
        }
    }
}

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

namespace Millenium
{
    public partial class Finish : Window
    {
        private Silver Silver { get; set; }

        //Initialisieren des Finish-Fensters
        public Finish(Silver silver)
        {
            InitializeComponent();
            Silver = silver;
        }

        //Auswertung und Anzeige
        internal void ShowFinish(Silver.Result result, String leftName, String rightName, int leftScore, int rightScore)
        {
            if(result == Silver.Result.LeftWin)
            {
                ResultText.Text = leftName + " wins!";
                ResultScore.Text += leftScore;
                AddHighscore(leftScore, leftName);
            }

            else if (result == Silver.Result.RightWin)
            {
                ResultText.Text = rightName + " wins!";
                ResultScore.Text += rightScore;
                AddHighscore(rightScore, rightName);
            }

            else if(result == Silver.Result.Tie)
            {
                ResultText.Text = "Draw!";
                ResultScore.Text += leftScore;
            }

            ShowDialog();
        }

        //Resetten des Games, Schließen des Dialogs
        private void ButtonRestartClick(object sender, RoutedEventArgs e)
        {
            Close();
            Silver.Reset();
        }

        //Übergeben des Gewinner-Scores an die Highscore-Tabelle
        private void AddHighscore(int score, string name)
        {
            Highscores.UserScore user = new Highscores.UserScore(score, name);

            if(name != "_CPU_")
                Highscores.highscores.Add(user);
        }
    }
}

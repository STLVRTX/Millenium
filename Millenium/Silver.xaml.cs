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
    public partial class Silver : UserControl
    {
        #region Fields
        private readonly Random rn = new Random();
        private readonly TextBox[] leftNum;
        private readonly TextBox[] rightNum;
        private State state;
        private GameMode gameMode;
        private Result result;
        private readonly DoubleAnimation fade;
        private readonly DoubleAnimation fadeShortLeft;
        private readonly DoubleAnimation fadeShortRight;
        private int[] sums;
        private static readonly Regex _regex = new("[^0-9 ]+");
        private readonly MainWindow mainWindow;
        #endregion

        #region Constructor
        public Silver(MainWindow mainWindow, string leftPlayerName, string rightPlayerName)
        {
            InitializeComponent();
            leftNum = new TextBox[] { leftNum0, leftNum1, leftNum2, leftNum3, leftNum4, leftNum5, leftNum6, leftNum7, leftNum8, leftNum9 };
            rightNum = new TextBox[] { rightNum0, rightNum1, rightNum2, rightNum3, rightNum4, rightNum5, rightNum6, rightNum7, rightNum8, rightNum9 };
            state = State.LeftSideInput;

            if (!rightPlayerName.Equals("_CPU_"))
                gameMode = GameMode.PlayerPlayer;
            else
                gameMode = GameMode.PlayerCPU;

            fade = CreateFadeAnimation(2);
            fadeShortLeft = CreateFadeAnimation(1);
            fadeShortRight = CreateFadeAnimation(1);
            sums = new int[2];
            Keyboard.Focus(leftNum[0]);
            leftNum[0].Focus();
            this.mainWindow = mainWindow;
            this.leftPlayerName.Text = leftPlayerName;
            this.rightPlayerName.Text = rightPlayerName;
        }
        #endregion

        #region Button Confirm
        private void ButtonConfirm_Click(object sender, RoutedEventArgs e)
        {
            //Handling bei aktiver linker Seite
            if (state == State.LeftSideInput)
            {
                //Prüfen ob alle Felder gefüllt sind
                foreach (TextBox box in leftNum)
                {
                    if (box.Text == "")
                    {
                        MessageBox.Show("Please fill in all fields!");
                        return;
                    }
                }

                //Eingabeboxen sperren
                foreach (TextBox box in leftNum)
                    box.IsReadOnly = true;

                //Handling bei PlayerVsPlayer-Gamemode
                if (gameMode == GameMode.PlayerPlayer)
                {
                    foreach (TextBox box in rightNum)
                        box.IsReadOnly = false;

                    BorderLeftPlayerName.BorderBrush = Brushes.Transparent;
                    BorderRightPlayerName.BorderBrush = Brushes.Red;

                    rightNum[0].Focus();

                    state = State.RightSideInput;
                }

                //Handling bei PlayerVsCPU-Gamemode
                else if (gameMode == GameMode.PlayerCPU)
                {
                    ButtonConfirm.IsEnabled = false;
                    BorderLeftPlayerName.BorderBrush = Brushes.Transparent;

                    ButtonCalculate.Focus();

                    state = State.Evaluate;
                }
            }
            //Handling bei aktiver rechter Seite
            else if (state == State.RightSideInput)
            {
                //Prüfen ob alle Felder gefüllt sind
                foreach (TextBox box in rightNum)
                {
                    if (box.Text == "")
                    {
                        MessageBox.Show("Please fill in all fields!");
                        return;
                    }
                }

                //Eingabeboxen sperren
                foreach (TextBox box in rightNum)
                    box.IsReadOnly = true;

                //Beenden der Eingabe
                ButtonConfirm.IsEnabled = false;
                BorderRightPlayerName.BorderBrush = Brushes.Transparent;

                state = State.Evaluate;
            }
        }
        #endregion

        #region Button Evaluate
        private void ButtonCalculate_Click(object sender, RoutedEventArgs e)
        {
            if (state == State.Evaluate)
            {
                randomNr.Text = rn.Next(1, 1001).ToString();

                if(gameMode == GameMode.PlayerCPU)
                    foreach(TextBox box in rightNum)
                        box.Text = rn.Next(1, 1001).ToString();

                sums = CalculateNums();
                leftPlayerSum.Text = sums[0].ToString();
                rightPlayerSum.Text = sums[1].ToString();

                //Registieren des Fade-Completed Events
                fade.Completed += new EventHandler(BeginAnimFade);

                //Registieren des FadeShortLeft-Completed Events
                fadeShortLeft.Completed += new EventHandler(BeginAnimFadeShortLeft);

                //Registieren des FadeShortRight-Completed Events
                fadeShortRight.Completed += new EventHandler(BeginAnimFadeShortRight);

                //Starten des Einblendens der Ergebnisse
                randomNr.BeginAnimation(OpacityProperty, fade);

                state = State.Finished;
                ButtonCalculate.IsEnabled = false;
            }
            else
            {
                MessageBox.Show("You can't calculate yet!");
                return;
            }
        }

        #region Fade Animations
        private void BeginAnimFade(object sender, EventArgs e)
        {
            leftPlayerSum.Visibility = Visibility.Visible;
            leftPlayerSum.BeginAnimation(OpacityProperty, fadeShortLeft);
        }
        private void BeginAnimFadeShortLeft(object sender, EventArgs e)
        {
            rightPlayerSum.Visibility = Visibility.Visible;
            rightPlayerSum.BeginAnimation(OpacityProperty, fadeShortRight);
        }
        private void BeginAnimFadeShortRight(object sender, EventArgs e)
        {
            //Auswerten der Punktzahl
            if (sums[0] < sums[1])
            {
                BorderLeftPlayerSum.BorderBrush = Brushes.Red;
                result = Result.LeftWin;
            }
            else if (sums[0] > sums[1])
            {
                BorderRightPlayerSum.BorderBrush = Brushes.Red;
                result = Result.RightWin;
            }
            else
            {
                BorderLeftPlayerSum.BorderBrush = Brushes.Gray;
                BorderRightPlayerSum.BorderBrush = Brushes.Gray;
                result = Result.Tie;
            }

            fade.Completed -= new EventHandler(BeginAnimFade);
            fadeShortLeft.Completed -= new EventHandler(BeginAnimFadeShortLeft);
            fadeShortRight.Completed -= new EventHandler(BeginAnimFadeShortRight);

            //Spielende-Popup
            GameFinish();
        }
        private DoubleAnimation CreateFadeAnimation(int secs)
        {
            DoubleAnimation fade = new DoubleAnimation();
            fade.From = 0;
            fade.To = 1;
            fade.Duration = new Duration(TimeSpan.FromSeconds(secs));
            return fade;
        }
        #endregion

        private int[] CalculateNums()
        {
            int leftNums = 0, rightNums = 0;

            foreach (TextBox box in leftNum)
                leftNums += Math.Abs(int.Parse(box.Text) - int.Parse(randomNr.Text));

            foreach (TextBox box in rightNum)
                rightNums += Math.Abs(int.Parse(box.Text) - int.Parse(randomNr.Text));

            return new int[] { leftNums, rightNums };
        }
        private void GameFinish()
        {
            Finish finish = new Finish(this);
            finish.ShowFinish(result, leftPlayerName.Text, rightPlayerName.Text, int.Parse(leftPlayerSum.Text), int.Parse(rightPlayerSum.Text));
        }
        #endregion

        #region Enums Game Properties
        internal enum State
        {
            LeftSideInput, RightSideInput, Evaluate, Finished
        }
        internal enum GameMode
        {
            PlayerCPU, PlayerPlayer
        }
        internal enum Result
        {
            LeftWin, RightWin, Tie
        }
        #endregion

        #region UserInputHandling (only 1-1000, no letters/characters)
        //Sperren von Zeichen und Buchstaben
        private void numBoxes_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            bool regexSucc = _regex.IsMatch(e.Text);
            bool limitSucc = false;
            TextBox tb = (TextBox)sender;

            if (!regexSucc)
                limitSucc = int.Parse(tb.Text + e.Text) > 1000 || int.Parse(tb.Text + e.Text) < 1;

            if (limitSucc || regexSucc)
                e.Handled = true;
        }

        //Sperren von Space
        private void numBoxes_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }
        #endregion

        #region Reset Game
        internal void Reset()
        {
            state = State.LeftSideInput;

            foreach (TextBox textBox in leftNum)
            {
                textBox.Text = string.Empty;
                textBox.IsReadOnly = false;
            }

            foreach (TextBox textBox in rightNum)
            {
                textBox.Text = string.Empty;
                textBox.IsReadOnly = false;
            }

            leftPlayerSum.Text = string.Empty;
            rightPlayerSum.Text = string.Empty;
            randomNr.Text = string.Empty;

            ButtonCalculate.IsEnabled = true;
            ButtonConfirm.IsEnabled = true;

            BorderLeftPlayerName.BorderBrush = Brushes.Red;
            BorderLeftPlayerSum.BorderBrush = Brushes.Transparent;
            BorderRightPlayerSum.BorderBrush = Brushes.Transparent;

            leftPlayerSum.Visibility = Visibility.Hidden;
            rightPlayerSum.Visibility = Visibility.Hidden;

            leftNum[0].Focus();
        }
        #endregion

        #region Button Menu
        private void ButtonMenu_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.CurrWindow = MainWindow.ActWindow.Menu;
        }
        #endregion
    }
}

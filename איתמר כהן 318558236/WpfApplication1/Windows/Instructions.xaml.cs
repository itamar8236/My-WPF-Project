using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for Instructions.xaml
    /// </summary>
    public partial class Instructions : Window
    {
        /// <summary>
        /// Game mode window
        /// </summary>
        public GameMode game = new GameMode();

        /// <summary>
        /// Construction for Instruction window
        /// </summary>
        public Instructions()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Updates data when window apears
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Activated(object sender, EventArgs e)
        {
            ConstInfo.Text = "In every game mode, control the cannon with the arrow keys, and fire with space key. \nYou can't shoot while moving the cannon or the barrel.";
            if (game.level == 0)
            {
                Information.Text = "This is the Trainig program. Here you can design the game as you wish. \n(You have an endless shots and time)";
                TargetsNumInvite.Visibility = Visibility.Visible;
                targetsNumber.Visibility = Visibility.Visible;
                SpeedInvite.Visibility = Visibility.Visible;
                Speed.Visibility = Visibility.Visible;
                NoPhs.Visibility = Visibility.Visible;
                WithPhs.Visibility = Visibility.Visible;
                VTargetInvite.Visibility = Visibility.Visible;
                VTarget.Visibility = Visibility.Visible;
                NoPhs.IsChecked = true;
            }
            else
            {
                easyinfo.Visibility = Visibility.Visible;
                easy.Visibility = Visibility.Visible;
                mediuminfo.Visibility = Visibility.Visible;
                medium.Visibility = Visibility.Visible;
                hardinfo.Visibility = Visibility.Visible;
                hard.Visibility = Visibility.Visible;
                easy.IsChecked = true;
                Information.Text = "In every mode, the speed of the cannonball targets and the number of the targets is diffrent.";
                if (game.level == 1)
                {
                    Information.Text += "\nThis is level 1, in this level you don't need to calculate gravity influance. \nThe ball will fly in the same height during the shot.";
                    easyinfo.Text = "In the easy mode, you have 1.5 minutes and 25 balls";
                    mediuminfo.Text = "In the medium mode, you have 1 minute and 10 balls";
                    hardinfo.Text = "In the hard mode, you have 30 seconds and 7 balls";
                }
                else
                {
                    Information.Text += "\nThis is level 2, in this level the gravity influance your shot. \nCalculate the shot and notice that the higher the Barrel, the deep the ball wiil go.";
                    easyinfo.Text = "In the easy mode, you have 1 minute and 20 balls";
                    mediuminfo.Text = "In the medium mode, you have 45 seconds and 25 balls";
                    hardinfo.Text = "In the hard mode, you have 30 seconds and 12 balls";
                }
            }
        }

        /// <summary>
        /// Starts game mode.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameStart(object sender, MouseButtonEventArgs e)
        {
            switch (game.level)
            {
                //training data
                case 0:
                    bool legalInput = int.TryParse(targetsNumber.Text, out game.numOftargets);//בודק שהקלט תקין ושם ערכים במקום
                    legalInput = game.numOftargets <= 20 && game.numOftargets >= 1 && legalInput;
                    legalInput = double.TryParse(Speed.Text, out game.speed) && legalInput;
                    legalInput = game.speed <= 1000 && game.speed >= 10 && legalInput;
                    legalInput = double.TryParse(VTarget.Text, out game.VfirstTarget) && legalInput;
                    legalInput = game.VfirstTarget <= 50 && game.VfirstTarget >= 0 && legalInput;
                    if (!legalInput)
                        MessageBox.Show("Illegal input!!!");
                    else
                    {
                        game.IsTraining = true;
                        if (NoPhs.IsChecked == true)
                            game.level = 1;
                        else
                            game.level = 2;
                        game.Show();
                        Close();
                    }
                    break;
                //update level 1 data
                case 1:
                    if (easy.IsChecked == true)
                    {
                        game.numOftargets = 7;
                        game.speed = 300;
                        game.VfirstTarget = 10;
                        game.NumOfShotsAllowd = 25;
                        game.NumOfSecsAllowd = 90;
                    }
                    else if (medium.IsChecked == true)
                    {
                        game.numOftargets = 5;
                        game.speed = 250;
                        game.VfirstTarget = 14;
                        game.NumOfShotsAllowd = 10;
                        game.NumOfSecsAllowd = 60;
                    }
                    else
                    {
                        game.numOftargets = 5;
                        game.speed = 200;
                        game.VfirstTarget = 18;
                        game.NumOfShotsAllowd = 7;
                        game.NumOfSecsAllowd = 30;
                    }
                    game.Show();
                    Close();
                    break;
                //update level 2 data.
                case 2:
                    if (easy.IsChecked == true)
                    {
                        game.numOftargets = 4;
                        game.speed = 180;
                        game.VfirstTarget = 12;
                        game.NumOfShotsAllowd = 20;
                        game.NumOfSecsAllowd = 60;
                    }
                    else if (medium.IsChecked == true)
                    {
                        game.numOftargets = 6;
                        game.speed = 150;
                        game.VfirstTarget = 15;
                        game.NumOfShotsAllowd = 25;
                        game.NumOfSecsAllowd = 45;
                    }
                    else
                    {
                        game.numOftargets = 5;
                        game.speed = 150;
                        game.VfirstTarget = 20;
                        game.NumOfShotsAllowd = 12;
                        game.NumOfSecsAllowd = 30;
                    }
                    game.Show();
                    Close();
                    break;
            }
        }

        /// <summary>
        /// This function increases the text when mouse is entering.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Mouse_Enter(object sender, MouseEventArgs e)
        {
            Growing.Grow_Up((TextBlock)sender, 15, 0.5);
        }

        /// <summary>
        /// This function decreasing the text when mouse is leaving.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Mouse_Leave(object sender, MouseEventArgs e)
        {
            Growing.Grow_Down((TextBlock)sender);
        }

        /// <summary>
        /// This function returns the game to Main window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoBack(object sender, MouseButtonEventArgs e)//אחורה
        {
            MainWindow mn = new MainWindow();
            mn.Show();
            Close();
        }
    }
}

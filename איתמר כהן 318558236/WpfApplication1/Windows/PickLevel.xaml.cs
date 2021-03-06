﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Bullseye
{
    /// <summary>
    /// Interaction logic for PickLevel.xaml
    /// </summary>
    public partial class PickLevel : Window
    {
        /// <summary>
        /// The instruction window.
        /// In the entire project and in all models:
        /// Transform.Children[0] - scale tranformation/animation.
        /// Transform.Children[1] - rotation tranformation/animation.
        /// Transform.Children[2] - location tranformation/animation.
        /// </summary>
        Instructions inst = new Instructions();

        /// <summary>
        /// Construction for PickLevekl
        /// </summary>
        public PickLevel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Opening instruction window and update level according to the user's choice.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PickLevel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source == Level1)
            {
                inst.game.level = 1;
                StartGame();
            }
            else if (e.Source == Level2)
            {
                inst.game.level = 2;
                StartGame();
            }
            else if (e.Source == Training)
            {
                inst.game.level = 0;
                StartGame();
            }
            else GoBack();
        }

        /// <summary>
        /// Opens instruction window
        /// </summary>
        public void StartGame()//מתחיל את המשחק
        {
            inst.Show();
            Close();
        }

        /// <summary>
        /// This function sents the user back to Main window.
        /// </summary>
        public void GoBack()
        {
            MainWindow mn = new MainWindow();
            mn.Show();
            Close();
        }

        /// <summary>
        /// This function increases the text when mouse is entering.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PickLevel_MouseEnter(object sender, MouseEventArgs e)//עכבר נכנס לאחד הטקסטבוקסים
        {
            Growing.Grow_Up((TextBlock)sender, 10, 0.5);
        }
        /// <summary>
        /// This function decreasing the text when mouse is leaving.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PickLevel_MouseLeave(object sender, MouseEventArgs e)//עכבר יצא מאחד הטקסטבוקסים
        {
            Growing.Grow_Down((TextBlock)sender);
        }
        /// <summary>
        /// Checking if the player exit the game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Application.Current.Shutdown();
        }
    }
}

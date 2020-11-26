using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// In the entire project and in all models:
    /// Transform.Children[0] - scale tranformation/animation.
    /// Transform.Children[1] - rotation tranformation/animation.
    /// Transform.Children[2] - location tranformation/animation.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Constructor for Main window
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Opening new window according to the user's choice.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainMenu_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source == Play)
                GamePlay();
            else if (e.Source == AboutProject)
                About();
            else if (e.Source == Quit)
                QuitProject();
        }

        /// <summary>
        /// This function opens the "About" window
        /// </summary>
        public void About()
        {
            TbAbout about = new TbAbout();
            about.Show();
        }
        /// <summary>
        /// This function increases the text when mouse is entering.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuMouseEnter(object sender, MouseEventArgs e)
        {
            Growing.Grow_Up((TextBlock)sender, 15, 0.5);
        }
        /// <summary>
        /// This function decreasing the text when mouse is leaving.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuMouseLeave(object sender, MouseEventArgs e)
        {
            Growing.Grow_Down((TextBlock)sender);
        }

        /// <summary>
        /// This function opens Pick Level window.
        /// </summary>
        public void GamePlay()
        {
            PickLevel pick = new PickLevel();
            pick.Show();
            Close();
        }
        /// <summary>
        /// Exit game.
        /// </summary>
        public void QuitProject()
        {
            Application.Current.Shutdown();
        }

    }
}

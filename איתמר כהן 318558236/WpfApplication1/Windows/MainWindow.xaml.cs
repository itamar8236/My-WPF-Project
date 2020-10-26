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

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainMenu_MouseDown(object sender, MouseButtonEventArgs e)//המשתמש לחץ על אחד מהאפשרויות
        {
            if (e.Source == Play)
                GamePlay();
            else if (e.Source == AboutProject)
                About();
            else if (e.Source == Quit)
                QuitProject();
        }

        public void About()//פותח חלון About
        {
            TbAbout about = new TbAbout();
            about.Show();
        }
        private void MenuMouseEnter(object sender, MouseEventArgs e)//העכבר נכנס לאחד הטקסטבוקסים
        {
            Growing.Grow_Up((TextBlock)sender, 15, 0.5);
        }
        private void MenuMouseLeave(object sender, MouseEventArgs e)//העכבר יצא מאחד הטקסטבוקסים
        {
            Growing.Grow_Down((TextBlock)sender);
        }

        public void GamePlay()//עובר לחלון בחירת שלב
        {
            PickLevel pick = new PickLevel();
            pick.Show();
            Close();
        }
        public void QuitProject()//יוצא מהמשחק
        {
            Application.Current.Shutdown();
        }
        
    }
}

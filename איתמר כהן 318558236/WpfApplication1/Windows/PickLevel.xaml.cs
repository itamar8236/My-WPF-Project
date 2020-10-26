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

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for PickLevel.xaml
    /// </summary>
    public partial class PickLevel : Window
    {

        Instructions inst = new Instructions();//חלון ההוראות
        public PickLevel()
        {
            InitializeComponent();
        }

        private void PickLevel_MouseDown(object sender, MouseButtonEventArgs e)//המשתמש לחץ על שלב
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
       
        public void StartGame()//מתחיל את המשחק
        {
            inst.Show();
            Close();
        }
        public void GoBack()//הולך אחורה
        {
            MainWindow mn = new MainWindow();
            mn.Show();
            Close();
        }
        private void PickLevel_MouseEnter(object sender, MouseEventArgs e)//עכבר נכנס לאחד הטקסטבוקסים
        {
            Growing.Grow_Up((TextBlock)sender, 10, 0.5);
        }

        private void PickLevel_MouseLeave(object sender, MouseEventArgs e)//עכבר יצא מאחד הטקסטבוקסים
        {
            Growing.Grow_Down((TextBlock)sender);
        }
    }
}

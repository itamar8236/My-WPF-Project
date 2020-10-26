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
using System.Windows.Media.Media3D;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace WpfApplication1
{
    public class InformationOfShot
    {
        public CannonBall ShotBall;//הכדור

        public double BallpxEnd;// שיעור האיקס של סוף היריה
        public double BallpyEnd;//שיעור הווי של סוף היריה
        public double BallpzEnd;//שיעור הזד של סוף היריה

        public double MaxHeight;//הגובה המקסימלי אליו מגיע הכדור
        public double AllshotDuration;//הזמן שך כל היריה
        public double DurationTillMaxHeight;//הזמן שלוקח לכדור להגיע לגובה המקסימלי
        
    }
}

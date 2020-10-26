using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace WpfApplication1
{
    class Growing
    {
        private static DoubleAnimation anim= new DoubleAnimation();//אנימציה
        private static double OrigFontSize, MaxFontSize;//מקסימום ומינימום גודל טקסט

        //אירוע זה מגדיל את התווית הנבחרת כאשר העכבר עליו
        public static void Grow_Up(TextBlock tb, double to, double time)
        {
            OrigFontSize = tb.FontSize;
            MaxFontSize = OrigFontSize + to;

            anim.From = tb.FontSize;
            anim.To = MaxFontSize;
            anim.Duration = TimeSpan.FromSeconds(time);
            tb.BeginAnimation(TextBlock.FontSizeProperty, anim);
        }

        //אירוע זה מקטין את התווית הנבחרת כאשר העכבר עזב אותו
        public static void Grow_Down(TextBlock tb)
        {
            anim.From = tb.FontSize;
            anim.To = OrigFontSize;
            tb.BeginAnimation(TextBlock.FontSizeProperty, anim);
        }

    }
}

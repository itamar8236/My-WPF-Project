using System;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace WpfApplication1
{
    /// <summary>
    /// Class for increaing text animation on textblocks.
    /// </summary>
    class Growing
    {
        /// <summary>
        /// Animation.
        /// </summary>
        private static DoubleAnimation anim= new DoubleAnimation();
        /// <summary>
        /// Original and maximum text size.
        /// </summary>
        private static double OrigFontSize, MaxFontSize;

        /// <summary>
        /// Increasing the text in the text block.
        /// </summary>
        /// <param name="tb">The text block.</param>
        /// <param name="to">Max size.</param>
        /// <param name="time">Time of animation.</param>
        public static void Grow_Up(TextBlock tb, double to, double time)
        {
            OrigFontSize = tb.FontSize;
            MaxFontSize = OrigFontSize + to;

            anim.From = tb.FontSize;
            anim.To = MaxFontSize;
            anim.Duration = TimeSpan.FromSeconds(time);
            tb.BeginAnimation(TextBlock.FontSizeProperty, anim);
        }

        /// <summary>
        /// Decreasing the text in the text block back to the original size.
        /// </summary>
        /// <param name="tb">The text block.</param>
        public static void Grow_Down(TextBlock tb)
        {
            anim.From = tb.FontSize;
            anim.To = OrigFontSize;
            tb.BeginAnimation(TextBlock.FontSizeProperty, anim);
        }

    }
}

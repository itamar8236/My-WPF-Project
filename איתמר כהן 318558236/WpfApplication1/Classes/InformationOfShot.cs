namespace WpfApplication1
{
    /// <summary>
    /// Class for the current shot information.
    /// </summary>
    public class InformationOfShot
    {
        /// <summary>
        /// The current cannon ball
        /// </summary>
        public CannonBall ShotBall;

        /// <summary>
        /// The ball's ending X position
        /// </summary>
        public double BallpxEnd;
        /// <summary>
        /// The ball's ending Y position
        /// </summary>
        public double BallpyEnd;
        /// <summary>
        /// The ball's ending Z position
        /// </summary>
        public double BallpzEnd;

        /// <summary>
        /// Max height of the ball
        /// </summary>
        public double MaxHeight;
        /// <summary>
        /// Duration for the entire shot.
        /// </summary>
        public double AllshotDuration;
        /// <summary>
        /// Duration until the ball reach max height
        /// </summary>
        public double DurationTillMaxHeight;
    }
}

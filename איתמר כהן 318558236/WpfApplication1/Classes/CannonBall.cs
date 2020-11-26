using System;
using System.Windows.Media.Media3D;
using System.Windows.Media.Animation;

namespace WpfApplication1
{
    /// <summary>
    /// Cannon ball class.
    /// </summary>
    public class CannonBall
    {
        /// <summary>
        /// The 3D model of the ball
        /// </summary>
        public Model3D ballModel;
        /// <summary>
        /// Type of ball. true if the the ball is about to hit a target and false otherwise.
        /// </summary>
        public bool IsThisHitBall;
        /// <summary>
        /// Animation for the X or Z axis
        /// </summary>
        public static DoubleAnimation Danim = new DoubleAnimation();
        /// <summary>
        /// Animation for hit ball. needed for adding function when the animation complited.
        /// </summary>
        public DoubleAnimation DanimForHitBall = new DoubleAnimation();
        /// <summary>
        /// Animation for the Y axis when the ball is going up
        /// </summary>
        public DoubleAnimation DanimY = new DoubleAnimation();
        /// <summary>
        /// Animation for the Y axis when the ball is going down
        /// </summary>
        public DoubleAnimation DanimY2 = new DoubleAnimation();
        /// <summary>
        /// Current game mode window
        /// </summary>
        public GameMode game;

        /// <summary>
        /// Construction for cannon ball
        /// </summary>
        /// <param name="bl">The 3D ball model</param>
        /// <param name="px">The ball's X posintion on the screen</param>
        /// <param name="py">The ball's Y posintion on the screen</param>
        /// <param name="pz">The ball's Z posintion on the screen</param>
        /// <param name="Kindofball">The ball type. true if the the ball is about to hit a target and false otherwise.</param>
        public CannonBall(Model3D bl, double px, double py, double pz, bool Kindofball)
        {
            ballModel = bl;
            ((Transform3DGroup)(ballModel.Transform)).Children[2] = new TranslateTransform3D(px, py, pz);
            IsThisHitBall = Kindofball;
            DanimForHitBall.Completed += EndOfHittedShot;
        }
        
        /// <summary>
        /// Function for knowing current X position
        /// </summary>
        /// <returns>The current X position on the screen</returns>
        public double GetCurXPos() 
        { return ((TranslateTransform3D)(((Transform3DGroup)(ballModel.Transform)).Children[2])).OffsetX; }
        /// <summary>
        /// Function for knowing current Y position
        /// </summary>
        /// <returns>The current Y position on the screen</returns>
        public double GetCurYPos()
        { return ((TranslateTransform3D)(((Transform3DGroup)(ballModel.Transform)).Children[2])).OffsetY; }
        /// <summary>
        /// Function for knowing current Z position
        /// </summary>
        /// <returns>The current Z position on the screen</returns>
        public double GetCurZPos()
        { return ((TranslateTransform3D)(((Transform3DGroup)(ballModel.Transform)).Children[2])).OffsetZ; }


        /// <summary>
        /// Animation on the X axis.
        /// </summary>
        /// <param name="to">X axis end position.</param>
        /// <param name="time">Time of the animation.</param>
        public void MoveX(double to, double time)
        {
            Danim.To = to;
            Danim.Duration = TimeSpan.FromSeconds(time);
            ((TranslateTransform3D)(((Transform3DGroup)(ballModel.Transform)).Children[2])).BeginAnimation(TranslateTransform3D.OffsetXProperty, Danim);
        }
        /// <summary>
        /// Animation on the Y axis (the ball goes up & down).
        /// </summary>
        /// <param name="to">The max height of the ball animation</param>
        /// <param name="time1">Time until ball at max height</param>
        /// <param name="time2">Time from max height to the end of animation</param>
        /// <param name="endY">The ending Y position</param>
        public void MoveY(double to, double time1, double time2, double endY)
        {
            DanimY.To = to;
            DanimY.Duration = TimeSpan.FromSeconds(time1);
            DanimY.DecelerationRatio = 1;
            DanimY.Completed += FromMaxToZeroOnY;
            ((TranslateTransform3D)(((Transform3DGroup)(ballModel.Transform)).Children[2])).BeginAnimation(TranslateTransform3D.OffsetYProperty, DanimY);

            DanimY2.Duration = TimeSpan.FromSeconds(time2);
            DanimY2.To = endY;
        }
        /// <summary>
        /// Second animation on the Y axis. starts animation from current until the end position
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FromMaxToZeroOnY(object sender, EventArgs e)
        {
            DanimY2.AccelerationRatio = 1;
            ((TranslateTransform3D)(((Transform3DGroup)(ballModel.Transform)).Children[2])).BeginAnimation(TranslateTransform3D.OffsetYProperty, DanimY2);
        }
        /// <summary>
        /// Animation on the Z axis
        /// </summary>
        /// <param name="to">Z axis end position.</param>
        /// <param name="time">Time of the animation.</param>
        public void MoveZ(double to, double time)
        {
            if(IsThisHitBall)
            {
                
                DanimForHitBall.To = to;
                DanimForHitBall.Duration = TimeSpan.FromSeconds(time);
                ((TranslateTransform3D)(((Transform3DGroup)(ballModel.Transform)).Children[2])).BeginAnimation(TranslateTransform3D.OffsetZProperty, DanimForHitBall);
            }
            else
            {
                Danim.To = to;
                Danim.Duration = TimeSpan.FromSeconds(time);
                ((TranslateTransform3D)(((Transform3DGroup)(ballModel.Transform)).Children[2])).BeginAnimation(TranslateTransform3D.OffsetZProperty, Danim);
            }

        }
        /// <summary>
        /// Function for the ending animation of hit ball.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EndOfHittedShot(object sender, EventArgs e)
        {
            //Find the hitted target and animate it's fall.
            game.HitTarget();
            //Animate the ball to fall along with the target.
            ((RotateTransform3D)(((Transform3DGroup)(ballModel.Transform)).Children[1])).CenterY = -GetCurYPos();
            Danim.To = -90;
            Danim.Duration = TimeSpan.FromSeconds(1);
            ((RotateTransform3D)(((Transform3DGroup)(ballModel.Transform)).Children[1])).Rotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, Danim);
            
        }
    }
}



    

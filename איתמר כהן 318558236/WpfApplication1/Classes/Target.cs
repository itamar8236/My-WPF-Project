using System;
using System.Windows.Media.Media3D;
using System.Windows.Media.Animation;

namespace WpfApplication1
{
    /// <summary>
    /// Target class
    /// </summary>
    class Target
    {
        /// <summary>
        /// The Y position of the target (height).
        /// </summary>
        public double PosY;
        /// <summary>
        /// The Z position of the target (distance).
        /// </summary>
        public double PosZ;
        /// <summary>
        /// The starting X position
        /// </summary>
        public double FromX;
        /// <summary>
        /// The ending X position
        /// </summary>
        public double ToX;
        /// <summary>
        /// Target's speed
        /// </summary>
        public double V;
        /// <summary>
        /// 3D model of the target.
        /// </summary>
        public Model3D TargetModel;
        /// <summary>
        /// Target's state. true if the target is about to fall and flase otherwise.
        /// </summary>
        public bool HittedAndNeedToFall;
        /// <summary>
        /// Target's state. true if the target has already fallen
        /// </summary>
        public bool HasFallen;
        /// <summary>
        /// Target's state. true if the target is moving right.
        /// </summary>
        public bool IsGoingRight;

        /// <summary>
        /// One time (falling) animation.
        /// </summary>
        public static DoubleAnimation DAnim = new DoubleAnimation();
        /// <summary>
        /// Animation forever.
        /// </summary>
        private DoubleAnimation DanimForever = new DoubleAnimation();

        /// <summary>
        /// Constructor for Target.
        /// </summary>
        /// <param name="T">3D model of the target.</param>
        /// <param name="px">Starting x positon</param>
        /// <param name="py">Starting y positon</param>
        /// <param name="pz">Starting z positon</param>
        /// <param name="fx">X starting position for the for the endless animation</param>
        /// <param name="tx">X ending position for the for the endless animation</param>
        /// <param name="speed">Target's speed</param>
        public Target(Model3D T, double px, double py, double pz, double fx, double tx, double speed)
        {
            TargetModel = T;
            PosY = py;
            PosZ = pz;
            FromX = fx;
            ToX = tx;
            V = speed;
            IsGoingRight = fx < tx;
            HasFallen = false;
            HittedAndNeedToFall = false;
            ((Transform3DGroup)(TargetModel.Transform)).Children[2] = new TranslateTransform3D(px, py, pz);

            ((RotateTransform3D)(((Transform3DGroup)(TargetModel.Transform)).Children[1])).CenterY = -TargetModel.Bounds.SizeY / 2;

            DanimForever.Completed += DanimForever_Completed;
        }

        /// <summary>
        /// This function continue the animation endlessly to the other direction if the target has not fallen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DanimForever_Completed(object sender, EventArgs e)
        {
            if (!HasFallen)
            {
                if ((IsGoingRight && FromX < ToX) || (!IsGoingRight && FromX > ToX))
                {
                    DanimForever.To = FromX;
                    DanimForever.From = ToX;
                }
                else
                {
                    DanimForever.To = ToX;
                    DanimForever.From = FromX;
                }
            ((Transform3DGroup)(TargetModel.Transform)).Children[2].BeginAnimation(TranslateTransform3D.OffsetXProperty, DanimForever);
                IsGoingRight = !IsGoingRight;
            }
        }
        /// <summary>
        /// This function calculate the time for the target in one animation
        /// </summary>
        /// <returns>The time for the target to move from one side to the other.</returns>
        public double CalculateTime()
        { return (Math.Abs(FromX - ToX)) / V; }

        /// <summary>
        /// Function for current x position
        /// </summary>
        /// <returns>The current X position of the target.</returns>
        public double GetCurXPos()
        { return ((TranslateTransform3D)((Transform3DGroup)TargetModel.Transform).Children[2]).OffsetX; }
        
        /// <summary>
        /// Starts the endless animation.
        /// </summary>
        public void AnimForever()
        {
            DanimForever.To = ToX;
            DanimForever.From = FromX;
            DanimForever.Duration = TimeSpan.FromSeconds(CalculateTime());
            ((Transform3DGroup)(TargetModel.Transform)).Children[2].BeginAnimation(TranslateTransform3D.OffsetXProperty, DanimForever);
        }
        /// <summary>
        /// Stop the animation
        /// </summary>
        public void StopAimation()
        {
            ((TranslateTransform3D)((Transform3DGroup)(TargetModel.Transform)).Children[2]).OffsetX = ((TranslateTransform3D)((Transform3DGroup)(TargetModel.Transform)).Children[2]).OffsetX;
            ((TranslateTransform3D)((Transform3DGroup)(TargetModel.Transform)).Children[2]).BeginAnimation(TranslateTransform3D.OffsetXProperty, null);
            V = 0;
        }
        /// <summary>
        /// Rotating the taget to fall. center axis is the ground(in xaml).
        /// </summary>
        /// <param name="To">The end angle in degrees. -90 deg is fallen target.</param>
        /// <param name="time">Time of animation</param>
        public void Rotate(double To, double time)//מפעיל אנימצית סיבוב
        {
            DAnim.RepeatBehavior = new RepeatBehavior(1);
            DAnim.AutoReverse = false;
            DAnim.To = To;
            DAnim.From = 0;
            DAnim.Duration = TimeSpan.FromSeconds(time);
            ((RotateTransform3D)((Transform3DGroup)TargetModel.Transform).Children[1]).Rotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, DAnim);
        }
    }
}

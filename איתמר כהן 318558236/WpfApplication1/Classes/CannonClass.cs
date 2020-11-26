using System;
using System.Windows.Media.Media3D;
using System.Windows.Media.Animation;

namespace WpfApplication1
{
    /// <summary>
    /// Cannon class
    /// </summary>
    public  class CannonClass
    {
        /// <summary>
        /// Firing' start speed.
        /// </summary>
        public double V0 = 150;
        /// <summary>
        /// 3D model of the entire cannon
        /// </summary>
        public Model3D CannonModel;
        /// <summary>
        /// 3D model of tje barrel
        /// </summary>
        public Model3D BarrelModel;
        /// <summary>
        /// Barrel's Y position
        /// </summary>
        public double PosY;
        /// <summary>
        /// Animation
        /// </summary>
        public DoubleAnimation Danim = new DoubleAnimation();
        /// <summary>
        /// The distance of the shot in no-physics mode.
        /// </summary>
        public double DisOfShotNophys;
        /// <summary>
        /// Barrel's length (X axis length)
        /// </summary>
        public double BarrelSizeX;
        /// <summary>
        /// Barrel's height (Y axis length)
        /// </summary>
        public double BarrelSizeY;
        /// <summary>
        /// Barrel's width (Z axis length)
        /// </summary>
        public double BarrelSizeZ;
        /// <summary>
        /// Current game mode window
        /// </summary>
        public GameMode game;
        /// <summary>
        /// Constructor for CannonClass
        /// </summary>
        /// <param name="AllCannon">3D model of the cannon</param>
        /// <param name="Barrel">3D model of the barrel</param>
        /// <param name="DisNoPhs">Distance of the shot in no-physics mode.</param>
        /// <param name="speed">Firing' start speed.</param>
        /// <param name="gm">Game mode current window</param>
        public CannonClass(Model3D AllCannon, Model3D Barrel, double DisNoPhs, double speed, GameMode gm)//פעולת בנאי
        {
            game = gm;
            CannonModel = AllCannon;
            BarrelModel = Barrel;
            DisOfShotNophys = DisNoPhs;
            PosY = CannonModel.Bounds.SizeY / 2;
            V0 = speed;

            (((Transform3DGroup)(CannonModel.Transform)).Children[2]) = new TranslateTransform3D(0, PosY, 0);

            if (game.level == 2)
                ((AxisAngleRotation3D)((RotateTransform3D)((Transform3DGroup)BarrelModel.Transform).Children[1]).Rotation).Angle = -0.1;

            BarrelSizeX = BarrelModel.Bounds.SizeX;
            BarrelSizeY = BarrelModel.Bounds.SizeY;
            BarrelSizeZ = BarrelModel.Bounds.SizeZ;

            ((RotateTransform3D)(((Transform3DGroup)(BarrelModel.Transform)).Children[1])).CenterY = 0.9;

            ((RotateTransform3D)(((Transform3DGroup)(BarrelModel.Transform)).Children[1])).CenterZ = -(BarrelModel.Bounds.SizeZ / 4);

        }

        /// <summary>
        /// Function for knowing the entire cannon's angle
        /// </summary>
        /// <returns>The current entire cannon's angle</returns>
        public double GetCurAllCannonAngle()
        {
            return 180 - ((AxisAngleRotation3D)((RotateTransform3D)((Transform3DGroup)CannonModel.Transform).Children[1]).Rotation).Angle;
        }
        /// <summary>
        /// Function for knowing the entire barrel's angle
        /// </summary>
        /// <returns>The current entire barrel's angle</returns>
        public double GetCurBarrelAngle()
        {
            return Math.Abs(((AxisAngleRotation3D)((RotateTransform3D)((Transform3DGroup)BarrelModel.Transform).Children[1]).Rotation).Angle);
        }
        /// <summary>
        /// Angle rotation animation on the entire cannon.
        /// </summary>
        /// <param name="to">The end angle in degrees. fully right is 90 deg, fully left is 270 deg.</param>
        /// <param name="time">Time of animation</param>
        public void RotateAllCannon(double to, double time)//אנימציית סיבוב על כל התותח
        {
            Rotate(CannonModel, to, time);
        }
        /// <summary>
        /// Stops the animation on the entire cannon.
        /// </summary>
        public  void StopAllCannon()
        {
            ((AxisAngleRotation3D)((RotateTransform3D)((Transform3DGroup)CannonModel.Transform).Children[1]).Rotation).Angle = ((AxisAngleRotation3D)((RotateTransform3D)((Transform3DGroup)CannonModel.Transform).Children[1]).Rotation).Angle;
            ((RotateTransform3D)(((Transform3DGroup)(CannonModel.Transform)).Children[1])).Rotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, null);
        }
        /// <summary>
        /// Angle rotation animation on the barrel.
        /// </summary>
        /// <param name="to">The end angle in degrees. fully up is -90 deg, straight down is 0 deg.</param>
        /// <param name="time">Time for the animaton</param>
        public void RotateBarrel(double to, double time)
        {
            Rotate(BarrelModel, to, time);
        }
        /// <summary>
        /// Stops the animation on the barrel.
        /// </summary>
        public  void StopBarrel()
        {
            ((AxisAngleRotation3D)((RotateTransform3D)((Transform3DGroup)BarrelModel.Transform).Children[1]).Rotation).Angle = ((AxisAngleRotation3D)((RotateTransform3D)((Transform3DGroup)BarrelModel.Transform).Children[1]).Rotation).Angle;
            ((RotateTransform3D)(((Transform3DGroup)(BarrelModel.Transform)).Children[1])).Rotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, null);
        }
        /// <summary>
        /// Roatate animation. the center axis is made in the model in xaml.
        /// </summary>
        /// <param name="Model">The 3D model to rotate</param>
        /// <param name="to">End angle</param>
        /// <param name="time">Time of animation.</param>
        private  void Rotate(Model3D Model, double to, double time)
        {
            Danim.To = to;
            Danim.Duration = TimeSpan.FromSeconds(time);
            ((RotateTransform3D)(((Transform3DGroup)(Model.Transform)).Children[1])).Rotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, Danim);
        }
    }
}

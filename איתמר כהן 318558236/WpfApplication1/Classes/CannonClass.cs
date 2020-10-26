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
    public  class CannonClass
    {
        public double V0 = 150;//מהירות בלי פיזיקה
        public Model3D CannonModel;//מודל התותח
        public Model3D BarrelModel;//מודל הקנה
        public double PosY;//הגובה של הקנה
        public DoubleAnimation Danim = new DoubleAnimation();//
        public double DisOfShotNophys;//מרחק היריה בלי פיזיקה
        public double BarrelSizeX;//אורך הקנה
        public double BarrelSizeY;//גובה הקנה
        public double BarrelSizeZ;//רוחב הקנה
       public GameMode game;//חלון המשחק
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

        public  double GetCurAllCannonAngle()//מחזיר זווית עכשווית של התותח
        {
            return 180 - ((AxisAngleRotation3D)((RotateTransform3D)((Transform3DGroup)CannonModel.Transform).Children[1]).Rotation).Angle;
        }
        public  double GetCurBarrelAngle()//מחזיר זווית עכשווית של הקנה
        {
            return Math.Abs(((AxisAngleRotation3D)((RotateTransform3D)((Transform3DGroup)BarrelModel.Transform).Children[1]).Rotation).Angle);
        }
        public  void RotateAllCannon(double to, double time)//אנימציית סיבוב על כל התותח
        {
            Rotate(CannonModel, to, time);
        }
        public  void StopAllCannon()//מפסיק אנימצית סיבוב על התותח
        {
            ((AxisAngleRotation3D)((RotateTransform3D)((Transform3DGroup)CannonModel.Transform).Children[1]).Rotation).Angle = ((AxisAngleRotation3D)((RotateTransform3D)((Transform3DGroup)CannonModel.Transform).Children[1]).Rotation).Angle;
            ((RotateTransform3D)(((Transform3DGroup)(CannonModel.Transform)).Children[1])).Rotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, null);
        }
        public  void RotateBarrel(double to, double time)//אנימציתץ סיבוב על הקנה
        {
            Rotate(BarrelModel, to, time);
        }
        public  void StopBarrel()//מפסיק אנימצית סיבוב על הקנה
        {
            ((AxisAngleRotation3D)((RotateTransform3D)((Transform3DGroup)BarrelModel.Transform).Children[1]).Rotation).Angle = ((AxisAngleRotation3D)((RotateTransform3D)((Transform3DGroup)BarrelModel.Transform).Children[1]).Rotation).Angle;
            ((RotateTransform3D)(((Transform3DGroup)(BarrelModel.Transform)).Children[1])).Rotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, null);
        }
        private  void Rotate(Model3D Model, double to, double time)//מפעיל אנימצית סיבוב
        {
            Danim.To = to;
            Danim.Duration = TimeSpan.FromSeconds(time);
            ((RotateTransform3D)(((Transform3DGroup)(Model.Transform)).Children[1])).Rotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, Danim);
        }
    }
}

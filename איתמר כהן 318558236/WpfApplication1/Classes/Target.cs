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
    class Target
    {
        public double PosY;//מיקום הווי של המטרה
        public double PosZ;//מיקום הזד של המטרה
        public double FromX;//מיקום האיקס ההתחלתי של המטרה
        public double ToX;//מיקום האיקס הסופי של המטרה
        public double V;//מהירות המטרה
        public Model3D TargetModel;//מודל המטרה
        public bool HittedAndNeedToFall;//המטרה הולכת להיפגע
        public bool HasFallen;//המטרה נפלה
        public bool IsGoingRight;//המטרה זזה כעת ימינה

        public static DoubleAnimation DAnim = new DoubleAnimation();//אנימציה חד פעמית
        private DoubleAnimation DanimForever = new DoubleAnimation();//אנימציה אינסופית
        public Target(Model3D T, double px, double py, double pz, double fx, double tx, double speed)//פעולת בנאי
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

        private void DanimForever_Completed(object sender, EventArgs e)//ממשיך את האנימציה שוב לכיוון הנגדי
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
        public double CalculateTime()//מחזיר את הזמן שלוקח למטרה לעבור מצד לצד
        { return (Math.Abs(FromX - ToX)) / V; }

        public double GetCurXPos()//מחזיר מיקום איקס עכשווי
        { return ((TranslateTransform3D)((Transform3DGroup)TargetModel.Transform).Children[2]).OffsetX; }

        public void AnimForever()//אנימציה אינסופית למטרות
        {
            DanimForever.To = ToX;
            DanimForever.From = FromX;
            DanimForever.Duration = TimeSpan.FromSeconds(CalculateTime());
            ((Transform3DGroup)(TargetModel.Transform)).Children[2].BeginAnimation(TranslateTransform3D.OffsetXProperty, DanimForever);
        }
        public void StopAimation()//מפסיק אנימציה
        {
            ((TranslateTransform3D)((Transform3DGroup)(TargetModel.Transform)).Children[2]).OffsetX = ((TranslateTransform3D)((Transform3DGroup)(TargetModel.Transform)).Children[2]).OffsetX;
            ((TranslateTransform3D)((Transform3DGroup)(TargetModel.Transform)).Children[2]).BeginAnimation(TranslateTransform3D.OffsetXProperty, null);
            V = 0;
        }
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

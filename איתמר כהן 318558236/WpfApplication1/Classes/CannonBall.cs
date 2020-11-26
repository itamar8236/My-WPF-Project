using System;
using System.Windows.Media.Media3D;
using System.Windows.Media.Animation;

namespace WpfApplication1
{
    public class CannonBall
    {
        public Model3D ballModel;//מודל הכדור
        public bool IsThisHitBall;//סוג הכדור
        public static DoubleAnimation Danim = new DoubleAnimation();//אנימציה
        public DoubleAnimation DanimForHitBall = new DoubleAnimation();//אנימציה עבור כדור "פוגע"
        public DoubleAnimation DanimY = new DoubleAnimation();//אנימציה ראשונה על ציר ה y
        public DoubleAnimation DanimY2 = new DoubleAnimation();//אנימציה שניה על ציר ה y

        public GameMode game;//חלון המשחק העכשווי


        public CannonBall(Model3D bl, double px, double py, double pz, bool Kindofball)//פעולת בנאי
        {
            ballModel = bl;
            ((Transform3DGroup)(ballModel.Transform)).Children[2] = new TranslateTransform3D(px, py, pz);
            IsThisHitBall = Kindofball;
            DanimForHitBall.Completed += EndOfHittedShot;
        }
        //מחזיר  את המיקום העכשווי
        public double GetCurXPos() 
        { return ((TranslateTransform3D)(((Transform3DGroup)(ballModel.Transform)).Children[2])).OffsetX; }
        public double GetCurYPos()
        { return ((TranslateTransform3D)(((Transform3DGroup)(ballModel.Transform)).Children[2])).OffsetY; }
        public double GetCurZPos()
        { return ((TranslateTransform3D)(((Transform3DGroup)(ballModel.Transform)).Children[2])).OffsetZ; }



        public void MoveX(double to, double time)//אנימציה על ציר הX
        {
            Danim.To = to;
            Danim.Duration = TimeSpan.FromSeconds(time);
            ((TranslateTransform3D)(((Transform3DGroup)(ballModel.Transform)).Children[2])).BeginAnimation(TranslateTransform3D.OffsetXProperty, Danim);
        }
        public void MoveY(double to, double time1, double time2, double endY)//אנימציה ראשונה על ציר הY
        {
            DanimY.To = to;
            DanimY.Duration = TimeSpan.FromSeconds(time1);
            DanimY.DecelerationRatio = 1;
            DanimY.Completed += FromMaxToZeroOnY;
            ((TranslateTransform3D)(((Transform3DGroup)(ballModel.Transform)).Children[2])).BeginAnimation(TranslateTransform3D.OffsetYProperty, DanimY);

            DanimY2.Duration = TimeSpan.FromSeconds(time2);
            DanimY2.To = endY;
        }

        private void FromMaxToZeroOnY(object sender, EventArgs e)//yאנימציה שניה על ציר ה
        {
            DanimY2.AccelerationRatio = 1;
            ((TranslateTransform3D)(((Transform3DGroup)(ballModel.Transform)).Children[2])).BeginAnimation(TranslateTransform3D.OffsetYProperty, DanimY2);
        }

        public void MoveZ(double to, double time)//אנימציה על ציר הZ
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

        private void EndOfHittedShot(object sender, EventArgs e)//כשמסתיים אנימציה של כדור "פוגע
        {
            //מפיל מטרה
            game.HitTarget();
            //מפיל כדור
            ((RotateTransform3D)(((Transform3DGroup)(ballModel.Transform)).Children[1])).CenterY = -GetCurYPos();
            Danim.To = -90;
            Danim.Duration = TimeSpan.FromSeconds(1);
            ((RotateTransform3D)(((Transform3DGroup)(ballModel.Transform)).Children[1])).Rotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, Danim);
            
        }
    }
}



    

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;



namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class GameMode : Window
    {
        //hi there
        //בכל הפרויקט: סקייל 0 זווית 1 מיקום 2

        public int level = 1;//שלב
        public int NumOfShotsAllowd = 10;//למקרה שזה אימונים ואז תמיד ימלא 10-0=10 בערימה מספר היריות שיש לו לשלב 
        public int NumOfSecsAllowd = 0;//כמה שניות יש לו לשלב
        public int numOftargets = 5;//מספר מטרות
        public double speed = 150;//מהירות היריה(ההתחלתית)
        public double VfirstTarget = 15;//מהירות המטרה הראשונה(הכי מהירה)
        public bool IsTraining = false;//אם זה אימונים ואז לא יכול להפסיד

        private int NumOfBallsInPile = 10;//מספר הכדורים בכל רגע בערימה
        private int ticknum = 0;//כמות השניות שעברו
        private bool AllCannonAlreadyMove = false; //בןדק אם התותח זז
        private bool BarrelMove = false;//בןדק אם הקנה זז
        private bool LockEvrything = false;//בודק אם צריך לנעול כל תזוזה של הצשתמש
        private bool alreadyOpen = false;//בודק אם החלון כבר עלה
        private MediaPlayer Fire = new MediaPlayer();//סאונד
        private DispatcherTimer timer = new DispatcherTimer();//טיימר
        private CannonBall[] CannonBallSInPile = new CannonBall[10];//מערך הכדורים בערימה
        private Target[] targets;//מערך מטרות
        private  CannonClass Cannon;//תותח 
        private int NumofShots = 0;//כמה יריות ירה

        public GameMode()
        {
            InitializeComponent();
        }
       
        private void startAnimation(object sender, EventArgs e)//מאתחל את המשחק
        {
            if (!alreadyOpen)//למקרה שהחלון נפתח יותר מפעם אחת, כגון "יורד" למטה ו"עולה" חזרה
            {
                alreadyOpen = true;

                //מערך מטרות
                targets = new Target[numOftargets];
                targets[0] = new Target(AllTargetModel, 0, AllTargetModel.Bounds.SizeY / 2, -100, 60, -60, VfirstTarget);//שם מטרה ראשונה במקום

                //התותח במשחק
                Cannon = new CannonClass(AllCannonModel, AllBarrelModel, 110+(50*(numOftargets-1)), speed, this);

                //יוצר מטרות
                for (int i = 1; i < targets.Length; i++)
                {
                    double z = targets[i - 1].PosZ - 50;//כל מטרה רחוקה ב50
                    double from, to, v;
                    if (i % 2 == 1)
                    {
                        from = (targets[i - 1].FromX + 20) * -1;//\כל מטרה זזה עוד 20 ימינה ושמאלה
                        to = (targets[i - 1].ToX * -1) + 20;    ///
                    }
                    else
                    {
                        from = (targets[i - 1].FromX * -1) + 20;
                        to = (targets[i - 1].ToX + 20) * -1;
                    }
                    v = targets[i - 1].V * 0.9;
                    targets[i] = new Target(AllTargetModel.Clone(), 0, targets[0].PosY, z, from, to, v);
                    scene.Children.Add(targets[i].TargetModel);
                } 

                //מתחיל אנימציה של מטרות לנצח
                for (int i = 0; i < targets.Length; i++)
                    targets[i].AnimForever();


                // יוצר מערך כדורים ומסדר בהתחלה בערימה
                NumOfBallsInPile = Math.Min(10, NumOfShotsAllowd);
                CreateBallsInPileArr(NumOfBallsInPile);

                //מאתחל טיימר 
                timer.Tick += Timer_Tick;
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Start();
            }
        }

        private void Pressed(object sender, KeyEventArgs e)//מקש נלחץ
        {
            
            if (!AllCannonAlreadyMove && !LockEvrything && !BarrelMove)
            {
                switch (e.Key)
                {
                    case Key.Right://מזיז את התותח ימינה
                        {
                            Cannon.RotateAllCannon(90 + 0.1, 4);
                            AllCannonAlreadyMove = true;
                            break;
                        }
                    case Key.Left://מזיז את התותח שמאלה
                        {
                           
                            Cannon.RotateAllCannon(270 - 0.1, 4);
                            AllCannonAlreadyMove = true;
                            break;
                        }
                    case Key.Up://מזיז את הקנה למעלה
                        {
                            if(level == 2)
                            {
                            Cannon.RotateBarrel(-45, 4);
                            BarrelMove = true;
                            }
                            break;
                        }
                    case Key.Down://מזיז את הקנה למטה
                        {
                            if (level == 2)
                            {
                                Cannon.RotateBarrel(-0.1, 4);//למנוע באג כאשר הקנה הכי למטה
                                BarrelMove = true;
                            }
                            break;
                        }
                    case Key.Space://יורה
                        {
                            if(!IsTraining)
                                NumofShots++;

                            if (!IsTraining && NumofShots > NumOfShotsAllowd)//בודק אם הפסיד
                            {
                                EndGame();
                                break;
                            }
                            //מפעיל רעש יריה
                            Fire.Open(new Uri(@".\sounds\Tank Firing-SoundBible.com-998264747.mp3", UriKind.Relative));
                            Fire.Volume = 1;
                            Fire.Play();


                            InformationOfShot CurShotInfo = CheckIfHit();//מידע על היריה הנוכחית

                            scene.Children.Add(CurShotInfo.ShotBall.ballModel);
                            //אנימציית היריה
                            if(level == 1)
                            {
                                CurShotInfo.ShotBall.MoveX(CurShotInfo.BallpxEnd, CurShotInfo.AllshotDuration);
                                CurShotInfo.ShotBall.MoveZ(CurShotInfo.BallpzEnd, CurShotInfo.AllshotDuration);
                            }
                            else
                            {
                                CurShotInfo.ShotBall.MoveX(CurShotInfo.BallpxEnd, CurShotInfo.AllshotDuration);

                                if (CurShotInfo.AllshotDuration > CurShotInfo.DurationTillMaxHeight)//בודק האם יתחיל "ליפול"
                                    CurShotInfo.ShotBall.MoveY(CurShotInfo.MaxHeight, CurShotInfo.DurationTillMaxHeight, CurShotInfo.AllshotDuration - CurShotInfo.DurationTillMaxHeight, CurShotInfo.BallpyEnd);
                                else
                                    CurShotInfo.ShotBall.MoveY(CurShotInfo.MaxHeight, CurShotInfo.AllshotDuration, 0, CurShotInfo.BallpyEnd);

                                CurShotInfo.ShotBall.MoveZ(CurShotInfo.BallpzEnd, CurShotInfo.AllshotDuration);
                            }

                                //מעדכן ערימה של הכדורים
                                if (NumOfBallsInPile >= 1)
                                    RemoveFromPile();
                                if((IsTraining && NumOfBallsInPile == 0) || (NumOfShotsAllowd - NumofShots > 0 && NumOfBallsInPile == 0))
                                    BallSInPile(Math.Min(10, NumOfShotsAllowd - NumofShots));

                          
                            break;
                        }
                    case Key.Enter:
                        {
                            for (int i = 0; i < targets.Length; i++)//צ'יט
                                targets[i].StopAimation();
                            
                            break;
                        }
                    case Key.Escape:
                        {
                            Application.Current.Shutdown();
                            break;
                        }
                }
            }
        }
        public InformationOfShot CheckIfHit()//בודק ומחזיר מידע על היריה
        {
            InformationOfShot ShotInfo = new InformationOfShot();//מידע על יריה
            double pxStart = 0;//\
            double pyStart = 0;// |מיקום התחלתי של הכדור
            double pzStart = 0;///

            bool ishit = false;//האם הכדור יפגע

            double EndPosXTarget;//מיקום האיקס של סוף היריה עד המטרה, של המטרה הנוכחית בבדיקה
            double time = 0;//זמן היריה עד המטרה הנוכחית בבדיקה

            double a, b, x;
            a = Cannon.GetCurAllCannonAngle() * Math.PI / 180;//זויות כל התותח
            b = Cannon.GetCurBarrelAngle() * Math.PI / 180;//זווית הקנה
            x = Cannon.BarrelSizeZ;

            double Vx, V0y;                    //\
            Vx = Cannon.V0 * Math.Cos(b); // |לשלב 2
            V0y = Cannon.V0 * Math.Sin(b);///

            double AllShotDisZ;//מיקום זד הסופי של הכדור בכל היריה

            if (level == 1)
            {
                pyStart = 0.9 + Cannon.PosY;//גובה בתוך הקנה
                ShotInfo.BallpyEnd = pyStart;
                AllShotDisZ = targets[targets.Length - 1].PosZ - 10;//רחוק יותר מהמטרה האחרונה

                ShotInfo.AllshotDuration = Cannon.DisOfShotNophys / (Cannon.V0 * Math.Cos((Cannon.GetCurAllCannonAngle() * Math.PI) / 180));//היריה על פי מהירות התחלתית קבועה ותמיד מגיעה לDis...
            }
            else
            {
                //שם כדור בקצה הקנה, מסובך בגלל 2 מרכזי סיבוב שונים                
                pxStart = ((3 * x / 4) * Math.Cos(b) - (x / 4)) * Math.Sin(a);
                pyStart = Cannon.PosY + 0.9 + (3 * x / 4) * Math.Sin(b);
                pzStart = -((3 * x / 4) * Math.Cos(b) * Math.Cos(a) - ((x / 4) * Math.Cos(a)));


                //חישוב הזמן של היריה
                double A, B, C;//מקדמי משוואה  ריבועית
                               //לפי הנוסחה< חישוב של הזמן הכולל של היריה x = x0 + v0t + 0.5at^2
                A = 9.8 / 2;
                B = -V0y;
                C = -pyStart;

                ShotInfo.AllshotDuration = (-B + Math.Sqrt(B * B - 4 * A * C)) / (2 * A);

                ShotInfo.DurationTillMaxHeight = V0y / 9.8;//על פי הנוסחה v=v0+at
                ShotInfo.MaxHeight = pyStart + (V0y * ShotInfo.DurationTillMaxHeight) + (0.5 * (-9.8) * Math.Pow(ShotInfo.DurationTillMaxHeight, 2));//על פי הנוסחה x = x0 + v0t + 0.5at^2
                AllShotDisZ = pzStart - (Vx * ShotInfo.AllshotDuration) * Math.Cos(a);
                
            }
            int i;//אינדקס מטרה בבדיקה
            for (i = 0; i < targets.Length && !ishit && Math.Abs(targets[i].PosZ) < Math.Abs(AllShotDisZ); i++)
            {
                if(!targets[i].HasFallen)
                {
                    if(level == 1)
                    {
                        ShotInfo.BallpxEnd = Math.Abs(targets[i].PosZ) * Math.Tan(a);
                        ShotInfo.BallpyEnd = pyStart;

                        time = Math.Abs(targets[i].PosZ) / (Cannon.V0 * Math.Cos(a));//דרך חלקי מהירות
                    }
                    else
                    {
                        ShotInfo.BallpxEnd = Math.Tan(a) * Math.Abs(targets[i].PosZ - pzStart);

                        //מחשב מרחק דו מימדי בין מיקום סופי להתחלה, ומחלק במהירות לקבל זמן
                        time = Math.Sqrt(Math.Pow(Math.Abs(targets[i].PosZ - pzStart), 2) + Math.Pow(ShotInfo.BallpxEnd, 2)) / Vx;

                        ShotInfo.BallpyEnd = pyStart + V0y * time + 0.5 * (-9.8) * Math.Pow(time, 2); //על פי הנוסחה x = x0 + v0t + 0.5at^2
                    }

                    ShotInfo.BallpzEnd = targets[i].PosZ;

                    EndPosXTarget = targets[i].GetCurXPos();//מחשב מיקום איקס סופי של המטרה
                    if (targets[i].IsGoingRight)
                    {
                        EndPosXTarget += targets[i].V * time;
                        if (Math.Abs(EndPosXTarget) > Math.Abs(targets[i].FromX))
                        {
                            EndPosXTarget = Math.Abs(targets[i].FromX) - ((time - ((double)Math.Abs((targets[i].FromX - targets[i].GetCurXPos())) / targets[i].V)) * targets[i].V);
                        }
                    }

                    else
                    {
                        EndPosXTarget -= targets[i].V * time;
                        if (Math.Abs(EndPosXTarget) > Math.Abs(targets[i].FromX))
                        {
                            EndPosXTarget = -Math.Abs(targets[i].FromX) + ((time - ((double)Math.Abs((targets[i].FromX - targets[i].GetCurXPos())) / targets[i].V)) * targets[i].V);
                        }
                    }

                    double R, D;//רדיוס המטרה ומרחק הכדור ממנה
                    R = targets[i].TargetModel.Bounds.SizeX / 2;
                    D = Math.Sqrt(Math.Pow(ShotInfo.BallpxEnd - EndPosXTarget, 2) + Math.Pow(ShotInfo.BallpyEnd - targets[i].PosY, 2));
                    ishit = D <= R;
                }
            }
            if (ishit)
            {
                ShotInfo.AllshotDuration = time;
                targets[i - 1].HittedAndNeedToFall = true;
                LockEvrything = true;
            }
            else
            {
                ShotInfo.BallpzEnd = AllShotDisZ;
                //לעדכן את כל השדות ליריה שלא פוגעת.
                if (level == 1)
                    ShotInfo.BallpxEnd = Cannon.DisOfShotNophys * Math.Tan(a);
                else
                {
                    ShotInfo.BallpyEnd = 0;
                    ShotInfo.BallpxEnd = pxStart + (Vx * ShotInfo.AllshotDuration) * Math.Sin(a);
                }
            }
                
            ShotInfo.ShotBall = new CannonBall(AllBallModel.Clone(), pxStart, pyStart, pzStart, ishit);
            ShotInfo.ShotBall.game = this;
            return ShotInfo;
        }
        
        private void Timer_Tick(object sender, EventArgs e)//טיימר כל שניה
        {

            ticknum++;
            //מראה זמן
            Time.Text = (ticknum / 600).ToString() + (ticknum / 60).ToString() + ":" + (ticknum % 60 / 10).ToString() + (ticknum % 60 % 10).ToString();
            
            //בודק אם הפסיד
            if (!IsTraining && ticknum == NumOfSecsAllowd)
                EndGame();
        }
        

        private void Released(object sender, KeyEventArgs e)//מקש משתחרר
        {
            if (e.Key == Key.Right || e.Key == Key.Left)//מפסיק אנימציה של התותח
            {
                AllCannonAlreadyMove = false;
                Cannon.StopAllCannon();
            }
            if(e.Key == Key.Up || e.Key == Key.Down)//מפסיק אנימציה של הקנה
            {
                BarrelMove = false;
                Cannon.StopBarrel();
            }
        }

        private void Click(object sender, MouseButtonEventArgs e)//שם מידע בלייבל
        {
            label.Content = "Barrel Angle: " + Cannon.GetCurBarrelAngle().ToString();
        }
        public void HitTarget()//פגיעה במטרה
        {
            //מפעיל רעש של מטרה נפגעת
            Fire.Open(new Uri(@".\sounds\Big Explosion Effect Video Mp4 HD Sound.mp3", UriKind.Relative));
            Fire.Volume = 1;
            Fire.Play();

            //מוצא את המטרה שצריך להפיל
            int targetNum;
            bool found = false;
            for (targetNum = 0; targetNum < targets.Length && !found; targetNum++)
                found = targets[targetNum].HittedAndNeedToFall;
            targetNum--;

            targets[targetNum].StopAimation();//עוצר

            targets[targetNum].HasFallen = true;
            targets[targetNum].HittedAndNeedToFall = false;
            
            targets[targetNum].Rotate(-90, 1);//מפיל

            
            //בודק אם ניצח
            bool won = targets[0].HasFallen;
            for (int i = 1; i < targets.Length && won; i++)
                won = targets[i].HasFallen;
            
            if (won)
                EndGame();
            else
                LockEvrything = false;


        }
        public void BallSInPile(int sum)//שם כדורים בערימה ליד התותח
        {
            for(int i = 0; i < sum; i++)
                scene.Children.Add(CannonBallSInPile[i].ballModel);

            NumOfBallsInPile = sum;
        }
        public void RemoveFromPile()//מסיר כדור מהערימה
        {
            NumOfBallsInPile--;
            scene.Children.Remove(CannonBallSInPile[NumOfBallsInPile].ballModel);
        }
        public void CreateBallsInPileArr(int sum)//מייצר מערך כדורים בערימה
        {
            double size = AllBallModel.Bounds.SizeX;
            for ( int i = 0; i < sum; i++)
            {
                switch (i)
                {
                    case 9:
                        CannonBallSInPile[i] = new CannonBall(AllBallModel.Clone(), 5 + size, size * 2 + size / 2, -size, false);
                            break;
                    case 8:
                        CannonBallSInPile[i] = new CannonBall(AllBallModel.Clone(), 5 + size, size + size / 2, -size * 1.5, false);
                            break;
                    case 7:
                        CannonBallSInPile[i] = new CannonBall(AllBallModel.Clone(), 5 + (size * 1.5), size + size / 2, -size / 2,false);
                            break;
                    case 6:
                        CannonBallSInPile[i] = new CannonBall(AllBallModel.Clone(), 5 + (size / 2), size + size / 2, -size / 2,false);
                            break;
                    case 5:
                        CannonBallSInPile[i] = new CannonBall(AllBallModel.Clone(), 5 + size, size / 2, -size * 2,false);
                            break;
                    case 4:
                        CannonBallSInPile[i] = new CannonBall(AllBallModel.Clone(), 5 + (size * 1.5), size / 2, -size,false);
                            break;
                    case 3:
                        CannonBallSInPile[i] = new CannonBall(AllBallModel.Clone(), 5 + (size / 2), size / 2, -size,false);
                            break;
                    case 2:
                        CannonBallSInPile[i] = new CannonBall(AllBallModel.Clone(), 5 + (size * 2), size / 2, 0,false);
                            break;
                    case 1:
                        CannonBallSInPile[i] = new CannonBall(AllBallModel.Clone(), 5 + size, size / 2, 0,false);
                            break;
                    case 0:
                        CannonBallSInPile[i] = new CannonBall(AllBallModel.Clone(), 5, AllBallModel.Bounds.SizeY / 2, 0,false);
                            break;
                }
                scene.Children.Add(CannonBallSInPile[i].ballModel);
            }
        }
        
        private void EndGame()//סיום המשחק
        {
            quit.Visibility = Visibility.Visible;
            continueGame.Visibility = Visibility.Visible;

            timer.Stop();

            LockEvrything = true;

            if((NumofShots <= NumOfShotsAllowd && ticknum < NumOfSecsAllowd) || (IsTraining))
                Massege.Text = "You Win!";
            else
            {
                Massege.Text = "You lost!";

                int count = 0;//כמה מטרות הפיל
                for (int i = 0; i < targets.Length; i++)
                    if (targets[i].HasFallen)
                        count++;
                if (NumofShots > NumOfShotsAllowd)
                    Massege.Text += "\nYou run out of Ammo!";
                else
                    Massege.Text += "\nYou run out of time!";

                Massege.Text += "\nYou've hit " + count.ToString() + " targets, out of " + targets.Length.ToString();
            }
            
            Growing.Grow_Up((TextBlock)Massege, 50, 0.5);
        }
        private void GameModeMouseEnter(object sender, MouseEventArgs e)//עכבר נכנס ל continue או quit
        {
            Growing.Grow_Up((TextBlock)sender, 15, 0.5);
        }

        private void GameModeMouseLeave(object sender, MouseEventArgs e)//עכבר יוצא ל continue או quit
        {
            Growing.Grow_Down((TextBlock)sender);
        }

        private void GameModeMouseDown(object sender, MouseButtonEventArgs e)//עכבר לוחץ על continue או quit
        {
            if (e.Source == continueGame)
            {
                //לא עובד
                PickLevel pick = new PickLevel();
                pick.Show();
                this.Close();
            }
            else
                Application.Current.Shutdown();
        }
    }
}

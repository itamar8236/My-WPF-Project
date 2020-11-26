using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;



namespace Bullseye
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// In the entire project and in all models:
    /// Transform.Children[0] - scale tranformation/animation.
    /// Transform.Children[1] - rotation tranformation/animation.
    /// Transform.Children[2] - location tranformation/animation.
    /// </summary>
    public partial class GameMode : Window
    {
        /// <summary>
        /// The level. defult is level one.
        /// </summary>
        public int level = 1;
        /// <summary>
        /// The number of shots the player have.
        /// defult is 10 for training level, in that level the number of shots allowed, that decreasing, after the pile empty, is 0.
        /// </summary>
        public int NumOfShotsAllowd = 10;
        /// <summary>
        /// The number of seconds the player have
        /// </summary>
        public int NumOfSecsAllowd = 0;
        /// <summary>
        /// Number of targets in the game.
        /// </summary>
        public int numOftargets = 5;
        /// <summary>
        /// Starting speed of the shot
        /// </summary>
        public double speed = 150;
        /// <summary>
        /// First target's speed. 
        /// </summary>
        public double VfirstTarget = 15;
        /// <summary>
        /// Game mode type. true if this is training and false otherwise.
        /// </summary>
        public bool IsTraining = false;

        /// <summary>
        /// The number of balls currently in the pile.
        /// </summary>
        private int NumOfBallsInPile = 10;
        /// <summary>
        /// Number of seconds passes from the beggining of the game. 
        /// </summary>
        private int ticknum = 0;
        /// <summary>
        /// Cannon state. true if cannon is currently moving
        /// </summary>
        private bool AllCannonAlreadyMove = false;
        /// <summary>
        /// Barrel state. true if barrel is currently moving
        /// </summary>
        private bool BarrelMove = false;
        /// <summary>
        /// Checking. true if now all actions from player should be locked.
        /// </summary>
        private bool LockEvrything = false;
        /// <summary>
        /// Window state. true if window already opened (and lowered down)
        /// </summary>
        private bool alreadyOpen = false;
        /// <summary>
        /// Sound
        /// </summary>
        private MediaPlayer Fire = new MediaPlayer();
        /// <summary>
        /// Timer
        /// </summary>
        private DispatcherTimer timer = new DispatcherTimer();
        /// <summary>
        /// The balls in the pile.
        /// </summary>
        private CannonBall[] CannonBallSInPile = new CannonBall[10];
        /// <summary>
        /// The targets in the game
        /// </summary>
        private Target[] targets;
        /// <summary>
        /// The cannon
        /// </summary>
        private CannonClass Cannon;
        /// <summary>
        /// Number of shots fired.
        /// </summary>
        private int NumofShots = 0;

        /// <summary>
        /// Constructor for Game mode
        /// </summary>
        public GameMode()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Starts the game in cannon pile and target's animation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void startAnimation(object sender, EventArgs e)
        {
            if (!alreadyOpen)
            {
                alreadyOpen = true;

                targets = new Target[numOftargets];
                //Starts the first target (always exsits) 60 distance and goes from -100 to 100 in X axis.
                targets[0] = new Target(AllTargetModel, 0, AllTargetModel.Bounds.SizeY / 2, -100, 60, -60, VfirstTarget);

                //The cannon. if there's no physics the shots goes to 110 after the last target.
                Cannon = new CannonClass(AllCannonModel, AllBarrelModel, 110 + (50 * (numOftargets - 1)), speed, this);

                //Creating the targets
                for (int i = 1; i < targets.Length; i++)
                {
                    //Every target's distance from previos target is 50.
                    double z = targets[i - 1].PosZ - 50;
                    double from, to, v;
                    //Every second target starts in different location and direction.
                    //All target's route increase by 20 to each size (40 in X axis total)
                    if (i % 2 == 1)
                    {
                        from = (targets[i - 1].FromX + 20) * -1;
                        to = (targets[i - 1].ToX * -1) + 20;
                    }
                    else
                    {
                        from = (targets[i - 1].FromX * -1) + 20;
                        to = (targets[i - 1].ToX + 20) * -1;
                    }
                    //Target's speed decreasing  by 10% each target.
                    v = targets[i - 1].V * 0.9;
                    targets[i] = new Target(AllTargetModel.Clone(), 0, targets[0].PosY, z, from, to, v);
                    //Adding target to the screen
                    scene.Children.Add(targets[i].TargetModel);
                }

                //Starts targets's animation
                for (int i = 0; i < targets.Length; i++)
                    targets[i].AnimForever();


                //Creating the pile of balls.
                NumOfBallsInPile = Math.Min(10, NumOfShotsAllowd);
                CreateBallsInPileArr(NumOfBallsInPile);

                //Starts timer.
                timer.Tick += Timer_Tick;
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Start();
            }
        }

        /// <summary>
        /// Key pressed function. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pressed(object sender, KeyEventArgs e)
        {

            if (!AllCannonAlreadyMove && !LockEvrything && !BarrelMove)
            {
                switch (e.Key)
                {
                    //Moving the entire cannon right.
                    case Key.Right:
                        {
                            //cannon can't be fully right.
                            Cannon.RotateAllCannon(90 + 0.1, 4);
                            AllCannonAlreadyMove = true;
                            break;
                        }
                    //Moving the entire cannon left.
                    case Key.Left:
                        {
                            //cannon can't be fully left.
                            Cannon.RotateAllCannon(270 - 0.1, 4);
                            AllCannonAlreadyMove = true;
                            break;
                        }
                    //Moving the barrel up.
                    case Key.Up:
                        {
                            if (level == 2)
                            {
                                Cannon.RotateBarrel(-45, 4);
                                BarrelMove = true;
                            }
                            break;
                        }
                    //Moving the barrel down.
                    case Key.Down:
                        {
                            if (level == 2)
                            {
                                //barrel can't be fully back down, creates a bug.
                                Cannon.RotateBarrel(-0.1, 4);
                                BarrelMove = true;
                            }
                            break;
                        }
                    //Fires.
                    case Key.Space:
                        {
                            if (!IsTraining)
                                NumofShots++;

                            //checks if the player loses
                            if (!IsTraining && NumofShots > NumOfShotsAllowd)
                            {
                                EndGame();
                                break;
                            }

                            //Starts fire sound.
                            Fire.Open(new Uri(@".\sounds\Tank Firing-SoundBible.com-998264747.mp3", UriKind.Relative));
                            Fire.Volume = 1;
                            Fire.Play();

                            //Checking the shot's information
                            InformationOfShot CurShotInfo = CheckIfHit();

                            //Adding current ball to the screen.
                            scene.Children.Add(CurShotInfo.ShotBall.ballModel);

                            //Starts shot animation
                            if (level == 1)
                            {
                                CurShotInfo.ShotBall.MoveX(CurShotInfo.BallpxEnd, CurShotInfo.AllshotDuration);
                                CurShotInfo.ShotBall.MoveZ(CurShotInfo.BallpzEnd, CurShotInfo.AllshotDuration);
                            }
                            else
                            {
                                CurShotInfo.ShotBall.MoveX(CurShotInfo.BallpxEnd, CurShotInfo.AllshotDuration);

                                //Checking if the ball will pass it's max height.
                                if (CurShotInfo.AllshotDuration > CurShotInfo.DurationTillMaxHeight)
                                    CurShotInfo.ShotBall.MoveY(CurShotInfo.MaxHeight, CurShotInfo.DurationTillMaxHeight, CurShotInfo.AllshotDuration - CurShotInfo.DurationTillMaxHeight, CurShotInfo.BallpyEnd);
                                else
                                    CurShotInfo.ShotBall.MoveY(CurShotInfo.MaxHeight, CurShotInfo.AllshotDuration, 0, CurShotInfo.BallpyEnd);

                                CurShotInfo.ShotBall.MoveZ(CurShotInfo.BallpzEnd, CurShotInfo.AllshotDuration);
                            }

                            //Updates pileof bals.
                            if (NumOfBallsInPile >= 1)
                                RemoveFromPile();
                            if ((IsTraining && NumOfBallsInPile == 0) || (NumOfShotsAllowd - NumofShots > 0 && NumOfBallsInPile == 0))
                                BallSInPile(Math.Min(10, NumOfShotsAllowd - NumofShots));
                            break;
                        }
                    //Cheat for checking.
                    case Key.Enter:
                        {
                            for (int i = 0; i < targets.Length; i++)
                                targets[i].StopAimation();

                            break;
                        }
                    //Exit game.
                    case Key.Escape:
                        {
                            Application.Current.Shutdown();
                            break;
                        }
                }
            }
        }

        /// <summary>
        /// This function checks the information of the shot
        /// </summary>
        /// <returns>All the shot information in InformationOfShot object</returns>
        public InformationOfShot CheckIfHit()
        {
            InformationOfShot ShotInfo = new InformationOfShot();

            //The ball's starting position
            double pxStart = 0;
            double pyStart = 0;
            double pzStart = 0;

            //If the balll will hit a target
            bool ishit = false;

            //The end X position of target in check
            double EndPosXTarget;
            //The time from now until thr ball reaches the X's postion of target in check.
            double time = 0;

            double a, b, x;
            //The entire cannon current angle in Rad.
            a = Cannon.GetCurAllCannonAngle() * Math.PI / 180;
            //The barrel current angle in Rad.
            b = Cannon.GetCurBarrelAngle() * Math.PI / 180;
            //Barrel's width
            x = Cannon.BarrelSizeZ;

            //For level 2:
            double Vx, V0y;
            //The speed in X axis
            Vx = Cannon.V0 * Math.Cos(b);
            //The starting speed in Y axis.
            V0y = Cannon.V0 * Math.Sin(b);

            //The ball's ending Z position.
            double AllShotDisZ;

            if (level == 1)
            {
                //0.9 is the height of the inside of the barrel.
                pyStart = 0.9 + Cannon.PosY;
                ShotInfo.BallpyEnd = pyStart;
                //Further than last target.
                AllShotDisZ = targets[targets.Length - 1].PosZ - 10;

                //Time = Distance / Speed. (Z axis)
                ShotInfo.AllshotDuration = Cannon.DisOfShotNophys / (Cannon.V0 * Math.Cos((Cannon.GetCurAllCannonAngle() * Math.PI) / 180));
            }
            else
            {
                //Starting the ball in the end of the barrel. 3D trigonometry.
                //The barrel's center Y axis of rotation is 1/4 from the back of the barrel.              
                pxStart = ((3 * x / 4) * Math.Cos(b) - (x / 4)) * Math.Sin(a);
                pyStart = Cannon.PosY + 0.9 + (3 * x / 4) * Math.Sin(b);
                pzStart = -((3 * x / 4) * Math.Cos(b) * Math.Cos(a) - ((x / 4) * Math.Cos(a)));

                double A, B, C;

                //X = X0 + V0 * t + 0.5 * a * t^2.
                //g = 9.8
                A = 9.8 / 2;
                B = -V0y;
                C = -pyStart;

                //calulating the duration by taking the positive root of the equation.
                ShotInfo.AllshotDuration = (-B + Math.Sqrt(B * B - 4 * A * C)) / (2 * A);

                //V = V0 + a * t. To calculate max height is when V = 0.
                ShotInfo.DurationTillMaxHeight = V0y / 9.8;
                //X = X0 + V0 * t + 0.5 * a * t^2.
                ShotInfo.MaxHeight = pyStart + (V0y * ShotInfo.DurationTillMaxHeight) + (0.5 * (-9.8) * Math.Pow(ShotInfo.DurationTillMaxHeight, 2));
                //2D trigonometry.
                AllShotDisZ = pzStart - (Vx * ShotInfo.AllshotDuration) * Math.Cos(a);

            }
            int i;
            for (i = 0; i < targets.Length && !ishit && Math.Abs(targets[i].PosZ) < Math.Abs(AllShotDisZ); i++)
            {
                //checking if target has not already fallen.
                if (!targets[i].HasFallen)
                {
                    if (level == 1)
                    {
                        ShotInfo.BallpxEnd = Math.Abs(targets[i].PosZ) * Math.Tan(a);
                        ShotInfo.BallpyEnd = pyStart;

                        //Time = Distance / Speed (Z axis).
                        time = Math.Abs(targets[i].PosZ) / (Cannon.V0 * Math.Cos(a));
                    }
                    else
                    {
                        //2D trigonometry.
                        ShotInfo.BallpxEnd = Math.Tan(a) * Math.Abs(targets[i].PosZ - pzStart);

                        //Time = |Distance| / Speed. |Distance| = air 2D distance between start pos and end.
                        time = Math.Sqrt(Math.Pow(Math.Abs(targets[i].PosZ - pzStart), 2) + Math.Pow(ShotInfo.BallpxEnd, 2)) / Vx;

                        //X = X0 + V0 * t + 0.5 * a * t^2.
                        ShotInfo.BallpyEnd = pyStart + V0y * time + 0.5 * (-9.8) * Math.Pow(time, 2);
                    }

                    ShotInfo.BallpzEnd = targets[i].PosZ;

                    //Claculating target's ending X position
                    EndPosXTarget = targets[i].GetCurXPos();
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

                    double R, D;
                    //Target's radius
                    R = targets[i].TargetModel.Bounds.SizeX / 2;
                    //Distance between the ball and the target as he reach the target's Z position.
                    D = Math.Sqrt(Math.Pow(ShotInfo.BallpxEnd - EndPosXTarget, 2) + Math.Pow(ShotInfo.BallpyEnd - targets[i].PosY, 2));
                    ishit = D <= R;
                }
            }
            if (ishit)
            {
                //Hitting ball.
                ShotInfo.AllshotDuration = time;
                targets[i - 1].HittedAndNeedToFall = true;
                LockEvrything = true;
            }
            else
            {
                ShotInfo.BallpzEnd = AllShotDisZ;
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

        /// <summary>
        /// The timer Tick. happens every second.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, EventArgs e)
        {

            ticknum++;
            //Update time text box.
            Time.Text = (ticknum / 600).ToString() + (ticknum / 60).ToString() + ":" + (ticknum % 60 / 10).ToString() + (ticknum % 60 % 10).ToString();

            //Check if the player loses.
            if (!IsTraining && ticknum == NumOfSecsAllowd)
                EndGame();
        }

        /// <summary>
        /// Key released
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Released(object sender, KeyEventArgs e)
        {
            //Stops cannon animation
            if (e.Key == Key.Right || e.Key == Key.Left)
            {
                AllCannonAlreadyMove = false;
                Cannon.StopAllCannon();
            }
            //Stops barrel animation
            if (e.Key == Key.Up || e.Key == Key.Down)
            {
                BarrelMove = false;
                Cannon.StopBarrel();
            }
        }

        /// <summary>
        /// Function for checking information. not-needed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Click(object sender, MouseButtonEventArgs e)
        {
            label.Content = "Barrel Angle: " + Cannon.GetCurBarrelAngle().ToString();
        }

        /// <summary>
        /// Hitting ball reaches the target
        /// </summary>
        public void HitTarget()
        {
            //Starts explotion sound.
            Fire.Open(new Uri(@".\sounds\Big Explosion Effect Video Mp4 HD Sound.mp3", UriKind.Relative));
            Fire.Volume = 1;
            Fire.Play();

            //Finding the target
            int targetNum;
            bool found = false;
            for (targetNum = 0; targetNum < targets.Length && !found; targetNum++)
                found = targets[targetNum].HittedAndNeedToFall;
            targetNum--;

            //Stops the target's animaton
            targets[targetNum].StopAimation();

            targets[targetNum].HasFallen = true;
            targets[targetNum].HittedAndNeedToFall = false;

            //Starts falling animation
            targets[targetNum].Rotate(-90, 1);

            //Cehck if the player wins.
            bool won = targets[0].HasFallen;
            for (int i = 1; i < targets.Length && won; i++)
                won = targets[i].HasFallen;

            if (won)
                EndGame();
            else
                LockEvrything = false;
        }

        /// <summary>
        /// Adding balls to pile in the screen
        /// </summary>
        /// <param name="sum">The number of balls. cannot be more than 10.</param>
        public void BallSInPile(int sum)
        {
            for (int i = 0; i < sum; i++)
                scene.Children.Add(CannonBallSInPile[i].ballModel);

            NumOfBallsInPile = sum;
        }
        /// <summary>
        /// Remove one ball from the pile.
        /// </summary>
        public void RemoveFromPile()
        {
            NumOfBallsInPile--;
            scene.Children.Remove(CannonBallSInPile[NumOfBallsInPile].ballModel);
        }
        /// <summary>
        /// Creating Balls in pile.
        /// </summary>
        /// <param name="sum">The number of balls. cannot be more than 10.</param>
        public void CreateBallsInPileArr(int sum)
        {
            double size = AllBallModel.Bounds.SizeX;
            for (int i = 0; i < sum; i++)
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
                        CannonBallSInPile[i] = new CannonBall(AllBallModel.Clone(), 5 + (size * 1.5), size + size / 2, -size / 2, false);
                        break;
                    case 6:
                        CannonBallSInPile[i] = new CannonBall(AllBallModel.Clone(), 5 + (size / 2), size + size / 2, -size / 2, false);
                        break;
                    case 5:
                        CannonBallSInPile[i] = new CannonBall(AllBallModel.Clone(), 5 + size, size / 2, -size * 2, false);
                        break;
                    case 4:
                        CannonBallSInPile[i] = new CannonBall(AllBallModel.Clone(), 5 + (size * 1.5), size / 2, -size, false);
                        break;
                    case 3:
                        CannonBallSInPile[i] = new CannonBall(AllBallModel.Clone(), 5 + (size / 2), size / 2, -size, false);
                        break;
                    case 2:
                        CannonBallSInPile[i] = new CannonBall(AllBallModel.Clone(), 5 + (size * 2), size / 2, 0, false);
                        break;
                    case 1:
                        CannonBallSInPile[i] = new CannonBall(AllBallModel.Clone(), 5 + size, size / 2, 0, false);
                        break;
                    case 0:
                        CannonBallSInPile[i] = new CannonBall(AllBallModel.Clone(), 5, AllBallModel.Bounds.SizeY / 2, 0, false);
                        break;
                }
                scene.Children.Add(CannonBallSInPile[i].ballModel);
            }
        }

        /// <summary>
        /// Ending the game
        /// </summary>
        private void EndGame()
        {
            quit.Visibility = Visibility.Visible;
            continueGame.Visibility = Visibility.Visible;

            timer.Stop();

            LockEvrything = true;

            if ((NumofShots <= NumOfShotsAllowd && ticknum < NumOfSecsAllowd) || (IsTraining))
                Massege.Text = "You Win!";
            else
            {
                Massege.Text = "You lost!";

                int count = 0;
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

        /// <summary>
        /// This function increases the text when mouse is entering.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameModeMouseEnter(object sender, MouseEventArgs e)//עכבר נכנס ל continue או quit
        {
            Growing.Grow_Up((TextBlock)sender, 15, 0.5);
        }
        /// <summary>
        /// This function decreasing the text when mouse is leaving.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameModeMouseLeave(object sender, MouseEventArgs e)//עכבר יוצא ל continue או quit
        {
            Growing.Grow_Down((TextBlock)sender);
        }

        /// <summary>
        /// Continue the game or shutting down according to the player's choice.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameModeMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source == continueGame)
            {
                PickLevel pick = new PickLevel();
                pick.Show();
                this.Close();
            }
            else
                Application.Current.Shutdown();
        }
    }
}

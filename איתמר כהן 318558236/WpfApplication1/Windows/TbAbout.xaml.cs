using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Media.Animation;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for About.xaml.
    /// This window were not written by me. 
    /// </summary>
    public partial class TbAbout : Window
    {
        public static Window ThisWindow;
        public TbAbout()
        {
            InitializeComponent();
        }

        private void TextBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            Growing.Grow_Up((TextBlock)sender, 5, 0.5);
        }
        private void TextBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            Growing.Grow_Down((TextBlock)sender);
        }

        public static void ApplyResolutionToElements(Panel ThisPanel)
        {
            double WidthRatio = SystemParameters.PrimaryScreenWidth / 1920, HeightRatio = SystemParameters.PrimaryScreenHeight / 1080;

            if (WidthRatio != 1 || HeightRatio != 1)
            {
                foreach (UIElement Element in ThisPanel.Children)
                {
                    if (Element is Panel)
                    {

                        Canvas.SetLeft(Element, Canvas.GetLeft(Element) * WidthRatio);
                        Canvas.SetTop(Element, Canvas.GetTop(Element) * HeightRatio);
                        ApplyResolutionToElements(Element as Panel);
                    }
                    else if (!(Element is Viewport3D))
                    {
                        Element.RenderTransform = new ScaleTransform(WidthRatio, HeightRatio);
                        Canvas.SetLeft(Element, Canvas.GetLeft(Element) * WidthRatio);
                        Canvas.SetTop(Element, Canvas.GetTop(Element) * HeightRatio);
                    }
                }
            }

        }

        private void OnBackMouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow mnw = new MainWindow();
            mnw.Show();
            //ThisWindow.Content = MainWindow.ThisContent;
            this.Close();
        }

        private void OnAboutLoaded(object sender, RoutedEventArgs e)
        {
            LoadScene(this);

        }


        private static void LoadScene(TbAbout About)
        {
            Model3DGroup TabletBook = new Model3DGroup();
            DoubleAnimation BringBook = new DoubleAnimation(), OpenAnimation = new DoubleAnimation(), Hideios = new DoubleAnimation(), ShowCodeTora = new DoubleAnimation();
            Brush iosImage1, CodeImage, iosImage2, ToraImage;
            DiscreteStringKeyFrame LetterAnimation;
            StringAnimationUsingKeyFrames StringAnimation;
            string CurrentString, FullString;

            BringBook.By = -4.1;
            BringBook.Duration = TimeSpan.FromSeconds(5);

            OpenAnimation.By = -185;
            OpenAnimation.Duration = TimeSpan.FromSeconds(5);
            OpenAnimation.BeginTime = TimeSpan.FromSeconds(5);

            Hideios.By = -1;
            Hideios.Duration = TimeSpan.FromSeconds(2);
            Hideios.BeginTime = TimeSpan.FromSeconds(10);

            ShowCodeTora.By = 1;
            ShowCodeTora.Duration = TimeSpan.FromSeconds(2);
            ShowCodeTora.BeginTime = TimeSpan.FromSeconds(10);


            TabletBook.Children.Add(((Model3DGroup)About.FindResource("Tablet")).Clone());


            TabletBook.Children.Add(((Model3DGroup)About.FindResource("Tablet")).Clone());
            ((Transform3DGroup)TabletBook.Children[1].Transform).Children.Add(new TranslateTransform3D(0, 0.06, 0));


            TabletBook.Children.Add(((Model3DGroup)About.FindResource("Tablet")).Clone());
            ((Transform3DGroup)TabletBook.Children[2].Transform).Children.Add(new TranslateTransform3D(0, 0.12, 0));
            ((GeometryModel3D)((Model3DGroup)TabletBook.Children[2]).Children[((Model3DGroup)TabletBook.Children[2]).Children.Count - 1]).Material = ((MaterialGroup)About.FindResource("CodeScreen")).Clone();
            iosImage1 = ((DiffuseMaterial)((MaterialGroup)((GeometryModel3D)((Model3DGroup)TabletBook.Children[2]).Children[((Model3DGroup)TabletBook.Children[2]).Children.Count - 1]).Material).Children[1]).Brush;
            CodeImage = ((DiffuseMaterial)((MaterialGroup)((GeometryModel3D)((Model3DGroup)TabletBook.Children[2]).Children[((Model3DGroup)TabletBook.Children[2]).Children.Count - 1]).Material).Children[0]).Brush;


            TabletBook.Children.Add(((Model3DGroup)About.FindResource("Tablet")).Clone());
            ((Transform3DGroup)TabletBook.Children[3].Transform).Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), 180)));
            ((Transform3DGroup)TabletBook.Children[3].Transform).Children.Add(new TranslateTransform3D(0, 0.24, 0));
            ((Transform3DGroup)TabletBook.Children[3].Transform).Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), 0), new Point3D(0, 0.2, -0.9)));
            ((GeometryModel3D)((Model3DGroup)TabletBook.Children[3]).Children[((Model3DGroup)TabletBook.Children[3]).Children.Count - 1]).Material = ((MaterialGroup)About.FindResource("ToraScreen")).Clone();
            iosImage2 = ((DiffuseMaterial)((MaterialGroup)((GeometryModel3D)((Model3DGroup)TabletBook.Children[3]).Children[((Model3DGroup)TabletBook.Children[2]).Children.Count - 1]).Material).Children[1]).Brush;
            ToraImage = ((DiffuseMaterial)((MaterialGroup)((GeometryModel3D)((Model3DGroup)TabletBook.Children[3]).Children[((Model3DGroup)TabletBook.Children[2]).Children.Count - 1]).Material).Children[0]).Brush;


            TabletBook.Children.Add(((Model3DGroup)About.FindResource("Tablet")).Clone());
            ((Transform3DGroup)TabletBook.Children[4].Transform).Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), 180)));
            ((Transform3DGroup)TabletBook.Children[4].Transform).Children.Add(new TranslateTransform3D(0, 0.3, 0));
            ((Transform3DGroup)TabletBook.Children[4].Transform).Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), 0), new Point3D(0, 0.2, -0.94)));


            TabletBook.Children.Add(((Model3DGroup)About.FindResource("Tablet")).Clone());
            ((Transform3DGroup)TabletBook.Children[5].Transform).Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), 180)));
            ((Transform3DGroup)TabletBook.Children[5].Transform).Children.Add(new TranslateTransform3D(0, 0.36, 0));
            ((Transform3DGroup)TabletBook.Children[5].Transform).Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), 0), new Point3D(0, 0.2, -0.98)));
            ((GeometryModel3D)((Model3DGroup)TabletBook.Children[5]).Children[0]).Material = About.FindResource("Logo") as MaterialGroup;


            About.BookScene.Children.Add(TabletBook);


            TabletBook.Transform = new TranslateTransform3D(4.1, 0, 0);

            ((TranslateTransform3D)TabletBook.Transform).BeginAnimation(TranslateTransform3D.OffsetXProperty, BringBook);

            ((AxisAngleRotation3D)((RotateTransform3D)((Transform3DGroup)TabletBook.Children[3].Transform).Children[((Transform3DGroup)TabletBook.Children[3].Transform).Children.Count - 1]).Rotation).BeginAnimation(AxisAngleRotation3D.AngleProperty, OpenAnimation);
            ((AxisAngleRotation3D)((RotateTransform3D)((Transform3DGroup)TabletBook.Children[4].Transform).Children[((Transform3DGroup)TabletBook.Children[4].Transform).Children.Count - 1]).Rotation).BeginAnimation(AxisAngleRotation3D.AngleProperty, OpenAnimation);
            ((AxisAngleRotation3D)((RotateTransform3D)((Transform3DGroup)TabletBook.Children[5].Transform).Children[((Transform3DGroup)TabletBook.Children[5].Transform).Children.Count - 1]).Rotation).BeginAnimation(AxisAngleRotation3D.AngleProperty, OpenAnimation);

            iosImage1.BeginAnimation(Brush.OpacityProperty, Hideios);
            iosImage2.BeginAnimation(Brush.OpacityProperty, Hideios);
            CodeImage.BeginAnimation(Brush.OpacityProperty, ShowCodeTora);
            ToraImage.BeginAnimation(Brush.OpacityProperty, ShowCodeTora);

            foreach (TextBlock TextBlock in About.Text.Children)
            {
                if (TextBlock != About.Back)
                {
                    StringAnimation = new StringAnimationUsingKeyFrames();
                    StringAnimation.Duration = TimeSpan.FromSeconds(TextBlock.Text.Length * 0.08);
                    StringAnimation.BeginTime = TimeSpan.FromSeconds(12);

                    FullString = TextBlock.Text;
                    CurrentString = "";
                    TextBlock.Text = "";



                    foreach (char Letter in FullString)
                    {
                        LetterAnimation = new DiscreteStringKeyFrame();
                        LetterAnimation.KeyTime = KeyTime.Paced;

                        CurrentString += Letter;

                        LetterAnimation.Value = CurrentString;
                        StringAnimation.KeyFrames.Add(LetterAnimation);
                    }
                    TextBlock.RenderTransformOrigin = new Point(1, 1);
                    TextBlock.BeginAnimation(TextBlock.TextProperty, StringAnimation);
                }
            }

        }
    }
}

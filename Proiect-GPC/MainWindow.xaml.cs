using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Point = System.Drawing.Point;
using Color = System.Drawing.Color;
using System.IO;

namespace Proiect_GPC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Point> points;
        bool isRotating = false;
        CancellationTokenSource infoUpdateToken;

        public MainWindow()
        {
            InitializeComponent();
            points = new List<Point>();

            Point point1 = new Point(0, 0);
            Point point2 = new Point(-15, -1);

            MidPoint.DrawLine(point1, point2);
        }

        private void AddNewPoint(object sender, RoutedEventArgs e)
        {
            try
            {
                int x = int.Parse(XInput.Text);
                int y = int.Parse(YInput.Text);
                points.Add(new Point(x, y));
                UpdateInfo("Point added!", 2000);
            }
            catch
            {
                UpdateInfo("Invalid point input!", 2000);
            }
            finally
            {
                XInput.Text = "";
                YInput.Text = "";
            }
        }

        private void ClearPoints(object sender, RoutedEventArgs e)
        {
            points.Clear();
        }

        private void ChangeRotationStatus(object sender, RoutedEventArgs e)
        {
            isRotating = !isRotating;
            RotationButton.Content = isRotating ? "Stop rotating" : "Start rotating";
        }

        private void TestDrawLine(Bitmap bmp)
        {
            for (int i = 100; i < 300; ++i)
            {
                bmp.SetPixel(i, 100, Color.White);
            }
        }

        private void UpdateInfo(string message, int timeout)
        {
            if (infoUpdateToken != null)
            {
                infoUpdateToken.Cancel();
            }

            InfoLabel.Content = message;
            infoUpdateToken = new CancellationTokenSource();

            new Task(() =>
            {
                CancellationTokenSource tokenSource = infoUpdateToken;
                tokenSource.Token.WaitHandle.WaitOne(timeout);

                if (tokenSource.Token.IsCancellationRequested)
                {
                    return;
                }
                Dispatcher.Invoke(() =>
                {
                    InfoLabel.Content = "";
                });

            }, infoUpdateToken.Token).Start();
        }

        private ImageSource FromBitmap(Bitmap bitmap)
        {

            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }


        private void WindowLoaded(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap((int)Math.Floor(MainDisplay.Width), (int)Math.Floor(MainDisplay.Height));
            TestDrawLine(bmp);
            MainDisplay.Source = FromBitmap(bmp);
        }
    }
}

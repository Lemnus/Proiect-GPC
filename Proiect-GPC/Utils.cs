using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Image = System.Windows.Controls.Image;

namespace Proiect_GPC
{
    class Utils
    {
        public static ImageSource FromBitmap(Bitmap bitmap)
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

        public static Bitmap GetEmptyBitmap(Image MainDisplay)
        {
            return new Bitmap((int)Math.Floor(MainDisplay.Width), (int)Math.Floor(MainDisplay.Height));
        }

        public static Point CenterPoint(List<Point> points)
        {
            HashSet<Point> hashPoints = new HashSet<Point>(points);
            int pivotX = hashPoints.Sum((point) => point.X) / hashPoints.Count;
            int pivotY = hashPoints.Sum((point) => point.Y) / hashPoints.Count;
            return new Point(pivotX, pivotY);
        }

        public static void TestData()
        {
            Bitmap bmp = new Bitmap(0, 0);
            MidPoint.DrawLine(bmp, new Point(0, 0), new Point(0, 0));
        }
    }
}

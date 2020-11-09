using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Point = System.Drawing.Point;

namespace Proiect_GPC
{
    static class MidPoint
    {
        private static void DrawLine(Point a, Point b, Bitmap bitmap)
        {
            decimal dX = b.X - a.X;
            decimal dY = b.Y - a.Y;

            (decimal, decimal) scale(decimal percentage)
            {
                return (a.X + percentage * dX, a.Y + percentage * dY);
            }

            int maxPixels = (int)Math.Max(Math.Abs(dX), Math.Abs(dY));
            decimal stepLength = 1M / maxPixels;

            for (int i = 0; i < maxPixels; ++i)
            {
                var (x, y) = scale(i * stepLength);
                int pixelX = (int)Math.Round(x);
                int pixelY = (int)Math.Round(y);
                try
                {
                    bitmap.SetPixel(pixelX, pixelY, Color.White);
                }
                catch (ArgumentOutOfRangeException) { }
            }
        }

        public static void DrawShape(List<Point> points, Bitmap bmp)
        {
            if (points.Count < 2)
            {
                return;
            }

            for (int i = 1; i < points.Count; ++i)
            {
                DrawLine(points[i - 1], points[i], bmp);
            }
        }

    }
}

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
        public static void DrawLine(Bitmap bmp, Point point1, Point point2)
        {
            // daca suntem in unul din cazurile 3/4/5/6 inversam punctele
            if (point1.X > point2.X)
                Swap(ref point1, ref point2);

            int X1 = point1.X;
            int Y1 = point1.Y;
            int X2 = point2.X;
            int Y2 = point2.Y;

            // setam dimensiunea cu cea mai mare diferenta intre puncte ca fiind axa X
            int dx = X2 - X1;
            int dy = Y2 - Y1;
            int moddx = Math.Abs(dx);
            int moddy = Math.Abs(dy);
            bool invertedAxis = moddx < moddy;
            bool negativeY = dy < 0;

            List<Point> deltaList;

            // calculam o lista intermediara care contine diferentele pe x si y fata de punctul initial care trebuie aplicate fiecarui punct din linie
            if (invertedAxis)
            {
                if (negativeY)
                    // caz 7
                    deltaList = GetDeltaList(-dy, dx);
                else
                    // caz 2
                    deltaList = GetDeltaList(dy, dx);
            }
            else
            {
                if (negativeY)
                    //caz 8
                    deltaList = GetDeltaList(dx, -dy);
                else
                    //caz 1
                    deltaList = GetDeltaList(dx, dy);
            }

            List<Point> ret = new List<Point>();

            // adaugam punctele mapate in lista de pixeli ce trebuie setati
            foreach (Point p in deltaList)
            {
                if (invertedAxis)
                {
                    if (negativeY)
                        // caz 7
                        ret.Add(new Point(X1 + p.Y, Y1 - p.X));
                    else
                        //caz 2
                        ret.Add(new Point(Y1 + p.Y, X1 + p.X));
                }
                else
                {
                    if (negativeY)
                        //caz 8
                        ret.Add(new Point(X1 + p.X, Y1 - p.Y));
                    else
                        // caz 1
                        ret.Add(new Point(X1 + p.X, Y1 + p.Y));
                }
            }

            // setam pixelii pe bitmap
            foreach (Point p in ret)
            {
                try
                {
                    bmp.SetPixel(p.X, p.Y, Color.White);
                }
                catch (ArgumentOutOfRangeException) { }
            }
        }

        // returneaza o lista de diferente pentru fiecare punct (fata de originea X1, Y1 - implicit punctul 0, 0)
        static List<Point> GetDeltaList(int X2, int Y2, int X1 = 0, int Y1 = 0)
        {
            List<Point> list = new List<Point>();
            int dx = X2 - X1;
            int dy = Y2 - Y1;

            // valoarea initiala a parametrului de decizie d
            int d = dy - (dx / 2);
            int x = X1, y = Y1;

            // iteram prin valorile lui x
            while (x <= X2)
            {
                list.Add(new Point(x, y));

                x++;

                // cadranul de est
                if (d < 0)
                    d = d + dy;

                // cadranul de nord est
                else
                {
                    d += (dy - dx);
                    y++;
                }
            }

            return list;
        }

        static void Swap(ref Point p1, ref Point p2)
        {
            Point temp = p1;
            p1 = p2;
            p2 = temp;
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
    }
}

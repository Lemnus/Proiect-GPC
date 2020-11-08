using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Point = System.Drawing.Point;

namespace Proiect_GPC
{
    static class MidPoint
    {
        public static void DrawLine(Point point1, Point point2)
        {
            // check if case 3/4/5/6 and inverted points when necesarry
            if (point1.X > point2.X)
                Swap(ref point1, ref point2);

            int X1 = point1.X;
            int Y1 = point1.Y;
            int X2 = point2.X;
            int Y2 = point2.Y;

            // set dimension with biggest difference as X-axis
            int dx = X2 - X1;
            int dy = Y2 - Y1;
            int moddx = Math.Abs(dx);
            int moddy = Math.Abs(dy);
            bool invertedAxis = moddx < moddy;
            bool negativeY = dy < 0;

            Console.WriteLine(invertedAxis + " INVERTEDAXISSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSS");

            List<Point> deltaList;

            if (invertedAxis)
            {
                if(negativeY)
                    // caz 7
                    deltaList = getDeltaList(-dy, dx);
                else
                    // caz 2
                    deltaList = getDeltaList(dy, dx);
            }
            else
            {
                if(negativeY)
                    //caz 8
                    deltaList = getDeltaList(dx, -dy);
                else
                    //caz 1
                    deltaList = getDeltaList(dx, dy);
            }

            List<Point> ret = new List<Point>();

            // add mapped values in ret
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

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Mapped: ");
            // display points
            foreach (Point p in ret)
            {
                Console.WriteLine(p);
            }
        }

        // return a list of deltas for each point (considering starting point as origin)
        static List<Point> getDeltaList(int X2, int Y2, int X1 = 0, int Y1 = 0)
        {
            List<Point> list = new List<Point>();
            // calculate dx & dy 
            int dx = X2 - X1;
            int dy = Y2 - Y1;

            // initial value of decision 
            // parameter d 
            int d = dy - (dx / 2);
            int x = X1, y = Y1;

            // iterate through value of X 
            while (x <= X2)
            {
                list.Add(new Point(x, y));

                x++;

                // E or East is chosen 
                if (d < 0)
                    d = d + dy;

                // NE or North East is chosen 
                else
                {
                    d += (dy - dx);
                    y++;
                }
            }

            // display delta
            Console.WriteLine("Delta list: ");
            foreach (Point p in list)
            {
                Console.WriteLine(p);
            }

            return list;
        }

        static void Swap(ref Point p1, ref Point p2)
        {
            Point temp = p1;
            p1 = p2;
            p2 = temp;
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Proiect_GPC
{
    static class Rotation
    {
        public static List<Point> RotatePoints(List<Point> points, decimal angle, Point pivot)
        {
            if (points.Count == 0)
            {
                throw new Exception("Nu exista puncte ce sa fie rotite!");
            }
            if (points.Count == 1 && points[0] == pivot)
            {
                throw new Exception("Nu se poate rotii un singur punct in jurul lui insusi!");
            }
            return points.Select((point) => point.RotateAround(pivot, angle)).ToList();
        }

        public static Point RotateAround(this Point point, Point pivot, decimal angle)
        {
            double radians = Convert.ToDouble(angle * (Convert.ToDecimal(Math.PI) / 180M));
            double s = Math.Sin(radians);
            double c = Math.Cos(radians);

            double x = point.X - pivot.X;
            double y = point.Y - pivot.Y;

            int pX = (int) Math.Round(x * c - y * s) + pivot.X;
            int pY = (int) Math.Round(x * s + y * c) + pivot.Y;

            return new Point(pX, pY);
        }
    }
}

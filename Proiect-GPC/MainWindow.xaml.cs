﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
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

namespace Proiect_GPC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Point> points;
        bool isRotating = false;
        public MainWindow()
        {
            InitializeComponent();
            points = new List<Point>();
        }

        private void AddNewPoint(object sender, RoutedEventArgs e)
        {
            int x = int.Parse(XInput.Text);
            XInput.Text = "";
            int y = int.Parse(YInput.Text);
            YInput.Text = "";
            points.Add(new Point(x, y));
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
    }
}

﻿using System;
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
        private static readonly Point DEFAULT_PIVOT = new Point(-1, -1);
        private bool customPivotSet = false;

        private List<Point> unrotatedPoints;
        private List<Point> displayPoints;
        private Point pivot = DEFAULT_PIVOT;
        private CancellationTokenSource infoUpdateToken;

        public MainWindow()
        {
            InitializeComponent();
            unrotatedPoints = new List<Point>();
            displayPoints = new List<Point>();
            MainDisplay.Source = Utils.FromBitmap(Utils.GetEmptyBitmap(MainDisplay));
        }

        private void AddNewPoint(object sender, RoutedEventArgs e)
        {
            bool added = false;

            void AddPoint(int x, int y)
            {
                if (x > (int)Math.Floor(MainDisplay.Width) || y > (int)Math.Floor(MainDisplay.Height) || x < 0 || y < 0)
                {
                    throw new Exception("Invalid point");
                }
                unrotatedPoints.Add(new Point(x, y));
                displayPoints = new List<Point>(unrotatedPoints);
                added = true;
            }

            try
            {
                int x = int.Parse(XInput.Text);
                int y = int.Parse(YInput.Text);
                AddPoint(x, y);
                if (!customPivotSet)
                {
                    pivot = Utils.CenterPoint(unrotatedPoints);
                }
                UpdateBoard();
                UpdateInfo("Punct adaugat!", 2000);
            }
            catch
            {
                if (added)
                {
                    unrotatedPoints.Remove(unrotatedPoints.Last());
                    displayPoints.Remove(unrotatedPoints.Last());
                }
                UpdateInfo("Punct invalid!", 2000);
            }
            finally
            {
                XInput.Text = "";
                YInput.Text = "";
            }
        }

        private void ClearPoints(object sender, RoutedEventArgs e)
        {
            displayPoints.Clear();
            unrotatedPoints.Clear();
            pivot = DEFAULT_PIVOT;
            customPivotSet = false;
            UpdateBoard();
            UpdateInfo("Tabla a fost resetata!", 2000);
        }

        private void RotatePoints(object sender, RoutedEventArgs e)
        {
            try
            {
                decimal angle = decimal.Parse(AngleInput.Text);
                displayPoints = Rotation.RotatePoints(unrotatedPoints, angle, pivot);
                UpdateBoard();
                UpdateInfo("Forma a fost rotita!", 2000);
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException || ex is FormatException || ex is OverflowException)
                {
                    UpdateInfo("Unghi de rotatie invalid!", 2000);
                }
                else
                {
                    UpdateInfo(ex.Message, 2000);
                }
            }
            finally
            {
                AngleInput.Text = "";
            }
        }

        private void SetPivot(object sender, RoutedEventArgs e)
        {
            try
            {
                int x = int.Parse(PivotInputX.Text);
                int y = int.Parse(PivotInputY.Text);
                pivot = new Point(x, y);
                customPivotSet = true;
                UpdateBoard();
                UpdateInfo("Pivotul de rotatie a fost setat!", 2000);
            }
            catch
            {
                UpdateInfo("Punct invalid!", 2000);
            }
            finally
            {
                PivotInputX.Text = "";
                PivotInputY.Text = "";
            }
        }

        private void UpdateBoard()
        {
            Bitmap bmp = Utils.GetEmptyBitmap(MainDisplay);
            MidPoint.DrawShape(unrotatedPoints, bmp, Color.Gray);
            MidPoint.DrawShape(displayPoints, bmp, Color.White);
            if (pivot != DEFAULT_PIVOT)
            {
                bmp.SetPixel(pivot.X, pivot.Y, Color.Red);
            }
            foreach (Point p in displayPoints)
            {
                bmp.SetPixel(p.X, p.Y, Color.White);
            }
            MainDisplay.Source = Utils.FromBitmap(bmp);
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
    }
}

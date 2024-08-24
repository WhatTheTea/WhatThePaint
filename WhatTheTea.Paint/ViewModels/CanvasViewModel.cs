using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Shapes;

using System.Diagnostics;

using Windows.UI;

namespace WhatTheTea.Paint.ViewModels
{
    internal enum DrawingState
    {
        None,
        Drawing,
        Erasing
    }
    public partial class CanvasViewModel : ObservableObject
    {
        private static SolidColorBrush BlackBrush = new SolidColorBrush(Color.FromArgb(255,0,0,0));
        private DrawingState drawingState = DrawingState.None;
        private PointerPoint? deltaPoint;

        public CanvasViewModel()
        {
        }

        public void Canvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (sender is Canvas canvas)
            {
                drawingState = DrawingState.Drawing;
            }
        }

        public void Canvas_PointerMove(object sender, PointerRoutedEventArgs e)
        {
            if (sender is Canvas canvas && drawingState == DrawingState.Drawing)
            {
                var point = e.GetCurrentPoint(canvas);
                Debug.WriteLine($"{point.Timestamp} - {point.Position.X} {point.Position.Y}");

                var element = new Line()
                {
                    Stroke = BlackBrush,
                    Fill = BlackBrush,
                    StrokeThickness = 2,

                    X2 = point.Position.X,
                    Y2 = point.Position.Y
                };

                if (deltaPoint is not null)
                {
                    element.X1 = deltaPoint.Position.X;
                    element.Y1 = deltaPoint.Position.Y;
                }
                else
                {
                    element.X1 = point.Position.X;
                    element.Y1 = point.Position.Y;
                }

                canvas.Children.Add(element);
                deltaPoint = point;
            }
        }

        public void Canvas_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (sender is Canvas canvas)
            {
                drawingState = DrawingState.None;
                deltaPoint = null;
            }
        }
    }
}

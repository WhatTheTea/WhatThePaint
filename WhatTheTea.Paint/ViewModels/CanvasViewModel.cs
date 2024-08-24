using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using WhatTheTea.Paint.Extensions;

using Windows.UI;

namespace WhatTheTea.Paint.ViewModels;

public enum DrawingMode
{
    None,
    Drawing,
    Erasing
}
public partial class CanvasViewModel : ObservableObject
{
    private static readonly SolidColorBrush BlackBrush = new(Color.FromArgb(255, 0, 0, 0));
    private static readonly SolidColorBrush WhiteBrush = new(Color.FromArgb(255, 255, 255, 255));
    [ObservableProperty]
    private DrawingMode drawingMode;
    [ObservableProperty]
    private int strokeThickness = 1;
    private DrawingMode drawingState = DrawingMode.None;
    private PointerPoint? deltaPoint;

    public CanvasViewModel()
    {
    }

    public void Canvas_PointerPressed(object sender, PointerRoutedEventArgs e)
    {
        if (sender is Canvas canvas)
        {
            drawingState = DrawingMode;
        }
    }

    public void Canvas_PointerMove(object sender, PointerRoutedEventArgs e)
    {
        if (sender is Canvas canvas 
            && drawingState != DrawingMode.None)
        {
            var point = e.GetCurrentPoint(canvas);
            DrawLine(canvas, point);
        }
    }

    private void DrawLine(Canvas canvas, PointerPoint point)
    {
        var brush = GetBrushFromDrawingMode();

        var element = new Path() 
        {
            Stroke = brush,
            Fill = brush,
            StrokeThickness = StrokeThickness,
            Data = GetCirclesData(point)
        };

        canvas.Children.Add(element);
        deltaPoint = point;
    }

    private GeometryGroup GetCirclesData(PointerPoint point)
    {
        var group = new GeometryGroup();
        var startPoint = GetLineStartPoint(point);
        var endPoint = (point.Position.X, point.Position.Y);

        var circle = new EllipseGeometry()
        {
            Center = new(endPoint.X, endPoint.Y),
            RadiusX = 0, 
            RadiusY = 0,
        };
        var line = new LineGeometry()
        {
            StartPoint = new Windows.Foundation.Point(startPoint.x, startPoint.y),
            EndPoint = new Windows.Foundation.Point(endPoint.X, endPoint.Y),
        };
        group.Children.Add(circle);
        group.Children.Add(line);
        return group;

    }

    private SolidColorBrush GetBrushFromDrawingMode() => drawingState switch { 
        DrawingMode.None => throw new System.NotImplementedException(), 
        DrawingMode.Drawing => BlackBrush, 
        DrawingMode.Erasing => WhiteBrush, 
        _ => throw new System.NotImplementedException(), 
    };

    private (int x, int y) GetLineStartPoint(PointerPoint point)
    {
        if (deltaPoint is not null)
        {
            return (x: (int)deltaPoint.Position.X, y: (int)deltaPoint.Position.Y);
        }
        else
        {
            return (x: (int)point.Position.X, y: (int)point.Position.Y);
        }
    }

    public void Canvas_PointerReleased(object sender, PointerRoutedEventArgs e)
    {
        if (sender is Canvas canvas)
        {
            drawingState = DrawingMode.None;
            deltaPoint = null;
        }
    }
}

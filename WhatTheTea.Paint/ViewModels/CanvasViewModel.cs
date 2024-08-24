using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Input;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;

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
        var element = new Line()
        {
            Stroke = brush,
            Fill = brush,
            StrokeThickness = StrokeThickness,
            X2 = point.Position.X,
            Y2 = point.Position.Y
        };
        EnsureLineStartPoint(point, element);

        canvas.Children.Add(element);
        deltaPoint = point;
    }

    private SolidColorBrush GetBrushFromDrawingMode() => drawingState switch { 
        DrawingMode.None => throw new System.NotImplementedException(), 
        DrawingMode.Drawing => BlackBrush, 
        DrawingMode.Erasing => WhiteBrush, 
        _ => throw new System.NotImplementedException(), 
    };

    private void EnsureLineStartPoint(PointerPoint point, Line element)
    {
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

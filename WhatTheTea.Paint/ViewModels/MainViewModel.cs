using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Xaml.Input;

using System;
using System.Collections.ObjectModel;

namespace WhatTheTea.Paint.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private CanvasViewModel _canvasViewModel = new();

    public ObservableCollection<bool> CommandSelections { get; } = [false, false];

    public MainViewModel()
    {
        var drawCommand = (XamlUICommand)App.Current.Resources["DrawCommand"];
        drawCommand.ExecuteRequested += (_, _) => SetDrawingMode(DrawingMode.Drawing);
        var eraseCommand = (XamlUICommand)App.Current.Resources["EraseCommand"];
        eraseCommand.ExecuteRequested += (_, _) => SetDrawingMode(DrawingMode.Erasing);
    }

    private void SetDrawingMode(DrawingMode drawingMode) 
    {
        CanvasViewModel.DrawingMode = drawingMode;
        UncheckSelections();
        var index = DrawingModeToSelectionIndex(drawingMode);
        CommandSelections[index] = true;
    }

    private Index DrawingModeToSelectionIndex(DrawingMode drawingMode) => drawingMode switch
    {
        DrawingMode.None => throw new NotImplementedException(),
        DrawingMode.Drawing => 0,
        DrawingMode.Erasing => 1,
        _ => throw new NotImplementedException(),
    };

    private void UncheckSelections()
    {
        for (int i = 0; i < CommandSelections.Count; i++)
        {
            CommandSelections[i] = false;
        }
    }

}

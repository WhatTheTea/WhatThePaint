using Microsoft.UI.Xaml.Controls;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatTheTea.Paint.Extensions
{
    internal static class MathEx
    {
        public static double Lerp(double a, double b, double by) => (a * (1 - by)) + b * by;

        public static (double x, double y) Lerp((double x, double y) a, (double x, double y) b, double by)
        {
            double retX = Lerp(a.x, b.x, by);
            double retY = Lerp(a.y, b.y, by);
            return (retX, retY);
        }

    }
}

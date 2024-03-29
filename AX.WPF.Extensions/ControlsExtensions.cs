﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace AX.WPF
{
    public static class ControlsExtensions
    {
        public static Rect GetVisibleRect(this FrameworkElement element)
        {
            var visualParent = VisualTreeHelper.GetParent(element);
            var transform = element.TransformToAncestor(visualParent as Visual);
            var inversed = transform.Inverse;
            Rect actualRect = new Rect(0, 0, element.ActualWidth, element.ActualHeight);
            if (inversed != null)
            {
                var transformedBounds = transform.Inverse.TransformBounds(actualRect);
                actualRect = transformedBounds;
            }

            return actualRect;
        }

        public static Rect GetRelativeBounds(this Visual visual, Visual relativeVisual)
        {
            var bounds = VisualTreeHelper.GetDescendantBounds(visual);
            var transform = visual.TransformToVisual(relativeVisual);
            return transform.TransformBounds(bounds);
        }
    }
}

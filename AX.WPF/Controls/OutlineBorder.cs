using AX.WPF.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AX.WPF.Controls
{
    public class OutlineBorder : Decorator
    {
        public Brush BorderBrush
        {
            get { return (Brush)GetValue(BorderBrushProperty); }
            set { SetValue(BorderBrushProperty, value); }
        }
        public static readonly DependencyProperty BorderBrushProperty =
            DependencyProperty.Register(nameof(BorderBrush), typeof(Brush), typeof(OutlineBorder), new FrameworkPropertyMetadata(Brushes.Transparent)
            {
                AffectsRender = true
            });


        public double Thickness
        {
            get { return (double)GetValue(ThicknessProperty); }
            set { SetValue(ThicknessProperty, value); }
        }

        public static readonly DependencyProperty ThicknessProperty =
            DependencyProperty.Register(nameof(Thickness), typeof(double), typeof(OutlineBorder), new FrameworkPropertyMetadata(1d)
            {
                AffectsRender = true,
                AffectsMeasure = true,
                AffectsArrange = true
            });


        public bool BuildGlyphs
        {
            get { return (bool)GetValue(BuildGlyphsProperty); }
            set { SetValue(BuildGlyphsProperty, value); }
        }

        public static readonly DependencyProperty BuildGlyphsProperty =
            DependencyProperty.Register(nameof(BuildGlyphs), typeof(bool), typeof(OutlineBorder), new FrameworkPropertyMetadata(false)
            {
                AffectsRender = true
            });

        private DrawingGroup drawingGroup = new DrawingGroup();
        private PathGeometry childGeometry = new PathGeometry();

        public OutlineBorder()
        {
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            var childSize = Child.DesiredSize;
            var childRect = new Rect(Thickness, Thickness, childSize.Width, childSize.Height);
            Child.Arrange(childRect);
            return arrangeSize;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            var childConstraint = constraint;
            Child.Measure(childConstraint);
            var childSize = Child.DesiredSize;
            childSize.Width += Thickness * 2;
            childSize.Height += Thickness * 2;
            return childSize;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            var unioned = Child.GetUnionedGeometry(false);
            var childGeometry = unioned.GetFlattenedPathGeometry();
            var toRemove = childGeometry.Figures.Take(childGeometry.Figures.Count - 1).ToList();
            toRemove.ForEach(x => childGeometry.Figures.Remove(x));
            childGeometry.Transform = new TranslateTransform(Thickness, Thickness);
            drawingContext.DrawGeometry(null, BorderBrush.ToPen(Thickness), childGeometry);
        }


    }
}

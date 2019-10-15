using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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


        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Fill.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FillProperty =
            DependencyProperty.Register(nameof(Fill), typeof(Brush), typeof(OutlineBorder), new FrameworkPropertyMetadata(Brushes.Transparent)
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

        public override UIElement Child
        {
            get => base.Child;
            set => SetChild(value);
        }

        private void SetChild(UIElement value)
        {
            void child_LayoutUpdated(object sender, EventArgs e)
            {
                this.InvalidateVisual();
            }

            if (Child != null)
            {
                Child.LayoutUpdated -= child_LayoutUpdated;
            }
            base.Child = value;
            if (Child != null)
            {
                Child.LayoutUpdated += child_LayoutUpdated;
            }
        }

        

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            Child.Arrange(new Rect(arrangeSize));
            return arrangeSize;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            var childConstraint = constraint;
            Child.Measure(childConstraint);
            var childSize = Child.DesiredSize;
            childSize.Width += Thickness * 2;
            childSize.Height += Thickness * 2;
            return constraint;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            var unioned = Child.GetUnionedGeometry(false);
            var childGeometry = unioned.GetFlattenedPathGeometry();
            var toRemove = childGeometry.Figures.Take(childGeometry.Figures.Count - 1).ToList();
            toRemove.ForEach(x => childGeometry.Figures.Remove(x));
            drawingContext.DrawGeometry(Fill, BorderBrush.ToPen(Thickness), childGeometry);
        }


    }
}

using System;
using System.Collections.Generic;
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

namespace AX.WPF.Controls
{  
    public class DiagramView : ListBox
    {
        static DiagramView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DiagramView), new FrameworkPropertyMetadata(typeof(DiagramView)));
        }

        public Brush SelectionBrush
        {
            get { return (Brush)GetValue(SelectionBrushProperty); }
            set { SetValue(SelectionBrushProperty, value); }
        }

        public static readonly DependencyProperty SelectionBrushProperty =
            DependencyProperty.Register(nameof(SelectionBrush), typeof(Brush), typeof(DiagramView), new PropertyMetadata(Brushes.Red));

        public DiagramView()
        {
            
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new DiagramViewItem();
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            var diagramViewItem = element as DiagramViewItem;            
        }

        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            base.ClearContainerForItemOverride(element, item);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            UnselectAll();
        }
    }
}

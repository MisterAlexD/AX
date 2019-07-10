using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AX.WPF
{
    public class CustomRenderControl : Control
    {
        private DrawingGroup _renderGroup = new DrawingGroup();

        public CustomRenderControl()
        {
            Loaded += CustomRenderElement_Loaded;
        }

        private void CustomRenderElement_Loaded(object sender, RoutedEventArgs e)
        {
            CallRender();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            drawingContext.DrawDrawing(_renderGroup);
        }

        protected void CallRender()
        {
            var dc = _renderGroup.Open();
            Render(dc);
            dc.Close();
        }

        protected virtual void Render(DrawingContext dc)
        { }
    }
}

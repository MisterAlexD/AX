using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace AX.WPF
{
    public class CustomRenderElement : FrameworkElement
    {
        private DrawingGroup _renderGroup = new DrawingGroup();

        public CustomRenderElement()
        {
            Loaded += CustomRenderElement_Loaded;
        }

        private void CustomRenderElement_Loaded(object sender, RoutedEventArgs e)
        {
            CallRender();
        }

        protected sealed override void OnRender(DrawingContext drawingContext)
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

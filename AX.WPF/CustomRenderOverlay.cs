using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace AX.WPF
{
    public class CustomRenderOverlay : Adorner
    {
        private DrawingGroup renderGroup;
        public event EventHandler<RenderEventArgs> Render;
        
        public CustomRenderOverlay(UIElement adornedElement) : base(adornedElement)
        {   
            IsHitTestVisible = false;
            AllowDrop = false;
            renderGroup = new DrawingGroup();
        }

        public void CallRender()
        {
            var dc = renderGroup.Open();
            Render?.Invoke(this, new RenderEventArgs(dc));
            dc.Close();
        }

        protected sealed override void OnRender(DrawingContext drawingContext)
        {
            drawingContext.DrawDrawing(renderGroup);
        }
    }
}

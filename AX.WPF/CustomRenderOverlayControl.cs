using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;

namespace AX.WPF
{
    public class CustomRenderOverlayControl : CustomRenderControl
    {
        protected class AdornerOverlay : Adorner
        {
            private CustomRenderOverlayControl _adornedElement;
            private DrawingGroup _renderGroup;

            public AdornerOverlay(CustomRenderOverlayControl adornedElement) : base(adornedElement)
            {
                IsHitTestVisible = false;
                AllowDrop = false;
                _adornedElement = adornedElement;
                _renderGroup = new DrawingGroup();
            }

            public void CallRender()
            {
                var dc = _renderGroup.Open();
                _adornedElement.DrawToOverlay(dc);
                dc.Close();
            }

            protected sealed override void OnRender(DrawingContext drawingContext)
            {
                drawingContext.DrawDrawing(_renderGroup);
            }
        }

        protected AdornerOverlay _overlay;

        public CustomRenderOverlayControl()
        {
            _overlay = new AdornerOverlay(this);
            Loaded += CustomRenderOverlayElement_Loaded;
        }

        private void CustomRenderOverlayElement_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var adornerLayer = AdornerLayer.GetAdornerLayer(this);
            adornerLayer.Add(_overlay);
            UpdateOverlay();
        }

        protected void UpdateOverlay()
        {
            _overlay.CallRender();
        }

        protected virtual void DrawToOverlay(DrawingContext dc)
        { }
    }
}
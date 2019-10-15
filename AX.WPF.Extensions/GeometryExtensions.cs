using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace AX.WPF
{
    public static class GeometryExtensions
    {
        public static Geometry GetGeometry(this DrawingGroup drawingGroup, bool buildGlyphs = true)
        {
            var result = new PathGeometry();
            foreach (var child in drawingGroup.Children)
            {
                if (child is GeometryDrawing)
                {
                    var geometryDrawing = child as GeometryDrawing;
                    result.AddGeometry(geometryDrawing.Geometry);
                }
                else if (child is DrawingGroup)
                {
                    var childDG = child as DrawingGroup;
                    result.AddGeometry(childDG.GetGeometry(buildGlyphs));
                }
                else if (child is GlyphRunDrawing)
                {
                    var glyphRunD = child as GlyphRunDrawing;
                    if (buildGlyphs)
                        result.AddGeometry(glyphRunD.GlyphRun.BuildGeometry());
                    else
                        result.AddGeometry(new RectangleGeometry(child.Bounds));
                }                
                else if (child is ImageDrawing)
                {
                    var imageDG = child as ImageDrawing;
                    result.AddGeometry(new RectangleGeometry(child.Bounds));
                }
                else
                {
                    throw new Exception($"Unknown Geometry: {child.GetType()}");
                }
            }
            return result;
        }

        public static Geometry GetUnionedGeometry(this DrawingGroup drawingGroup, bool buildGlyphs = true)
        {
            Geometry result = new PathGeometry();
            foreach (var child in drawingGroup.Children)
            {
                if (child is GeometryDrawing)
                {
                    var geometryDrawing = child as GeometryDrawing;
                    if (geometryDrawing.Pen != null)
                        result = new CombinedGeometry(GeometryCombineMode.Union, result, geometryDrawing.Geometry.GetWidenedPathGeometry(geometryDrawing.Pen));
                    else
                        result = new CombinedGeometry(GeometryCombineMode.Union, result, geometryDrawing.Geometry);
                }
                else if (child is DrawingGroup)
                {
                    var childDG = child as DrawingGroup;
                    result = new CombinedGeometry(GeometryCombineMode.Union, result, childDG.GetGeometry(buildGlyphs));
                }
                else if (child is GlyphRunDrawing)
                {
                    var glyphRunD = child as GlyphRunDrawing;
                    if (buildGlyphs)
                        result = new CombinedGeometry(GeometryCombineMode.Union, result, glyphRunD.GlyphRun.BuildGeometry());
                    else
                        result = new CombinedGeometry(GeometryCombineMode.Union, result, new RectangleGeometry(child.Bounds));
                }
                else
                {
                    throw new Exception($"Unknown Geometry: {child.GetType()}");
                }
            }
            return result;
        }

        public static Geometry GetGeometry(this Visual visual, bool buildGlyphs = true)
        {
            var result = new PathGeometry();
            var drawing = VisualTreeHelper.GetDrawing(visual);
            if (drawing != null)
                result.AddGeometry(drawing.GetGeometry(buildGlyphs));
            var childrenCount = VisualTreeHelper.GetChildrenCount(visual);

            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(visual, i) as Visual;
                if (child != null)
                {
                    var geometry = child.GetGeometry(buildGlyphs);
                    var transform = child.TransformToAncestor(visual);
                    geometry.Transform = transform as Transform;
                    result.AddGeometry(geometry);
                }
            }
            return result;
        }

        public static Geometry GetUnionedGeometry(this Visual visual, bool buildGlyphs = true)
        {
            Geometry result = new PathGeometry();
            var drawing = VisualTreeHelper.GetDrawing(visual);
            if (drawing != null)
                result = new CombinedGeometry(GeometryCombineMode.Union, result, drawing.GetUnionedGeometry(buildGlyphs));
            var childrenCount = VisualTreeHelper.GetChildrenCount(visual);

            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(visual, i) as Visual;
                if (child != null)
                {
                    var geometry = child.GetUnionedGeometry(buildGlyphs);
                    var transform = child.TransformToAncestor(visual);
                    geometry.Transform = transform as Transform;
                    result = new CombinedGeometry(GeometryCombineMode.Union, result, geometry);
                }
            }
            return result;
        }
    }
}

using VagabondK.Indicators;
using VagabondK.Indicators.GeometryUtil;
using VagabondK.OpenType.Geometry;

namespace DigiFont.FontFile
{
    public class GlyphContourDrawingContext : PartDrawingContext<GlyphContour>
    {
        private readonly double height;
        private readonly double leftMargin;

        public GlyphContourDrawingContext(double height, double leftMargin)
        {
            this.height = height;
            this.leftMargin = leftMargin;
        }

        protected override void OnBeginPath(in Point startPoint)
            => Renderer.MoveTo(ShiftX(startPoint.X), FlipY(startPoint.Y));
        protected override void OnDrawLine(in Point endPoint)
            => Renderer.LineTo(ShiftX(endPoint.X), FlipY(endPoint.Y));
        protected override void OnDrawQuadraticBezier(in Point controlPoint, in Point endPoint)
            => Renderer.QuadTo(ShiftX(controlPoint.X), FlipY(controlPoint.Y), ShiftX(endPoint.X), FlipY(endPoint.Y));
        protected override void OnDrawCubicBezier(in Point controlPoint1, in Point controlPoint2, in Point endPoint)
            => Renderer.CurveTo(ShiftX(controlPoint1.X), FlipY(controlPoint1.Y), ShiftX(controlPoint2.X), FlipY(controlPoint2.Y), ShiftX(endPoint.X), FlipY(endPoint.Y));
        protected override void OnClosePath() { }

        private double ShiftX(double x) => x + leftMargin;
        private double FlipY(double y) => height - y;
    }
}

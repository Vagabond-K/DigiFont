using VagabondK.Indicators;
using VagabondK.Indicators.DigitalFonts;
using VagabondK.Indicators.GeometryUtil;
using VagabondK.OpenType.Geometry;

namespace DigiFont.FontFile
{
    public class SevenSegmentFontFileBuilder : DigitalFontFileBuilder<SevenSegmentFont>
    {
        public double SeparatorSize { get; set; } = 0.15;

        public override IEnumerable<KeyValuePair<int, Glyph>> GetCustomGlyphs()
        {
            var actualHeight = DigitalFont.Size.ToValidValue();
            var thickness = Math.Min(DigitalFont.Thickness.ToValidValue(), 1) * actualHeight / 3;
            var slantX = -Math.Max(Math.Min(DigitalFont.SlantAngle.ToValidValue(), 45), 0) / 180d * Math.PI;
            var spacing = DigitalFont.Width * Spacing;
            var separatorSize = actualHeight * Math.Min(SeparatorSize.ToValidValue(), 1);
            var advanceWidth = (int)Math.Ceiling(separatorSize + spacing);
            var drawingContext = new GlyphContourDrawingContext(actualHeight, spacing / 2);

            var y = actualHeight - separatorSize;
            var skew = Transform.CreateSkew(slantX, 0, 0, actualHeight);

            drawingContext.Renderer = new GlyphContour();
            drawingContext.DrawPart(Part.CreateEllipse(0, y, separatorSize, separatorSize, skew));
            yield return new KeyValuePair<int, Glyph>('.', new Glyph([drawingContext.Renderer], advanceWidth));

            drawingContext.Renderer = new GlyphContour();
            drawingContext.DrawPart(Part.CreateComma(0, y, separatorSize, skew));
            yield return new KeyValuePair<int, Glyph>(',', new Glyph([drawingContext.Renderer], advanceWidth));

            var colonY1 = thickness + (actualHeight - thickness * 3) / 4;
            var colonY2 = actualHeight - colonY1;
            var colonSize = Math.Min(separatorSize, colonY2 - colonY1);
            colonY1 -= colonSize / 2;
            colonY2 -= colonSize / 2;
            advanceWidth = (int)Math.Ceiling(colonSize + spacing);
            drawingContext.Renderer = new GlyphContour();
            drawingContext.DrawPart(Part.CreateEllipse(0, colonY1, colonSize, colonSize, skew));
            var colon1 = drawingContext.Renderer;

            var outline = new GlyphOutline { colon1 };
            drawingContext.Renderer = outline.BeginContour();
            drawingContext.DrawPart(Part.CreateEllipse(0, colonY2, colonSize, colonSize, skew));
            yield return new KeyValuePair<int, Glyph>(':', new Glyph(outline, advanceWidth));

            outline = new GlyphOutline { colon1 };
            drawingContext.Renderer = outline.BeginContour();
            drawingContext.DrawPart(Part.CreateComma(0, colonY2, colonSize, skew));
            yield return new KeyValuePair<int, Glyph>(';', new Glyph(outline, advanceWidth));
        }
    }

    static class DoubleExtensions
    {
        public static double ToValidValue(this double value, double defaultValue = 0d)
            => double.IsNaN(value) || double.IsInfinity(value) ? defaultValue : Math.Max(value, 0d);
    }
}

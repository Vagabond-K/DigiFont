using VagabondK.Indicators.DigitalFonts;
using VagabondK.OpenType;
using VagabondK.OpenType.Geometry;

namespace DigiFont.FontFile
{
    public class DigitalFontFileBuilder<TDigitalFont> : IDigitalFontFileBuilder where TDigitalFont : DigitalFont
    {
        public TDigitalFont DigitalFont { get; set; }
        public double Spacing { get; set; } = 0.3;
        public string FamilyName { get; set; } = "Digital";
        public uint MajorVersion { get; set; } = 1;
        public uint MinorVersion { get; set; } = 0;
        public Weight Weight { get; set; } = Weight.Normal;
        public bool Invert { get; set; }

        DigitalFont IDigitalFontFileBuilder.DigitalFont => DigitalFont;

        public virtual IEnumerable<KeyValuePair<int, Glyph>> GetCustomGlyphs() { yield break; }

        public byte[] Build()
        {
            var actualHeight = DigitalFont.Size;
            var slantAngle = DigitalFont.SlantAngle;
            var weight = Weight;
            var spacing = DigitalFont.Width * Spacing;
            var advanceWidth = (int)Math.Ceiling(DigitalFont.Width + spacing);

            var drawingContext = new GlyphContourDrawingContext(actualHeight, spacing / 2);
            var segmentDictionary = DigitalFont.Segments.Select(segment =>
            {
                var contour = new GlyphContour();
                drawingContext.Renderer = contour;
                drawingContext.DrawPart(segment);
                return new
                {
                    segment,
                    contour
                };
            }).ToDictionary(item => item.segment, item => item.contour);

            var familyName = FamilyName;
            var subfamily = weight == Weight.Normal
                ? $"{(slantAngle == 0 ? FontSelection.Regular.ToString() : FontSelection.Italic.ToString())}"
                : (slantAngle == 0 ? weight.ToString() : $"{weight} {FontSelection.Italic}");

            var glyphs = DigitalFont.GetAllBinaryCodes().Select(item => item.Key)
                .Select(c =>
                {
                    var outline = new GlyphOutline();
                    foreach (var part in DigitalFont.GetCharacterSegments(c, Invert ? DigitalSegmentFilter.InactiveOnly : DigitalSegmentFilter.ActiveOnly))
                        if (segmentDictionary.TryGetValue(part, out var contour))
                            outline.AddContour(contour);

                    if (outline.Count == 0)
                        outline.BeginContour().MoveTo(0, 0);
                    return new KeyValuePair<int, Glyph>(c, new Glyph(outline, advanceWidth));
                }).ToDictionary(item => item.Key, item => item.Value);

            var customGlyphs = GetCustomGlyphs();
            if (customGlyphs != null)
                foreach (var item in customGlyphs)
                    glyphs[item.Key] = item.Value;

            var otfFont = new FontBuilder()
                .UnitsPerEm(1000)
                .BreakChar(0x20) // U+0020 SPACE
                .UseTypoMetrics()
                .Family(familyName)
                .Subfamily(subfamily)
                .UniqueId($"{familyName} {subfamily}: {MajorVersion}.{MinorVersion}")
                .Version($"Version {MajorVersion}.{MinorVersion}")
                .Metrics(new FontMetrics
                {
                    Ascender = 1125,
                    Descender = -250,
                    LineGap = 0,
                })
                //.XAvgCharWidth(advanceWidth) // 고정폭: 모든 글리프가 동일한 advance width
                //.FixedPitch(true)
                .ItalicAngle(-slantAngle)
                .WeightClass(Weight)
                .AddGlyphs(glyphs)
                .Build<OtfFont>();

            if (weight > Weight.Normal)
                otfFont.Os2.FsSelection |= FontSelection.Bold;

            return otfFont.ToBytes();
        }
    }
}

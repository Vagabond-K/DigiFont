using VagabondK.Indicators.DigitalFonts;
using VagabondK.OpenType;

namespace DigiFont.FontFile
{
    public interface IDigitalFontFileBuilder
    {
        DigitalFont DigitalFont { get; }
        double Spacing { get; set; }
        string FamilyName { get; set; }
        uint MajorVersion { get; set; }
        uint MinorVersion { get; set; }
        Weight Weight { get; set; }
        bool Invert { get; set; }
        byte[] Build();
    }
}

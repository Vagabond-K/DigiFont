using DigiFont.FontFile;
using Microsoft.AspNetCore.Components;
using VagabondK.Indicators.DigitalFonts;

namespace DigiFont.Components
{
    public class FontSetting<TBuilder, TDigitalFont> : ComponentBase
        where TBuilder : DigitalFontFileBuilder<TDigitalFont>
        where TDigitalFont : DigitalFont
    {
        [Parameter, EditorRequired]
        public TBuilder Builder { get; set; }

        [Parameter, EditorRequired]
        public TDigitalFont Font { get; set; }

        [Parameter]
        public EventCallback<ChangeEventArgs> OnSettingChanged { get; set; }
    }
}

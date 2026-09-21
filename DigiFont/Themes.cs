using MudBlazor;
using MudBlazor.Utilities;

namespace DigiFont
{
    public class Themes
    {
        private static readonly PaletteDark palette = new()
        {
            HoverOpacity = 0.1,

            OverlayDark = new MudColor(Colors.Shades.Black).SetAlpha(0.5).ToString(MudColorOutputFormats.RGBA),
            OverlayLight = new MudColor(Colors.Shades.White).SetAlpha(0.5).ToString(MudColorOutputFormats.RGBA),

            Black = Colors.Shades.Black,
            Background = Colors.Shades.Black,
            BackgroundGray = "#202020",
            Surface = "rgba(32,32,32,0.7)",
            DrawerBackground = "rgba(32,32,32,0.7)",
            DrawerText = "#f0f0f0",
            DrawerIcon = "#f0f0f0",
            AppbarBackground = "rgba(32,32,32,0.7)",
            AppbarText = "#ffffff",
            TextPrimary = "#f0f0f0",
            TextSecondary = "#e0e0e0",
            ActionDefault = "#ffffff",
            Divider = "rgba(255,255,255,0.24)",
            DividerLight = "rgba(255,255,255,0.12)",
            TableLines = "rgba(255,255,255,0.24)",
            TableStriped = "rgba(255,255,255,0.4)",
            LinesDefault = "rgba(255,255,255,0.24)",
            LinesInputs = "rgba(255,255,255,0.6)",
            TextDisabled = "rgba(255,255,255,0.4)",

            Primary = "#ffffff",
            PrimaryContrastText = Colors.Shades.Black,
            Secondary = "#606060",
            Dark = "#303030",
        };

        public static MudTheme DefaultTheme { get; } = new()
        {
            PaletteDark = palette,
            PaletteLight = palette,
            Typography = new()
            {
                Default = new DefaultTypography { FontFamily = ["IBM Plex Sans KR", "sans-serif"], },
                H1 = new H1Typography { FontWeight = "500" },
                H2 = new H2Typography { FontWeight = "500" },
                H3 = new H3Typography { FontWeight = "500" },
                H4 = new H4Typography { FontWeight = "500" },
                H5 = new H5Typography { FontWeight = "500" },
                H6 = new H6Typography { FontWeight = "500" },
                Body1 = new Body1Typography { },
                Body2 = new Body2Typography { },
                Subtitle1 = new Subtitle1Typography { FontFamily = ["Gowun Batang", "serif"] },
                Subtitle2 = new Subtitle2Typography { FontFamily = ["Gowun Batang", "serif"] },
                Button = new ButtonTypography { },
                Caption = new CaptionTypography { },
                Overline = new OverlineTypography { }
            },
            LayoutProperties = new LayoutProperties { DefaultBorderRadius = "10px" }
        };
    }
}

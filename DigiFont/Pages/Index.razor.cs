using DigiFont.Components;
using DigiFont.FontFile;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using VagabondK.Indicators.DigitalFonts;

namespace DigiFont.Pages
{
    public partial class Index : IAsyncDisposable
    {
        public class FontSettingMetadata
        {
            public FontSettingMetadata(string name, Type type, IDigitalFontFileBuilder builder, object receiver, Action<ChangeEventArgs> callback)
            {
                Name = name;
                Type = type;
                Builder = builder;
                Parameters[nameof(FontSetting<,>.Builder)] = builder;
                Parameters[nameof(FontSetting<,>.Font)] = builder.DigitalFont;
                Parameters[nameof(FontSetting<,>.OnSettingChanged)] = EventCallback.Factory.Create<ChangeEventArgs>(receiver, callback);
            }

            public string Name { get; }
            public Type Type { get; }
            public IDigitalFontFileBuilder Builder { get; }
            public Dictionary<string, object> Parameters { get; } = [];
        }

        private readonly FontSettingMetadata[] fontSettings;

        public Index()
        {
            fontSettings =
            [
                new FontSettingMetadata("7 Segment", typeof(SevenSegmentFontSetting), new SevenSegmentFontFileBuilder
                {
                    DigitalFont = new SevenSegmentFont(){ Size = 800, SlantAngle = 10, CustomBinaryCodes = new Dictionary<char, long>() },
                    FamilyName = "VagabondK-7Segment"
                }, this, OnSettingChanged),
                new FontSettingMetadata("5×7 Dot Matrix", typeof(RoundedRectCell5x7FontSetting), new DigitalFontFileBuilder<RoundedRectCell5x7Font>
                {
                    DigitalFont = new RoundedRectCell5x7Font(){ Size = 800, CustomBinaryCodes = new Dictionary<char, long>() },
                    FamilyName = "VagabondK-5x7DotMatrix"
                }, this, OnSettingChanged),
            ];

            CurrentSetting = fontSettings[0];
        }

        private FontSettingMetadata CurrentSetting
        {
            get;
            set
            {
                if (field != value)
                {
                    field = value;
                    _ = UpdatePreviewFont();
                }
            }
        }

        private IJSObjectReference? jsModule;
        private byte[]? fontBinary;
        private string previewText = "01:23 - 4,567.89";
        private CancellationTokenSource? cancellation;

        protected override async Task OnInitializedAsync()
        {
            jsModule = await JS.InvokeAsync<IJSObjectReference>("import", "./Pages/Index.razor.js");
            await UpdatePreviewFont();
        }

        public async ValueTask DisposeAsync()
        {
            if (jsModule is not null)
            {
                try
                {
                    await (jsModule?.DisposeAsync() ?? ValueTask.CompletedTask);
                }
                catch { }
            }
            GC.SuppressFinalize(this);
        }

        private ValueTask InvokeAsyncJS(string identifier, params object?[]? args)
            => jsModule?.InvokeVoidAsync(identifier, args: args) ?? ValueTask.CompletedTask;
        private async void OnSettingChanged(ChangeEventArgs e) => await UpdatePreviewFont();

        private async Task UpdatePreviewFont()
        {
            fontBinary = null;
            var cancellation = new CancellationTokenSource();

            this.cancellation?.Cancel();
            this.cancellation = cancellation;
            try
            {
                await Task.Run(() =>
                {
                    fontBinary = CurrentSetting.Builder.Build();
                }, cancellation.Token);
            }
            catch { }

            if (fontBinary != null)
            {
                using var stream = new MemoryStream(fontBinary);
                using var streamRef = new DotNetStreamReference(stream: stream);
                await InvokeAsyncJS("updatePreviewFont", streamRef);
            }
        }

        private async Task DownloadOtf()
        {
            var stream = new MemoryStream(fontBinary ?? CurrentSetting.Builder.Build());
            var fileName = CurrentSetting.Builder.FamilyName + ".otf";
            using var streamRef = new DotNetStreamReference(stream: stream);
            await InvokeAsyncJS("downloadFileFromStream", fileName, streamRef);
        }
    }
}

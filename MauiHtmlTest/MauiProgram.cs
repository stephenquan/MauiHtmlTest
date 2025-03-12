using Microsoft.Extensions.Logging;

namespace MauiHtmlTest
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("NotoSans-Regular.ttf", "NotoSansRegular");
                    fonts.AddFont("NotoSans-Bold.ttf", "NotoSansBold");
                    fonts.AddFont("NotoSans-BoldItalic.ttf", "NotoSansBoldItalic");
                    fonts.AddFont("NotoSans-Italic.ttf", "NotoSansItalic");
                    fonts.AddFont("myfont-sub.ttf", "MyFontSub");
                    fonts.AddFont("myfont-sup.ttf", "MyFontSup");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            FontMap.Register("NotoSansRegular", FontAttributes.Bold, "NotoSansBold");
            FontMap.Register("NotoSansRegular", FontAttributes.Italic, "NotoSansItalic");
            FontMap.Register("NotoSansRegular", FontAttributes.Bold | FontAttributes.Italic, "NotoSansBoldItalic");

            return builder.Build();
        }
    }
}

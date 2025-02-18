namespace MauiHtmlTest;

public partial class MainPage : ContentPage
{

    int count = 0;

    public FormattedString NumberTest { get; } = new FormattedString()
    {
        Spans =
        {
            new Span { Text = "0123456789", FontFamily = "MyFontSup", FontSize = 10, },
            new Span { Text = "0123456789", FontSize = 14, },
            new Span { Text = "0123456789", FontFamily = "MyFontSub", FontSize = 10, },
        }
    };

    public FormattedString FormulaTest { get; } = new FormattedString()
    {
        Spans =
        {
            new Span { Text = "1", FontFamily = "MyFontSup", FontSize = 10, },
            new Span { Text = "Almost every developer's favorite molecule is C", FontSize = 14, },
            new Span { Text = "8", FontFamily = "MyFontSub", FontSize = 10, },
            new Span { Text = "H", FontSize = 14, },
            new Span { Text = "10", FontFamily = "MyFontSub", FontSize = 10, },
            new Span { Text = "N", FontSize = 14, },
            new Span { Text = "4", FontFamily = "MyFontSub", FontSize = 10, },
            new Span { Text = "O", FontSize = 14, },
            new Span { Text = "2", FontFamily = "MyFontSub", FontSize = 10, },
            new Span { Text = ", also known as ", FontSize = 14 },
            new Span { Text = "\"caffeine.\"", TextColor = Colors.Red, FontSize = 14, },
        }
    };

    public string NumberHtml { get; } = "<sup>0123456789</sup>0123456789<sub>0123456789</sub>";
    public string FormulaHtml { get; } = """<sup>1</sup>Almost every developer's favorite molecule is C<sub>8</sub>H<sub>10</sub>N<sub>4</sub>O<sub>2</sub>, also known as <span style="color:red">"caffeine."</span> """;

    public MainPage()
    {
        InitializeComponent();
    }

    private void OnCounterClicked(object sender, EventArgs e)
    {
        count++;

        if (count == 1)
            CounterBtn.Text = $"Clicked {count} time";
        else
            CounterBtn.Text = $"Clicked {count} times";

        SemanticScreenReader.Announce(CounterBtn.Text);
    }
}

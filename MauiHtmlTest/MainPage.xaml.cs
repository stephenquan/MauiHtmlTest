namespace MauiHtmlTest;

public partial class MainPage : ContentPage
{

    int count = 0;

    public FormattedString FormulaTest { get; } = new FormattedString()
    {
        Spans =
        {
            new Span { Text = "1", FontFamily = "MyFontSup", FontSize = 10, },
            new Span { Text = "Almost every ", FontFamily = "NotoSansRegular", FontSize = 14, },
            new Span { Text = "developer's", FontFamily = "NotoSansBoldItalic", FontSize = 14, TextColor = Colors.Red },
            new Span { Text = " favorite molecule is C", FontFamily = "NotoSansRegular", FontSize = 14, },
            new Span { Text = "8", FontFamily = "MyFontSub", FontSize = 10, },
            new Span { Text = "H", FontFamily = "NotoSansRegular", FontSize = 14, },
            new Span { Text = "10", FontFamily = "MyFontSub", FontSize = 10, },
            new Span { Text = "N", FontFamily = "NotoSansRegular", FontSize = 14, },
            new Span { Text = "4", FontFamily = "MyFontSub", FontSize = 10, },
            new Span { Text = "O", FontFamily = "NotoSansRegular", FontSize = 14, },
            new Span { Text = "2", FontFamily = "MyFontSub", FontSize = 10, },
            new Span { Text = ", also known as ", FontFamily = "NotoSansRegular", FontSize = 14 },
            new Span { Text = "\"caffeine.\"", FontFamily = "NotoSansRegular", FontSize = 14, TextColor = Colors.Blue, TextDecorations = TextDecorations.Underline },
        }
    };

    private string demoHtml = @"
<p>
  <sup>1</sup>Almost every <b><i><font color=""red"">developer's</font></i></b> favorite molecule is C<sub>8</sub>H<sub>10</sub>N<sub>4</sub>O<sub>2</sub>, also known as
  <a href=""https://developer.mozilla.org/en-US/docs/Web/HTML/Element/sub"">""caffeine.""</a>
</p>
";

    /// <summary>
    /// Gets or sets a HTML demo string.
    /// </summary>
    public string DemoHtml
    {
        get => demoHtml;
        set
        {
            demoHtml = value;
            OnPropertyChanged(nameof(DemoHtml));
        }
    }

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

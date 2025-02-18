namespace MauiHtmlTest;

public partial class MainPage : ContentPage
{
    public string HtmlText { get; } = "<sup>0123456789</sup>0123456789<sub>0123456789</sub>";

    int count = 0;

    public FormattedString FormattedString { get; } = new FormattedString()
    {
        Spans =
        {
            new Span { Text = "0123456789", FontFamily = "MyFontSup", FontSize = 10, },
            new Span { Text = "0123456789", FontSize = 14, },
            new Span { Text = "0123456789", FontFamily = "MyFontSub", FontSize = 10, },
        }
    };

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

using System.Diagnostics;
using System.Windows.Input;

namespace MauiHtmlTest;

/// <summary>
/// Alternate Label component that uses FormattedText to render HTML content.
/// </summary>
public partial class HtmlLabel : Label
{
    NormalizeHtmlBuilder builder = new();

    /// <summary>
    /// Bindable property for the <see cref="Text"/> property.
    /// </summary>
    public static new BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(HtmlLabel), string.Empty);

    /// <summary>
    /// Gets or sets the HTML content to render.
    /// </summary>
    public new string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    /// <summary>
    /// Returns the <see cref="FormattedString"/> that represents the HTML content.
    /// </summary>
    public FormattedString FormattedString => builder.GetFormattedString(FontFamily, FontSize, FontAttributes, TextColor, TextDecorations, LinkActivatedCommand);

    /// <summary>
    /// Bindable property for the <see cref="LinkActivatedCommand"/> property.
    /// </summary>
    public static BindableProperty LinkActivatedCommandProperty = BindableProperty.Create(nameof(LinkActivatedCommand), typeof(ICommand), typeof(HtmlLabel), null);

    /// <summary>
    /// The command to execute when a link is activated.
    /// </summary>
    public ICommand LinkActivatedCommand
    {
        get => (ICommand)GetValue(LinkActivatedCommandProperty);
        set => SetValue(LinkActivatedCommandProperty, value);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HtmlLabel"/> class.
    /// </summary>
    public HtmlLabel()
    {
        InitializeComponent();

        this.SetBinding(Label.FormattedTextProperty, new Binding(nameof(FormattedString), source: this));

        PropertyChanged += (s, e) =>
        {
            switch (e.PropertyName)
            {
                case nameof(Text):
                    builder.Clear();
                    builder.AddHtml(Text);
                    OnPropertyChanged(nameof(FormattedString));
                    break;
                case nameof(FontFamily):
                case nameof(FontSize):
                case nameof(FontAttributes):
                case nameof(TextColor):
                case nameof(LinkActivatedCommand):
                    OnPropertyChanged(nameof(FormattedString));
                    break;
            }
        };

        OnPropertyChanged(nameof(FormattedString));
    }

    /// <summary>
    /// Implements a default handler for HTML links.
    /// </summary>
    /// <param name="link"></param>
    /// <returns></returns>
    //[RelayCommand]
    public async Task DefaultLinkActivated(string link)
    {
        await Task.Delay(50);

        try
        {
            Uri uri = new Uri(link);
            await Launcher.Default.OpenAsync(uri);
        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex.Message);
            Trace.WriteLine($"Unable to open link: {link}");
        }
    }
}

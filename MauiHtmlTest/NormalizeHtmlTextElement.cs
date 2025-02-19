namespace MauiHtmlTest;

/// <summary>
/// A normalized HTML element containing text and formatting attributes.
/// </summary>
public class NormalizeHtmlTextElement : INormalizeHtmlElement
{
    /// <summary>
    /// Gets or sets the font attributes.
    /// </summary>
    public FontAttributes? FontAttributes { get; set; } = null;

    /// <summary>
    /// Gets or sets the font family.
    /// </summary>
    public string? FontFamily { get; set; } = null;

    /// <summary>
    /// Gets or sets the font size.
    /// </summary>
    public double? FontSize { get; set; } = null;

    /// <summary>
    /// Gets or sets a hyperlink.
    /// </summary>
    public string? Link { get; set; } = null;

    /// <summary>
    /// Gets or sets the text.
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the text color.
    /// </summary>
    public Microsoft.Maui.Graphics.Color? TextColor { get; set; } = null;

    /// <summary>
    /// Gets or sets the text decorations.
    /// </summary>
    public TextDecorations? TextDecorations { get; set; } = null;
}

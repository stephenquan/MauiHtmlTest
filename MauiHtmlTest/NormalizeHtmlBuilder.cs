using HtmlAgilityPack;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace MauiHtmlTest;

/// <summary>
/// A class that helps to normalize HTML content into a list of normalized HTML elements.
/// </summary>
public class NormalizeHtmlBuilder
{
    /// <summary>
    /// This property contains the list of Spans that is the result of parsing the HTML text.
    /// </summary>
    public List<INormalizeHtmlElement> Elements { get; } = new();

    /// <summary>
    /// This is a state variable that helps to determine if a newline is needed.
    /// IsNewLine = -2 means we have completed a paragraph state, but have deferred adding the newlines.
    /// IsNewLine = -1 means we have completed a newline state, but have deferred adding the newline.
    /// IsNewline = 0 means we are not in a newline state, perhaps in the middle of a sentence.
    /// IsNewLine = 1 means we are in a newline state, typically after a line break tag.
    /// IsNewLine = 2 means we are in a paragraph state, typically after a paragraph tag.
    /// </summary>
    private int IsNewline = 2;

    /// <summary>
    /// Sanitizes HTML text to Span text.
    /// </summary>
    /// <param name="txt"></param>
    /// <returns></returns>
    public static string NoNewlines(string txt)
    {
        return Regex.Replace(txt, @"[\n\r\s]+", " ", RegexOptions.Multiline);
    }

    /// <summary>
    /// This method traverse the HTML nodes recursively, detecting the type of node and adding the appropriate Span to the FormattedString.
    /// </summary>
    /// <param name="node"></param>
    /// <param name="ListIndexes"></param>
    void AddHtmlNode(HtmlNode node, List<int> ListIndexes)
    {
        switch (node.Name)
        {
            case "#text":
                string text = NoNewlines(node.InnerText);
                if (IsNewline != 0)
                {
                    text = text.TrimStart();
                }
                if (text.Length > 0)
                {
                    AddDeferredNewLines();
                    AddTextElement(node, text);
                }
                return;

            case "pre":
                AddDeferredNewLines();
                AddNewLineIfRequired();
                AddTextElement(node, node.InnerText);
                return;

            case "br":
            case "hr":
                AddDeferredNewLines();
                IsNewline = -1;
                break;

            case "ol":
                AddDeferredNewLines();
                AddTwoNewLinesIfRequired();
                ListIndexes = [0, .. ListIndexes];
                break;

            case "ul":
                AddDeferredNewLines();
                AddTwoNewLinesIfRequired();
                ListIndexes = [-1, .. ListIndexes];
                break;

            case "li":
                AddDeferredNewLines();
                AddNewLineIfRequired();
                StringBuilder bullet = new();
                if (ListIndexes.Count > 1)
                {
                    bullet.Insert(0, "   ", ListIndexes.Count - 1);
                }
                bullet.Append(ListIndexes[0] == -1 ? "\u2022 " : $"{++ListIndexes[0]}. ");
                AddTextElement(node, bullet.ToString());
                break;

            case "p":
            case "h1":
            case "h2":
            case "h3":
            case "h4":
            case "h5":
            case "h6":
                AddDeferredNewLines();
                AddTwoNewLinesIfRequired();
                break;
        }

        foreach (HtmlNode nextChildNode in node.ChildNodes)
        {
            AddHtmlNode(nextChildNode, ListIndexes);
        }

        switch (node.Name)
        {
            case "p":
            case "h1":
            case "h2":
            case "h3":
            case "h4":
            case "h5":
            case "h6":
            case "ol":
            case "ul":
                IsNewline = -2;
                break;

            case "pre":
            case "hr":
            case "br":
                IsNewline = -1;
                break;
        }
    }

    /// <summary>
    /// Add deferred new lines as a result of completing paragraph or heading elements.
    /// </summary>
    void AddDeferredNewLines()
    {
        switch (IsNewline)
        {
            case -2:
                Elements.Add(new NormalizeHtmlTextElement() { Text = "\n" });
                Elements.Add(new NormalizeHtmlTextElement() { Text = "\n" });
                IsNewline = 2;
                break;
            case -1:
                Elements.Add(new NormalizeHtmlTextElement() { Text = "\n" });
                IsNewline = 1;
                break;
        }
    }

    /// <summary>
    /// Add two new lines for handling HTML block elements such as p, h1..h6, ol, ul, etc.
    /// </summary>
    void AddTwoNewLinesIfRequired()
    {
        AddNewLineIfRequired();
        switch (IsNewline)
        {
            case 1:
                Elements.Add(new NormalizeHtmlTextElement() { Text = "\n" });
                IsNewline = 2;
                break;
        }
    }

    /// <summary>
    /// add new lines for handling HTML block elements like pre, li, etc.
    /// </summary>
    void AddNewLineIfRequired()
    {
        if (IsNewline == 0)
        {
            Elements.Add(new NormalizeHtmlTextElement() { Text = "\n" });
            IsNewline = 1;
        }
    }

    /// <summary>
    /// Add a normalized HTML text element flattened from the HTML node.
    /// </summary>
    /// <param name="node"></param>
    /// <param name="text"></param>
    void AddTextElement(HtmlNode node, string text)
    {
        NormalizeHtmlTextElement textElement = new NormalizeHtmlTextElement() { Text = HtmlEntity.DeEntitize(text) };

        int supCount = 0;
        int subCount = 0;
        foreach (HtmlNode parent in Parents(node))
        {
            if (parent is null)
            {
                continue;
            }

            switch (parent.Name)
            {
                case "a":
                    textElement.TextDecorations ??= TextDecorations.None;
                    textElement.TextDecorations |= TextDecorations.Underline;
                    textElement.TextColor = Colors.Blue;
                    if (parent.Attributes is HtmlAttributeCollection attributes
                        && attributes.Contains("href")
                        && attributes["href"] is HtmlAttribute href
                        && href.Value is string link)
                    {
                        textElement.Link = link;
                    }
                    break;

                case "i":
                case "em":
                    textElement.FontAttributes ??= FontAttributes.None;
                    textElement.FontAttributes |= FontAttributes.Italic;
                    break;

                case "b":
                case "strong":
                    textElement.FontAttributes ??= FontAttributes.None;
                    textElement.FontAttributes |= FontAttributes.Bold;
                    break;

                case "h1":
                    SetHeading(textElement, 18.0);
                    break;

                case "h2":
                    SetHeading(textElement, 16.5);
                    break;

                case "h3":
                    SetHeading(textElement, 15.0);
                    break;

                case "h4":
                    SetHeading(textElement, 13.5);
                    break;

                case "h5":
                    SetHeading(textElement, 12.0);
                    break;

                case "h6":
                    SetHeading(textElement, 10.5);
                    break;

                case "u":
                    textElement.TextDecorations ??= TextDecorations.None;
                    textElement.TextDecorations |= TextDecorations.Underline;
                    break;

                case "s":
                case "strike":
                    textElement.TextDecorations ??= TextDecorations.None;
                    textElement.TextDecorations |= TextDecorations.Strikethrough;
                    break;

                case "span":
                    break;

                case "sup":
                    supCount++;
                    break;

                case "sub":
                    subCount++;
                    break;

                case "font":
                    if (parent.GetAttributeValue("color", null) is string colorText)
                    {
                        SetTextColor(textElement, colorText);
                    }
                    break;
            }

            if (parent.Attributes.Count > 0)
            {
                foreach (HtmlAttribute attribute in parent.Attributes)
                {
                    switch (attribute.Name)
                    {
                        case "style":
                            if (attribute.Value is not null)
                            {
                                Dictionary<string, string> styleDict = StyleDictionary(attribute.Value);
                                if (GetStyleValue(styleDict, "color") is string styleColor)
                                {
                                    SetTextColor(textElement, styleColor);
                                }

                                if (GetStyleValue(styleDict, "font-size") is string sizeText)
                                {
                                    SetFontSize(textElement, sizeText);
                                }

                                if (GetStyleValue(styleDict, "font-style") is string fontStyle)
                                {
                                    if (fontStyle == "italic")
                                    {
                                        textElement.FontAttributes ??= FontAttributes.None;
                                        textElement.FontAttributes |= FontAttributes.Italic;
                                    }
                                }
                            }
                            break;
                    }
                }
            }
        }

        if (subCount > supCount)
        {
            textElement.FontFamily = "MyFontSub";
            textElement.FontSize = 10;
        }
        else if (supCount > subCount)
        {
            textElement.FontFamily = "MyFontSup";
            textElement.FontSize = 10;
        }

        Elements.Add(textElement);
        IsNewline = 0;
    }

    /// <summary>
    /// Sets the heading style for the text element.
    /// </summary>
    /// <param name="textElement"></param>
    /// <param name="fontSize"></param>
    static void SetHeading(NormalizeHtmlTextElement textElement, double fontSize)
    {
        textElement.FontAttributes = FontAttributes.Bold;
        textElement.FontSize = fontSize;
        textElement.TextColor = null;
    }

    /// <summary>
    /// Used for styling any text node, a check of the parent nodes is done to determine the style.
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    static List<HtmlNode> Parents(HtmlNode node)
    {
        List<HtmlNode> parents = [];
        for (HtmlNode? parent = node.ParentNode; parent is not null; parent = parent.ParentNode)
        {
            parents.Insert(0, parent);
        }
        return parents;
    }

    /// <summary>
    /// Converts the HTML style attribute to a dictionary.
    /// </summary>
    /// <param name="style"></param>
    /// <returns></returns>
    static Dictionary<string, string> StyleDictionary(string style)
    {
        Dictionary<string, string> dict = new();
        string[] parts = style.Split(';');
        foreach (string part in parts)
        {
            string[] pair = part.Split(':');
            if (pair.Length == 2)
            {
                dict[pair[0].Trim()] = pair[1].Trim();
            }
        }
        return dict;
    }

    /// <summary>
    /// Gets the value of a key from the style dictionary.
    /// </summary>
    /// <param name="styleDict"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    static string GetStyleValue(Dictionary<string, string> styleDict, string key)
    {
        if (styleDict.ContainsKey(key))
        {
            return styleDict[key];
        }
        return string.Empty;
    }

    /// <summary>
    /// Helper for setting the color of a text element.
    /// </summary>
    /// <param name="textElement"></param>
    /// <param name="colorText"></param>
    static void SetTextColor(NormalizeHtmlTextElement textElement, string colorText)
    {
        if (string.IsNullOrWhiteSpace(colorText))
        {
            return; // Skip empty or whitespace values
        }

        TypeConverter cc = TypeDescriptor.GetConverter(typeof(Color));
        try
        {
            if (cc.ConvertFromString(colorText) is Color c)
            {
                textElement.TextColor = c;
            }
        }
        catch (NotSupportedException)
        {
            // Handle the case where the color conversion is not supported
        }
        catch (FormatException)
        {
            // Handle the case where the color format is invalid
        }
        catch (Exception)
        {
            // Handle any other unexpected exceptions
        }
    }

    /// <summary>
    /// Helper for setting the font size of a text element.
    /// </summary>
    /// <param name="textElement"></param>
    /// <param name="sizeText"></param>
    public static void SetFontSize(NormalizeHtmlTextElement textElement, string sizeText)
    {
        if (double.TryParse(sizeText, out double sizeDouble))
        {
            textElement.FontSize = sizeDouble;
        }
    }

    /// <summary>
    /// Processes the HTML text and adds the normalized HTML elements to the list.
    /// </summary>
    /// <param name="html"></param>
    public void AddHtml(string html)
    {
        HtmlDocument doc = new HtmlDocument();
        doc.LoadHtml(html);

        if (doc.DocumentNode is not null && doc.DocumentNode.ChildNodes.Count > 0)
        {
            HtmlNode rootNode = doc.DocumentNode.ChildNodes[0];
            AddHtmlNode(doc.DocumentNode, [-1]);
        }
    }

    /// <summary>
    /// Clears the HTML elements.
    /// </summary>
    public void Clear()
    {
        Elements.Clear();
        IsNewline = 2;
    }

    /// <summary>
    /// Converts the normalized HTML elements to a FormattedString.
    /// </summary>
    /// <returns></returns>
    public FormattedString GetFormattedString(string fontFamily, double fontSize, FontAttributes fontAttributes, Color textColor, TextDecorations textDecorations, ICommand linkActivatedCommand)
    {
        var formattedString = new FormattedString();
        foreach (var element in Elements)
        {
            if (element is NormalizeHtmlTextElement textElement)
            {
                Span span = new Span()
                {
                    Text = textElement.Text,
                    FontFamily = textElement.FontFamily ?? fontFamily,
                    FontSize = textElement.FontSize ?? (fontSize > 0.0 ? fontSize : 12.0),
                    FontAttributes = textElement.FontAttributes ?? fontAttributes,
                    TextColor = textElement.TextColor ?? textColor ?? Colors.Black,
                    TextDecorations = textDecorations
                };
                if (textElement.FontSize is double _fontSize)
                {
                    span.FontSize = _fontSize;
                }
                if (textElement.FontAttributes is FontAttributes _fontAttributes)
                {
                    span.FontAttributes = _fontAttributes;
                }
                if (textElement.TextColor is Color _textColor)
                {
                    span.TextColor = _textColor;
                }
                if (textElement.TextDecorations is TextDecorations _textDecorations)
                {
                    span.TextDecorations = _textDecorations;
                }
                if (span.FontFamily is string spanFontFamily && span.FontAttributes is FontAttributes spanFontAttributes)
                {
                    if (FontMap.Apply(ref spanFontFamily, ref spanFontAttributes))
                    {
                        span.FontFamily = spanFontFamily;
                        span.FontAttributes = spanFontAttributes;
                    }
                }
                if (linkActivatedCommand is not null && textElement.Link is string link)
                {
                    TapGestureRecognizer tapGesture = new();
                    tapGesture.Tapped += (s, e) =>
                    {
                        linkActivatedCommand?.Execute(link);
                    };
                    span.GestureRecognizers.Clear();
                    span.GestureRecognizers.Add(tapGesture);
                }
                formattedString.Spans.Add(span);
            }
        }
        return formattedString;
    }
}

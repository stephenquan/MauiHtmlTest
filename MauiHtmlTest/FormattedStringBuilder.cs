using HtmlAgilityPack;

namespace MauiHtmlTest;

/// <summary>
/// A class that converts an HTML string to a formatted string.
/// </summary>
public class FormattedStringBuilder
{
    /// <summary>
    /// The formatted string.
    /// </summary>
    public FormattedString FormattedString { get; } = new FormattedString();

    /// <summary>
    /// Adds an HTML string to the formatted string.
    /// </summary>
    /// <param name="htmlText"></param>
    public void AddHtmlText(string htmlText)
    {
        if (!string.IsNullOrEmpty(htmlText))
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlText);
            AddHtmlNode(doc.DocumentNode);
        }
    }

    /// <summary>
    /// Adds an HTML node to the formatted string.
    /// </summary>
    /// <param name="node"></param>
    public void AddHtmlNode(HtmlNode node)
    {
        if (node.NodeType == HtmlNodeType.Text)
        {
            FormattedString.Spans.Add(NewTextNode(node));
        }

        foreach (var child in node.ChildNodes)
        {
            AddHtmlNode(child);
        }
    }

    /// <summary>
    /// Applies a text node to a span.
    /// </summary>
    /// <param name="node"></param>
    public static Span NewTextNode(HtmlNode node)
    {
        Span span = new Span
        {
            Text = HtmlEntity.DeEntitize(node.InnerText),
            TextColor = Colors.Black,
            TextDecorations = TextDecorations.None,
            FontAttributes = FontAttributes.None,
            FontSize = 14
        };

        List<HtmlNode> parentNodes = new List<HtmlNode>();
        HtmlNode parentNode = node.ParentNode;
        while (parentNode != null)
        {
            parentNodes.Insert(0, parentNode);
            parentNode = parentNode.ParentNode;
        }

        int subCount = 0;
        int supCount = 0;

        foreach (HtmlNode _parentNode in parentNodes)
        {
            switch (_parentNode.Name.ToLower())
            {
                case "sub":
                    subCount++;
                    break;
                case "sup":
                    supCount++;
                    break;
                case "b":
                    span.FontAttributes = FontAttributes.Bold;
                    break;
                case "i":
                    span.FontAttributes = FontAttributes.Italic;
                    break;
                case "u":
                    span.TextDecorations = TextDecorations.Underline;
                    break;
                case "font":
                    ApplyFont(span, _parentNode);
                    break;
            }

            ApplyStyle(span, _parentNode);
        }

        if (subCount > supCount)
        {
            span.FontFamily = "MyFontSub";
            span.FontSize = 10;
        }
        else if (supCount > subCount)
        {
            span.FontFamily = "MyFontSup";
            span.FontSize = 10;
        }

        return span;
    }

    /// <summary>
    /// Applies HTML font attributes to a span.
    /// </summary>
    /// <param name="span"></param>
    /// <param name="fontNode"></param>
    public static void ApplyFont(Span span, HtmlNode fontNode)
    {
        ApplyFontColor(span, fontNode.GetAttributeValue("color", string.Empty));
    }

    /// <summary>
    /// Applies HTML font color to a span.
    /// </summary>
    /// <param name="span"></param>
    /// <param name="color"></param>
    public static void ApplyFontColor(Span span, string color)
    {
        if (!string.IsNullOrEmpty(color) && Color.TryParse(color, out Color colorValue))
        {
            span.TextColor = colorValue;
        }
    }

    /// <summary>
    /// Applies HTML style to a span.
    /// </summary>
    /// <param name="span"></param>
    /// <param name="node"></param>
    public static void ApplyStyle(Span span, HtmlNode node)
    {
        string style = node.GetAttributeValue("style", string.Empty);
        if (string.IsNullOrEmpty(style))
        {
            return;
        }

        foreach (string stylePair in style.Split(';'))
        {
            string[] styleParts = stylePair.Split(':');
            if (styleParts.Length == 2)
            {
                switch (styleParts[0].Trim().ToLower())
                {
                    case "color":
                        ApplyFontColor(span, styleParts[1].Trim());
                        break;
                }
            }
        }
    }

}


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
            Span span = new Span
            {
                Text = node.InnerText,
                TextColor = Colors.Black,
                TextDecorations = TextDecorations.None,
                FontAttributes = FontAttributes.None,
                FontSize = 14,
            };
            List<HtmlNode> parentNodes = new List<HtmlNode>();
            HtmlNode parentNode = node.ParentNode;
            while (parentNode != null)
            {
                parentNodes.Insert(0, parentNode);
                parentNode = parentNode.ParentNode;
            }
            foreach (HtmlNode _parentNode in parentNodes)
            {
                switch (_parentNode.Name)
                {
                    case "sub":
                        span.FontFamily = "MyFontSub";
                        span.FontSize = 10;
                        break;
                    case "sup":
                        span.FontFamily = "MyFontSup";
                        span.FontSize = 10;
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
                }
            }
            FormattedString.Spans.Add(span);
        }

        foreach (var child in node.ChildNodes)
        {
            AddHtmlNode(child);
        }
    }
}


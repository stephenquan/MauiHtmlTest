namespace MauiHtmlTest;

public static class FontMap
{
    public static Dictionary<Tuple<string, FontAttributes>, string> FontMapDictionary = new();

    public static void Register(string fontFamily, FontAttributes fontAttributes, string newFontFamily)
    {
        FontMapDictionary.Add(new Tuple<string, FontAttributes>(fontFamily, fontAttributes), newFontFamily);
    }

    public static bool Apply(ref string fontFamily, ref FontAttributes fontAttributes)
    {
        if (FontMapDictionary.TryGetValue(new Tuple<string, FontAttributes>(fontFamily, fontAttributes), out var newFontFamily))
        {
            fontFamily = newFontFamily;
            fontAttributes = FontAttributes.None;
            return true;
        }

        return false;
    }
}

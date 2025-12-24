// C#
// put in a utilities file, e.g. FontResolvers/WindowsFontResolver.cs
using System;
using System.IO;
using PdfSharp.Fonts;

public class WindowsFontResolver : IFontResolver
{
    public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
    {
        if (string.Equals(familyName, "Arial", StringComparison.OrdinalIgnoreCase))
        {
            if (isBold && isItalic) return new FontResolverInfo("Arial#BI");
            if (isBold) return new FontResolverInfo("Arial#B");
            if (isItalic) return new FontResolverInfo("Arial#I");
            return new FontResolverInfo("Arial#R");
        }

        // fallback to Arial regular
        return new FontResolverInfo("Arial#R");
    }

    public byte[] GetFont(string faceName)
    {
        string fonts = Environment.GetFolderPath(Environment.SpecialFolder.Fonts);
        string file = faceName switch
        {
            "Arial#R" => Path.Combine(fonts, "arial.ttf"),
            "Arial#B" => Path.Combine(fonts, "arialbd.ttf"),
            "Arial#I" => Path.Combine(fonts, "ariali.ttf"),
            "Arial#BI" => Path.Combine(fonts, "arialbi.ttf"),
            _ => Path.Combine(fonts, "arial.ttf")
        };

        if (!File.Exists(file))
            throw new FileNotFoundException($"Font file not found: {file}");

        return File.ReadAllBytes(file);
    }
}
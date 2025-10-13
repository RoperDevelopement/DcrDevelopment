using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMRMobileApp.Utilites
{
    public class ConvertHexToImage
    {
        public ImageSource RenderGlyphAsImage(string hexCode, string fontFamily, float size)
        {
            var glyph = char.ConvertFromUtf32(Convert.ToInt32(hexCode, 16));
            var label = new Label
            {
                Text = glyph,
                FontFamily = fontFamily,
                FontSize = size,
                TextColor = ImageColor,
            };

            // Render to bitmap (requires SkiaSharp or platform-specific drawing)
            // Then convert to ImageSource
            // Placeholder: actual rendering logic depends on platform
            return null; // Replace with rendered image source
        }
        public Color ImageColor { get; set; }
        public async Task<ImageSource> HexValueToImgeAsync(string hexVlue, string fontFamily, float size)
        {
            Image iconImage = new Image
            {

                Source = new FontImageSource
                {
                    Glyph = hexVlue, // Unicode for the icon
                    FontFamily = "MaterialIcons", // Ensure the font is added
                    Color = ImageColor,
                    Size = size
                }
            };
            
            return iconImage.Source;
        }
        public ImageSource HexValueToImge(string hexVlue, string fontFamily, float size )
        {
            Image iconImage = new Image
            {

                Source = new FontImageSource
                {
                    Glyph = hexVlue, // Unicode for the icon
                    FontFamily = "MaterialIcons", // Ensure the font is added
                    Color = ImageColor,
                    Size = size
                }
            };

            return iconImage.Source;
        }
       //public Color FromHex(string hex)
       // {
       //     if (hex.StartsWith("#")) hex = hex.Substring(1);

       //     byte a = 255;
       //     int start = 0;

       //     if (hex.Length == 8)
       //     {
       //         a = Convert.ToByte(hex.Substring(0, 2), 16);
       //         start = 2;
       //     }

       //     byte r = Convert.ToByte(hex.Substring(start, 2), 16);
       //     byte g = Convert.ToByte(hex.Substring(start + 2, 2), 16);
       //     byte b = Convert.ToByte(hex.Substring(start + 4, 2), 16);

       //     return Color.FromArgb(a, r, g, b);
       // }

    }
}

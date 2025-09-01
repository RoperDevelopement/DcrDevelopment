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

    }
}

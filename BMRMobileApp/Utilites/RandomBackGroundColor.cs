using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Graphics;

namespace BMRMobileApp.Utilites
{
public    class RandomBackGroundColor
    {
        public async Task<Color> RandomColor()
        {
            // Generate random RGB values
            Random random = new Random();
            float red = (float)random.NextDouble();
            float green = (float)random.NextDouble();
            float blue = (float)random.NextDouble();

            // Create a Color object
            Color randomColor = new Color(red, green, blue);
            return randomColor;

            // Set the background color of the page

        }
    }
}


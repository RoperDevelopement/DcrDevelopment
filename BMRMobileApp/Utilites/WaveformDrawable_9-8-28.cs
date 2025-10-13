using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMRMobileApp.Utilites
{
    public class WaveformDrawable_9_8_25 : IDrawable
    {
        public float[] Samples { get; set; }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (Samples == null) return;

            float width = dirtyRect.Width;
            float height = dirtyRect.Height;
            float step = width / Samples.Length;

            for (int i = 0; i < Samples.Length; i++)
            {
                float x = i * step;
                float y = height / 2;
                float amplitude = Samples[i] * height / 2;
                canvas.DrawLine(x, y - amplitude, x, y + amplitude);
            }
        }
        public void Draw2(ICanvas canvas, RectF dirtyRect)
        {
            if (Samples == null || Samples.Length == 0) return;

            float width = dirtyRect.Width;
            float height = dirtyRect.Height;
            float step = width / Samples.Length;

            for (int i = 0; i < Samples.Length; i++)
            {
                float x = i * step;
                float y = height / 2;
                float amp = Samples[i] * height / 2;
             //   canvas.StrokeColor = WaveColor;
                canvas.DrawLine(x, y - amp, x, y + amp);
            }
        }
    }
}

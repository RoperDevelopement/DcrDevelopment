 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMRMobileApp.Utilites
{
    public class WaveformDrawable : IDrawable
    {
     private readonly List<float> _waveData;
        public WaveformDrawable(List<float> waveData)
        {
            _waveData = waveData;
        }
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeColor = Colors.Blue;
            canvas.StrokeSize = 2;
            float centerY = dirtyRect.Height / 2;
            float step = dirtyRect.Width / _waveData.Count;


            for (int i = 1; i < _waveData.Count; i++)
            {
                float x1 = (i - 1) * step;
                float y1 = centerY - (_waveData[i - 1] * centerY);
                float x2 = i * step;
                float y2 = centerY - (_waveData[i] * centerY);
                canvas.DrawLine(x1, y1, x2, y2);
            }
        }
    }
}
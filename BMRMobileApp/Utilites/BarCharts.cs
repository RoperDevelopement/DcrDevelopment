using BMRMobileApp.Models;

using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMRMobileApp.Utilites
{
 public enum ChartTypes
    {
        RadialGaugeChart,
        BarChart,
        
        HalfRadialGaugeChart,
        LineChart,
        PieChart,
        PointChart,
        RadarChart,
        


    }
  
    public class BarCharts
    {
        public Chart MoodChart { get; set; }

        public async Task<IList<string>> GetChartTypes()
        {
            IList<string> chartTypes = new List<string>();
            chartTypes.Add(nameof(RadialGaugeChart));
            chartTypes.Add(nameof(BarChart));
            chartTypes.Add(nameof(HalfRadialGaugeChart));
            chartTypes.Add(nameof(LineChart));
            chartTypes.Add(nameof(PieChart));
            chartTypes.Add(nameof(PointChart));
            chartTypes.Add(nameof(RadarChart));
           return chartTypes.OrderBy(x => x).ToList();



        }
        public async Task<Chart> CreateChart(IList<EmotionsCountModel> emotionsCountModels, ChartTypes chartType)
        {

            var entries = emotionsCountModels.Select(e => new ChartEntry(emotionsCountModels.Count)
            {
                Label = e.Label,
                ValueLabel = e.ValueLabel,
                Color = GetColorForEmotion(e.EmotionsColor)
              
            });
            //foreach (EmotionsCountModel item in me)
            //    MoodEntry.Add(new EmotionsCountModel { Label = item.Label, ValueLabel = item.ValueLabel, EmotionsColor = $"{SKColor.Parse(item.EmotionsColor)}" });
            //MoodChart = new RadialGaugeChart { Entries = (IEnumerable<ChartEntry>)MoodEntry };
            switch (chartType)
            {
                case ChartTypes.RadialGaugeChart: return (MoodChart = new Microcharts.RadialGaugeChart { Entries = entries, LabelColor = SKColor.Parse("#FF000000") });
                case ChartTypes.BarChart: return (MoodChart = new Microcharts.BarChart { Entries = entries,LabelTextSize=24,LabelColor=SKColor.Parse("#FF000000") });


                case ChartTypes.HalfRadialGaugeChart:
                    return (MoodChart = new Microcharts.HalfRadialGaugeChart { Entries = entries, LabelColor = SKColor.Parse("#FF000000") });

                case ChartTypes.LineChart:
                    return (MoodChart = new Microcharts.LineChart { Entries = entries, LabelColor = SKColor.Parse("#FF000000") });
                case ChartTypes.PieChart:
                    return (MoodChart = new Microcharts.PieChart { Entries = entries, LabelColor = SKColor.Parse("#FF000000") });
                case ChartTypes.PointChart:
                    return (MoodChart = new Microcharts.PointChart { Entries = entries, LabelColor = SKColor.Parse("#FF000000") });
                case ChartTypes.RadarChart:
                    return (MoodChart = new Microcharts.RadarChart { Entries = entries, LabelColor = SKColor.Parse("#FF000000") });

                default: return null;


            }
            
            return null;
        }
        private SKColor GetColorForEmotion(string emotion)
        {
            //  return SKColors.Red;
            return SKColor.Parse(emotion);
        }
    }
}

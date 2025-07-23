using Polenter.Serialization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace BinMonitor.Common
{
    [Serializable]
    public class SerializableColor
    {        
        protected static ColorConverter ColorConverter = new ColorConverter();

        private Color _Value = Color.White;
        [ExcludeFromSerialization]
        public Color Value
        {
            get { return _Value; }
            set { _Value = value; }
        }

        public string Text
        {
            get {return ColorConverter.ConvertToString(this.Value);}
            set { this.Value = (Color)(ColorConverter.ConvertFromString(value)); }
        }

        public SerializableColor()
        { }

        public SerializableColor(Color color)
        { this.Value = color; }

        public SerializableColor(string color)
        { this.Text = color; }
    }
}

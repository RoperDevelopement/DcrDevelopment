using System;
using System.Drawing;

namespace EdocsUSA.Utilities
{
	[Serializable]
	public class SerializableColor
	{
		private static ColorConverter Converter = new ColorConverter();
		
		private string _SerializedValue;
		public string SerializedValue
		{
			get { return _SerializedValue; }
			set 
			{
				if (string.IsNullOrWhiteSpace(value))
				{
                    Logging.TraceLogger.TraceLoggerInstance.TraceError("Color value cannot be empty");
                    throw new ArgumentNullException("value");
                }
				_SerializedValue = value;
				_Value = (Color)Converter.ConvertFromString(value);
			}
		}
		
		private Color _Value = default(Color);
		public Color Value
		{ get { return _Value; } }
		
		public SerializableColor(Color value)
		{ SerializedValue = Converter.ConvertToString(value); }
		
		public SerializableColor(string serializedValue)
		{ SerializedValue = serializedValue; }
		
		public SerializableColor()
		{ }
	}
}

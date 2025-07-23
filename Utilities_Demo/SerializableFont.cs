using System;
using System.Drawing;

namespace EdocsUSA.Utilities
{
	[Serializable]
	public class SerializableFont
	{
		private static FontConverter Converter = new FontConverter();
		
		private string _SerializedValue;
		public string SerializedValue
		{
			get { return _SerializedValue; }
			set 
			{ 
				_SerializedValue = value;
				_Value = (Font)Converter.ConvertFromString(value);
			}
		}
		
		private Font _Value = default(Font);
		public Font Value
		{ get { return _Value; } }
		
		public SerializableFont(Font value)
		{ SerializedValue = Converter.ConvertToString(value); }
		
		public SerializableFont(string serializedValue)
		{ SerializedValue = serializedValue; }
		
		public SerializableFont()
		{ }
	}
}

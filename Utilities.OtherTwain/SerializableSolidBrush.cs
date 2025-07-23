using System;
using System.Drawing;

namespace EdocsUSA.Utilities
{
	[Serializable]
	public class SerializableSolidBrush : ISerializableBrush
	{
		private SerializableColor _Color = null;
			public SerializableColor Color
			{
				get { return _Color; }
				set 
				{
					_Color = value;
					_Brush = new SolidBrush(value.Value);
				}
			}
		
			private SolidBrush _Brush = default(SolidBrush);
			public SolidBrush Brush
			{ get { return _Brush; } }
			
			public SerializableSolidBrush(Color color)
			{ Color = new SerializableColor(color); }
			
			public SerializableSolidBrush(SerializableColor color)
			{ Color = color; }
			
			public SerializableSolidBrush()
			{ }
	}
}

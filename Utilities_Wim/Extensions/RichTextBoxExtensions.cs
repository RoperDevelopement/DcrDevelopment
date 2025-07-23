using System;
using System.Drawing;
using System.Windows.Forms;

namespace EdocsUSA.Utilities.Extensions
{
	public static class RichTextBoxExtensions
	{
		/// <credit>http://stackoverflow.com/questions/11851908/highlight-all-searched-word-in-richtextbox-c-sharp</credit>
		public static void HighlightText(this RichTextBox source, string text, Color color, StringComparison stringComparison)
		{
			int previousSelectionStart = source.SelectionStart;
			int previousSelectionLength = source.SelectionLength;
			Color previousSelectionColor = source.SelectionColor;
			
			int startIndex = 0;
			int index;
			try
			{
				while((index = source.Text.IndexOf(text, startIndex, stringComparison)) != -1)
				{
					source.Select(index, text.Length);
					source.SelectionColor = color;
					
					startIndex = index + text.Length;
				}
			}
			finally
			{
				source.SelectionStart = previousSelectionStart;
				source.SelectionLength = previousSelectionLength;
				source.SelectionColor = previousSelectionColor;
			}
		}
	}
}

using System;
using System.Linq;
using System.Windows.Forms;


namespace EdocsUSA.Utilities.Extensions
{
	public static class ListBoxExtensions
	{
		/// <returns>
		/// True if one or more items is selected.
		/// False if no items are selected.
		/// </returns>
		public static bool HasSelection(this ListBox listBox)
		{ return listBox.SelectedItems.Count > 0; }
		
		/// <exception cref="InvalidOperationException">If no items are selected.</exception>
		public static void EnsureHasSelection(this ListBox listBox)
		{
			if (listBox.HasSelection() == false)
			{ throw new InvalidOperationException("The requested operation requires an item to be selected in the ListBox"); }
		}
		
		/// <returns>
		/// True if one or more items was selected.
		/// False if no items are selected.
		/// </returns>
		public static bool TryGetSelectedText(this ListBox listBox, out string selectedText)
		{
			if (string.IsNullOrWhiteSpace(listBox.Text))
			{
				selectedText = null;
				return false;
			}
			else
			{
				selectedText = listBox.Text;
				return true;
			}
		}
		
		/// <returns>
		/// True if one or more items was selected.
		/// False if no items are selected.
		/// </returns>
		public static bool TryGetSelectedItem(this ListBox listBox, out object item)
		{
			if (listBox.SelectedItem == null)
			{
				item = default(object);
				return false;
			}
			else
			{
				item = (listBox.SelectedItem);
				return true;
			}			
		}
		
		/// <returns>
		/// True if one or more items was selected.
		/// False if no items are selected.
		/// </returns>
		public static bool TryGetSelectedItem<T>(this ListBox listBox, out T item)
		{
			object tItem;
			if (listBox.TryGetSelectedItem(out tItem) == true)
			{
				item = (T)(tItem);
				return true;
			}
			else
			{
				item = default(T);
				return false;
			}
		}
		
		/// <returns>
		/// True if one or more items was selected.
		/// False if no items are selected.
		/// </returns>
		public static bool TryGetSelectedItems(this ListBox listBox, out object[] items)
		{
			if (listBox.SelectedItems.Count == 0)
			{
				items = default(object[]);
				return false;
			}
			else
			{
				items = new object[listBox.SelectedItems.Count];
				listBox.SelectedItems.CopyTo(items, 0);
				return true;
			}
		}
		
		/// <returns>
		/// True if one or more items was selected.
		/// False if no items are selected.
		/// </returns>
		public static bool TryGetSelectedItems<T>(this ListBox listBox, out T[] items)
		{
			object[] tItems;
			if (listBox.TryGetSelectedItems(out tItems) == true)
			{
				items = (from T item in tItems select (T)item).ToArray();
				return true;
			}
			else
			{
				items = default(T[]);
				return false;
			}
		}
	}
}

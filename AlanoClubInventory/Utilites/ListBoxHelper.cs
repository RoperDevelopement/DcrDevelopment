using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AlanoClubInventory.Utilites
{
    public static class ListBoxHelper
    {
        public static readonly DependencyProperty BindableSelectedItemsProperty =
            DependencyProperty.RegisterAttached(
                "BindableSelectedItems",
                typeof(IList),
                typeof(ListBoxHelper),
                new PropertyMetadata(null, OnBindableSelectedItemsChanged));

        public static void SetBindableSelectedItems(DependencyObject obj, IList value) =>
            obj.SetValue(BindableSelectedItemsProperty, value);

        public static IList GetBindableSelectedItems(DependencyObject obj) =>
            (IList)obj.GetValue(BindableSelectedItemsProperty);

        private static void OnBindableSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ListBox listBox)
            {
                listBox.SelectionChanged += (s, args) =>
                {
                    SetBindableSelectedItems(listBox, listBox.SelectedItems);
                };
            }
        }
    }

}

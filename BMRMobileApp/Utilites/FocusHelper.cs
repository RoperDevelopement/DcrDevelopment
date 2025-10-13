using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMRMobileApp.Utilites
{
    public static class FocusHelper
    {
        public static readonly BindableProperty IsFocusedProperty =
            BindableProperty.CreateAttached(
                "IsFocused",
                typeof(bool),
                typeof(FocusHelper),
                false,
                BindingMode.TwoWay,
                propertyChanged: OnIsFocusedChanged);

        public static bool GetIsFocused(BindableObject view) =>
            (bool)view.GetValue(IsFocusedProperty);

        public static void SetIsFocused(BindableObject view, bool value) =>
            view.SetValue(IsFocusedProperty, value);

        private static void OnIsFocusedChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is VisualElement element && newValue is bool isFocused && isFocused)
                element.Focus();
        }
    }

}

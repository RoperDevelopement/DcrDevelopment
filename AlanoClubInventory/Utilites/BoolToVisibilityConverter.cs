using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace AlanoClubInventory.Utilites
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public bool Invert { get; set; } = false;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool flag = value is bool b && b;
            if (Invert) flag = !flag;
            return flag ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility v)
                return v == Visibility.Visible;
            return false;
        }
    }
    public class CheckBoxCommandBehavior : Behavior<CheckBox>
    {
        public ICommand CheckedCommand
        {
            get => (ICommand)GetValue(CheckedCommandProperty);
            set => SetValue(CheckedCommandProperty, value);
        }

        public static readonly DependencyProperty CheckedCommandProperty =
            DependencyProperty.Register(nameof(CheckedCommand), typeof(ICommand), typeof(CheckBoxCommandBehavior));

        public ICommand UncheckedCommand
        {
            get => (ICommand)GetValue(UncheckedCommandProperty);
            set => SetValue(UncheckedCommandProperty, value);
        }

        public static readonly DependencyProperty UncheckedCommandProperty =
            DependencyProperty.Register(nameof(UncheckedCommand), typeof(ICommand), typeof(CheckBoxCommandBehavior));

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Checked += (s, e) => CheckedCommand?.Execute(null);
            AssociatedObject.Unchecked += (s, e) => UncheckedCommand?.Execute(null);
        }
    }


}

﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Wasp.UI.DataEditor
{
    public class BoolToFontWeightConverter : DependencyObject, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value) ? FontWeights.Bold : FontWeights.Normal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
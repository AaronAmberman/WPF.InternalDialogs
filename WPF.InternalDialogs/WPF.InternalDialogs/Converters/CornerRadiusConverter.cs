using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WPF.InternalDialogs.Converters
{
    public class CornerRadiusConverter : IValueConverter
    {
        /// <summary>
        /// Returns a CornerRadius that can be parts of another CornerRadius. This is so binding the top left and top right corners
        /// of a border to another border without taking the bottom left and bottom right corners. TL, TR, BL and BR as Converter 
        /// Parameters separated by a vertical pipe |. So in the example we'd feed in TL|TR.
        /// </summary>
        /// <param name="value">The base CornerRadius to reference.</param>
        /// <param name="targetType">Must be CornerRadius.</param>
        /// <param name="parameter">
        /// Can be any combination of TL, TR, BL or BR. 
        /// Example: top would be TL|TR, bottom would be BL|BR, left would be TL|BL and right would be TR|BR.
        /// </param>
        /// <param name="culture">The culture.</param>
        /// <returns>A CornerRadius object that has the corner elements specified by ConverterParameter.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is CornerRadius)
            {
                CornerRadius cr = (CornerRadius)value;

                if (targetType != typeof(CornerRadius))
                    throw new ArgumentException("TargetType must be CornerRadius.", nameof(targetType));

                string[]? cornerOperations = parameter == null ? null : parameter?.ToString()?.Split('|');

                // we were unable to convert the parameter so just give them what they gave us
                if (cornerOperations == null) return value;

                bool takeTL = false, takeTR = false, takeBL = false, takeBR = false;

                foreach (string cornerOperation in cornerOperations)
                {
                    if (!string.IsNullOrWhiteSpace(cornerOperation))
                    {
                        if (cornerOperation.Equals("tl", StringComparison.OrdinalIgnoreCase))
                        {
                            takeTL = true;
                        }
                        else if (cornerOperation.Equals("tr", StringComparison.OrdinalIgnoreCase))
                        {
                            takeTR = true;
                        }
                        else if (cornerOperation.Equals("bl", StringComparison.OrdinalIgnoreCase))
                        {
                            takeBL = true;
                        }
                        else if (cornerOperation.Equals("br", StringComparison.OrdinalIgnoreCase))
                        {
                            takeBR = true;
                        }
                    }
                }

                // 4 corner CornerRadius binding doesn't need a converter

                // triple corner
                if (takeTL && takeTR && takeBR)
                {
                    return new CornerRadius(cr.TopLeft, cr.TopRight, cr.BottomRight, 0);
                }

                if (takeTL && takeTR && takeBL)
                {
                    return new CornerRadius(cr.TopLeft, cr.TopRight, 0, cr.BottomLeft);
                }

                if (takeTL && takeBL && takeBR)
                {
                    return new CornerRadius(cr.TopLeft, 0, cr.BottomRight, cr.BottomLeft);
                }

                if (takeTR && takeBL && takeBR)
                {
                    return new CornerRadius(0, cr.TopRight, cr.BottomRight, cr.BottomLeft);
                }

                // double corner
                if (takeTL && takeTR)
                {
                    return new CornerRadius(cr.TopLeft, cr.TopRight, 0, 0);
                }

                if (takeTL && takeBL)
                {
                    return new CornerRadius(cr.TopLeft, 0, 0, cr.BottomLeft);
                }

                if (takeTL && takeBR)
                {
                    return new CornerRadius(cr.TopLeft, 0, cr.BottomRight, 0);
                }

                if (takeTR && takeBL)
                {
                    return new CornerRadius(0, cr.TopRight, 0, cr.BottomLeft);
                }

                if (takeTR && takeBR)
                {
                    return new CornerRadius(0, cr.TopRight, cr.BottomRight, 0);
                }

                if (takeBL && takeBR)
                {
                    return new CornerRadius(0, 0, cr.BottomRight, cr.BottomLeft);
                }

                // sigle corner
                if (takeTL)
                {
                    return new CornerRadius(cr.TopLeft, 0, 0, 0);
                }

                if (takeTR)
                {
                    return new CornerRadius(0, cr.TopRight, 0, 0);
                }

                if (takeBL)
                {
                    return new CornerRadius(0, 0, 0, cr.BottomLeft);
                }

                if (takeBR)
                {
                    return new CornerRadius(0, 0, cr.BottomRight, 0);
                }
            }

            return DependencyProperty.UnsetValue;
        }

        /// <summary>Not used!</summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}

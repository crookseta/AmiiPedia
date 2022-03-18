using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AmiiPedia.Extensions
{
	/// <summary>
	/// Simple Button
	/// </summary>
	public class SimpleButton : Button
	{
        public static readonly DependencyProperty HoverColorProperty = DependencyProperty.Register
            (
                 "hoverColor",
                 typeof(SolidColorBrush),
                 typeof(SimpleButton),
                 new PropertyMetadata(new BrushConverter().ConvertFrom("#5D5D5D"))
            );

        public SolidColorBrush HoverColor
        {
            get { return (SolidColorBrush)GetValue(HoverColorProperty); }
            set { SetValue(HoverColorProperty, value); }
        }

        public static readonly DependencyProperty BackgroundColorProperty = DependencyProperty.Register
         (
              "bgColor",
              typeof(SolidColorBrush),
              typeof(SimpleButton),
              new PropertyMetadata(new SolidColorBrush(Colors.Red))
         );

        public SolidColorBrush BackgroundColor
        {
            get { return (SolidColorBrush)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DnTool.ValueConverters
{
    public class CanNullConverter : IValueConverter
    {
          public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
              return value;
          }
  
          public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
              NullableConverter nullableConvert;
              var toType = targetType;
              if (targetType.IsGenericType && targetType.GetGenericTypeDefinition().Equals(typeof(Nullable<>))) {
                 nullableConvert = new NullableConverter(targetType);
                 toType = nullableConvert.UnderlyingType;
             }
 
             return value.GetType().Equals(toType) ? value : null;
         }
     }
}

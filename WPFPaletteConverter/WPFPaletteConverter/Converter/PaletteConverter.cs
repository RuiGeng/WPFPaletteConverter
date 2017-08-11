using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;

namespace WPFPaletteConverter.Converter
{
    [ContentProperty("Value")]
    public class PalleteItem
    {
        public object Id { get; set; }
        public object Value { get; set; }
    }

    [ContentProperty("Items")]
    public class PaletteConverter : IValueConverter
    {
        public ObservableCollection<PalleteItem> Items { get; } = new ObservableCollection<PalleteItem>();

        private Dictionary<object, object> valuesById;
        private object defaultValue;

        public object Convert(object value, Type targetType, object parameter = null, CultureInfo culture = null)
        {
            if (valuesById == null)
            {
                defaultValue = Items.FirstOrDefault(i => i.Id == null)?.Value;
                valuesById = Items
                    .Where(i => i.Id != null)
                    .GroupBy(i => i.Id)
                    .ToDictionary(i => i.Key, i => i.First().Value);
            }

            object result;
            if (value != null && valuesById.TryGetValue(value, out result))
            {
                return result;
            }

            return defaultValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            valuesById = null;
        }

        public PaletteConverter()
        {
            Items.CollectionChanged += OnCollectionChanged;
        }
    }
}

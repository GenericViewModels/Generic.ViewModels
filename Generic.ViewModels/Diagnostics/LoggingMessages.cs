using System;
using System.Collections.Generic;
using System.Text;

namespace GenericViewModels.Diagnostics
{
    public static class LoggingMessages
    {
        public static string PropertyChangedEvent(Type type, string propertyName)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return $"{type.Name}: PropertyChanged event for property {propertyName} fired";
        }

        public static string Refresh(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return $"{type.Name}: Refresh";
        }

        public static string SelectedItemChanged(Type type, object? item)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return $"{type.Name}: Selected item changed: {item}";
        }
    }
}

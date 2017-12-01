﻿using System;
using System.Collections.Generic;
using System.Windows;

namespace ReVersion.Utilities.Extensions
{
    public static class ICollectionExtensions
    {
        public static void AddOnUI<T>(this ICollection<T> collection, T item)
        {
            Action<T> addMethod = collection.Add;
            Application.Current.Dispatcher.BeginInvoke(addMethod, item);
        }
    }
}

﻿using System.Globalization;

namespace FluentAssertionsAsync.Formatting;

public class Int32ValueFormatter : IValueFormatter
{
    /// <summary>
    /// Indicates whether the current <see cref="IValueFormatter"/> can handle the specified <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The value for which to create a <see cref="string"/>.</param>
    /// <returns>
    /// <see langword="true"/> if the current <see cref="IValueFormatter"/> can handle the specified value; otherwise, <see langword="false"/>.
    /// </returns>
    public bool CanHandle(object value)
    {
        return value is int;
    }

    public void Format(object value, FormattedObjectGraph formattedGraph, FormattingContext context, FormatChild formatChild)
    {
        formattedGraph.AddFragment(((int)value).ToString(CultureInfo.InvariantCulture));
    }
}

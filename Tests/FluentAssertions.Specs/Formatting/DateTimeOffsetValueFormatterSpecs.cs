﻿using System;
using System.Globalization;
using FluentAssertionsAsync.Extensions;
using FluentAssertionsAsync.Formatting;
using Xunit;

namespace FluentAssertionsAsync.Specs.Formatting;

public class DateTimeOffsetValueFormatterSpecs
{
    [Fact]
    public void When_time_is_not_relevant_it_should_not_be_included_in_the_output()
    {
        // Act
        string result = Formatter.ToString(new DateTime(1973, 9, 20));

        // Assert
        result.Should().Be("<1973-09-20>");
    }

    [Fact]
    public void When_the_offset_is_not_relevant_it_should_not_be_included_in_the_output()
    {
        // Act
        string result = Formatter.ToString(new DateTime(1973, 9, 20, 12, 59, 59));

        // Assert
        result.Should().Be("<1973-09-20 12:59:59>");
    }

    [Fact]
    public void When_the_offset_is_negative_it_should_include_it_in_the_output()
    {
        // Arrange
        DateTimeOffset date = new(1973, 9, 20, 12, 59, 59, -3.Hours());

        // Act
        string result = Formatter.ToString(date);

        // Assert
        result.Should().Be("<1973-09-20 12:59:59 -3h>");
    }

    [Fact]
    public void When_the_offset_is_positive_it_should_include_it_in_the_output()
    {
        // Arrange / Act
        string result = Formatter.ToString(new DateTimeOffset(1973, 9, 20, 12, 59, 59, 3.Hours()));

        // Assert
        result.Should().Be("<1973-09-20 12:59:59 +3h>");
    }

    [Fact]
    public void When_date_is_not_relevant_it_should_not_be_included_in_the_output()
    {
        // Act
        DateTime emptyDate = 1.January(0001);
        var dateTime = emptyDate.At(08, 20, 01);
        string result = Formatter.ToString(dateTime);

        // Assert
        result.Should().Be("<08:20:01>");
    }

    [InlineData("0001-01-02 04:05:06", "<0001-01-02 04:05:06>")]
    [InlineData("0001-02-01 04:05:06", "<0001-02-01 04:05:06>")]
    [InlineData("0002-01-01 04:05:06", "<0002-01-01 04:05:06>")]
    [InlineData("0001-02-02 04:05:06", "<0001-02-02 04:05:06>")]
    [InlineData("0002-01-02 04:05:06", "<0002-01-02 04:05:06>")]
    [InlineData("0002-02-01 04:05:06", "<0002-02-01 04:05:06>")]
    [InlineData("0002-02-02 04:05:06", "<0002-02-02 04:05:06>")]
    [Theory]
    public void When_date_is_relevant_it_should_be_included_in_the_output(string actual, string expected)
    {
        // Arrange
        var value = DateTime.Parse(actual, CultureInfo.InvariantCulture);

        // Act
        string result = Formatter.ToString(value);

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void When_a_full_date_and_time_is_specified_all_parts_should_be_included_in_the_output()
    {
        // Act
        var dateTime = 1.May(2012).At(20, 15, 30, 318);
        string result = Formatter.ToString(dateTime);

        // Assert
        result.Should().Be("<2012-05-01 20:15:30.318>");
    }

    [Theory]
    [InlineData("0001-02-03", "<0001-02-03>")]
    [InlineData("0001-02-03 04:05:06", "<0001-02-03 04:05:06>")]
    [InlineData("0001-02-03 04:05:06.123", "<0001-02-03 04:05:06.123>")]
    [InlineData("0001-02-03 04:05:06.123456", "<0001-02-03 04:05:06.123456>")]
    [InlineData("0001-02-03 04:05:06.1234567", "<0001-02-03 04:05:06.1234567>")]
    [InlineData("0001-02-03 00:00:01", "<0001-02-03 00:00:01>")]
    [InlineData("0001-02-03 00:01:00", "<0001-02-03 00:01:00>")]
    [InlineData("0001-02-03 01:00:00", "<0001-02-03 01:00:00>")]
    [InlineData("0001-02-03 00:00:00.1000000", "<0001-02-03 00:00:00.100>")]
    [InlineData("0001-02-03 00:00:00.0100000", "<0001-02-03 00:00:00.010>")]
    [InlineData("0001-02-03 00:00:00.0010000", "<0001-02-03 00:00:00.001>")]
    [InlineData("0001-02-03 00:00:00.0001000", "<0001-02-03 00:00:00.000100>")]
    [InlineData("0001-02-03 00:00:00.0000100", "<0001-02-03 00:00:00.000010>")]
    [InlineData("0001-02-03 00:00:00.0000010", "<0001-02-03 00:00:00.000001>")]
    [InlineData("0001-02-03 00:00:00.0000001", "<0001-02-03 00:00:00.0000001>")]
    public void When_datetime_components_are_not_relevant_they_should_not_be_included_in_the_output(string actual,
        string expected)
    {
        // Arrange
        var value = DateTime.Parse(actual, CultureInfo.InvariantCulture);

        // Act
        string result = Formatter.ToString(value);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("0001-02-03 +1", "<0001-02-03 +1h>")]
    [InlineData("0001-02-03 04:05:06 +1", "<0001-02-03 04:05:06 +1h>")]
    [InlineData("0001-02-03 04:05:06.123 +1", "<0001-02-03 04:05:06.123 +1h>")]
    [InlineData("0001-02-03 04:05:06.123456 +1", "<0001-02-03 04:05:06.123456 +1h>")]
    [InlineData("0001-02-03 04:05:06.1234567 +1", "<0001-02-03 04:05:06.1234567 +1h>")]
    [InlineData("0001-02-03 00:00:01 +1", "<0001-02-03 00:00:01 +1h>")]
    [InlineData("0001-02-03 00:01:00 +1", "<0001-02-03 00:01:00 +1h>")]
    [InlineData("0001-02-03 01:00:00 +1", "<0001-02-03 01:00:00 +1h>")]
    [InlineData("0001-02-03 00:00:00.1000000 +1", "<0001-02-03 00:00:00.100 +1h>")]
    [InlineData("0001-02-03 00:00:00.0100000 +1", "<0001-02-03 00:00:00.010 +1h>")]
    [InlineData("0001-02-03 00:00:00.0010000 +1", "<0001-02-03 00:00:00.001 +1h>")]
    [InlineData("0001-02-03 00:00:00.0001000 +1", "<0001-02-03 00:00:00.000100 +1h>")]
    [InlineData("0001-02-03 00:00:00.0000100 +1", "<0001-02-03 00:00:00.000010 +1h>")]
    [InlineData("0001-02-03 00:00:00.0000010 +1", "<0001-02-03 00:00:00.000001 +1h>")]
    [InlineData("0001-02-03 00:00:00.0000001 +1", "<0001-02-03 00:00:00.0000001 +1h>")]
    public void When_datetimeoffset_components_are_not_relevant_they_should_not_be_included_in_the_output(string actual,
        string expected)
    {
        // Arrange
        var value = DateTimeOffset.Parse(actual, CultureInfo.InvariantCulture);

        // Act
        string result = Formatter.ToString(value);

        // Assert
        result.Should().Be(expected);
    }
}

﻿using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

public partial class DateTimeAssertionSpecs
{
    public class BeAfter
    {
        [Fact]
        public void When_asserting_subject_datetime_is_after_earlier_expected_datetime_should_succeed()
        {
            // Arrange
            DateTime subject = new(2016, 06, 04);
            DateTime expectation = new(2016, 06, 03);

            // Act
            Action act = () => subject.Should().BeAfter(expectation);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_subject_datetime_is_after_later_expected_datetime_should_throw()
        {
            // Arrange
            DateTime subject = new(2016, 06, 04);
            DateTime expectation = new(2016, 06, 05);

            // Act
            Action act = () => subject.Should().BeAfter(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be after <2016-06-05>, but found <2016-06-04>.");
        }

        [Fact]
        public void When_asserting_subject_datetime_is_after_the_same_expected_datetime_should_throw()
        {
            // Arrange
            DateTime subject = new(2016, 06, 04);
            DateTime expectation = new(2016, 06, 04);

            // Act
            Action act = () => subject.Should().BeAfter(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be after <2016-06-04>, but found <2016-06-04>.");
        }
    }

    public class NotBeAfter
    {
        [Fact]
        public void When_asserting_subject_datetime_is_not_after_earlier_expected_datetime_should_throw()
        {
            // Arrange
            DateTime subject = new(2016, 06, 04);
            DateTime expectation = new(2016, 06, 03);

            // Act
            Action act = () => subject.Should().NotBeAfter(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be on or before <2016-06-03>, but found <2016-06-04>.");
        }

        [Fact]
        public void When_asserting_subject_datetime_is_not_after_later_expected_datetime_should_succeed()
        {
            // Arrange
            DateTime subject = new(2016, 06, 04);
            DateTime expectation = new(2016, 06, 05);

            // Act
            Action act = () => subject.Should().NotBeAfter(expectation);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_subject_datetime_is_not_after_the_same_expected_datetime_should_succeed()
        {
            // Arrange
            DateTime subject = new(2016, 06, 04);
            DateTime expectation = new(2016, 06, 04);

            // Act
            Action act = () => subject.Should().NotBeAfter(expectation);

            // Assert
            act.Should().NotThrow();
        }
    }
}

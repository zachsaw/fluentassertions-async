using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertionsAsync.Common;
using FluentAssertionsAsync.Execution;

namespace FluentAssertionsAsync.Specialized;

/// <summary>
/// Contains a number of methods to assert that an asynchronous method yields the expected result.
/// </summary>
public class NonGenericAsyncFunctionAssertions : AsyncFunctionAssertions<Task, NonGenericAsyncFunctionAssertions>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NonGenericAsyncFunctionAssertions"/> class.
    /// </summary>
    public NonGenericAsyncFunctionAssertions(Func<Task> subject, IExtractExceptions extractor)
        : this(subject, extractor, new Clock())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NonGenericAsyncFunctionAssertions"/> class with custom <see cref="IClock"/>.
    /// </summary>
    public NonGenericAsyncFunctionAssertions(Func<Task> subject, IExtractExceptions extractor, IClock clock)
        : base(subject, extractor, clock)
    {
    }

    /// <summary>
    /// Asserts that the current <see cref="Task"/> will complete within the specified time.
    /// </summary>
    /// <param name="timeSpan">The allowed time span for the operation.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public async Task<AndConstraint<NonGenericAsyncFunctionAssertions>> CompleteWithinAsync(
        TimeSpan timeSpan, string because = "", params object[] becauseArgs)
    {
        bool success = Execute.Assertion
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:task} to complete within {0}{reason}, but found <null>.", timeSpan);

        if (success)
        {
            (Task task, TimeSpan remainingTime) = InvokeWithTimer(timeSpan);

            success = Execute.Assertion
                .ForCondition(remainingTime >= TimeSpan.Zero)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:task} to complete within {0}{reason}.", timeSpan);

            if (success)
            {
                bool completesWithinTimeout = await CompletesWithinTimeoutAsync(task, remainingTime);

                Execute.Assertion
                    .ForCondition(completesWithinTimeout)
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:task} to complete within {0}{reason}.", timeSpan);
            }
        }

        return new AndConstraint<NonGenericAsyncFunctionAssertions>(this);
    }

    /// <summary>
    /// Asserts that the current <see cref="Func{Task}"/> does not throw any exception.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public async Task<AndConstraint<NonGenericAsyncFunctionAssertions>> NotThrowAsync(string because = "", params object[] becauseArgs)
    {
        bool success = Execute.Assertion
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context} not to throw{reason}, but found <null>.");

        if (success)
        {
            try
            {
                await Subject!.Invoke();
            }
            catch (Exception exception)
            {
                return NotThrowInternal(exception, because, becauseArgs);
            }
        }

        return new AndConstraint<NonGenericAsyncFunctionAssertions>(this);
    }

    /// <summary>
    /// Asserts that the current <see cref="Func{T}"/> stops throwing any exception
    /// after a specified amount of time.
    /// </summary>
    /// <remarks>
    /// The <see cref="Func{T}"/> is invoked. If it raises an exception,
    /// the invocation is repeated until it either stops raising any exceptions
    /// or the specified wait time is exceeded.
    /// </remarks>
    /// <param name="waitTime">
    /// The time after which the <see cref="Func{T}"/> should have stopped throwing any exception.
    /// </param>
    /// <param name="pollInterval">
    /// The time between subsequent invocations of the <see cref="Func{T}"/>.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="waitTime"/> or <paramref name="pollInterval"/> are negative.</exception>
    public Task<AndConstraint<NonGenericAsyncFunctionAssertions>> NotThrowAfterAsync(TimeSpan waitTime, TimeSpan pollInterval, string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNegative(waitTime);
        Guard.ThrowIfArgumentIsNegative(pollInterval);

        bool success = Execute.Assertion
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context} not to throw any exceptions after {0}{reason}, but found <null>.", waitTime);

        if (success)
        {
            return AssertionTaskAsync();

            async Task<AndConstraint<NonGenericAsyncFunctionAssertions>> AssertionTaskAsync()
            {
                TimeSpan? invocationEndTime = null;
                Exception exception = null;
                ITimer timer = Clock.StartTimer();

                while (invocationEndTime is null || invocationEndTime < waitTime)
                {
                    exception = await InvokeWithInterceptionAsync(Subject);

                    if (exception is null)
                    {
                        return new AndConstraint<NonGenericAsyncFunctionAssertions>(this);
                    }

                    await Clock.DelayAsync(pollInterval, CancellationToken.None);
                    invocationEndTime = timer.Elapsed;
                }

                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Did not expect any exceptions after {0}{reason}, but found {1}.", waitTime, exception);

                return new AndConstraint<NonGenericAsyncFunctionAssertions>(this);
            }
        }

        return Task.FromResult(new AndConstraint<NonGenericAsyncFunctionAssertions>(this));
    }
}

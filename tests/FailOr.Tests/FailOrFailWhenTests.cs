using TUnit.Assertions;
using TUnit.Assertions.Extensions;
using TUnit.Core;

namespace FailOr.Tests;

public class FailOrFailWhenTests
{
    [Test]
    [Arguments(1)]
    [Arguments(5)]
    public async Task FailWhen_preserves_successful_values_when_predicate_returns_false(int value)
    {
        var source = FailOr.Success(value);
        var failure = Failure.General("value must be negative");
        var calls = 0;
        var observed = 0;

        var result = source.FailWhen(
            x =>
            {
                observed = x;
                calls++;
                return x < 0;
            },
            failure
        );

        using var _ = Assert.Multiple();
        await Assert.That(calls).IsEqualTo(1);
        await Assert.That(observed).IsEqualTo(value);
        await ThenAssertions.AssertEquivalent(result, source);
    }

    [Test]
    [Arguments(1)]
    [Arguments(5)]
    public async Task FailWhen_returns_failure_when_predicate_returns_true(int value)
    {
        var failure = Failure.General("value must be odd");
        var calls = 0;
        var observed = 0;

        var result = FailOr
            .Success(value)
            .FailWhen(
                x =>
                {
                    observed = x;
                    calls++;
                    return true;
                },
                failure
            );

        using var _ = Assert.Multiple();
        await Assert.That(calls).IsEqualTo(1);
        await Assert.That(observed).IsEqualTo(value);
        await ThenAssertions.AssertFailure(result, failure);
    }

    [Test]
    [Arguments(2)]
    [Arguments(6)]
    public async Task FailWhenAsync_preserves_successful_values_when_predicate_returns_false(
        int value
    )
    {
        var source = FailOr.Success(value);
        var failure = Failure.General("value must be negative");
        var calls = 0;
        var observed = 0;

        var result = await source.FailWhenAsync(
            x =>
            {
                observed = x;
                calls++;
                return Task.FromResult(x < 0);
            },
            failure
        );

        using var _ = Assert.Multiple();
        await Assert.That(calls).IsEqualTo(1);
        await Assert.That(observed).IsEqualTo(value);
        await ThenAssertions.AssertEquivalent(result, source);
    }

    [Test]
    [Arguments(1)]
    [Arguments(3)]
    public async Task FailWhenAsync_returns_failure_when_predicate_returns_true(int value)
    {
        var failure = Failure.General("value must be odd");
        var calls = 0;
        var observed = 0;

        var result = await FailOr
            .Success(value)
            .FailWhenAsync(
                x =>
                {
                    observed = x;
                    calls++;
                    return Task.FromResult(true);
                },
                failure
            );

        using var _ = Assert.Multiple();
        await Assert.That(calls).IsEqualTo(1);
        await Assert.That(observed).IsEqualTo(value);
        await ThenAssertions.AssertFailure(result, failure);
    }

    [Test]
    [MethodDataSource(
        typeof(FailWhenTestData),
        nameof(FailWhenTestData.DirectFailureShortCircuitCases)
    )]
    public async Task Failed_sources_short_circuit_all_direct_FailWhen_overloads(
        string operation,
        Func<FailOr<int>, InvocationCounter, Task<FailOr<int>>> invoke
    )
    {
        var failure = Failure.General($"{operation} failed");
        var source = FailOr.Fail<int>(failure);
        var counter = new InvocationCounter();

        var result = await invoke(source, counter);

        using var _ = Assert.Multiple();
        await Assert.That(counter.Calls).IsEqualTo(0);
        await ThenAssertions.AssertFailure(result, failure);
    }

    [Test]
    [MethodDataSource(typeof(FailWhenTestData), nameof(FailWhenTestData.DirectNullSelectorCases))]
    public async Task Null_predicates_throw_for_direct_FailWhen_overloads(
        string operation,
        Action invoke,
        string parameterName
    )
    {
        await Assert.That(invoke).Throws<ArgumentNullException>().WithParameterName(parameterName);
    }

    [Test]
    [MethodDataSource(typeof(FailWhenTestData), nameof(FailWhenTestData.DirectNullFailureCases))]
    public async Task Null_failures_throw_for_direct_FailWhen_overloads(
        string operation,
        Action invoke,
        string parameterName
    )
    {
        await Assert.That(invoke).Throws<ArgumentNullException>().WithParameterName(parameterName);
    }

    [Test]
    [MethodDataSource(typeof(FailWhenTestData), nameof(FailWhenTestData.DirectNullTaskCases))]
    public async Task Null_tasks_throw_for_direct_async_FailWhen_overloads(
        string operation,
        Func<Task> invoke,
        string parameterName
    )
    {
        await Assert.That(invoke).Throws<ArgumentNullException>().WithParameterName(parameterName);
    }

    [Test]
    public async Task FailWhen_propagates_delegate_exceptions()
    {
        var expected = new InvalidOperationException("FailWhen failed");

        try
        {
            _ = FailOr
                .Success(1)
                .FailWhen(
                    (Func<int, bool>)(
                        _ =>
                        {
                            throw expected;
                        }
                    ),
                    Failure.General("predicate matched")
                );
            throw new Exception("Expected FailWhen to rethrow the original exception.");
        }
        catch (InvalidOperationException actual)
        {
            await Assert.That(ReferenceEquals(actual, expected)).IsTrue();
        }
    }

    [Test]
    public async Task FailWhenAsync_propagates_delegate_exceptions()
    {
        var expected = new InvalidOperationException("FailWhenAsync failed");

        try
        {
            _ = await FailOr
                .Success(1)
                .FailWhenAsync(
                    (Func<int, Task<bool>>)(_ => Task.FromException<bool>(expected)),
                    Failure.General("predicate matched")
                );
            throw new Exception("Expected FailWhenAsync to rethrow the original exception.");
        }
        catch (InvalidOperationException actual)
        {
            await Assert.That(ReferenceEquals(actual, expected)).IsTrue();
        }
    }
}

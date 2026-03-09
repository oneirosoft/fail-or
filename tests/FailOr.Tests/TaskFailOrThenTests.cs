using TUnit.Assertions;
using TUnit.Assertions.Extensions;
using TUnit.Core;

namespace FailOr.Tests;

public class TaskFailOrThenTests
{
    [Test]
    [Arguments(1, 2)]
    [Arguments(5, 6)]
    public async Task Then_map_transforms_successful_task_values(int value, int expected)
    {
        var result = await Task.FromResult(FailOr.Success(value)).Then(x => x + 1);

        await ThenAssertions.AssertSuccess(result, expected);
    }

    [Test]
    [Arguments(1, 3)]
    [Arguments(5, 15)]
    public async Task Then_bind_returns_inner_task_result(int value, int expected)
    {
        var result = await Task.FromResult(FailOr.Success(value)).Then(x => FailOr.Success(x * 3));

        await ThenAssertions.AssertSuccess(result, expected);
    }

    [Test]
    [Arguments(1)]
    [Arguments(5)]
    public async Task ThenEnsure_preserves_successful_task_values(int value)
    {
        var source = FailOr.Success(value);
        var calls = 0;
        var observed = 0;

        var result = await Task.FromResult(source)
            .ThenEnsure(x =>
            {
                observed = x;
                calls++;
                return FailOr.Success(x * 2);
            });

        using var _ = Assert.Multiple();
        await Assert.That(calls).IsEqualTo(1);
        await Assert.That(observed).IsEqualTo(value);
        await ThenAssertions.AssertEquivalent(result, source);
    }

    [Test]
    public async Task ThenEnsure_returns_inner_task_failures()
    {
        var firstFailure = Failure.General("validation failed");
        var secondFailure = Failure.Validation("Value", "Out of range");

        var result = await Task.FromResult(FailOr.Success(5))
            .ThenEnsure(_ => FailOr.Fail<bool>(firstFailure, secondFailure));

        await ThenAssertions.AssertFailure(result, firstFailure, secondFailure);
    }

    [Test]
    [Arguments(1, 4)]
    [Arguments(5, 8)]
    public async Task ThenAsync_map_transforms_successful_task_values(int value, int expected)
    {
        var result = await Task.FromResult(FailOr.Success(value))
            .ThenAsync(x => Task.FromResult(x + 3));

        await ThenAssertions.AssertSuccess(result, expected);
    }

    [Test]
    [Arguments(1, 5)]
    [Arguments(5, 9)]
    public async Task ThenAsync_bind_returns_inner_task_result(int value, int expected)
    {
        var result = await Task.FromResult(FailOr.Success(value))
            .ThenAsync(x => Task.FromResult(FailOr.Success(x + 4)));

        await ThenAssertions.AssertSuccess(result, expected);
    }

    [Test]
    [Arguments(1)]
    [Arguments(5)]
    public async Task ThenEnsureAsync_preserves_successful_task_values(int value)
    {
        var source = FailOr.Success(value);
        var calls = 0;
        var observed = 0;

        var result = await Task.FromResult(source)
            .ThenEnsureAsync(x =>
            {
                observed = x;
                calls++;
                return Task.FromResult(FailOr.Success(x * 2));
            });

        using var _ = Assert.Multiple();
        await Assert.That(calls).IsEqualTo(1);
        await Assert.That(observed).IsEqualTo(value);
        await ThenAssertions.AssertEquivalent(result, source);
    }

    [Test]
    public async Task ThenEnsureAsync_returns_inner_task_failures()
    {
        var firstFailure = Failure.General("async validation failed");
        var secondFailure = Failure.Validation("Value", "Still out of range");

        var result = await Task.FromResult(FailOr.Success(5))
            .ThenEnsureAsync(_ => Task.FromResult(FailOr.Fail<bool>(firstFailure, secondFailure)));

        await ThenAssertions.AssertFailure(result, firstFailure, secondFailure);
    }

    [Test]
    [Arguments(1)]
    [Arguments(5)]
    public async Task ThenDo_preserves_successful_task_values(int value)
    {
        var source = FailOr.Success(value);
        var calls = 0;
        var observed = 0;

        var result = await Task.FromResult(source)
            .ThenDo(x =>
            {
                observed = x;
                calls++;
            });

        using var _ = Assert.Multiple();
        await Assert.That(calls).IsEqualTo(1);
        await Assert.That(observed).IsEqualTo(value);
        await ThenAssertions.AssertEquivalent(result, source);
    }

    [Test]
    [Arguments(1)]
    [Arguments(5)]
    public async Task ThenDoAsync_preserves_successful_task_values(int value)
    {
        var source = FailOr.Success(value);
        var calls = 0;
        var observed = 0;

        var result = await Task.FromResult(source)
            .ThenDoAsync(x =>
            {
                observed = x;
                calls++;
                return Task.CompletedTask;
            });

        using var _ = Assert.Multiple();
        await Assert.That(calls).IsEqualTo(1);
        await Assert.That(observed).IsEqualTo(value);
        await ThenAssertions.AssertEquivalent(result, source);
    }

    [Test]
    [MethodDataSource(typeof(ThenTestData), nameof(ThenTestData.LiftedFailureShortCircuitCases))]
    public async Task Failed_sources_short_circuit_all_lifted_overloads(
        string operation,
        Func<Task<FailOr<int>>, InvocationCounter, Task<FailOr<int>>> invoke
    )
    {
        var failure = Failure.General($"{operation} failed");
        var sourceTask = Task.FromResult(FailOr.Fail<int>(failure));
        var counter = new InvocationCounter();

        var result = await invoke(sourceTask, counter);

        using var _ = Assert.Multiple();
        await Assert.That(counter.Calls).IsEqualTo(0);
        await ThenAssertions.AssertFailure(result, failure);
    }

    [Test]
    [MethodDataSource(typeof(ThenTestData), nameof(ThenTestData.LiftedNullSelectorCases))]
    public async Task Null_selectors_throw_for_lifted_overloads(
        string operation,
        Action invoke,
        string parameterName
    )
    {
        await Assert.That(invoke).Throws<ArgumentNullException>().WithParameterName(parameterName);
    }

    [Test]
    [MethodDataSource(typeof(ThenTestData), nameof(ThenTestData.LiftedNullSourceCases))]
    public async Task Null_source_tasks_throw_for_lifted_ThenDo_overloads(
        string operation,
        Action invoke,
        string parameterName
    )
    {
        await Assert.That(invoke).Throws<ArgumentNullException>().WithParameterName(parameterName);
    }

    [Test]
    [MethodDataSource(typeof(ThenTestData), nameof(ThenTestData.LiftedNullTaskCases))]
    public async Task Null_tasks_throw_for_lifted_async_ThenDo_overloads(
        string operation,
        Func<Task> invoke,
        string parameterName
    )
    {
        await Assert.That(invoke).Throws<ArgumentNullException>().WithParameterName(parameterName);
    }

    [Test]
    public async Task ThenDo_propagates_delegate_exceptions_for_task_wrapped_sources()
    {
        var expected = new InvalidOperationException("ThenDo task failed");

        try
        {
            _ = await Task.FromResult(FailOr.Success(1))
                .ThenDo(_ =>
                {
                    throw expected;
                });
            throw new Exception("Expected ThenDo to rethrow the original exception.");
        }
        catch (InvalidOperationException actual)
        {
            await Assert.That(ReferenceEquals(actual, expected)).IsTrue();
        }
    }

    [Test]
    public async Task ThenEnsure_propagates_delegate_exceptions_for_task_wrapped_sources()
    {
        var expected = new InvalidOperationException("ThenEnsure task failed");

        try
        {
            _ = await Task.FromResult(FailOr.Success(1))
                .ThenEnsure(
                    (Func<int, FailOr<bool>>)(
                        _ =>
                        {
                            throw expected;
                        }
                    )
                );
            throw new Exception("Expected ThenEnsure to rethrow the original exception.");
        }
        catch (InvalidOperationException actual)
        {
            await Assert.That(ReferenceEquals(actual, expected)).IsTrue();
        }
    }

    [Test]
    public async Task ThenDoAsync_propagates_delegate_exceptions_for_task_wrapped_sources()
    {
        var expected = new InvalidOperationException("ThenDoAsync task failed");

        try
        {
            _ = await Task.FromResult(FailOr.Success(1))
                .ThenDoAsync(_ => Task.FromException(expected));
            throw new Exception("Expected ThenDoAsync to rethrow the original exception.");
        }
        catch (InvalidOperationException actual)
        {
            await Assert.That(ReferenceEquals(actual, expected)).IsTrue();
        }
    }

    [Test]
    public async Task ThenEnsureAsync_propagates_delegate_exceptions_for_task_wrapped_sources()
    {
        var expected = new InvalidOperationException("ThenEnsureAsync task failed");

        try
        {
            _ = await Task.FromResult(FailOr.Success(1))
                .ThenEnsureAsync(
                    (Func<int, Task<FailOr<bool>>>)(_ => Task.FromException<FailOr<bool>>(expected))
                );
            throw new Exception("Expected ThenEnsureAsync to rethrow the original exception.");
        }
        catch (InvalidOperationException actual)
        {
            await Assert.That(ReferenceEquals(actual, expected)).IsTrue();
        }
    }

    [Test]
    [MethodDataSource(typeof(ThenTestData), nameof(ThenTestData.LiftedParityCases))]
    public async Task Lifted_overloads_match_direct_behavior(
        string operation,
        Func<FailOr<int>, Task<FailOr<int>>> direct,
        Func<Task<FailOr<int>>, Task<FailOr<int>>> lifted,
        int expected
    )
    {
        var source = FailOr.Success(1);

        var directResult = await direct(source);
        var liftedResult = await lifted(Task.FromResult(source));

        using var _ = Assert.Multiple();
        await ThenAssertions.AssertEquivalent(liftedResult, directResult);
        await ThenAssertions.AssertSuccess(liftedResult, expected);
    }

    [Test]
    [MethodDataSource(typeof(ThenTestData), nameof(ThenTestData.LiftedIfFailThenSuccessCases))]
    public async Task IfFailThen_preserves_success_for_all_lifted_overloads(
        string operation,
        Func<Task<FailOr<int>>, InvocationCounter, Task<FailOr<int>>> invoke,
        int expectedCalls
    )
    {
        var source = FailOr.Success(7);
        var counter = new InvocationCounter();

        var result = await invoke(Task.FromResult(source), counter);

        using var _ = Assert.Multiple();
        await Assert.That(counter.Calls).IsEqualTo(expectedCalls);
        await ThenAssertions.AssertEquivalent(result, source);
    }

    [Test]
    [MethodDataSource(typeof(ThenTestData), nameof(ThenTestData.LiftedIfFailThenFailureCases))]
    public async Task IfFailThen_uses_fallback_for_failed_lifted_sources(
        string operation,
        Func<Task<FailOr<int>>, InvocationCounter, Task<FailOr<int>>> invoke,
        int expectedCalls,
        int expectedValue
    )
    {
        var sourceTask = Task.FromResult(FailOr.Fail<int>(Failure.General($"{operation} failed")));
        var counter = new InvocationCounter();

        var result = await invoke(sourceTask, counter);

        using var _ = Assert.Multiple();
        await Assert.That(counter.Calls).IsEqualTo(expectedCalls);
        await ThenAssertions.AssertSuccess(result, expectedValue);
    }

    [Test]
    [MethodDataSource(
        typeof(ThenTestData),
        nameof(ThenTestData.LiftedIfFailThenFailuresAwareCases)
    )]
    public async Task IfFailThen_failures_aware_lifted_overloads_receive_the_original_failure_sequence(
        string operation,
        Func<Task<FailOr<int>>, Task<IReadOnlyList<Failures>>> capture
    )
    {
        var firstFailure = Failure.General($"{operation} first");
        var secondFailure = Failure.General($"{operation} second");
        var source = FailOr.Fail<int>(firstFailure, secondFailure);

        var observed = await capture(Task.FromResult(source));

        using var _ = Assert.Multiple();
        await Assert.That(observed.Count).IsEqualTo(2);
        await Assert.That(observed[0]).IsEqualTo(firstFailure);
        await Assert.That(observed[1]).IsEqualTo(secondFailure);
    }

    [Test]
    [MethodDataSource(typeof(ThenTestData), nameof(ThenTestData.LiftedIfFailThenNullSourceCases))]
    public async Task Null_source_tasks_throw_for_lifted_IfFailThen_overloads(
        string operation,
        Action invoke,
        string parameterName
    )
    {
        await Assert.That(invoke).Throws<ArgumentNullException>().WithParameterName(parameterName);
    }

    [Test]
    [MethodDataSource(typeof(ThenTestData), nameof(ThenTestData.LiftedIfFailThenNullSelectorCases))]
    public async Task Null_selectors_throw_for_lifted_IfFailThen_overloads(
        string operation,
        Action invoke,
        string parameterName
    )
    {
        await Assert.That(invoke).Throws<ArgumentNullException>().WithParameterName(parameterName);
    }

    [Test]
    [MethodDataSource(typeof(ThenTestData), nameof(ThenTestData.LiftedIfFailThenNullTaskCases))]
    public async Task Null_tasks_throw_for_lifted_async_IfFailThen_overloads(
        string operation,
        Func<Task> invoke,
        string parameterName
    )
    {
        await Assert.That(invoke).Throws<ArgumentNullException>().WithParameterName(parameterName);
    }

    [Test]
    [MethodDataSource(typeof(ThenTestData), nameof(ThenTestData.LiftedIfFailThenParityCases))]
    public async Task Lifted_IfFailThen_overloads_match_direct_behavior(
        string operation,
        FailOr<int> source,
        Func<FailOr<int>, Task<FailOr<int>>> direct,
        Func<Task<FailOr<int>>, Task<FailOr<int>>> lifted
    )
    {
        var directResult = await direct(source);
        var liftedResult = await lifted(Task.FromResult(source));

        await ThenAssertions.AssertEquivalent(liftedResult, directResult);
    }
}

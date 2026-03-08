using TUnit.Assertions;
using TUnit.Assertions.Extensions;
using TUnit.Core;

namespace FailOr.Tests;

public class FailOrTryTests
{
    [Test]
    [Arguments(1, 2)]
    [Arguments(5, 6)]
    public async Task Try_map_transforms_successful_values(int value, int expected)
    {
        var result = FailOr.Success(value).Try(x => x + 1);

        await ThenAssertions.AssertSuccess(result, expected);
    }

    [Test]
    [Arguments(1, 3)]
    [Arguments(5, 7)]
    public async Task Try_with_exception_handler_transforms_successful_values_without_invoking_handler(
        int value,
        int expected
    )
    {
        var calls = 0;

        var result = FailOr
            .Success(value)
            .Try(
                x => x + 2,
                _ =>
                {
                    calls++;
                    return 99;
                }
            );

        using var _ = Assert.Multiple();
        await Assert.That(calls).IsEqualTo(0);
        await ThenAssertions.AssertSuccess(result, expected);
    }

    [Test]
    [Arguments(1, 4)]
    [Arguments(5, 8)]
    public async Task TryAsync_map_transforms_successful_values(int value, int expected)
    {
        var result = await FailOr.Success(value).TryAsync(x => Task.FromResult(x + 3));

        await ThenAssertions.AssertSuccess(result, expected);
    }

    [Test]
    [Arguments(1, 5)]
    [Arguments(5, 9)]
    public async Task TryAsync_with_exception_handler_transforms_successful_values_without_invoking_handler(
        int value,
        int expected
    )
    {
        var calls = 0;

        var result = await FailOr
            .Success(value)
            .TryAsync(
                x => Task.FromResult(x + 4),
                _ =>
                {
                    calls++;
                    return 99;
                }
            );

        using var _ = Assert.Multiple();
        await Assert.That(calls).IsEqualTo(0);
        await ThenAssertions.AssertSuccess(result, expected);
    }

    [Test]
    [MethodDataSource(typeof(TryTestData), nameof(TryTestData.DirectFailureShortCircuitCases))]
    public async Task Failed_sources_short_circuit_all_direct_try_overloads(
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
    public async Task Try_converts_delegate_exceptions_to_exceptional_failures_by_default()
    {
        var expected = new InvalidOperationException("Try failed");
        Func<int, int> map = _ => throw expected;

        var result = FailOr.Success(1).Try(map);

        await TryAssertions.AssertExceptionalFailure(result, expected);
    }

    [Test]
    public async Task TryAsync_converts_delegate_exceptions_to_exceptional_failures_by_default()
    {
        var expected = new InvalidOperationException("TryAsync failed");
        Func<int, Task<int>> mapAsync = _ => Task.FromException<int>(expected);

        var result = await FailOr.Success(1).TryAsync(mapAsync);

        await TryAssertions.AssertExceptionalFailure(result, expected);
    }

    [Test]
    public async Task Try_uses_custom_exception_mapping()
    {
        var expected = new InvalidOperationException("Try custom failed");
        var customFailure = Failure.General("mapping failed");
        Exception? observed = null;
        Func<int, int> map = _ => throw expected;

        var result = FailOr
            .Success(1)
            .Try(
                map,
                exception =>
                {
                    observed = exception;
                    return customFailure;
                }
            );

        using var _ = Assert.Multiple();
        await Assert.That(ReferenceEquals(observed, expected)).IsTrue();
        await ThenAssertions.AssertFailure(result, customFailure);
    }

    [Test]
    public async Task TryAsync_uses_custom_exception_mapping()
    {
        var expected = new InvalidOperationException("TryAsync custom failed");
        var customFailure = Failure.General("mapping failed");
        Exception? observed = null;
        Func<int, Task<int>> mapAsync = _ => Task.FromException<int>(expected);

        var result = await FailOr
            .Success(1)
            .TryAsync(
                mapAsync,
                exception =>
                {
                    observed = exception;
                    return customFailure;
                }
            );

        using var _ = Assert.Multiple();
        await Assert.That(ReferenceEquals(observed, expected)).IsTrue();
        await ThenAssertions.AssertFailure(result, customFailure);
    }

    [Test]
    [MethodDataSource(typeof(TryTestData), nameof(TryTestData.DirectNullSelectorCases))]
    public async Task Null_selectors_throw_for_direct_try_overloads(
        string operation,
        Action invoke,
        string parameterName
    )
    {
        await Assert.That(invoke).Throws<ArgumentNullException>().WithParameterName(parameterName);
    }

    [Test]
    [MethodDataSource(typeof(TryTestData), nameof(TryTestData.DirectNullOnExceptionCases))]
    public async Task Null_on_exception_handlers_throw_for_direct_try_overloads(
        string operation,
        Action invoke,
        string parameterName
    )
    {
        await Assert.That(invoke).Throws<ArgumentNullException>().WithParameterName(parameterName);
    }

    [Test]
    [MethodDataSource(typeof(TryTestData), nameof(TryTestData.DirectNullTaskCases))]
    public async Task Null_tasks_throw_for_direct_async_try_overloads(
        string operation,
        Func<Task> invoke,
        string parameterName
    )
    {
        await Assert.That(invoke).Throws<ArgumentNullException>().WithParameterName(parameterName);
    }
}

using TUnit.Assertions;
using TUnit.Assertions.Extensions;
using TUnit.Core;

namespace FailOr.Tests;

public class FailOrThenTests
{
    [Test]
    [Arguments(1, 2)]
    [Arguments(5, 6)]
    public async Task Then_map_transforms_successful_values(int value, int expected)
    {
        var result = FailOr.Success(value).Then(x => x + 1);

        await ThenAssertions.AssertSuccess(result, expected);
    }

    [Test]
    [Arguments(1, 3)]
    [Arguments(5, 15)]
    public async Task Then_bind_returns_inner_result(int value, int expected)
    {
        var result = FailOr.Success(value).Then(x => FailOr.Success(x * 3));

        await ThenAssertions.AssertSuccess(result, expected);
    }

    [Test]
    [Arguments(1, 4)]
    [Arguments(5, 8)]
    public async Task ThenAsync_map_transforms_successful_values(int value, int expected)
    {
        var result = await FailOr.Success(value).ThenAsync(x => Task.FromResult(x + 3));

        await ThenAssertions.AssertSuccess(result, expected);
    }

    [Test]
    [Arguments(1, 5)]
    [Arguments(5, 9)]
    public async Task ThenAsync_bind_returns_inner_result(int value, int expected)
    {
        var result = await FailOr
            .Success(value)
            .ThenAsync(x => Task.FromResult(FailOr.Success(x + 4)));

        await ThenAssertions.AssertSuccess(result, expected);
    }

    [Test]
    [MethodDataSource(typeof(ThenTestData), nameof(ThenTestData.DirectFailureShortCircuitCases))]
    public async Task Failed_sources_short_circuit_all_direct_overloads(
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
    [MethodDataSource(typeof(ThenTestData), nameof(ThenTestData.DirectNullSelectorCases))]
    public async Task Null_selectors_throw_for_direct_overloads(
        string operation,
        Action invoke,
        string parameterName
    )
    {
        await Assert.That(invoke).Throws<ArgumentNullException>().WithParameterName(parameterName);
    }

    [Test]
    [MethodDataSource(typeof(ThenTestData), nameof(ThenTestData.DirectIfFailThenSuccessCases))]
    public async Task IfFailThen_preserves_success_for_all_direct_overloads(
        string operation,
        Func<FailOr<int>, InvocationCounter, Task<FailOr<int>>> invoke,
        int expectedCalls
    )
    {
        var source = FailOr.Success(7);
        var counter = new InvocationCounter();

        var result = await invoke(source, counter);

        using var _ = Assert.Multiple();
        await Assert.That(counter.Calls).IsEqualTo(expectedCalls);
        await ThenAssertions.AssertEquivalent(result, source);
    }

    [Test]
    [MethodDataSource(typeof(ThenTestData), nameof(ThenTestData.DirectIfFailThenFailureCases))]
    public async Task IfFailThen_uses_fallback_for_failed_direct_sources(
        string operation,
        Func<FailOr<int>, InvocationCounter, Task<FailOr<int>>> invoke,
        int expectedCalls,
        int expectedValue
    )
    {
        var source = FailOr.Fail<int>(Failure.General($"{operation} failed"));
        var counter = new InvocationCounter();

        var result = await invoke(source, counter);

        using var _ = Assert.Multiple();
        await Assert.That(counter.Calls).IsEqualTo(expectedCalls);
        await ThenAssertions.AssertSuccess(result, expectedValue);
    }

    [Test]
    [MethodDataSource(
        typeof(ThenTestData),
        nameof(ThenTestData.DirectIfFailThenFailuresAwareCases)
    )]
    public async Task IfFailThen_failures_aware_direct_overloads_receive_the_original_failure_sequence(
        string operation,
        Func<FailOr<int>, Task<IReadOnlyList<Failures>>> capture
    )
    {
        var firstFailure = Failure.General($"{operation} first");
        var secondFailure = Failure.General($"{operation} second");
        var source = FailOr.Fail<int>(firstFailure, secondFailure);

        var observed = await capture(source);

        using var _ = Assert.Multiple();
        await Assert.That(observed.Count).IsEqualTo(2);
        await Assert.That(observed[0]).IsEqualTo(firstFailure);
        await Assert.That(observed[1]).IsEqualTo(secondFailure);
    }

    [Test]
    [MethodDataSource(typeof(ThenTestData), nameof(ThenTestData.DirectIfFailThenNullSelectorCases))]
    public async Task Null_selectors_throw_for_direct_IfFailThen_overloads(
        string operation,
        Action invoke,
        string parameterName
    )
    {
        await Assert.That(invoke).Throws<ArgumentNullException>().WithParameterName(parameterName);
    }

    [Test]
    [MethodDataSource(typeof(ThenTestData), nameof(ThenTestData.DirectIfFailThenNullTaskCases))]
    public async Task Null_tasks_throw_for_direct_async_IfFailThen_overloads(
        string operation,
        Func<Task> invoke,
        string parameterName
    )
    {
        await Assert.That(invoke).Throws<ArgumentNullException>().WithParameterName(parameterName);
    }

    [Test]
    public async Task IfFailThen_can_preserve_the_original_failure()
    {
        var failure = Failure.General("same failure");
        var source = FailOr.Fail<int>(failure);

        var result = source.IfFailThen(FailOr.Fail<int>(failure));

        await ThenAssertions.AssertFailure(result, failure);
    }

    [Test]
    public async Task IfFailThen_can_replace_the_original_failure()
    {
        var sourceFailure = Failure.General("source failure");
        var fallbackFailure = Failure.General("fallback failure");
        var source = FailOr.Fail<int>(sourceFailure);

        var result = source.IfFailThen(() => FailOr.Fail<int>(fallbackFailure));

        await ThenAssertions.AssertFailure(result, fallbackFailure);
    }
}

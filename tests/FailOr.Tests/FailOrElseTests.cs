using TUnit.Assertions;
using TUnit.Assertions.Extensions;
using TUnit.Core;

namespace FailOr.Tests;

public class FailOrElseTests
{
    [Test]
    [MethodDataSource(typeof(ElseTestData), nameof(ElseTestData.DirectSuccessCases))]
    public async Task Else_preserves_success_for_all_direct_overloads(
        string operation,
        Func<FailOr<int>, ElseInvocationCounter, Task<int>> invoke
    )
    {
        var source = FailOr.Success(7);
        var counter = new ElseInvocationCounter();

        var result = await invoke(source, counter);

        using var _ = Assert.Multiple();
        await Assert.That(counter.Calls).IsEqualTo(0);
        await Assert.That(result).IsEqualTo(7);
    }

    [Test]
    [MethodDataSource(typeof(ElseTestData), nameof(ElseTestData.DirectFailureCases))]
    public async Task Else_uses_fallback_for_failed_direct_sources(
        string operation,
        Func<FailOr<int>, ElseInvocationCounter, Task<int>> invoke,
        int expectedValue,
        int expectedCalls
    )
    {
        var source = FailOr.Fail<int>(Failure.General($"{operation} failed"));
        var counter = new ElseInvocationCounter();

        var result = await invoke(source, counter);

        using var _ = Assert.Multiple();
        await Assert.That(counter.Calls).IsEqualTo(expectedCalls);
        await Assert.That(result).IsEqualTo(expectedValue);
    }

    [Test]
    [MethodDataSource(typeof(ElseTestData), nameof(ElseTestData.DirectFailuresAwareCases))]
    public async Task Else_failures_aware_direct_overloads_receive_the_original_failure_sequence(
        string operation,
        Func<FailOr<int>, Task<IReadOnlyList<Failures>>> capture
    )
    {
        var firstFailure = Failure.General($"{operation} first");
        var secondFailure = Failure.Validation("Value", $"{operation} second");
        var source = FailOr.Fail<int>(firstFailure, secondFailure);

        var observed = await capture(source);

        using var _ = Assert.Multiple();
        await Assert.That(observed.Count).IsEqualTo(2);
        await Assert.That(observed[0]).IsEqualTo(firstFailure);
        await Assert.That(observed[1]).IsEqualTo(secondFailure);
    }

    [Test]
    [MethodDataSource(typeof(ElseTestData), nameof(ElseTestData.DirectNullSelectorCases))]
    public async Task Null_selectors_throw_for_direct_Else_overloads(
        string operation,
        Action invoke,
        string parameterName
    )
    {
        await Assert.That(invoke).Throws<ArgumentNullException>().WithParameterName(parameterName);
    }

    [Test]
    [MethodDataSource(typeof(ElseTestData), nameof(ElseTestData.DirectNullTaskCases))]
    public async Task Null_tasks_throw_for_direct_async_Else_overloads(
        string operation,
        Func<Task> invoke,
        string parameterName
    )
    {
        await Assert.That(invoke).Throws<ArgumentNullException>().WithParameterName(parameterName);
    }
}

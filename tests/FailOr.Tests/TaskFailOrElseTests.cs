using TUnit.Assertions;
using TUnit.Assertions.Extensions;
using TUnit.Core;

namespace FailOr.Tests;

public class TaskFailOrElseTests
{
    [Test]
    [MethodDataSource(typeof(ElseTestData), nameof(ElseTestData.LiftedSuccessCases))]
    public async Task Else_preserves_success_for_all_lifted_overloads(
        string operation,
        Func<Task<FailOr<int>>, ElseInvocationCounter, Task<int>> invoke
    )
    {
        var source = FailOr.Success(7);
        var counter = new ElseInvocationCounter();

        var result = await invoke(Task.FromResult(source), counter);

        using var _ = Assert.Multiple();
        await Assert.That(counter.Calls).IsEqualTo(0);
        await Assert.That(result).IsEqualTo(7);
    }

    [Test]
    [MethodDataSource(typeof(ElseTestData), nameof(ElseTestData.LiftedFailureCases))]
    public async Task Else_uses_fallback_for_failed_lifted_sources(
        string operation,
        Func<Task<FailOr<int>>, ElseInvocationCounter, Task<int>> invoke,
        int expectedValue,
        int expectedCalls
    )
    {
        var sourceTask = Task.FromResult(FailOr.Fail<int>(Failure.General($"{operation} failed")));
        var counter = new ElseInvocationCounter();

        var result = await invoke(sourceTask, counter);

        using var _ = Assert.Multiple();
        await Assert.That(counter.Calls).IsEqualTo(expectedCalls);
        await Assert.That(result).IsEqualTo(expectedValue);
    }

    [Test]
    [MethodDataSource(typeof(ElseTestData), nameof(ElseTestData.LiftedFailuresAwareCases))]
    public async Task Else_failures_aware_lifted_overloads_receive_the_original_failure_sequence(
        string operation,
        Func<Task<FailOr<int>>, Task<IReadOnlyList<Failures>>> capture
    )
    {
        var firstFailure = Failure.General($"{operation} first");
        var secondFailure = Failure.Validation("Value", $"{operation} second");
        var source = FailOr.Fail<int>(firstFailure, secondFailure);

        var observed = await capture(Task.FromResult(source));

        using var _ = Assert.Multiple();
        await Assert.That(observed.Count).IsEqualTo(2);
        await Assert.That(observed[0]).IsEqualTo(firstFailure);
        await Assert.That(observed[1]).IsEqualTo(secondFailure);
    }

    [Test]
    [MethodDataSource(typeof(ElseTestData), nameof(ElseTestData.LiftedNullSourceCases))]
    public async Task Null_source_tasks_throw_for_lifted_Else_overloads(
        string operation,
        Action invoke,
        string parameterName
    )
    {
        await Assert.That(invoke).Throws<ArgumentNullException>().WithParameterName(parameterName);
    }

    [Test]
    [MethodDataSource(typeof(ElseTestData), nameof(ElseTestData.LiftedNullSelectorCases))]
    public async Task Null_selectors_throw_for_lifted_Else_overloads(
        string operation,
        Action invoke,
        string parameterName
    )
    {
        await Assert.That(invoke).Throws<ArgumentNullException>().WithParameterName(parameterName);
    }

    [Test]
    [MethodDataSource(typeof(ElseTestData), nameof(ElseTestData.LiftedNullTaskCases))]
    public async Task Null_tasks_throw_for_lifted_async_Else_overloads(
        string operation,
        Func<Task> invoke,
        string parameterName
    )
    {
        await Assert.That(invoke).Throws<ArgumentNullException>().WithParameterName(parameterName);
    }

    [Test]
    [MethodDataSource(typeof(ElseTestData), nameof(ElseTestData.LiftedParityCases))]
    public async Task Lifted_Else_overloads_match_direct_behavior(
        string operation,
        FailOr<int> source,
        Func<FailOr<int>, Task<int>> direct,
        Func<Task<FailOr<int>>, Task<int>> lifted,
        int expectedValue
    )
    {
        var directResult = await direct(source);
        var liftedResult = await lifted(Task.FromResult(source));

        using var _ = Assert.Multiple();
        await Assert.That(liftedResult).IsEqualTo(directResult);
        await Assert.That(liftedResult).IsEqualTo(expectedValue);
    }
}

using TUnit.Assertions;
using TUnit.Assertions.Extensions;
using TUnit.Core;

namespace FailOr.Tests;

public class FailOrMatchTests
{
    [Test]
    [MethodDataSource(typeof(MatchTestData), nameof(MatchTestData.DirectMatchSuccessCases))]
    public async Task Match_overloads_return_success_projection_for_successful_sources(
        string operation,
        Func<FailOr<int>, MatchBranchTracker, Task<int>> invoke,
        int expected
    )
    {
        var source = FailOr.Success(1);
        var tracker = new MatchBranchTracker();

        var result = await invoke(source, tracker);

        using var _ = Assert.Multiple();
        await Assert.That(result).IsEqualTo(expected);
        await MatchAssertions.AssertSuccessBranch(tracker);
    }

    [Test]
    [MethodDataSource(typeof(MatchTestData), nameof(MatchTestData.DirectMatchFailureCases))]
    public async Task Match_overloads_return_failure_projection_with_all_failures(
        string operation,
        Func<FailOr<int>, MatchBranchTracker, Task<int>> invoke,
        int expected
    )
    {
        var firstFailure = Failure.General("first");
        var secondFailure = Failure.General("second");
        var source = FailOr.Fail<int>(firstFailure, secondFailure);
        var tracker = new MatchBranchTracker();

        var result = await invoke(source, tracker);

        using var _ = Assert.Multiple();
        await Assert.That(result).IsEqualTo(expected);
        await MatchAssertions.AssertFailureListBranch(tracker, firstFailure, secondFailure);
    }

    [Test]
    [MethodDataSource(typeof(MatchTestData), nameof(MatchTestData.DirectMatchFirstSuccessCases))]
    public async Task MatchFirst_overloads_return_success_projection_for_successful_sources(
        string operation,
        Func<FailOr<int>, MatchBranchTracker, Task<int>> invoke,
        int expected
    )
    {
        var source = FailOr.Success(1);
        var tracker = new MatchBranchTracker();

        var result = await invoke(source, tracker);

        using var _ = Assert.Multiple();
        await Assert.That(result).IsEqualTo(expected);
        await MatchAssertions.AssertSuccessBranch(tracker);
    }

    [Test]
    [MethodDataSource(typeof(MatchTestData), nameof(MatchTestData.DirectMatchFirstFailureCases))]
    public async Task MatchFirst_overloads_return_failure_projection_with_first_failure(
        string operation,
        Func<FailOr<int>, MatchBranchTracker, Task<int>> invoke,
        int expected
    )
    {
        var firstFailure = Failure.General("first");
        var secondFailure = Failure.General("second");
        var source = FailOr.Fail<int>(firstFailure, secondFailure);
        var tracker = new MatchBranchTracker();

        var result = await invoke(source, tracker);

        using var _ = Assert.Multiple();
        await Assert.That(result).IsEqualTo(expected);
        await MatchAssertions.AssertFailureFirstBranch(tracker, firstFailure);
    }

    [Test]
    [MethodDataSource(typeof(MatchTestData), nameof(MatchTestData.DirectNullSelectorCases))]
    public async Task Null_selectors_throw_for_direct_match_overloads(
        string operation,
        Action invoke,
        string parameterName
    )
    {
        await Assert.That(invoke).Throws<ArgumentNullException>().WithParameterName(parameterName);
    }

    [Test]
    [MethodDataSource(typeof(MatchTestData), nameof(MatchTestData.DirectNullReturnedTaskCases))]
    public async Task Null_selected_tasks_throw_for_direct_async_match_overloads(
        string operation,
        Action invoke
    )
    {
        await Assert.That(invoke).Throws<ArgumentNullException>();
    }
}

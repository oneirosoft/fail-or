using TUnit.Assertions;
using TUnit.Assertions.Extensions;
using TUnit.Core;

namespace FailOr.Tests;

public class TaskFailOrMatchTests
{
    [Test]
    [MethodDataSource(typeof(MatchTestData), nameof(MatchTestData.LiftedMatchSuccessCases))]
    public async Task Lifted_match_overloads_match_direct_success_behavior(
        string operation,
        Func<FailOr<int>, MatchBranchTracker, Task<int>> direct,
        Func<Task<FailOr<int>>, MatchBranchTracker, Task<int>> lifted,
        int expected
    )
    {
        var source = FailOr.Success(1);
        var directTracker = new MatchBranchTracker();
        var liftedTracker = new MatchBranchTracker();

        var directResult = await direct(source, directTracker);
        var liftedResult = await lifted(Task.FromResult(source), liftedTracker);

        using var _ = Assert.Multiple();
        await Assert.That(directResult).IsEqualTo(expected);
        await Assert.That(liftedResult).IsEqualTo(directResult);
        await MatchAssertions.AssertEquivalent(liftedTracker, directTracker);
        await MatchAssertions.AssertSuccessBranch(liftedTracker);
    }

    [Test]
    [MethodDataSource(typeof(MatchTestData), nameof(MatchTestData.LiftedMatchFailureCases))]
    public async Task Lifted_match_overloads_match_direct_failure_behavior(
        string operation,
        Func<FailOr<int>, MatchBranchTracker, Task<int>> direct,
        Func<Task<FailOr<int>>, MatchBranchTracker, Task<int>> lifted,
        int expected
    )
    {
        var firstFailure = Failure.General("first");
        var secondFailure = Failure.General("second");
        var source = FailOr.Fail<int>(firstFailure, secondFailure);
        var directTracker = new MatchBranchTracker();
        var liftedTracker = new MatchBranchTracker();

        var directResult = await direct(source, directTracker);
        var liftedResult = await lifted(Task.FromResult(source), liftedTracker);

        using var _ = Assert.Multiple();
        await Assert.That(directResult).IsEqualTo(expected);
        await Assert.That(liftedResult).IsEqualTo(directResult);
        await MatchAssertions.AssertEquivalent(liftedTracker, directTracker);
        await MatchAssertions.AssertFailureListBranch(liftedTracker, firstFailure, secondFailure);
    }

    [Test]
    [MethodDataSource(typeof(MatchTestData), nameof(MatchTestData.LiftedMatchFirstSuccessCases))]
    public async Task Lifted_match_first_overloads_match_direct_success_behavior(
        string operation,
        Func<FailOr<int>, MatchBranchTracker, Task<int>> direct,
        Func<Task<FailOr<int>>, MatchBranchTracker, Task<int>> lifted,
        int expected
    )
    {
        var source = FailOr.Success(1);
        var directTracker = new MatchBranchTracker();
        var liftedTracker = new MatchBranchTracker();

        var directResult = await direct(source, directTracker);
        var liftedResult = await lifted(Task.FromResult(source), liftedTracker);

        using var _ = Assert.Multiple();
        await Assert.That(directResult).IsEqualTo(expected);
        await Assert.That(liftedResult).IsEqualTo(directResult);
        await MatchAssertions.AssertEquivalent(liftedTracker, directTracker);
        await MatchAssertions.AssertSuccessBranch(liftedTracker);
    }

    [Test]
    [MethodDataSource(typeof(MatchTestData), nameof(MatchTestData.LiftedMatchFirstFailureCases))]
    public async Task Lifted_match_first_overloads_match_direct_failure_behavior(
        string operation,
        Func<FailOr<int>, MatchBranchTracker, Task<int>> direct,
        Func<Task<FailOr<int>>, MatchBranchTracker, Task<int>> lifted,
        int expected
    )
    {
        var firstFailure = Failure.General("first");
        var secondFailure = Failure.General("second");
        var source = FailOr.Fail<int>(firstFailure, secondFailure);
        var directTracker = new MatchBranchTracker();
        var liftedTracker = new MatchBranchTracker();

        var directResult = await direct(source, directTracker);
        var liftedResult = await lifted(Task.FromResult(source), liftedTracker);

        using var _ = Assert.Multiple();
        await Assert.That(directResult).IsEqualTo(expected);
        await Assert.That(liftedResult).IsEqualTo(directResult);
        await MatchAssertions.AssertEquivalent(liftedTracker, directTracker);
        await MatchAssertions.AssertFailureFirstBranch(liftedTracker, firstFailure);
    }

    [Test]
    [MethodDataSource(typeof(MatchTestData), nameof(MatchTestData.LiftedNullSelectorCases))]
    public async Task Null_selectors_throw_for_lifted_match_overloads(
        string operation,
        Action invoke,
        string parameterName
    )
    {
        await Assert.That(invoke).Throws<ArgumentNullException>().WithParameterName(parameterName);
    }

    [Test]
    [MethodDataSource(typeof(MatchTestData), nameof(MatchTestData.LiftedNullReturnedTaskCases))]
    public async Task Null_selected_tasks_throw_for_lifted_async_match_overloads(
        string operation,
        Func<Task> invoke
    )
    {
        await Assert.That(invoke).Throws<ArgumentNullException>();
    }
}

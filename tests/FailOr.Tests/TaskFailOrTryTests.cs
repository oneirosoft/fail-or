using TUnit.Assertions;
using TUnit.Assertions.Extensions;
using TUnit.Core;

namespace FailOr.Tests;

public class TaskFailOrTryTests
{
    [Test]
    [MethodDataSource(typeof(TryTestData), nameof(TryTestData.LiftedParityCases))]
    public async Task Lifted_try_overloads_match_direct_behavior(
        string operation,
        FailOr<int> source,
        Func<FailOr<int>, Task<FailOr<int>>> direct,
        Func<Task<FailOr<int>>, Task<FailOr<int>>> lifted,
        FailOr<int> expected
    )
    {
        var directResult = await direct(source);
        var liftedResult = await lifted(Task.FromResult(source));

        using var _ = Assert.Multiple();
        await ThenAssertions.AssertEquivalent(directResult, expected);
        await ThenAssertions.AssertEquivalent(liftedResult, expected);
    }

    [Test]
    [MethodDataSource(typeof(TryTestData), nameof(TryTestData.LiftedNullSourceCases))]
    public async Task Null_source_tasks_throw_for_lifted_try_overloads(
        string operation,
        Action invoke,
        string parameterName
    )
    {
        await Assert.That(invoke).Throws<ArgumentNullException>().WithParameterName(parameterName);
    }

    [Test]
    [MethodDataSource(typeof(TryTestData), nameof(TryTestData.LiftedNullSelectorCases))]
    public async Task Null_selectors_throw_for_lifted_try_overloads(
        string operation,
        Action invoke,
        string parameterName
    )
    {
        await Assert.That(invoke).Throws<ArgumentNullException>().WithParameterName(parameterName);
    }

    [Test]
    [MethodDataSource(typeof(TryTestData), nameof(TryTestData.LiftedNullOnExceptionCases))]
    public async Task Null_on_exception_handlers_throw_for_lifted_try_overloads(
        string operation,
        Action invoke,
        string parameterName
    )
    {
        await Assert.That(invoke).Throws<ArgumentNullException>().WithParameterName(parameterName);
    }

    [Test]
    [MethodDataSource(typeof(TryTestData), nameof(TryTestData.LiftedNullTaskCases))]
    public async Task Null_tasks_throw_for_lifted_async_try_overloads(
        string operation,
        Func<Task> invoke,
        string parameterName
    )
    {
        await Assert.That(invoke).Throws<ArgumentNullException>().WithParameterName(parameterName);
    }
}

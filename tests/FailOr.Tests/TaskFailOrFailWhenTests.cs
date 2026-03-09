using TUnit.Assertions;
using TUnit.Assertions.Extensions;
using TUnit.Core;

namespace FailOr.Tests;

public class TaskFailOrFailWhenTests
{
    [Test]
    [MethodDataSource(typeof(FailWhenTestData), nameof(FailWhenTestData.LiftedParityCases))]
    public async Task Lifted_FailWhen_overloads_match_direct_behavior(
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
    [MethodDataSource(
        typeof(FailWhenTestData),
        nameof(FailWhenTestData.LiftedFailureShortCircuitCases)
    )]
    public async Task Failed_sources_short_circuit_all_lifted_FailWhen_overloads(
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
    [MethodDataSource(typeof(FailWhenTestData), nameof(FailWhenTestData.LiftedNullSourceCases))]
    public async Task Null_source_tasks_throw_for_lifted_FailWhen_overloads(
        string operation,
        Action invoke,
        string parameterName
    )
    {
        await Assert.That(invoke).Throws<ArgumentNullException>().WithParameterName(parameterName);
    }

    [Test]
    [MethodDataSource(typeof(FailWhenTestData), nameof(FailWhenTestData.LiftedNullSelectorCases))]
    public async Task Null_predicates_throw_for_lifted_FailWhen_overloads(
        string operation,
        Action invoke,
        string parameterName
    )
    {
        await Assert.That(invoke).Throws<ArgumentNullException>().WithParameterName(parameterName);
    }

    [Test]
    [MethodDataSource(typeof(FailWhenTestData), nameof(FailWhenTestData.LiftedNullFailureCases))]
    public async Task Null_failures_throw_for_lifted_FailWhen_overloads(
        string operation,
        Action invoke,
        string parameterName
    )
    {
        await Assert.That(invoke).Throws<ArgumentNullException>().WithParameterName(parameterName);
    }

    [Test]
    [MethodDataSource(typeof(FailWhenTestData), nameof(FailWhenTestData.LiftedNullTaskCases))]
    public async Task Null_tasks_throw_for_lifted_async_FailWhen_overloads(
        string operation,
        Func<Task> invoke,
        string parameterName
    )
    {
        await Assert.That(invoke).Throws<ArgumentNullException>().WithParameterName(parameterName);
    }

    [Test]
    public async Task FailWhen_propagates_delegate_exceptions_for_task_wrapped_sources()
    {
        var expected = new InvalidOperationException("FailWhen task failed");
        Func<int, bool> predicate = _ => throw expected;

        try
        {
            _ = await Task.FromResult(FailOr.Success(1))
                .FailWhen(predicate, Failure.General("predicate matched"));
            throw new Exception("Expected FailWhen to rethrow the original exception.");
        }
        catch (InvalidOperationException actual)
        {
            await Assert.That(ReferenceEquals(actual, expected)).IsTrue();
        }
    }

    [Test]
    public async Task FailWhenAsync_propagates_delegate_exceptions_for_task_wrapped_sources()
    {
        var expected = new InvalidOperationException("FailWhenAsync task failed");
        Func<int, Task<bool>> predicateAsync = _ => Task.FromException<bool>(expected);

        try
        {
            _ = await Task.FromResult(FailOr.Success(1))
                .FailWhenAsync(predicateAsync, Failure.General("predicate matched"));
            throw new Exception("Expected FailWhenAsync to rethrow the original exception.");
        }
        catch (InvalidOperationException actual)
        {
            await Assert.That(ReferenceEquals(actual, expected)).IsTrue();
        }
    }
}

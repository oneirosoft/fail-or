using TUnit.Assertions;
using TUnit.Assertions.Extensions;
using TUnit.Core;

namespace FailOr.Tests;

public class TaskFailOrIfFailTests
{
    [Test]
    [MethodDataSource(typeof(IfFailTestData), nameof(IfFailTestData.LiftedSuccessCases))]
    public async Task IfFail_does_not_invoke_delegates_for_successful_lifted_sources(
        string operation,
        Func<Task<FailOr<int>>, IfFailInvocationCounter, Task> invoke
    )
    {
        var source = FailOr.Success(7);
        var counter = new IfFailInvocationCounter();

        await invoke(Task.FromResult(source), counter);

        await Assert.That(counter.Calls).IsEqualTo(0);
    }

    [Test]
    [MethodDataSource(typeof(IfFailTestData), nameof(IfFailTestData.LiftedFailureCases))]
    public async Task IfFail_invokes_delegates_once_for_failed_lifted_sources(
        string operation,
        Func<Task<FailOr<int>>, IfFailInvocationCounter, Task> invoke,
        int expectedCalls
    )
    {
        var sourceTask = Task.FromResult(FailOr.Fail<int>(Failure.General($"{operation} failed")));
        var counter = new IfFailInvocationCounter();

        await invoke(sourceTask, counter);

        await Assert.That(counter.Calls).IsEqualTo(expectedCalls);
    }

    [Test]
    [MethodDataSource(typeof(IfFailTestData), nameof(IfFailTestData.LiftedFailuresAwareCases))]
    public async Task IfFail_lifted_overloads_receive_the_original_failure_sequence(
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
    [MethodDataSource(typeof(IfFailTestData), nameof(IfFailTestData.LiftedNullSourceCases))]
    public async Task Null_source_tasks_throw_for_lifted_IfFail_overloads(
        string operation,
        Action invoke,
        string parameterName
    )
    {
        await Assert.That(invoke).Throws<ArgumentNullException>().WithParameterName(parameterName);
    }

    [Test]
    [MethodDataSource(typeof(IfFailTestData), nameof(IfFailTestData.LiftedNullSelectorCases))]
    public async Task Null_selectors_throw_for_lifted_IfFail_overloads(
        string operation,
        Action invoke,
        string parameterName
    )
    {
        await Assert.That(invoke).Throws<ArgumentNullException>().WithParameterName(parameterName);
    }

    [Test]
    [MethodDataSource(typeof(IfFailTestData), nameof(IfFailTestData.LiftedNullTaskCases))]
    public async Task Null_tasks_throw_for_lifted_async_IfFail_overloads(
        string operation,
        Func<Task> invoke,
        string parameterName
    )
    {
        await Assert.That(invoke).Throws<ArgumentNullException>().WithParameterName(parameterName);
    }

    [Test]
    [MethodDataSource(typeof(IfFailTestData), nameof(IfFailTestData.ParityCases))]
    public async Task Lifted_IfFail_overloads_match_direct_behavior(
        string operation,
        FailOr<int> source,
        Func<FailOr<int>, IfFailInvocationCounter, Task> direct,
        Func<Task<FailOr<int>>, IfFailInvocationCounter, Task> lifted
    )
    {
        var directCounter = new IfFailInvocationCounter();
        var liftedCounter = new IfFailInvocationCounter();

        await direct(source, directCounter);
        await lifted(Task.FromResult(source), liftedCounter);

        await Assert.That(liftedCounter.Calls).IsEqualTo(directCounter.Calls);
    }

    [Test]
    public async Task IfFail_propagates_delegate_exceptions_for_task_wrapped_sources()
    {
        var expected = new InvalidOperationException("IfFail task failed");

        try
        {
            await Task.FromResult(FailOr.Fail<int>(Failure.General("failed")))
                .IfFail(_ => throw expected);
            throw new Exception("Expected IfFail to rethrow the original exception.");
        }
        catch (InvalidOperationException actual)
        {
            await Assert.That(ReferenceEquals(actual, expected)).IsTrue();
        }
    }

    [Test]
    public async Task IfFailAsync_propagates_delegate_exceptions_for_task_wrapped_sources()
    {
        var expected = new InvalidOperationException("IfFailAsync task failed");

        try
        {
            await Task.FromResult(FailOr.Fail<int>(Failure.General("failed")))
                .IfFailAsync(_ => Task.FromException(expected));
            throw new Exception("Expected IfFailAsync to rethrow the original exception.");
        }
        catch (InvalidOperationException actual)
        {
            await Assert.That(ReferenceEquals(actual, expected)).IsTrue();
        }
    }
}

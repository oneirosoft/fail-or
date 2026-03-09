using TUnit.Assertions;
using TUnit.Assertions.Extensions;
using TUnit.Core;

namespace FailOr.Tests;

public class TaskFailOrIfSuccessTests
{
    [Test]
    [Arguments(1)]
    [Arguments(5)]
    public async Task IfSuccess_invokes_delegate_once_with_success_task_value(int value)
    {
        var calls = 0;
        var observed = 0;

        await Task.FromResult(FailOr.Success(value))
            .IfSuccess(x =>
            {
                observed = x;
                calls++;
            });

        using var _ = Assert.Multiple();
        await Assert.That(calls).IsEqualTo(1);
        await Assert.That(observed).IsEqualTo(value);
    }

    [Test]
    [Arguments(1)]
    [Arguments(5)]
    public async Task IfSuccessAsync_invokes_delegate_once_with_success_task_value(int value)
    {
        var calls = 0;
        var observed = 0;

        await Task.FromResult(FailOr.Success(value))
            .IfSuccessAsync(x =>
            {
                observed = x;
                calls++;
                return Task.CompletedTask;
            });

        using var _ = Assert.Multiple();
        await Assert.That(calls).IsEqualTo(1);
        await Assert.That(observed).IsEqualTo(value);
    }

    [Test]
    [MethodDataSource(
        typeof(IfSuccessTestData),
        nameof(IfSuccessTestData.LiftedFailureShortCircuitCases)
    )]
    public async Task Failed_sources_do_not_invoke_lifted_overloads(
        string operation,
        Func<Task<FailOr<int>>, InvocationCounter, Task> invoke
    )
    {
        var failure = Failure.General($"{operation} failed");
        var sourceTask = Task.FromResult(FailOr.Fail<int>(failure));
        var counter = new InvocationCounter();

        await invoke(sourceTask, counter);

        await Assert.That(counter.Calls).IsEqualTo(0);
    }

    [Test]
    [MethodDataSource(typeof(IfSuccessTestData), nameof(IfSuccessTestData.LiftedNullDelegateCases))]
    public async Task Null_delegates_throw_for_lifted_overloads(
        string operation,
        Action invoke,
        string parameterName
    )
    {
        await Assert.That(invoke).Throws<ArgumentNullException>().WithParameterName(parameterName);
    }

    [Test]
    [MethodDataSource(typeof(IfSuccessTestData), nameof(IfSuccessTestData.LiftedNullSourceCases))]
    public async Task Null_source_tasks_throw_for_lifted_overloads(
        string operation,
        Action invoke,
        string parameterName
    )
    {
        await Assert.That(invoke).Throws<ArgumentNullException>().WithParameterName(parameterName);
    }

    [Test]
    [MethodDataSource(typeof(IfSuccessTestData), nameof(IfSuccessTestData.LiftedNullTaskCases))]
    public async Task Null_tasks_throw_for_lifted_async_overloads(
        string operation,
        Func<Task> invoke,
        string parameterName
    )
    {
        await Assert.That(invoke).Throws<ArgumentNullException>().WithParameterName(parameterName);
    }

    [Test]
    public async Task IfSuccess_propagates_delegate_exceptions_for_task_wrapped_sources()
    {
        var expected = new InvalidOperationException("IfSuccess task failed");

        try
        {
            await Task.FromResult(FailOr.Success(1))
                .IfSuccess(_ =>
                {
                    throw expected;
                });
            throw new Exception("Expected IfSuccess to rethrow the original exception.");
        }
        catch (InvalidOperationException actual)
        {
            await Assert.That(ReferenceEquals(actual, expected)).IsTrue();
        }
    }

    [Test]
    public async Task IfSuccessAsync_propagates_delegate_exceptions_for_task_wrapped_sources()
    {
        var expected = new InvalidOperationException("IfSuccessAsync task failed");

        try
        {
            await Task.FromResult(FailOr.Success(1))
                .IfSuccessAsync(_ =>
                {
                    throw expected;
                });
            throw new Exception("Expected IfSuccessAsync to rethrow the original exception.");
        }
        catch (InvalidOperationException actual)
        {
            await Assert.That(ReferenceEquals(actual, expected)).IsTrue();
        }
    }

    [Test]
    [MethodDataSource(typeof(IfSuccessTestData), nameof(IfSuccessTestData.LiftedParityCases))]
    public async Task Lifted_overloads_match_direct_behavior_for_successes(
        string operation,
        Func<FailOr<int>, InvocationCounter, Task> direct,
        Func<Task<FailOr<int>>, InvocationCounter, Task> lifted
    )
    {
        var source = FailOr.Success(1);
        var directCounter = new InvocationCounter();
        var liftedCounter = new InvocationCounter();

        await direct(source, directCounter);
        await lifted(Task.FromResult(source), liftedCounter);

        using var _ = Assert.Multiple();
        await Assert.That(directCounter.Calls).IsEqualTo(1);
        await Assert.That(liftedCounter.Calls).IsEqualTo(1);
    }

    [Test]
    [MethodDataSource(typeof(IfSuccessTestData), nameof(IfSuccessTestData.LiftedParityCases))]
    public async Task Lifted_overloads_match_direct_behavior_for_failures(
        string operation,
        Func<FailOr<int>, InvocationCounter, Task> direct,
        Func<Task<FailOr<int>>, InvocationCounter, Task> lifted
    )
    {
        var source = FailOr.Fail<int>(Failure.General($"{operation} failed"));
        var directCounter = new InvocationCounter();
        var liftedCounter = new InvocationCounter();

        await direct(source, directCounter);
        await lifted(Task.FromResult(source), liftedCounter);

        using var _ = Assert.Multiple();
        await Assert.That(directCounter.Calls).IsEqualTo(0);
        await Assert.That(liftedCounter.Calls).IsEqualTo(0);
    }
}

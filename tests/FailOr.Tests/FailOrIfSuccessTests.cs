using TUnit.Assertions;
using TUnit.Assertions.Extensions;
using TUnit.Core;

namespace FailOr.Tests;

public class FailOrIfSuccessTests
{
    [Test]
    [Arguments(1)]
    [Arguments(5)]
    public async Task IfSuccess_invokes_delegate_once_with_success_value(int value)
    {
        var calls = 0;
        var observed = 0;

        FailOr
            .Success(value)
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
    public async Task IfSuccessAsync_invokes_delegate_once_with_success_value(int value)
    {
        var calls = 0;
        var observed = 0;

        await FailOr
            .Success(value)
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
        nameof(IfSuccessTestData.DirectFailureShortCircuitCases)
    )]
    public async Task Failed_sources_do_not_invoke_direct_sync_overloads(
        string operation,
        Action<FailOr<int>, InvocationCounter> invoke
    )
    {
        var failure = Failure.General($"{operation} failed");
        var source = FailOr.Fail<int>(failure);
        var counter = new InvocationCounter();

        invoke(source, counter);

        await Assert.That(counter.Calls).IsEqualTo(0);
    }

    [Test]
    [MethodDataSource(
        typeof(IfSuccessTestData),
        nameof(IfSuccessTestData.DirectAsyncFailureShortCircuitCases)
    )]
    public async Task Failed_sources_do_not_invoke_direct_async_overloads(
        string operation,
        Func<FailOr<int>, InvocationCounter, Task> invoke
    )
    {
        var failure = Failure.General($"{operation} failed");
        var source = FailOr.Fail<int>(failure);
        var counter = new InvocationCounter();

        await invoke(source, counter);

        await Assert.That(counter.Calls).IsEqualTo(0);
    }

    [Test]
    [MethodDataSource(typeof(IfSuccessTestData), nameof(IfSuccessTestData.DirectNullDelegateCases))]
    public async Task Null_delegates_throw_for_direct_overloads(
        string operation,
        Action invoke,
        string parameterName
    )
    {
        await Assert.That(invoke).Throws<ArgumentNullException>().WithParameterName(parameterName);
    }

    [Test]
    [MethodDataSource(typeof(IfSuccessTestData), nameof(IfSuccessTestData.DirectNullTaskCases))]
    public async Task Null_tasks_throw_for_direct_async_overloads(
        string operation,
        Func<Task> invoke,
        string parameterName
    )
    {
        await Assert.That(invoke).Throws<ArgumentNullException>().WithParameterName(parameterName);
    }

    [Test]
    public async Task IfSuccess_propagates_delegate_exceptions()
    {
        var expected = new InvalidOperationException("IfSuccess failed");

        try
        {
            FailOr
                .Success(1)
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
    public async Task IfSuccessAsync_propagates_delegate_exceptions()
    {
        var expected = new InvalidOperationException("IfSuccessAsync failed");

        try
        {
            await FailOr
                .Success(1)
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
}

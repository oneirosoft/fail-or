using TUnit.Assertions;
using TUnit.Assertions.Extensions;
using TUnit.Core;

namespace FailOr.Tests;

public class FailOrIfFailTests
{
    [Test]
    [MethodDataSource(typeof(IfFailTestData), nameof(IfFailTestData.DirectSuccessCases))]
    public async Task IfFail_does_not_invoke_delegates_for_successful_direct_sources(
        string operation,
        Func<FailOr<int>, IfFailInvocationCounter, Task> invoke
    )
    {
        var source = FailOr.Success(7);
        var counter = new IfFailInvocationCounter();

        await invoke(source, counter);

        await Assert.That(counter.Calls).IsEqualTo(0);
    }

    [Test]
    [MethodDataSource(typeof(IfFailTestData), nameof(IfFailTestData.DirectFailureCases))]
    public async Task IfFail_invokes_delegates_once_for_failed_direct_sources(
        string operation,
        Func<FailOr<int>, IfFailInvocationCounter, Task> invoke,
        int expectedCalls
    )
    {
        var source = FailOr.Fail<int>(Failure.General($"{operation} failed"));
        var counter = new IfFailInvocationCounter();

        await invoke(source, counter);

        await Assert.That(counter.Calls).IsEqualTo(expectedCalls);
    }

    [Test]
    [MethodDataSource(typeof(IfFailTestData), nameof(IfFailTestData.DirectFailuresAwareCases))]
    public async Task IfFail_direct_overloads_receive_the_original_failure_sequence(
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
    [MethodDataSource(typeof(IfFailTestData), nameof(IfFailTestData.DirectNullSelectorCases))]
    public async Task Null_selectors_throw_for_direct_IfFail_overloads(
        string operation,
        Action invoke,
        string parameterName
    )
    {
        await Assert.That(invoke).Throws<ArgumentNullException>().WithParameterName(parameterName);
    }

    [Test]
    [MethodDataSource(typeof(IfFailTestData), nameof(IfFailTestData.DirectNullTaskCases))]
    public async Task Null_tasks_throw_for_direct_async_IfFail_overloads(
        string operation,
        Func<Task> invoke,
        string parameterName
    )
    {
        await Assert.That(invoke).Throws<ArgumentNullException>().WithParameterName(parameterName);
    }

    [Test]
    public async Task IfFail_propagates_delegate_exceptions()
    {
        var expected = new InvalidOperationException("IfFail failed");

        try
        {
            FailOr.Fail<int>(Failure.General("failed")).IfFail(_ => throw expected);
            throw new Exception("Expected IfFail to rethrow the original exception.");
        }
        catch (InvalidOperationException actual)
        {
            await Assert.That(ReferenceEquals(actual, expected)).IsTrue();
        }
    }

    [Test]
    public async Task IfFailAsync_propagates_delegate_exceptions()
    {
        var expected = new InvalidOperationException("IfFailAsync failed");

        try
        {
            await FailOr
                .Fail<int>(Failure.General("failed"))
                .IfFailAsync(_ => Task.FromException(expected));
            throw new Exception("Expected IfFailAsync to rethrow the original exception.");
        }
        catch (InvalidOperationException actual)
        {
            await Assert.That(ReferenceEquals(actual, expected)).IsTrue();
        }
    }
}

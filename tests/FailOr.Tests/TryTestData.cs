namespace FailOr.Tests;

public static class TryTestData
{
    public static IEnumerable<
        Func<(string Operation, Func<FailOr<int>, InvocationCounter, Task<FailOr<int>>> Invoke)>
    > DirectFailureShortCircuitCases()
    {
        yield return () =>
            (
                "Try map",
                (source, counter) => Task.FromResult(source.Try(x => x + counter.Increment()))
            );
        yield return () =>
            (
                "Try map custom",
                (source, counter) =>
                    Task.FromResult(
                        source.Try(x => x + counter.Increment(), _ => counter.Increment())
                    )
            );
        yield return () =>
            (
                "TryAsync map",
                (source, counter) => source.TryAsync(x => Task.FromResult(x + counter.Increment()))
            );
        yield return () =>
            (
                "TryAsync map custom",
                (source, counter) =>
                    source.TryAsync(
                        x => Task.FromResult(x + counter.Increment()),
                        _ => counter.Increment()
                    )
            );
    }

    public static IEnumerable<
        Func<(string Operation, Action Invoke, string ParameterName)>
    > DirectNullSelectorCases()
    {
        yield return () => ("Try map", () => FailOr.Success(1).Try((Func<int, int>)null!), "map");
        yield return () =>
            ("Try map custom", () => FailOr.Success(1).Try((Func<int, int>)null!, _ => 1), "map");
        yield return () =>
            (
                "TryAsync map",
                () => FailOr.Success(1).TryAsync((Func<int, Task<int>>)null!),
                "mapAsync"
            );
        yield return () =>
            (
                "TryAsync map custom",
                () => FailOr.Success(1).TryAsync((Func<int, Task<int>>)null!, _ => 1),
                "mapAsync"
            );
    }

    public static IEnumerable<
        Func<(string Operation, Action Invoke, string ParameterName)>
    > DirectNullOnExceptionCases()
    {
        yield return () =>
            (
                "Try map custom",
                () => FailOr.Success(1).Try(x => x + 1, (Func<Exception, FailOr<int>>)null!),
                "onException"
            );
        yield return () =>
            (
                "TryAsync map custom",
                () =>
                    FailOr
                        .Success(1)
                        .TryAsync(x => Task.FromResult(x + 1), (Func<Exception, FailOr<int>>)null!),
                "onException"
            );
    }

    public static IEnumerable<
        Func<(string Operation, Func<Task> Invoke, string ParameterName)>
    > DirectNullTaskCases()
    {
        yield return () =>
        {
            Func<int, Task<int>> mapAsync = _ => null!;

            return ("TryAsync map", () => FailOr.Success(1).TryAsync(mapAsync), "resultTask");
        };
        yield return () =>
        {
            Func<int, Task<int>> mapAsync = _ => null!;

            return (
                "TryAsync map custom",
                () => FailOr.Success(1).TryAsync(mapAsync, _ => 1),
                "resultTask"
            );
        };
    }

    public static IEnumerable<
        Func<(
            string Operation,
            FailOr<int> Source,
            Func<FailOr<int>, Task<FailOr<int>>> Direct,
            Func<Task<FailOr<int>>, Task<FailOr<int>>> Lifted,
            FailOr<int> Expected
        )>
    > LiftedParityCases()
    {
        yield return () =>
            (
                "Try map success",
                FailOr.Success(1),
                source => Task.FromResult(source.Try(x => x + 1)),
                sourceTask => sourceTask.Try(x => x + 1),
                FailOr.Success(2)
            );
        yield return () =>
            (
                "Try map custom success",
                FailOr.Success(1),
                source => Task.FromResult(source.Try(x => x + 2, _ => 99)),
                sourceTask => sourceTask.Try(x => x + 2, _ => 99),
                FailOr.Success(3)
            );
        yield return () =>
            (
                "TryAsync map success",
                FailOr.Success(1),
                source => source.TryAsync(x => Task.FromResult(x + 3)),
                sourceTask => sourceTask.TryAsync(x => Task.FromResult(x + 3)),
                FailOr.Success(4)
            );
        yield return () =>
            (
                "TryAsync map custom success",
                FailOr.Success(1),
                source => source.TryAsync(x => Task.FromResult(x + 4), _ => 99),
                sourceTask => sourceTask.TryAsync(x => Task.FromResult(x + 4), _ => 99),
                FailOr.Success(5)
            );
        yield return () =>
        {
            var failure = Failure.General("failed");
            var source = FailOr.Fail<int>(failure);

            return (
                "Try map failure",
                source,
                sourceResult => Task.FromResult(sourceResult.Try(x => x + 1)),
                sourceTask => sourceTask.Try(x => x + 1),
                source
            );
        };
        yield return () =>
        {
            var failure = Failure.General("failed async");
            var source = FailOr.Fail<int>(failure);

            return (
                "TryAsync map failure",
                source,
                sourceResult => sourceResult.TryAsync(x => Task.FromResult(x + 1)),
                sourceTask => sourceTask.TryAsync(x => Task.FromResult(x + 1)),
                source
            );
        };
        yield return () =>
        {
            var exception = new InvalidOperationException("Try map exception");
            var expected = FailOr.Fail<int>(Failure.Exceptional(exception));
            Func<int, int> map = _ => throw exception;

            return (
                "Try map exception",
                FailOr.Success(1),
                source => Task.FromResult(source.Try(map)),
                sourceTask => sourceTask.Try(map),
                expected
            );
        };
        yield return () =>
        {
            var exception = new InvalidOperationException("Try map custom exception");
            var expected = FailOr.Fail<int>(Failure.General("mapping failed"));
            Func<int, int> map = _ => throw exception;

            return (
                "Try map custom exception",
                FailOr.Success(1),
                source => Task.FromResult(source.Try(map, _ => expected)),
                sourceTask => sourceTask.Try(map, _ => expected),
                expected
            );
        };
        yield return () =>
        {
            var exception = new InvalidOperationException("TryAsync map exception");
            var expected = FailOr.Fail<int>(Failure.Exceptional(exception));
            Func<int, Task<int>> mapAsync = _ => Task.FromException<int>(exception);

            return (
                "TryAsync map exception",
                FailOr.Success(1),
                source => source.TryAsync(mapAsync),
                sourceTask => sourceTask.TryAsync(mapAsync),
                expected
            );
        };
        yield return () =>
        {
            var exception = new InvalidOperationException("TryAsync map custom exception");
            var expected = FailOr.Fail<int>(Failure.General("mapping failed"));
            Func<int, Task<int>> mapAsync = _ => Task.FromException<int>(exception);

            return (
                "TryAsync map custom exception",
                FailOr.Success(1),
                source => source.TryAsync(mapAsync, _ => expected),
                sourceTask => sourceTask.TryAsync(mapAsync, _ => expected),
                expected
            );
        };
    }

    public static IEnumerable<
        Func<(string Operation, Action Invoke, string ParameterName)>
    > LiftedNullSourceCases()
    {
        yield return () => ("Try map", () => ((Task<FailOr<int>>)null!).Try(_ => 1), "sourceTask");
        yield return () =>
            ("Try map custom", () => ((Task<FailOr<int>>)null!).Try(_ => 1, _ => 1), "sourceTask");
        yield return () =>
            (
                "TryAsync map",
                () => ((Task<FailOr<int>>)null!).TryAsync(_ => Task.FromResult(1)),
                "sourceTask"
            );
        yield return () =>
            (
                "TryAsync map custom",
                () => ((Task<FailOr<int>>)null!).TryAsync(_ => Task.FromResult(1), _ => 1),
                "sourceTask"
            );
    }

    public static IEnumerable<
        Func<(string Operation, Action Invoke, string ParameterName)>
    > LiftedNullSelectorCases()
    {
        yield return () =>
            ("Try map", () => Task.FromResult(FailOr.Success(1)).Try((Func<int, int>)null!), "map");
        yield return () =>
            (
                "Try map custom",
                () => Task.FromResult(FailOr.Success(1)).Try((Func<int, int>)null!, _ => 1),
                "map"
            );
        yield return () =>
            (
                "TryAsync map",
                () => Task.FromResult(FailOr.Success(1)).TryAsync((Func<int, Task<int>>)null!),
                "mapAsync"
            );
        yield return () =>
            (
                "TryAsync map custom",
                () =>
                    Task.FromResult(FailOr.Success(1))
                        .TryAsync((Func<int, Task<int>>)null!, _ => 1),
                "mapAsync"
            );
    }

    public static IEnumerable<
        Func<(string Operation, Action Invoke, string ParameterName)>
    > LiftedNullOnExceptionCases()
    {
        yield return () =>
            (
                "Try map custom",
                () =>
                    Task.FromResult(FailOr.Success(1))
                        .Try(x => x + 1, (Func<Exception, FailOr<int>>)null!),
                "onException"
            );
        yield return () =>
            (
                "TryAsync map custom",
                () =>
                    Task.FromResult(FailOr.Success(1))
                        .TryAsync(x => Task.FromResult(x + 1), (Func<Exception, FailOr<int>>)null!),
                "onException"
            );
    }

    public static IEnumerable<
        Func<(string Operation, Func<Task> Invoke, string ParameterName)>
    > LiftedNullTaskCases()
    {
        yield return () =>
        {
            Func<int, Task<int>> mapAsync = _ => null!;

            return (
                "TryAsync map",
                () => Task.FromResult(FailOr.Success(1)).TryAsync(mapAsync),
                "resultTask"
            );
        };
        yield return () =>
        {
            Func<int, Task<int>> mapAsync = _ => null!;

            return (
                "TryAsync map custom",
                () => Task.FromResult(FailOr.Success(1)).TryAsync(mapAsync, _ => 1),
                "resultTask"
            );
        };
    }
}

public static class TryAssertions
{
    public static async Task AssertExceptionalFailure(
        FailOr<int> result,
        Exception expectedException
    )
    {
        using var _ = Assert.Multiple();

        await Assert.That(result.IsFailure).IsTrue();
        await Assert.That(result.Failures.Count).IsEqualTo(1);
        await Assert.That(result.Failures[0] is Failures.Exceptional).IsTrue();

        var failure = (Failures.Exceptional)result.Failures[0];
        await Assert.That(failure.Code).IsEqualTo("Exceptional");
        await Assert.That(ReferenceEquals(failure.Exception, expectedException)).IsTrue();
    }
}

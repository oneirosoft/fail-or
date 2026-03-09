namespace FailOr.Tests;

public static class FailWhenTestData
{
    public static IEnumerable<
        Func<(string Operation, Func<FailOr<int>, InvocationCounter, Task<FailOr<int>>> Invoke)>
    > DirectFailureShortCircuitCases()
    {
        yield return () =>
            (
                "FailWhen",
                (source, counter) =>
                    Task.FromResult(
                        source.FailWhen(
                            _ =>
                            {
                                counter.Increment();
                                return true;
                            },
                            Failure.General("predicate matched")
                        )
                    )
            );
        yield return () =>
            (
                "FailWhenAsync",
                (source, counter) =>
                    source.FailWhenAsync(
                        _ =>
                        {
                            counter.Increment();
                            return Task.FromResult(true);
                        },
                        Failure.General("predicate matched")
                    )
            );
    }

    public static IEnumerable<
        Func<(string Operation, Action Invoke, string ParameterName)>
    > DirectNullSelectorCases()
    {
        yield return () =>
            (
                "FailWhen",
                () => FailOr.Success(1).FailWhen((Func<int, bool>)null!, Failure.General("failed")),
                "predicate"
            );
        yield return () =>
            (
                "FailWhenAsync",
                () =>
                    FailOr
                        .Success(1)
                        .FailWhenAsync((Func<int, Task<bool>>)null!, Failure.General("failed")),
                "predicateAsync"
            );
    }

    public static IEnumerable<
        Func<(string Operation, Action Invoke, string ParameterName)>
    > DirectNullFailureCases()
    {
        yield return () =>
            ("FailWhen", () => FailOr.Success(1).FailWhen(_ => true, (Failures)null!), "failure");
        yield return () =>
            (
                "FailWhenAsync",
                () => FailOr.Success(1).FailWhenAsync(_ => Task.FromResult(true), (Failures)null!),
                "failure"
            );
    }

    public static IEnumerable<
        Func<(string Operation, Func<Task> Invoke, string ParameterName)>
    > DirectNullTaskCases()
    {
        yield return () =>
        {
            Func<int, Task<bool>> predicateAsync = _ => null!;

            return (
                "FailWhenAsync",
                () => FailOr.Success(1).FailWhenAsync(predicateAsync, Failure.General("failed")),
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
        {
            var failure = Failure.General("must be positive");

            return (
                "FailWhen success preserved",
                FailOr.Success(1),
                source => Task.FromResult(source.FailWhen(x => x < 0, failure)),
                sourceTask => sourceTask.FailWhen(x => x < 0, failure),
                FailOr.Success(1)
            );
        };
        yield return () =>
        {
            var failure = Failure.General("must be even");

            return (
                "FailWhen success failed",
                FailOr.Success(3),
                source => Task.FromResult(source.FailWhen(x => x % 2 != 0, failure)),
                sourceTask => sourceTask.FailWhen(x => x % 2 != 0, failure),
                FailOr.Fail<int>(failure)
            );
        };
        yield return () =>
        {
            var existingFailure = Failure.General("source failed");
            var source = FailOr.Fail<int>(existingFailure);
            var replacementFailure = Failure.General("predicate matched");

            return (
                "FailWhen failed source",
                source,
                sourceResult =>
                    Task.FromResult(sourceResult.FailWhen(_ => true, replacementFailure)),
                sourceTask => sourceTask.FailWhen(_ => true, replacementFailure),
                source
            );
        };
        yield return () =>
        {
            var failure = Failure.General("must be positive");

            return (
                "FailWhenAsync success preserved",
                FailOr.Success(1),
                source => source.FailWhenAsync(x => Task.FromResult(x < 0), failure),
                sourceTask => sourceTask.FailWhenAsync(x => Task.FromResult(x < 0), failure),
                FailOr.Success(1)
            );
        };
        yield return () =>
        {
            var failure = Failure.General("must be even");

            return (
                "FailWhenAsync success failed",
                FailOr.Success(3),
                source => source.FailWhenAsync(x => Task.FromResult(x % 2 != 0), failure),
                sourceTask => sourceTask.FailWhenAsync(x => Task.FromResult(x % 2 != 0), failure),
                FailOr.Fail<int>(failure)
            );
        };
        yield return () =>
        {
            var existingFailure = Failure.General("source failed async");
            var source = FailOr.Fail<int>(existingFailure);
            var replacementFailure = Failure.General("predicate matched");

            return (
                "FailWhenAsync failed source",
                source,
                sourceResult =>
                    sourceResult.FailWhenAsync(_ => Task.FromResult(true), replacementFailure),
                sourceTask =>
                    sourceTask.FailWhenAsync(_ => Task.FromResult(true), replacementFailure),
                source
            );
        };
    }

    public static IEnumerable<
        Func<(
            string Operation,
            Func<Task<FailOr<int>>, InvocationCounter, Task<FailOr<int>>> Invoke
        )>
    > LiftedFailureShortCircuitCases()
    {
        yield return () =>
            (
                "FailWhen",
                (sourceTask, counter) =>
                    sourceTask.FailWhen(
                        _ =>
                        {
                            counter.Increment();
                            return true;
                        },
                        Failure.General("predicate matched")
                    )
            );
        yield return () =>
            (
                "FailWhenAsync",
                (sourceTask, counter) =>
                    sourceTask.FailWhenAsync(
                        _ =>
                        {
                            counter.Increment();
                            return Task.FromResult(true);
                        },
                        Failure.General("predicate matched")
                    )
            );
    }

    public static IEnumerable<
        Func<(string Operation, Action Invoke, string ParameterName)>
    > LiftedNullSourceCases()
    {
        yield return () =>
            (
                "FailWhen",
                () => ((Task<FailOr<int>>)null!).FailWhen(_ => true, Failure.General("failed")),
                "sourceTask"
            );
        yield return () =>
            (
                "FailWhenAsync",
                () =>
                    ((Task<FailOr<int>>)null!).FailWhenAsync(
                        _ => Task.FromResult(true),
                        Failure.General("failed")
                    ),
                "sourceTask"
            );
    }

    public static IEnumerable<
        Func<(string Operation, Action Invoke, string ParameterName)>
    > LiftedNullSelectorCases()
    {
        yield return () =>
            (
                "FailWhen",
                () =>
                    Task.FromResult(FailOr.Success(1))
                        .FailWhen((Func<int, bool>)null!, Failure.General("failed")),
                "predicate"
            );
        yield return () =>
            (
                "FailWhenAsync",
                () =>
                    Task.FromResult(FailOr.Success(1))
                        .FailWhenAsync((Func<int, Task<bool>>)null!, Failure.General("failed")),
                "predicateAsync"
            );
    }

    public static IEnumerable<
        Func<(string Operation, Action Invoke, string ParameterName)>
    > LiftedNullFailureCases()
    {
        yield return () =>
            (
                "FailWhen",
                () => Task.FromResult(FailOr.Success(1)).FailWhen(_ => true, (Failures)null!),
                "failure"
            );
        yield return () =>
            (
                "FailWhenAsync",
                () =>
                    Task.FromResult(FailOr.Success(1))
                        .FailWhenAsync(_ => Task.FromResult(true), (Failures)null!),
                "failure"
            );
    }

    public static IEnumerable<
        Func<(string Operation, Func<Task> Invoke, string ParameterName)>
    > LiftedNullTaskCases()
    {
        yield return () =>
        {
            Func<int, Task<bool>> predicateAsync = _ => null!;

            return (
                "FailWhenAsync",
                () =>
                    Task.FromResult(FailOr.Success(1))
                        .FailWhenAsync(predicateAsync, Failure.General("failed")),
                "resultTask"
            );
        };
    }
}

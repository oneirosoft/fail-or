namespace FailOr.Tests;

public static class ElseTestData
{
    public static IEnumerable<
        Func<(string Operation, Func<FailOr<int>, ElseInvocationCounter, Task<int>> Invoke)>
    > DirectSuccessCases()
    {
        yield return () => ("Else value", (source, _) => Task.FromResult(source.Else(99)));
        yield return () =>
            (
                "Else deferred",
                (source, counter) => Task.FromResult(source.Else(() => counter.Increment()))
            );
        yield return () =>
            (
                "Else deferred failures-aware",
                (source, counter) =>
                    Task.FromResult(source.Else(failures => failures.Count + counter.Increment()))
            );
        yield return () =>
            (
                "ElseAsync deferred",
                (source, counter) => source.ElseAsync(() => Task.FromResult(counter.Increment()))
            );
        yield return () =>
            (
                "ElseAsync deferred failures-aware",
                (source, counter) =>
                    source.ElseAsync(failures =>
                        Task.FromResult(failures.Count + counter.Increment())
                    )
            );
    }

    public static IEnumerable<
        Func<(
            string Operation,
            Func<FailOr<int>, ElseInvocationCounter, Task<int>> Invoke,
            int ExpectedValue,
            int ExpectedCalls
        )>
    > DirectFailureCases()
    {
        yield return () => ("Else value", (source, _) => Task.FromResult(source.Else(90)), 90, 0);
        yield return () =>
            (
                "Else deferred",
                (source, counter) => Task.FromResult(source.Else(() => 40 + counter.Increment())),
                41,
                1
            );
        yield return () =>
            (
                "Else deferred failures-aware",
                (source, counter) =>
                    Task.FromResult(
                        source.Else(failures => 50 + failures.Count + counter.Increment())
                    ),
                52,
                1
            );
        yield return () =>
            (
                "ElseAsync deferred",
                (source, counter) =>
                    source.ElseAsync(() => Task.FromResult(60 + counter.Increment())),
                61,
                1
            );
        yield return () =>
            (
                "ElseAsync deferred failures-aware",
                (source, counter) =>
                    source.ElseAsync(failures =>
                        Task.FromResult(70 + failures.Count + counter.Increment())
                    ),
                72,
                1
            );
    }

    public static IEnumerable<
        Func<(string Operation, Func<FailOr<int>, Task<IReadOnlyList<Failures>>> Capture)>
    > DirectFailuresAwareCases()
    {
        yield return () =>
            (
                "Else deferred failures-aware",
                source =>
                {
                    IReadOnlyList<Failures>? observed = null;

                    _ = source.Else(failures =>
                    {
                        observed = failures;
                        return 1;
                    });

                    return Task.FromResult(observed!);
                }
            );

        yield return () =>
            (
                "ElseAsync deferred failures-aware",
                async source =>
                {
                    IReadOnlyList<Failures>? observed = null;

                    _ = await source.ElseAsync(failures =>
                    {
                        observed = failures;
                        return Task.FromResult(1);
                    });

                    return observed!;
                }
            );
    }

    public static IEnumerable<
        Func<(string Operation, Action Invoke, string ParameterName)>
    > DirectNullSelectorCases()
    {
        yield return () =>
            ("Else deferred", () => FailOr.Success(1).Else((Func<int>)null!), "alternative");
        yield return () =>
            (
                "Else deferred failures-aware",
                () => FailOr.Success(1).Else((Func<IReadOnlyList<Failures>, int>)null!),
                "alternative"
            );
        yield return () =>
            (
                "ElseAsync deferred",
                () => FailOr.Success(1).ElseAsync((Func<Task<int>>)null!),
                "alternativeAsync"
            );
        yield return () =>
            (
                "ElseAsync deferred failures-aware",
                () => FailOr.Success(1).ElseAsync((Func<IReadOnlyList<Failures>, Task<int>>)null!),
                "alternativeAsync"
            );
    }

    public static IEnumerable<
        Func<(string Operation, Func<Task> Invoke, string ParameterName)>
    > DirectNullTaskCases()
    {
        yield return () =>
            (
                "ElseAsync deferred",
                () => FailOr.Fail<int>(Failure.General("failed")).ElseAsync(() => null!),
                "resultTask"
            );
        yield return () =>
            (
                "ElseAsync deferred failures-aware",
                () => FailOr.Fail<int>(Failure.General("failed")).ElseAsync(_ => null!),
                "resultTask"
            );
    }

    public static IEnumerable<
        Func<(string Operation, Func<Task<FailOr<int>>, ElseInvocationCounter, Task<int>> Invoke)>
    > LiftedSuccessCases()
    {
        yield return () => ("Else value", (sourceTask, _) => sourceTask.Else(99));
        yield return () =>
            ("Else deferred", (sourceTask, counter) => sourceTask.Else(() => counter.Increment()));
        yield return () =>
            (
                "Else deferred failures-aware",
                (sourceTask, counter) =>
                    sourceTask.Else(failures => failures.Count + counter.Increment())
            );
        yield return () =>
            (
                "ElseAsync deferred",
                (sourceTask, counter) =>
                    sourceTask.ElseAsync(() => Task.FromResult(counter.Increment()))
            );
        yield return () =>
            (
                "ElseAsync deferred failures-aware",
                (sourceTask, counter) =>
                    sourceTask.ElseAsync(failures =>
                        Task.FromResult(failures.Count + counter.Increment())
                    )
            );
    }

    public static IEnumerable<
        Func<(
            string Operation,
            Func<Task<FailOr<int>>, ElseInvocationCounter, Task<int>> Invoke,
            int ExpectedValue,
            int ExpectedCalls
        )>
    > LiftedFailureCases()
    {
        yield return () => ("Else value", (sourceTask, _) => sourceTask.Else(90), 90, 0);
        yield return () =>
            (
                "Else deferred",
                (sourceTask, counter) => sourceTask.Else(() => 40 + counter.Increment()),
                41,
                1
            );
        yield return () =>
            (
                "Else deferred failures-aware",
                (sourceTask, counter) =>
                    sourceTask.Else(failures => 50 + failures.Count + counter.Increment()),
                52,
                1
            );
        yield return () =>
            (
                "ElseAsync deferred",
                (sourceTask, counter) =>
                    sourceTask.ElseAsync(() => Task.FromResult(60 + counter.Increment())),
                61,
                1
            );
        yield return () =>
            (
                "ElseAsync deferred failures-aware",
                (sourceTask, counter) =>
                    sourceTask.ElseAsync(failures =>
                        Task.FromResult(70 + failures.Count + counter.Increment())
                    ),
                72,
                1
            );
    }

    public static IEnumerable<
        Func<(string Operation, Func<Task<FailOr<int>>, Task<IReadOnlyList<Failures>>> Capture)>
    > LiftedFailuresAwareCases()
    {
        yield return () =>
            (
                "Else deferred failures-aware",
                async sourceTask =>
                {
                    IReadOnlyList<Failures>? observed = null;

                    _ = await sourceTask.Else(failures =>
                    {
                        observed = failures;
                        return 1;
                    });

                    return observed!;
                }
            );

        yield return () =>
            (
                "ElseAsync deferred failures-aware",
                async sourceTask =>
                {
                    IReadOnlyList<Failures>? observed = null;

                    _ = await sourceTask.ElseAsync(failures =>
                    {
                        observed = failures;
                        return Task.FromResult(1);
                    });

                    return observed!;
                }
            );
    }

    public static IEnumerable<
        Func<(string Operation, Action Invoke, string ParameterName)>
    > LiftedNullSourceCases()
    {
        yield return () => ("Else value", () => ((Task<FailOr<int>>)null!).Else(1), "sourceTask");
        yield return () =>
            ("Else deferred", () => ((Task<FailOr<int>>)null!).Else(() => 1), "sourceTask");
        yield return () =>
            (
                "Else deferred failures-aware",
                () => ((Task<FailOr<int>>)null!).Else(_ => 1),
                "sourceTask"
            );
        yield return () =>
            (
                "ElseAsync deferred",
                () => ((Task<FailOr<int>>)null!).ElseAsync(() => Task.FromResult(1)),
                "sourceTask"
            );
        yield return () =>
            (
                "ElseAsync deferred failures-aware",
                () => ((Task<FailOr<int>>)null!).ElseAsync(_ => Task.FromResult(1)),
                "sourceTask"
            );
    }

    public static IEnumerable<
        Func<(string Operation, Action Invoke, string ParameterName)>
    > LiftedNullSelectorCases()
    {
        yield return () =>
            (
                "Else deferred",
                () => Task.FromResult(FailOr.Success(1)).Else((Func<int>)null!),
                "alternative"
            );
        yield return () =>
            (
                "Else deferred failures-aware",
                () =>
                    Task.FromResult(FailOr.Success(1))
                        .Else((Func<IReadOnlyList<Failures>, int>)null!),
                "alternative"
            );
        yield return () =>
            (
                "ElseAsync deferred",
                () => Task.FromResult(FailOr.Success(1)).ElseAsync((Func<Task<int>>)null!),
                "alternativeAsync"
            );
        yield return () =>
            (
                "ElseAsync deferred failures-aware",
                () =>
                    Task.FromResult(FailOr.Success(1))
                        .ElseAsync((Func<IReadOnlyList<Failures>, Task<int>>)null!),
                "alternativeAsync"
            );
    }

    public static IEnumerable<
        Func<(string Operation, Func<Task> Invoke, string ParameterName)>
    > LiftedNullTaskCases()
    {
        yield return () =>
            (
                "ElseAsync deferred",
                () =>
                    Task.FromResult(FailOr.Fail<int>(Failure.General("failed")))
                        .ElseAsync(() => null!),
                "resultTask"
            );
        yield return () =>
            (
                "ElseAsync deferred failures-aware",
                () =>
                    Task.FromResult(FailOr.Fail<int>(Failure.General("failed")))
                        .ElseAsync(_ => null!),
                "resultTask"
            );
    }

    public static IEnumerable<
        Func<(
            string Operation,
            FailOr<int> Source,
            Func<FailOr<int>, Task<int>> Direct,
            Func<Task<FailOr<int>>, Task<int>> Lifted,
            int ExpectedValue
        )>
    > LiftedParityCases()
    {
        yield return () =>
            (
                "Else value success",
                FailOr.Success(1),
                source => Task.FromResult(source.Else(90)),
                sourceTask => sourceTask.Else(90),
                1
            );
        yield return () =>
            (
                "Else deferred success",
                FailOr.Success(1),
                source => Task.FromResult(source.Else(() => 90)),
                sourceTask => sourceTask.Else(() => 90),
                1
            );
        yield return () =>
            (
                "Else deferred failures-aware success",
                FailOr.Success(1),
                source => Task.FromResult(source.Else(_ => 90)),
                sourceTask => sourceTask.Else(_ => 90),
                1
            );
        yield return () =>
            (
                "ElseAsync deferred success",
                FailOr.Success(1),
                source => source.ElseAsync(() => Task.FromResult(90)),
                sourceTask => sourceTask.ElseAsync(() => Task.FromResult(90)),
                1
            );
        yield return () =>
            (
                "ElseAsync deferred failures-aware success",
                FailOr.Success(1),
                source => source.ElseAsync(_ => Task.FromResult(90)),
                sourceTask => sourceTask.ElseAsync(_ => Task.FromResult(90)),
                1
            );
        yield return () =>
            (
                "Else value failure",
                FailOr.Fail<int>(Failure.General("failed")),
                source => Task.FromResult(source.Else(90)),
                sourceTask => sourceTask.Else(90),
                90
            );
        yield return () =>
            (
                "Else deferred failure",
                FailOr.Fail<int>(Failure.General("failed")),
                source => Task.FromResult(source.Else(() => 90)),
                sourceTask => sourceTask.Else(() => 90),
                90
            );
        yield return () =>
            (
                "Else deferred failures-aware failure",
                FailOr.Fail<int>(Failure.General("failed")),
                source => Task.FromResult(source.Else(_ => 90)),
                sourceTask => sourceTask.Else(_ => 90),
                90
            );
        yield return () =>
            (
                "ElseAsync deferred failure",
                FailOr.Fail<int>(Failure.General("failed")),
                source => source.ElseAsync(() => Task.FromResult(90)),
                sourceTask => sourceTask.ElseAsync(() => Task.FromResult(90)),
                90
            );
        yield return () =>
            (
                "ElseAsync deferred failures-aware failure",
                FailOr.Fail<int>(Failure.General("failed")),
                source => source.ElseAsync(_ => Task.FromResult(90)),
                sourceTask => sourceTask.ElseAsync(_ => Task.FromResult(90)),
                90
            );
    }
}

public sealed class ElseInvocationCounter
{
    public int Calls { get; private set; }

    public int Increment() => ++Calls;
}

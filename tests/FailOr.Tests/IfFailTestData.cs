namespace FailOr.Tests;

public static class IfFailTestData
{
    public static IEnumerable<
        Func<(string Operation, Func<FailOr<int>, IfFailInvocationCounter, Task> Invoke)>
    > DirectSuccessCases()
    {
        yield return () =>
            (
                "IfFail",
                (source, counter) =>
                {
                    source.IfFail(_ => counter.Increment());
                    return Task.CompletedTask;
                }
            );
        yield return () =>
            (
                "IfFailAsync",
                (source, counter) =>
                    source.IfFailAsync(_ =>
                    {
                        counter.Increment();
                        return Task.CompletedTask;
                    })
            );
    }

    public static IEnumerable<
        Func<(
            string Operation,
            Func<FailOr<int>, IfFailInvocationCounter, Task> Invoke,
            int ExpectedCalls
        )>
    > DirectFailureCases()
    {
        yield return () =>
            (
                "IfFail",
                (source, counter) =>
                {
                    source.IfFail(_ => counter.Increment());
                    return Task.CompletedTask;
                },
                1
            );
        yield return () =>
            (
                "IfFailAsync",
                (source, counter) =>
                    source.IfFailAsync(_ =>
                    {
                        counter.Increment();
                        return Task.CompletedTask;
                    }),
                1
            );
    }

    public static IEnumerable<
        Func<(string Operation, Func<FailOr<int>, Task<IReadOnlyList<Failures>>> Capture)>
    > DirectFailuresAwareCases()
    {
        yield return () =>
            (
                "IfFail",
                source =>
                {
                    IReadOnlyList<Failures> observed = [];

                    source.IfFail(failures => observed = failures);

                    return Task.FromResult(observed);
                }
            );
        yield return () =>
            (
                "IfFailAsync",
                async source =>
                {
                    IReadOnlyList<Failures> observed = [];

                    await source.IfFailAsync(failures =>
                    {
                        observed = failures;
                        return Task.CompletedTask;
                    });

                    return observed;
                }
            );
    }

    public static IEnumerable<
        Func<(string Operation, Action Invoke, string ParameterName)>
    > DirectNullSelectorCases()
    {
        yield return () =>
            (
                "IfFail",
                () => FailOr.Success(1).IfFail((Action<IReadOnlyList<Failures>>)null!),
                "action"
            );
        yield return () =>
            (
                "IfFailAsync",
                () => FailOr.Success(1).IfFailAsync((Func<IReadOnlyList<Failures>, Task>)null!),
                "actionAsync"
            );
    }

    public static IEnumerable<
        Func<(string Operation, Func<Task> Invoke, string ParameterName)>
    > DirectNullTaskCases()
    {
        yield return () =>
            (
                "IfFailAsync",
                () => FailOr.Fail<int>(Failure.General("failed")).IfFailAsync(_ => null!),
                "resultTask"
            );
    }

    public static IEnumerable<
        Func<(string Operation, Func<Task<FailOr<int>>, IfFailInvocationCounter, Task> Invoke)>
    > LiftedSuccessCases()
    {
        yield return () =>
            ("IfFail", (sourceTask, counter) => sourceTask.IfFail(_ => counter.Increment()));
        yield return () =>
            (
                "IfFailAsync",
                (sourceTask, counter) =>
                    sourceTask.IfFailAsync(_ =>
                    {
                        counter.Increment();
                        return Task.CompletedTask;
                    })
            );
    }

    public static IEnumerable<
        Func<(
            string Operation,
            Func<Task<FailOr<int>>, IfFailInvocationCounter, Task> Invoke,
            int ExpectedCalls
        )>
    > LiftedFailureCases()
    {
        yield return () =>
            ("IfFail", (sourceTask, counter) => sourceTask.IfFail(_ => counter.Increment()), 1);
        yield return () =>
            (
                "IfFailAsync",
                (sourceTask, counter) =>
                    sourceTask.IfFailAsync(_ =>
                    {
                        counter.Increment();
                        return Task.CompletedTask;
                    }),
                1
            );
    }

    public static IEnumerable<
        Func<(string Operation, Func<Task<FailOr<int>>, Task<IReadOnlyList<Failures>>> Capture)>
    > LiftedFailuresAwareCases()
    {
        yield return () =>
            (
                "IfFail",
                async sourceTask =>
                {
                    IReadOnlyList<Failures> observed = [];

                    await sourceTask.IfFail(failures => observed = failures);

                    return observed;
                }
            );
        yield return () =>
            (
                "IfFailAsync",
                async sourceTask =>
                {
                    IReadOnlyList<Failures> observed = [];

                    await sourceTask.IfFailAsync(failures =>
                    {
                        observed = failures;
                        return Task.CompletedTask;
                    });

                    return observed;
                }
            );
    }

    public static IEnumerable<
        Func<(string Operation, Action Invoke, string ParameterName)>
    > LiftedNullSourceCases()
    {
        yield return () =>
            ("IfFail", () => ((Task<FailOr<int>>)null!).IfFail(_ => { }), "sourceTask");
        yield return () =>
            (
                "IfFailAsync",
                () => ((Task<FailOr<int>>)null!).IfFailAsync(_ => Task.CompletedTask),
                "sourceTask"
            );
    }

    public static IEnumerable<
        Func<(string Operation, Action Invoke, string ParameterName)>
    > LiftedNullSelectorCases()
    {
        yield return () =>
            (
                "IfFail",
                () =>
                    Task.FromResult(FailOr.Success(1))
                        .IfFail((Action<IReadOnlyList<Failures>>)null!),
                "action"
            );
        yield return () =>
            (
                "IfFailAsync",
                () =>
                    Task.FromResult(FailOr.Success(1))
                        .IfFailAsync((Func<IReadOnlyList<Failures>, Task>)null!),
                "actionAsync"
            );
    }

    public static IEnumerable<
        Func<(string Operation, Func<Task> Invoke, string ParameterName)>
    > LiftedNullTaskCases()
    {
        yield return () =>
            (
                "IfFailAsync",
                () =>
                    Task.FromResult(FailOr.Fail<int>(Failure.General("failed")))
                        .IfFailAsync(_ => null!),
                "resultTask"
            );
    }

    public static IEnumerable<
        Func<(
            string Operation,
            FailOr<int> Source,
            Func<FailOr<int>, IfFailInvocationCounter, Task> Direct,
            Func<Task<FailOr<int>>, IfFailInvocationCounter, Task> Lifted
        )>
    > ParityCases()
    {
        yield return () =>
            (
                "IfFail success",
                FailOr.Success(1),
                (source, counter) =>
                {
                    source.IfFail(_ => counter.Increment());
                    return Task.CompletedTask;
                },
                (sourceTask, counter) => sourceTask.IfFail(_ => counter.Increment())
            );
        yield return () =>
            (
                "IfFail failure",
                FailOr.Fail<int>(Failure.General("failed")),
                (source, counter) =>
                {
                    source.IfFail(_ => counter.Increment());
                    return Task.CompletedTask;
                },
                (sourceTask, counter) => sourceTask.IfFail(_ => counter.Increment())
            );
        yield return () =>
            (
                "IfFailAsync success",
                FailOr.Success(1),
                (source, counter) =>
                    source.IfFailAsync(_ =>
                    {
                        counter.Increment();
                        return Task.CompletedTask;
                    }),
                (sourceTask, counter) =>
                    sourceTask.IfFailAsync(_ =>
                    {
                        counter.Increment();
                        return Task.CompletedTask;
                    })
            );
        yield return () =>
            (
                "IfFailAsync failure",
                FailOr.Fail<int>(Failure.General("failed")),
                (source, counter) =>
                    source.IfFailAsync(_ =>
                    {
                        counter.Increment();
                        return Task.CompletedTask;
                    }),
                (sourceTask, counter) =>
                    sourceTask.IfFailAsync(_ =>
                    {
                        counter.Increment();
                        return Task.CompletedTask;
                    })
            );
    }
}

public sealed class IfFailInvocationCounter
{
    public int Calls { get; private set; }

    public int Increment() => ++Calls;
}

namespace FailOr.Tests;

public static class ThenTestData
{
    public static IEnumerable<
        Func<(string Operation, Func<FailOr<int>, InvocationCounter, Task<FailOr<int>>> Invoke)>
    > DirectFailureShortCircuitCases()
    {
        yield return () =>
            (
                "Then map",
                (source, counter) => Task.FromResult(source.Then(x => x + counter.Increment()))
            );
        yield return () =>
            (
                "Then bind",
                (source, counter) =>
                    Task.FromResult(source.Then(x => FailOr.Success(x + counter.Increment())))
            );
        yield return () =>
            (
                "ThenAsync map",
                (source, counter) => source.ThenAsync(x => Task.FromResult(x + counter.Increment()))
            );
        yield return () =>
            (
                "ThenAsync bind",
                (source, counter) =>
                    source.ThenAsync(x => Task.FromResult(FailOr.Success(x + counter.Increment())))
            );
    }

    public static IEnumerable<
        Func<(string Operation, Action Invoke, string ParameterName)>
    > DirectNullSelectorCases()
    {
        yield return () => ("Then map", () => FailOr.Success(1).Then((Func<int, int>)null!), "map");
        yield return () =>
            ("Then bind", () => FailOr.Success(1).Then((Func<int, FailOr<int>>)null!), "bind");
        yield return () =>
            (
                "ThenAsync map",
                () => FailOr.Success(1).ThenAsync((Func<int, Task<int>>)null!),
                "mapAsync"
            );
        yield return () =>
            (
                "ThenAsync bind",
                () => FailOr.Success(1).ThenAsync((Func<int, Task<FailOr<int>>>)null!),
                "bindAsync"
            );
    }

    public static IEnumerable<
        Func<(
            string Operation,
            Func<Task<FailOr<int>>, InvocationCounter, Task<FailOr<int>>> Invoke
        )>
    > LiftedFailureShortCircuitCases()
    {
        yield return () =>
            ("Then map", (sourceTask, counter) => sourceTask.Then(x => x + counter.Increment()));
        yield return () =>
            (
                "Then bind",
                (sourceTask, counter) =>
                    sourceTask.Then(x => FailOr.Success(x + counter.Increment()))
            );
        yield return () =>
            (
                "ThenAsync map",
                (sourceTask, counter) =>
                    sourceTask.ThenAsync(x => Task.FromResult(x + counter.Increment()))
            );
        yield return () =>
            (
                "ThenAsync bind",
                (sourceTask, counter) =>
                    sourceTask.ThenAsync(x =>
                        Task.FromResult(FailOr.Success(x + counter.Increment()))
                    )
            );
    }

    public static IEnumerable<
        Func<(string Operation, Action Invoke, string ParameterName)>
    > LiftedNullSelectorCases()
    {
        yield return () =>
            (
                "Then map",
                () => Task.FromResult(FailOr.Success(1)).Then((Func<int, int>)null!),
                "map"
            );
        yield return () =>
            (
                "Then bind",
                () => Task.FromResult(FailOr.Success(1)).Then((Func<int, FailOr<int>>)null!),
                "bind"
            );
        yield return () =>
            (
                "ThenAsync map",
                () => Task.FromResult(FailOr.Success(1)).ThenAsync((Func<int, Task<int>>)null!),
                "mapAsync"
            );
        yield return () =>
            (
                "ThenAsync bind",
                () =>
                    Task.FromResult(FailOr.Success(1))
                        .ThenAsync((Func<int, Task<FailOr<int>>>)null!),
                "bindAsync"
            );
    }

    public static IEnumerable<
        Func<(
            string Operation,
            Func<FailOr<int>, Task<FailOr<int>>> Direct,
            Func<Task<FailOr<int>>, Task<FailOr<int>>> Lifted,
            int Expected
        )>
    > LiftedParityCases()
    {
        yield return () =>
            (
                "Then map",
                source => Task.FromResult(source.Then(x => x + 1)),
                sourceTask => sourceTask.Then(x => x + 1),
                2
            );
        yield return () =>
            (
                "Then bind",
                source => Task.FromResult(source.Then(x => FailOr.Success(x + 1))),
                sourceTask => sourceTask.Then(x => FailOr.Success(x + 1)),
                2
            );
        yield return () =>
            (
                "ThenAsync map",
                source => source.ThenAsync(x => Task.FromResult(x + 1)),
                sourceTask => sourceTask.ThenAsync(x => Task.FromResult(x + 1)),
                2
            );
        yield return () =>
            (
                "ThenAsync bind",
                source => source.ThenAsync(x => Task.FromResult(FailOr.Success(x + 1))),
                sourceTask => sourceTask.ThenAsync(x => Task.FromResult(FailOr.Success(x + 1))),
                2
            );
    }

    public static IEnumerable<
        Func<(
            string Operation,
            Func<FailOr<int>, InvocationCounter, Task<FailOr<int>>> Invoke,
            int ExpectedCalls
        )>
    > DirectIfFailThenSuccessCases()
    {
        yield return () =>
            (
                "IfFailThen result",
                (source, _) => Task.FromResult(source.IfFailThen(FailOr.Success(99))),
                0
            );
        yield return () =>
            (
                "IfFailThen deferred",
                (source, counter) =>
                    Task.FromResult(source.IfFailThen(() => FailOr.Success(counter.Increment()))),
                0
            );
        yield return () =>
            (
                "IfFailThen deferred failures-aware",
                (source, counter) =>
                    Task.FromResult(
                        source.IfFailThen(failures =>
                            FailOr.Success(failures.Count + counter.Increment())
                        )
                    ),
                0
            );
        yield return () =>
            (
                "IfFailThenAsync deferred",
                (source, counter) =>
                    source.IfFailThenAsync(() =>
                        Task.FromResult(FailOr.Success(counter.Increment()))
                    ),
                0
            );
        yield return () =>
            (
                "IfFailThenAsync deferred failures-aware",
                (source, counter) =>
                    source.IfFailThenAsync(failures =>
                        Task.FromResult(FailOr.Success(failures.Count + counter.Increment()))
                    ),
                0
            );
    }

    public static IEnumerable<
        Func<(
            string Operation,
            Func<FailOr<int>, InvocationCounter, Task<FailOr<int>>> Invoke,
            int ExpectedCalls,
            int ExpectedValue
        )>
    > DirectIfFailThenFailureCases()
    {
        yield return () =>
            (
                "IfFailThen result",
                (source, _) => Task.FromResult(source.IfFailThen(FailOr.Success(90))),
                0,
                90
            );
        yield return () =>
            (
                "IfFailThen deferred",
                (source, counter) =>
                    Task.FromResult(
                        source.IfFailThen(() => FailOr.Success(40 + counter.Increment()))
                    ),
                1,
                41
            );
        yield return () =>
            (
                "IfFailThen deferred failures-aware",
                (source, counter) =>
                    Task.FromResult(
                        source.IfFailThen(failures =>
                            FailOr.Success(50 + failures.Count + counter.Increment())
                        )
                    ),
                1,
                52
            );
        yield return () =>
            (
                "IfFailThenAsync deferred",
                (source, counter) =>
                    source.IfFailThenAsync(() =>
                        Task.FromResult(FailOr.Success(60 + counter.Increment()))
                    ),
                1,
                61
            );
        yield return () =>
            (
                "IfFailThenAsync deferred failures-aware",
                (source, counter) =>
                    source.IfFailThenAsync(failures =>
                        Task.FromResult(FailOr.Success(70 + failures.Count + counter.Increment()))
                    ),
                1,
                72
            );
    }

    public static IEnumerable<
        Func<(string Operation, Func<FailOr<int>, Task<IReadOnlyList<Failures>>> Capture)>
    > DirectIfFailThenFailuresAwareCases()
    {
        yield return () =>
            (
                "IfFailThen deferred failures-aware",
                source =>
                {
                    IReadOnlyList<Failures>? observed = null;

                    _ = source.IfFailThen(failures =>
                    {
                        observed = failures;
                        return FailOr.Success(1);
                    });

                    return Task.FromResult(observed!);
                }
            );

        yield return () =>
            (
                "IfFailThenAsync deferred failures-aware",
                async source =>
                {
                    IReadOnlyList<Failures>? observed = null;

                    await source.IfFailThenAsync(failures =>
                    {
                        observed = failures;
                        return Task.FromResult(FailOr.Success(1));
                    });

                    return observed!;
                }
            );
    }

    public static IEnumerable<
        Func<(string Operation, Action Invoke, string ParameterName)>
    > DirectIfFailThenNullSelectorCases()
    {
        yield return () =>
            (
                "IfFailThen deferred",
                () => FailOr.Success(1).IfFailThen((Func<FailOr<int>>)null!),
                "alternative"
            );
        yield return () =>
            (
                "IfFailThen deferred failures-aware",
                () =>
                    FailOr.Success(1).IfFailThen((Func<IReadOnlyList<Failures>, FailOr<int>>)null!),
                "alternative"
            );
        yield return () =>
            (
                "IfFailThenAsync deferred",
                () => FailOr.Success(1).IfFailThenAsync((Func<Task<FailOr<int>>>)null!),
                "alternativeAsync"
            );
        yield return () =>
            (
                "IfFailThenAsync deferred failures-aware",
                () =>
                    FailOr
                        .Success(1)
                        .IfFailThenAsync((Func<IReadOnlyList<Failures>, Task<FailOr<int>>>)null!),
                "alternativeAsync"
            );
    }

    public static IEnumerable<
        Func<(string Operation, Func<Task> Invoke, string ParameterName)>
    > DirectIfFailThenNullTaskCases()
    {
        yield return () =>
            (
                "IfFailThenAsync deferred",
                () => FailOr.Fail<int>(Failure.General("failed")).IfFailThenAsync(() => null!),
                "resultTask"
            );
        yield return () =>
            (
                "IfFailThenAsync deferred failures-aware",
                () => FailOr.Fail<int>(Failure.General("failed")).IfFailThenAsync(_ => null!),
                "resultTask"
            );
    }

    public static IEnumerable<
        Func<(
            string Operation,
            Func<Task<FailOr<int>>, InvocationCounter, Task<FailOr<int>>> Invoke,
            int ExpectedCalls
        )>
    > LiftedIfFailThenSuccessCases()
    {
        yield return () =>
            ("IfFailThen result", (sourceTask, _) => sourceTask.IfFailThen(FailOr.Success(99)), 0);
        yield return () =>
            (
                "IfFailThen deferred",
                (sourceTask, counter) =>
                    sourceTask.IfFailThen(() => FailOr.Success(counter.Increment())),
                0
            );
        yield return () =>
            (
                "IfFailThen deferred failures-aware",
                (sourceTask, counter) =>
                    sourceTask.IfFailThen(failures =>
                        FailOr.Success(failures.Count + counter.Increment())
                    ),
                0
            );
        yield return () =>
            (
                "IfFailThenAsync deferred",
                (sourceTask, counter) =>
                    sourceTask.IfFailThenAsync(() =>
                        Task.FromResult(FailOr.Success(counter.Increment()))
                    ),
                0
            );
        yield return () =>
            (
                "IfFailThenAsync deferred failures-aware",
                (sourceTask, counter) =>
                    sourceTask.IfFailThenAsync(failures =>
                        Task.FromResult(FailOr.Success(failures.Count + counter.Increment()))
                    ),
                0
            );
    }

    public static IEnumerable<
        Func<(
            string Operation,
            Func<Task<FailOr<int>>, InvocationCounter, Task<FailOr<int>>> Invoke,
            int ExpectedCalls,
            int ExpectedValue
        )>
    > LiftedIfFailThenFailureCases()
    {
        yield return () =>
            (
                "IfFailThen result",
                (sourceTask, _) => sourceTask.IfFailThen(FailOr.Success(90)),
                0,
                90
            );
        yield return () =>
            (
                "IfFailThen deferred",
                (sourceTask, counter) =>
                    sourceTask.IfFailThen(() => FailOr.Success(40 + counter.Increment())),
                1,
                41
            );
        yield return () =>
            (
                "IfFailThen deferred failures-aware",
                (sourceTask, counter) =>
                    sourceTask.IfFailThen(failures =>
                        FailOr.Success(50 + failures.Count + counter.Increment())
                    ),
                1,
                52
            );
        yield return () =>
            (
                "IfFailThenAsync deferred",
                (sourceTask, counter) =>
                    sourceTask.IfFailThenAsync(() =>
                        Task.FromResult(FailOr.Success(60 + counter.Increment()))
                    ),
                1,
                61
            );
        yield return () =>
            (
                "IfFailThenAsync deferred failures-aware",
                (sourceTask, counter) =>
                    sourceTask.IfFailThenAsync(failures =>
                        Task.FromResult(FailOr.Success(70 + failures.Count + counter.Increment()))
                    ),
                1,
                72
            );
    }

    public static IEnumerable<
        Func<(string Operation, Func<Task<FailOr<int>>, Task<IReadOnlyList<Failures>>> Capture)>
    > LiftedIfFailThenFailuresAwareCases()
    {
        yield return () =>
            (
                "IfFailThen deferred failures-aware",
                async sourceTask =>
                {
                    IReadOnlyList<Failures>? observed = null;

                    await sourceTask.IfFailThen(failures =>
                    {
                        observed = failures;
                        return FailOr.Success(1);
                    });

                    return observed!;
                }
            );

        yield return () =>
            (
                "IfFailThenAsync deferred failures-aware",
                async sourceTask =>
                {
                    IReadOnlyList<Failures>? observed = null;

                    await sourceTask.IfFailThenAsync(failures =>
                    {
                        observed = failures;
                        return Task.FromResult(FailOr.Success(1));
                    });

                    return observed!;
                }
            );
    }

    public static IEnumerable<
        Func<(string Operation, Action Invoke, string ParameterName)>
    > LiftedIfFailThenNullSourceCases()
    {
        yield return () =>
            (
                "IfFailThen result",
                () => ((Task<FailOr<int>>)null!).IfFailThen(FailOr.Success(1)),
                "sourceTask"
            );
        yield return () =>
            (
                "IfFailThen deferred",
                () => ((Task<FailOr<int>>)null!).IfFailThen(() => FailOr.Success(1)),
                "sourceTask"
            );
        yield return () =>
            (
                "IfFailThen deferred failures-aware",
                () => ((Task<FailOr<int>>)null!).IfFailThen(_ => FailOr.Success(1)),
                "sourceTask"
            );
        yield return () =>
            (
                "IfFailThenAsync deferred",
                () =>
                    ((Task<FailOr<int>>)null!).IfFailThenAsync(() =>
                        Task.FromResult(FailOr.Success(1))
                    ),
                "sourceTask"
            );
        yield return () =>
            (
                "IfFailThenAsync deferred failures-aware",
                () =>
                    ((Task<FailOr<int>>)null!).IfFailThenAsync(_ =>
                        Task.FromResult(FailOr.Success(1))
                    ),
                "sourceTask"
            );
    }

    public static IEnumerable<
        Func<(string Operation, Action Invoke, string ParameterName)>
    > LiftedIfFailThenNullSelectorCases()
    {
        yield return () =>
            (
                "IfFailThen deferred",
                () => Task.FromResult(FailOr.Success(1)).IfFailThen((Func<FailOr<int>>)null!),
                "alternative"
            );
        yield return () =>
            (
                "IfFailThen deferred failures-aware",
                () =>
                    Task.FromResult(FailOr.Success(1))
                        .IfFailThen((Func<IReadOnlyList<Failures>, FailOr<int>>)null!),
                "alternative"
            );
        yield return () =>
            (
                "IfFailThenAsync deferred",
                () =>
                    Task.FromResult(FailOr.Success(1))
                        .IfFailThenAsync((Func<Task<FailOr<int>>>)null!),
                "alternativeAsync"
            );
        yield return () =>
            (
                "IfFailThenAsync deferred failures-aware",
                () =>
                    Task.FromResult(FailOr.Success(1))
                        .IfFailThenAsync((Func<IReadOnlyList<Failures>, Task<FailOr<int>>>)null!),
                "alternativeAsync"
            );
    }

    public static IEnumerable<
        Func<(string Operation, Func<Task> Invoke, string ParameterName)>
    > LiftedIfFailThenNullTaskCases()
    {
        yield return () =>
            (
                "IfFailThenAsync deferred",
                () =>
                    Task.FromResult(FailOr.Fail<int>(Failure.General("failed")))
                        .IfFailThenAsync(() => null!),
                "resultTask"
            );
        yield return () =>
            (
                "IfFailThenAsync deferred failures-aware",
                () =>
                    Task.FromResult(FailOr.Fail<int>(Failure.General("failed")))
                        .IfFailThenAsync(_ => null!),
                "resultTask"
            );
    }

    public static IEnumerable<
        Func<(
            string Operation,
            FailOr<int> Source,
            Func<FailOr<int>, Task<FailOr<int>>> Direct,
            Func<Task<FailOr<int>>, Task<FailOr<int>>> Lifted
        )>
    > LiftedIfFailThenParityCases()
    {
        yield return () =>
            (
                "IfFailThen result success",
                FailOr.Success(1),
                source => Task.FromResult(source.IfFailThen(FailOr.Success(90))),
                sourceTask => sourceTask.IfFailThen(FailOr.Success(90))
            );
        yield return () =>
            (
                "IfFailThen deferred success",
                FailOr.Success(1),
                source => Task.FromResult(source.IfFailThen(() => FailOr.Success(90))),
                sourceTask => sourceTask.IfFailThen(() => FailOr.Success(90))
            );
        yield return () =>
            (
                "IfFailThen deferred failures-aware success",
                FailOr.Success(1),
                source => Task.FromResult(source.IfFailThen(_ => FailOr.Success(90))),
                sourceTask => sourceTask.IfFailThen(_ => FailOr.Success(90))
            );
        yield return () =>
            (
                "IfFailThenAsync deferred success",
                FailOr.Success(1),
                source => source.IfFailThenAsync(() => Task.FromResult(FailOr.Success(90))),
                sourceTask => sourceTask.IfFailThenAsync(() => Task.FromResult(FailOr.Success(90)))
            );
        yield return () =>
            (
                "IfFailThenAsync deferred failures-aware success",
                FailOr.Success(1),
                source => source.IfFailThenAsync(_ => Task.FromResult(FailOr.Success(90))),
                sourceTask => sourceTask.IfFailThenAsync(_ => Task.FromResult(FailOr.Success(90)))
            );

        yield return () =>
            (
                "IfFailThen result failure",
                FailOr.Fail<int>(Failure.General("failed")),
                source => Task.FromResult(source.IfFailThen(FailOr.Success(90))),
                sourceTask => sourceTask.IfFailThen(FailOr.Success(90))
            );
        yield return () =>
            (
                "IfFailThen deferred failure",
                FailOr.Fail<int>(Failure.General("failed")),
                source => Task.FromResult(source.IfFailThen(() => FailOr.Success(90))),
                sourceTask => sourceTask.IfFailThen(() => FailOr.Success(90))
            );
        yield return () =>
            (
                "IfFailThen deferred failures-aware failure",
                FailOr.Fail<int>(Failure.General("failed")),
                source => Task.FromResult(source.IfFailThen(_ => FailOr.Success(90))),
                sourceTask => sourceTask.IfFailThen(_ => FailOr.Success(90))
            );
        yield return () =>
            (
                "IfFailThenAsync deferred failure",
                FailOr.Fail<int>(Failure.General("failed")),
                source => source.IfFailThenAsync(() => Task.FromResult(FailOr.Success(90))),
                sourceTask => sourceTask.IfFailThenAsync(() => Task.FromResult(FailOr.Success(90)))
            );
        yield return () =>
            (
                "IfFailThenAsync deferred failures-aware failure",
                FailOr.Fail<int>(Failure.General("failed")),
                source => source.IfFailThenAsync(_ => Task.FromResult(FailOr.Success(90))),
                sourceTask => sourceTask.IfFailThenAsync(_ => Task.FromResult(FailOr.Success(90)))
            );
    }
}

public sealed class InvocationCounter
{
    public int Calls { get; private set; }

    public int Increment() => ++Calls;
}

public static class ThenAssertions
{
    public static async Task AssertSuccess(FailOr<int> result, int expected)
    {
        using var _ = Assert.Multiple();

        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(result.IsFailure).IsFalse();
        await Assert.That(result.UnsafeUnwrap()).IsEqualTo(expected);
        await Assert.That(result.Failures.Count).IsEqualTo(0);
    }

    public static async Task AssertFailure(FailOr<int> result, params Failures[] expectedFailures)
    {
        using var _ = Assert.Multiple();

        await Assert.That(result.IsFailure).IsTrue();
        await Assert.That(result.IsSuccess).IsFalse();
        await Assert.That(result.Failures.Count).IsEqualTo(expectedFailures.Length);

        for (var index = 0; index < expectedFailures.Length; index++)
        {
            await Assert.That(result.Failures[index]).IsEqualTo(expectedFailures[index]);
        }
    }

    public static async Task AssertEquivalent(FailOr<int> actual, FailOr<int> expected)
    {
        using var _ = Assert.Multiple();

        await Assert.That(actual.IsSuccess).IsEqualTo(expected.IsSuccess);
        await Assert.That(actual.IsFailure).IsEqualTo(expected.IsFailure);
        await Assert.That(actual.Failures.Count).IsEqualTo(expected.Failures.Count);

        if (expected.IsSuccess)
        {
            await Assert.That(actual.UnsafeUnwrap()).IsEqualTo(expected.UnsafeUnwrap());
            return;
        }

        for (var index = 0; index < expected.Failures.Count; index++)
        {
            await Assert.That(actual.Failures[index]).IsEqualTo(expected.Failures[index]);
        }
    }
}

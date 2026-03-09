namespace FailOr.Tests;

public static class IfSuccessTestData
{
    public static IEnumerable<
        Func<(string Operation, Action<FailOr<int>, InvocationCounter> Invoke)>
    > DirectFailureShortCircuitCases()
    {
        yield return () =>
            (
                "IfSuccess",
                (source, counter) =>
                    source.IfSuccess(_ =>
                    {
                        counter.Increment();
                    })
            );
    }

    public static IEnumerable<
        Func<(string Operation, Func<FailOr<int>, InvocationCounter, Task> Invoke)>
    > DirectAsyncFailureShortCircuitCases()
    {
        yield return () =>
            (
                "IfSuccessAsync",
                (source, counter) =>
                    source.IfSuccessAsync(_ =>
                    {
                        counter.Increment();
                        return Task.CompletedTask;
                    })
            );
    }

    public static IEnumerable<
        Func<(string Operation, Action Invoke, string ParameterName)>
    > DirectNullDelegateCases()
    {
        yield return () =>
            ("IfSuccess", () => FailOr.Success(1).IfSuccess((Action<int>)null!), "action");
        yield return () =>
            (
                "IfSuccessAsync",
                () => FailOr.Success(1).IfSuccessAsync((Func<int, Task>)null!),
                "actionAsync"
            );
    }

    public static IEnumerable<
        Func<(string Operation, Func<Task> Invoke, string ParameterName)>
    > DirectNullTaskCases()
    {
        yield return () =>
            ("IfSuccessAsync", () => FailOr.Success(1).IfSuccessAsync(_ => null!), "resultTask");
    }

    public static IEnumerable<
        Func<(string Operation, Func<Task<FailOr<int>>, InvocationCounter, Task> Invoke)>
    > LiftedFailureShortCircuitCases()
    {
        yield return () =>
            (
                "IfSuccess",
                (sourceTask, counter) =>
                    sourceTask.IfSuccess(_ =>
                    {
                        counter.Increment();
                    })
            );
        yield return () =>
            (
                "IfSuccessAsync",
                (sourceTask, counter) =>
                    sourceTask.IfSuccessAsync(_ =>
                    {
                        counter.Increment();
                        return Task.CompletedTask;
                    })
            );
    }

    public static IEnumerable<
        Func<(string Operation, Action Invoke, string ParameterName)>
    > LiftedNullDelegateCases()
    {
        yield return () =>
            (
                "IfSuccess",
                () => Task.FromResult(FailOr.Success(1)).IfSuccess((Action<int>)null!),
                "action"
            );
        yield return () =>
            (
                "IfSuccessAsync",
                () => Task.FromResult(FailOr.Success(1)).IfSuccessAsync((Func<int, Task>)null!),
                "actionAsync"
            );
    }

    public static IEnumerable<
        Func<(string Operation, Action Invoke, string ParameterName)>
    > LiftedNullSourceCases()
    {
        yield return () =>
            ("IfSuccess", () => ((Task<FailOr<int>>)null!).IfSuccess(_ => { }), "sourceTask");
        yield return () =>
            (
                "IfSuccessAsync",
                () => ((Task<FailOr<int>>)null!).IfSuccessAsync(_ => Task.CompletedTask),
                "sourceTask"
            );
    }

    public static IEnumerable<
        Func<(string Operation, Func<Task> Invoke, string ParameterName)>
    > LiftedNullTaskCases()
    {
        yield return () =>
            (
                "IfSuccessAsync",
                () => Task.FromResult(FailOr.Success(1)).IfSuccessAsync(_ => null!),
                "resultTask"
            );
    }

    public static IEnumerable<
        Func<(
            string Operation,
            Func<FailOr<int>, InvocationCounter, Task> Direct,
            Func<Task<FailOr<int>>, InvocationCounter, Task> Lifted
        )>
    > LiftedParityCases()
    {
        yield return () =>
            (
                "IfSuccess",
                (source, counter) =>
                {
                    source.IfSuccess(_ =>
                    {
                        counter.Increment();
                    });
                    return Task.CompletedTask;
                },
                (sourceTask, counter) =>
                    sourceTask.IfSuccess(_ =>
                    {
                        counter.Increment();
                    })
            );
        yield return () =>
            (
                "IfSuccessAsync",
                (source, counter) =>
                    source.IfSuccessAsync(_ =>
                    {
                        counter.Increment();
                        return Task.CompletedTask;
                    }),
                (sourceTask, counter) =>
                    sourceTask.IfSuccessAsync(_ =>
                    {
                        counter.Increment();
                        return Task.CompletedTask;
                    })
            );
    }
}

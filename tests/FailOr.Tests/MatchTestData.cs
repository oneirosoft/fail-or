using TUnit.Assertions;

namespace FailOr.Tests;

public static class MatchTestData
{
    public static IEnumerable<
        Func<(
            string Operation,
            Func<FailOr<int>, MatchBranchTracker, Task<int>> Invoke,
            int Expected
        )>
    > DirectMatchSuccessCases()
    {
        yield return () =>
            (
                "Match",
                (source, tracker) =>
                    Task.FromResult(
                        source.Match(
                            success: value =>
                            {
                                tracker.CaptureSuccess();
                                return value + 1;
                            },
                            failure: failures =>
                            {
                                tracker.CaptureFailures(failures);
                                return failures.Count;
                            }
                        )
                    ),
                2
            );

        yield return () =>
            (
                "MatchAsync success async",
                (source, tracker) =>
                    source.MatchAsync(
                        successAsync: value =>
                        {
                            tracker.CaptureSuccess();
                            return Task.FromResult(value + 1);
                        },
                        failure: failures =>
                        {
                            tracker.CaptureFailures(failures);
                            return failures.Count;
                        }
                    ),
                2
            );

        yield return () =>
            (
                "MatchAsync failure async",
                (source, tracker) =>
                    source.MatchAsync(
                        success: value =>
                        {
                            tracker.CaptureSuccess();
                            return value + 1;
                        },
                        failureAsync: failures =>
                        {
                            tracker.CaptureFailures(failures);
                            return Task.FromResult(failures.Count);
                        }
                    ),
                2
            );

        yield return () =>
            (
                "MatchAsync both async",
                (source, tracker) =>
                    source.MatchAsync(
                        successAsync: value =>
                        {
                            tracker.CaptureSuccess();
                            return Task.FromResult(value + 1);
                        },
                        failureAsync: failures =>
                        {
                            tracker.CaptureFailures(failures);
                            return Task.FromResult(failures.Count);
                        }
                    ),
                2
            );
    }

    public static IEnumerable<
        Func<(
            string Operation,
            Func<FailOr<int>, MatchBranchTracker, Task<int>> Invoke,
            int Expected
        )>
    > DirectMatchFailureCases()
    {
        yield return () =>
            (
                "Match",
                (source, tracker) =>
                    Task.FromResult(
                        source.Match(
                            success: value =>
                            {
                                tracker.CaptureSuccess();
                                return value + 1;
                            },
                            failure: failures =>
                            {
                                tracker.CaptureFailures(failures);
                                return failures.Count;
                            }
                        )
                    ),
                2
            );

        yield return () =>
            (
                "MatchAsync success async",
                (source, tracker) =>
                    source.MatchAsync(
                        successAsync: value =>
                        {
                            tracker.CaptureSuccess();
                            return Task.FromResult(value + 1);
                        },
                        failure: failures =>
                        {
                            tracker.CaptureFailures(failures);
                            return failures.Count;
                        }
                    ),
                2
            );

        yield return () =>
            (
                "MatchAsync failure async",
                (source, tracker) =>
                    source.MatchAsync(
                        success: value =>
                        {
                            tracker.CaptureSuccess();
                            return value + 1;
                        },
                        failureAsync: failures =>
                        {
                            tracker.CaptureFailures(failures);
                            return Task.FromResult(failures.Count);
                        }
                    ),
                2
            );

        yield return () =>
            (
                "MatchAsync both async",
                (source, tracker) =>
                    source.MatchAsync(
                        successAsync: value =>
                        {
                            tracker.CaptureSuccess();
                            return Task.FromResult(value + 1);
                        },
                        failureAsync: failures =>
                        {
                            tracker.CaptureFailures(failures);
                            return Task.FromResult(failures.Count);
                        }
                    ),
                2
            );
    }

    public static IEnumerable<
        Func<(
            string Operation,
            Func<FailOr<int>, MatchBranchTracker, Task<int>> Invoke,
            int Expected
        )>
    > DirectMatchFirstSuccessCases()
    {
        yield return () =>
            (
                "MatchFirst",
                (source, tracker) =>
                    Task.FromResult(
                        source.MatchFirst(
                            success: value =>
                            {
                                tracker.CaptureSuccess();
                                return value + 1;
                            },
                            failure: failure =>
                            {
                                tracker.CaptureFailure(failure);
                                return failure.Details.Length;
                            }
                        )
                    ),
                2
            );

        yield return () =>
            (
                "MatchFirstAsync success async",
                (source, tracker) =>
                    source.MatchFirstAsync(
                        successAsync: value =>
                        {
                            tracker.CaptureSuccess();
                            return Task.FromResult(value + 1);
                        },
                        failure: failure =>
                        {
                            tracker.CaptureFailure(failure);
                            return failure.Details.Length;
                        }
                    ),
                2
            );

        yield return () =>
            (
                "MatchFirstAsync failure async",
                (source, tracker) =>
                    source.MatchFirstAsync(
                        success: value =>
                        {
                            tracker.CaptureSuccess();
                            return value + 1;
                        },
                        failureAsync: failure =>
                        {
                            tracker.CaptureFailure(failure);
                            return Task.FromResult(failure.Details.Length);
                        }
                    ),
                2
            );

        yield return () =>
            (
                "MatchFirstAsync both async",
                (source, tracker) =>
                    source.MatchFirstAsync(
                        successAsync: value =>
                        {
                            tracker.CaptureSuccess();
                            return Task.FromResult(value + 1);
                        },
                        failureAsync: failure =>
                        {
                            tracker.CaptureFailure(failure);
                            return Task.FromResult(failure.Details.Length);
                        }
                    ),
                2
            );
    }

    public static IEnumerable<
        Func<(
            string Operation,
            Func<FailOr<int>, MatchBranchTracker, Task<int>> Invoke,
            int Expected
        )>
    > DirectMatchFirstFailureCases()
    {
        yield return () =>
            (
                "MatchFirst",
                (source, tracker) =>
                    Task.FromResult(
                        source.MatchFirst(
                            success: value =>
                            {
                                tracker.CaptureSuccess();
                                return value + 1;
                            },
                            failure: failure =>
                            {
                                tracker.CaptureFailure(failure);
                                return failure.Details.Length;
                            }
                        )
                    ),
                5
            );

        yield return () =>
            (
                "MatchFirstAsync success async",
                (source, tracker) =>
                    source.MatchFirstAsync(
                        successAsync: value =>
                        {
                            tracker.CaptureSuccess();
                            return Task.FromResult(value + 1);
                        },
                        failure: failure =>
                        {
                            tracker.CaptureFailure(failure);
                            return failure.Details.Length;
                        }
                    ),
                5
            );

        yield return () =>
            (
                "MatchFirstAsync failure async",
                (source, tracker) =>
                    source.MatchFirstAsync(
                        success: value =>
                        {
                            tracker.CaptureSuccess();
                            return value + 1;
                        },
                        failureAsync: failure =>
                        {
                            tracker.CaptureFailure(failure);
                            return Task.FromResult(failure.Details.Length);
                        }
                    ),
                5
            );

        yield return () =>
            (
                "MatchFirstAsync both async",
                (source, tracker) =>
                    source.MatchFirstAsync(
                        successAsync: value =>
                        {
                            tracker.CaptureSuccess();
                            return Task.FromResult(value + 1);
                        },
                        failureAsync: failure =>
                        {
                            tracker.CaptureFailure(failure);
                            return Task.FromResult(failure.Details.Length);
                        }
                    ),
                5
            );
    }

    public static IEnumerable<
        Func<(string Operation, Action Invoke, string ParameterName)>
    > DirectNullSelectorCases()
    {
        yield return () =>
            (
                "Match success",
                () => FailOr.Success(1).Match((Func<int, int>)null!, _ => 0),
                "success"
            );
        yield return () =>
            (
                "Match failure",
                () => FailOr.Success(1).Match(_ => 0, (Func<IReadOnlyList<Failures>, int>)null!),
                "failure"
            );
        yield return () =>
            (
                "MatchAsync success async",
                () => FailOr.Success(1).MatchAsync((Func<int, Task<int>>)null!, _ => 0),
                "successAsync"
            );
        yield return () =>
            (
                "MatchAsync failure",
                () =>
                    FailOr
                        .Success(1)
                        .MatchAsync(
                            _ => Task.FromResult(0),
                            (Func<IReadOnlyList<Failures>, int>)null!
                        ),
                "failure"
            );
        yield return () =>
            (
                "MatchAsync success",
                () => FailOr.Success(1).MatchAsync((Func<int, int>)null!, _ => Task.FromResult(0)),
                "success"
            );
        yield return () =>
            (
                "MatchAsync failure async",
                () =>
                    FailOr
                        .Success(1)
                        .MatchAsync(_ => 0, (Func<IReadOnlyList<Failures>, Task<int>>)null!),
                "failureAsync"
            );
        yield return () =>
            (
                "MatchAsync both async success",
                () =>
                    FailOr
                        .Success(1)
                        .MatchAsync((Func<int, Task<int>>)null!, _ => Task.FromResult(0)),
                "successAsync"
            );
        yield return () =>
            (
                "MatchAsync both async failure",
                () =>
                    FailOr
                        .Success(1)
                        .MatchAsync(
                            _ => Task.FromResult(0),
                            (Func<IReadOnlyList<Failures>, Task<int>>)null!
                        ),
                "failureAsync"
            );
        yield return () =>
            (
                "MatchFirst success",
                () => FailOr.Success(1).MatchFirst((Func<int, int>)null!, _ => 0),
                "success"
            );
        yield return () =>
            (
                "MatchFirst failure",
                () => FailOr.Success(1).MatchFirst(_ => 0, (Func<Failures, int>)null!),
                "failure"
            );
        yield return () =>
            (
                "MatchFirstAsync success async",
                () => FailOr.Success(1).MatchFirstAsync((Func<int, Task<int>>)null!, _ => 0),
                "successAsync"
            );
        yield return () =>
            (
                "MatchFirstAsync failure",
                () =>
                    FailOr
                        .Success(1)
                        .MatchFirstAsync(_ => Task.FromResult(0), (Func<Failures, int>)null!),
                "failure"
            );
        yield return () =>
            (
                "MatchFirstAsync success",
                () =>
                    FailOr
                        .Success(1)
                        .MatchFirstAsync((Func<int, int>)null!, _ => Task.FromResult(0)),
                "success"
            );
        yield return () =>
            (
                "MatchFirstAsync failure async",
                () => FailOr.Success(1).MatchFirstAsync(_ => 0, (Func<Failures, Task<int>>)null!),
                "failureAsync"
            );
        yield return () =>
            (
                "MatchFirstAsync both async success",
                () =>
                    FailOr
                        .Success(1)
                        .MatchFirstAsync((Func<int, Task<int>>)null!, _ => Task.FromResult(0)),
                "successAsync"
            );
        yield return () =>
            (
                "MatchFirstAsync both async failure",
                () =>
                    FailOr
                        .Success(1)
                        .MatchFirstAsync(_ => Task.FromResult(0), (Func<Failures, Task<int>>)null!),
                "failureAsync"
            );
    }

    public static IEnumerable<Func<(string Operation, Action Invoke)>> DirectNullReturnedTaskCases()
    {
        yield return () =>
            (
                "MatchAsync success async",
                () => FailOr.Success(1).MatchAsync(_ => (Task<int>)null!, _ => 0)
            );
        yield return () =>
            (
                "MatchAsync failure async",
                () =>
                    FailOr
                        .Fail<int>(Failure.General("first"), Failure.General("second"))
                        .MatchAsync(_ => 0, _ => (Task<int>)null!)
            );
        yield return () =>
            (
                "MatchAsync both async success",
                () => FailOr.Success(1).MatchAsync(_ => (Task<int>)null!, _ => Task.FromResult(0))
            );
        yield return () =>
            (
                "MatchAsync both async failure",
                () =>
                    FailOr
                        .Fail<int>(Failure.General("first"), Failure.General("second"))
                        .MatchAsync(_ => Task.FromResult(0), _ => (Task<int>)null!)
            );
        yield return () =>
            (
                "MatchFirstAsync success async",
                () => FailOr.Success(1).MatchFirstAsync(_ => (Task<int>)null!, _ => 0)
            );
        yield return () =>
            (
                "MatchFirstAsync failure async",
                () =>
                    FailOr
                        .Fail<int>(Failure.General("first"), Failure.General("second"))
                        .MatchFirstAsync(_ => 0, _ => (Task<int>)null!)
            );
        yield return () =>
            (
                "MatchFirstAsync both async success",
                () =>
                    FailOr
                        .Success(1)
                        .MatchFirstAsync(_ => (Task<int>)null!, _ => Task.FromResult(0))
            );
        yield return () =>
            (
                "MatchFirstAsync both async failure",
                () =>
                    FailOr
                        .Fail<int>(Failure.General("first"), Failure.General("second"))
                        .MatchFirstAsync(_ => Task.FromResult(0), _ => (Task<int>)null!)
            );
    }

    public static IEnumerable<
        Func<(
            string Operation,
            Func<FailOr<int>, MatchBranchTracker, Task<int>> Direct,
            Func<Task<FailOr<int>>, MatchBranchTracker, Task<int>> Lifted,
            int Expected
        )>
    > LiftedMatchSuccessCases()
    {
        yield return () =>
            (
                "Match",
                DirectMatch(successAsync: false, failureAsync: false),
                LiftedMatch(successAsync: false, failureAsync: false),
                2
            );
        yield return () =>
            (
                "MatchAsync success async",
                DirectMatch(successAsync: true, failureAsync: false),
                LiftedMatch(successAsync: true, failureAsync: false),
                2
            );
        yield return () =>
            (
                "MatchAsync failure async",
                DirectMatch(successAsync: false, failureAsync: true),
                LiftedMatch(successAsync: false, failureAsync: true),
                2
            );
        yield return () =>
            (
                "MatchAsync both async",
                DirectMatch(successAsync: true, failureAsync: true),
                LiftedMatch(successAsync: true, failureAsync: true),
                2
            );
    }

    public static IEnumerable<
        Func<(
            string Operation,
            Func<FailOr<int>, MatchBranchTracker, Task<int>> Direct,
            Func<Task<FailOr<int>>, MatchBranchTracker, Task<int>> Lifted,
            int Expected
        )>
    > LiftedMatchFailureCases()
    {
        yield return () =>
            (
                "Match",
                DirectMatch(successAsync: false, failureAsync: false),
                LiftedMatch(successAsync: false, failureAsync: false),
                2
            );
        yield return () =>
            (
                "MatchAsync success async",
                DirectMatch(successAsync: true, failureAsync: false),
                LiftedMatch(successAsync: true, failureAsync: false),
                2
            );
        yield return () =>
            (
                "MatchAsync failure async",
                DirectMatch(successAsync: false, failureAsync: true),
                LiftedMatch(successAsync: false, failureAsync: true),
                2
            );
        yield return () =>
            (
                "MatchAsync both async",
                DirectMatch(successAsync: true, failureAsync: true),
                LiftedMatch(successAsync: true, failureAsync: true),
                2
            );
    }

    public static IEnumerable<
        Func<(
            string Operation,
            Func<FailOr<int>, MatchBranchTracker, Task<int>> Direct,
            Func<Task<FailOr<int>>, MatchBranchTracker, Task<int>> Lifted,
            int Expected
        )>
    > LiftedMatchFirstSuccessCases()
    {
        yield return () =>
            (
                "MatchFirst",
                DirectMatchFirst(successAsync: false, failureAsync: false),
                LiftedMatchFirst(successAsync: false, failureAsync: false),
                2
            );
        yield return () =>
            (
                "MatchFirstAsync success async",
                DirectMatchFirst(successAsync: true, failureAsync: false),
                LiftedMatchFirst(successAsync: true, failureAsync: false),
                2
            );
        yield return () =>
            (
                "MatchFirstAsync failure async",
                DirectMatchFirst(successAsync: false, failureAsync: true),
                LiftedMatchFirst(successAsync: false, failureAsync: true),
                2
            );
        yield return () =>
            (
                "MatchFirstAsync both async",
                DirectMatchFirst(successAsync: true, failureAsync: true),
                LiftedMatchFirst(successAsync: true, failureAsync: true),
                2
            );
    }

    public static IEnumerable<
        Func<(
            string Operation,
            Func<FailOr<int>, MatchBranchTracker, Task<int>> Direct,
            Func<Task<FailOr<int>>, MatchBranchTracker, Task<int>> Lifted,
            int Expected
        )>
    > LiftedMatchFirstFailureCases()
    {
        yield return () =>
            (
                "MatchFirst",
                DirectMatchFirst(successAsync: false, failureAsync: false),
                LiftedMatchFirst(successAsync: false, failureAsync: false),
                5
            );
        yield return () =>
            (
                "MatchFirstAsync success async",
                DirectMatchFirst(successAsync: true, failureAsync: false),
                LiftedMatchFirst(successAsync: true, failureAsync: false),
                5
            );
        yield return () =>
            (
                "MatchFirstAsync failure async",
                DirectMatchFirst(successAsync: false, failureAsync: true),
                LiftedMatchFirst(successAsync: false, failureAsync: true),
                5
            );
        yield return () =>
            (
                "MatchFirstAsync both async",
                DirectMatchFirst(successAsync: true, failureAsync: true),
                LiftedMatchFirst(successAsync: true, failureAsync: true),
                5
            );
    }

    public static IEnumerable<
        Func<(string Operation, Action Invoke, string ParameterName)>
    > LiftedNullSelectorCases()
    {
        yield return () =>
            (
                "Match sourceTask",
                () => ((Task<FailOr<int>>)null!).Match(_ => 0, _ => 0),
                "sourceTask"
            );
        yield return () =>
            (
                "Match success",
                () => Task.FromResult(FailOr.Success(1)).Match((Func<int, int>)null!, _ => 0),
                "success"
            );
        yield return () =>
            (
                "Match failure",
                () =>
                    Task.FromResult(FailOr.Success(1))
                        .Match(_ => 0, (Func<IReadOnlyList<Failures>, int>)null!),
                "failure"
            );
        yield return () =>
            (
                "MatchAsync sourceTask success async",
                () => ((Task<FailOr<int>>)null!).MatchAsync(_ => Task.FromResult(0), _ => 0),
                "sourceTask"
            );
        yield return () =>
            (
                "MatchAsync success async",
                () =>
                    Task.FromResult(FailOr.Success(1))
                        .MatchAsync((Func<int, Task<int>>)null!, _ => 0),
                "successAsync"
            );
        yield return () =>
            (
                "MatchAsync failure",
                () =>
                    Task.FromResult(FailOr.Success(1))
                        .MatchAsync(
                            _ => Task.FromResult(0),
                            (Func<IReadOnlyList<Failures>, int>)null!
                        ),
                "failure"
            );
        yield return () =>
            (
                "MatchAsync sourceTask failure async",
                () => ((Task<FailOr<int>>)null!).MatchAsync(_ => 0, _ => Task.FromResult(0)),
                "sourceTask"
            );
        yield return () =>
            (
                "MatchAsync success",
                () =>
                    Task.FromResult(FailOr.Success(1))
                        .MatchAsync((Func<int, int>)null!, _ => Task.FromResult(0)),
                "success"
            );
        yield return () =>
            (
                "MatchAsync failure async",
                () =>
                    Task.FromResult(FailOr.Success(1))
                        .MatchAsync(_ => 0, (Func<IReadOnlyList<Failures>, Task<int>>)null!),
                "failureAsync"
            );
        yield return () =>
            (
                "MatchAsync sourceTask both async",
                () =>
                    ((Task<FailOr<int>>)null!).MatchAsync(
                        _ => Task.FromResult(0),
                        _ => Task.FromResult(0)
                    ),
                "sourceTask"
            );
        yield return () =>
            (
                "MatchAsync both async success",
                () =>
                    Task.FromResult(FailOr.Success(1))
                        .MatchAsync((Func<int, Task<int>>)null!, _ => Task.FromResult(0)),
                "successAsync"
            );
        yield return () =>
            (
                "MatchAsync both async failure",
                () =>
                    Task.FromResult(FailOr.Success(1))
                        .MatchAsync(
                            _ => Task.FromResult(0),
                            (Func<IReadOnlyList<Failures>, Task<int>>)null!
                        ),
                "failureAsync"
            );
        yield return () =>
            (
                "MatchFirst sourceTask",
                () => ((Task<FailOr<int>>)null!).MatchFirst(_ => 0, _ => 0),
                "sourceTask"
            );
        yield return () =>
            (
                "MatchFirst success",
                () => Task.FromResult(FailOr.Success(1)).MatchFirst((Func<int, int>)null!, _ => 0),
                "success"
            );
        yield return () =>
            (
                "MatchFirst failure",
                () =>
                    Task.FromResult(FailOr.Success(1))
                        .MatchFirst(_ => 0, (Func<Failures, int>)null!),
                "failure"
            );
        yield return () =>
            (
                "MatchFirstAsync sourceTask success async",
                () => ((Task<FailOr<int>>)null!).MatchFirstAsync(_ => Task.FromResult(0), _ => 0),
                "sourceTask"
            );
        yield return () =>
            (
                "MatchFirstAsync success async",
                () =>
                    Task.FromResult(FailOr.Success(1))
                        .MatchFirstAsync((Func<int, Task<int>>)null!, _ => 0),
                "successAsync"
            );
        yield return () =>
            (
                "MatchFirstAsync failure",
                () =>
                    Task.FromResult(FailOr.Success(1))
                        .MatchFirstAsync(_ => Task.FromResult(0), (Func<Failures, int>)null!),
                "failure"
            );
        yield return () =>
            (
                "MatchFirstAsync sourceTask failure async",
                () => ((Task<FailOr<int>>)null!).MatchFirstAsync(_ => 0, _ => Task.FromResult(0)),
                "sourceTask"
            );
        yield return () =>
            (
                "MatchFirstAsync success",
                () =>
                    Task.FromResult(FailOr.Success(1))
                        .MatchFirstAsync((Func<int, int>)null!, _ => Task.FromResult(0)),
                "success"
            );
        yield return () =>
            (
                "MatchFirstAsync failure async",
                () =>
                    Task.FromResult(FailOr.Success(1))
                        .MatchFirstAsync(_ => 0, (Func<Failures, Task<int>>)null!),
                "failureAsync"
            );
        yield return () =>
            (
                "MatchFirstAsync sourceTask both async",
                () =>
                    ((Task<FailOr<int>>)null!).MatchFirstAsync(
                        _ => Task.FromResult(0),
                        _ => Task.FromResult(0)
                    ),
                "sourceTask"
            );
        yield return () =>
            (
                "MatchFirstAsync both async success",
                () =>
                    Task.FromResult(FailOr.Success(1))
                        .MatchFirstAsync((Func<int, Task<int>>)null!, _ => Task.FromResult(0)),
                "successAsync"
            );
        yield return () =>
            (
                "MatchFirstAsync both async failure",
                () =>
                    Task.FromResult(FailOr.Success(1))
                        .MatchFirstAsync(_ => Task.FromResult(0), (Func<Failures, Task<int>>)null!),
                "failureAsync"
            );
    }

    public static IEnumerable<
        Func<(string Operation, Func<Task> Invoke)>
    > LiftedNullReturnedTaskCases()
    {
        yield return () =>
            (
                "MatchAsync success async",
                async () =>
                    await Task.FromResult(FailOr.Success(1))
                        .MatchAsync(_ => (Task<int>)null!, _ => 0)
            );
        yield return () =>
            (
                "MatchAsync failure async",
                async () =>
                    await Task.FromResult(
                            FailOr.Fail<int>(Failure.General("first"), Failure.General("second"))
                        )
                        .MatchAsync(_ => 0, _ => (Task<int>)null!)
            );
        yield return () =>
            (
                "MatchAsync both async success",
                async () =>
                    await Task.FromResult(FailOr.Success(1))
                        .MatchAsync(_ => (Task<int>)null!, _ => Task.FromResult(0))
            );
        yield return () =>
            (
                "MatchAsync both async failure",
                async () =>
                    await Task.FromResult(
                            FailOr.Fail<int>(Failure.General("first"), Failure.General("second"))
                        )
                        .MatchAsync(_ => Task.FromResult(0), _ => (Task<int>)null!)
            );
        yield return () =>
            (
                "MatchFirstAsync success async",
                async () =>
                    await Task.FromResult(FailOr.Success(1))
                        .MatchFirstAsync(_ => (Task<int>)null!, _ => 0)
            );
        yield return () =>
            (
                "MatchFirstAsync failure async",
                async () =>
                    await Task.FromResult(
                            FailOr.Fail<int>(Failure.General("first"), Failure.General("second"))
                        )
                        .MatchFirstAsync(_ => 0, _ => (Task<int>)null!)
            );
        yield return () =>
            (
                "MatchFirstAsync both async success",
                async () =>
                    await Task.FromResult(FailOr.Success(1))
                        .MatchFirstAsync(_ => (Task<int>)null!, _ => Task.FromResult(0))
            );
        yield return () =>
            (
                "MatchFirstAsync both async failure",
                async () =>
                    await Task.FromResult(
                            FailOr.Fail<int>(Failure.General("first"), Failure.General("second"))
                        )
                        .MatchFirstAsync(_ => Task.FromResult(0), _ => (Task<int>)null!)
            );
    }

    private static Func<FailOr<int>, MatchBranchTracker, Task<int>> DirectMatch(
        bool successAsync,
        bool failureAsync
    ) =>
        (source, tracker) =>
        {
            if (successAsync && failureAsync)
            {
                return source.MatchAsync(
                    successAsync: value =>
                    {
                        tracker.CaptureSuccess();
                        return Task.FromResult(value + 1);
                    },
                    failureAsync: failures =>
                    {
                        tracker.CaptureFailures(failures);
                        return Task.FromResult(failures.Count);
                    }
                );
            }

            if (successAsync)
            {
                return source.MatchAsync(
                    successAsync: value =>
                    {
                        tracker.CaptureSuccess();
                        return Task.FromResult(value + 1);
                    },
                    failure: failures =>
                    {
                        tracker.CaptureFailures(failures);
                        return failures.Count;
                    }
                );
            }

            if (failureAsync)
            {
                return source.MatchAsync(
                    success: value =>
                    {
                        tracker.CaptureSuccess();
                        return value + 1;
                    },
                    failureAsync: failures =>
                    {
                        tracker.CaptureFailures(failures);
                        return Task.FromResult(failures.Count);
                    }
                );
            }

            return Task.FromResult(
                source.Match(
                    success: value =>
                    {
                        tracker.CaptureSuccess();
                        return value + 1;
                    },
                    failure: failures =>
                    {
                        tracker.CaptureFailures(failures);
                        return failures.Count;
                    }
                )
            );
        };

    private static Func<Task<FailOr<int>>, MatchBranchTracker, Task<int>> LiftedMatch(
        bool successAsync,
        bool failureAsync
    ) =>
        (sourceTask, tracker) =>
        {
            if (successAsync && failureAsync)
            {
                return sourceTask.MatchAsync(
                    successAsync: value =>
                    {
                        tracker.CaptureSuccess();
                        return Task.FromResult(value + 1);
                    },
                    failureAsync: failures =>
                    {
                        tracker.CaptureFailures(failures);
                        return Task.FromResult(failures.Count);
                    }
                );
            }

            if (successAsync)
            {
                return sourceTask.MatchAsync(
                    successAsync: value =>
                    {
                        tracker.CaptureSuccess();
                        return Task.FromResult(value + 1);
                    },
                    failure: failures =>
                    {
                        tracker.CaptureFailures(failures);
                        return failures.Count;
                    }
                );
            }

            if (failureAsync)
            {
                return sourceTask.MatchAsync(
                    success: value =>
                    {
                        tracker.CaptureSuccess();
                        return value + 1;
                    },
                    failureAsync: failures =>
                    {
                        tracker.CaptureFailures(failures);
                        return Task.FromResult(failures.Count);
                    }
                );
            }

            return sourceTask.Match(
                success: value =>
                {
                    tracker.CaptureSuccess();
                    return value + 1;
                },
                failure: failures =>
                {
                    tracker.CaptureFailures(failures);
                    return failures.Count;
                }
            );
        };

    private static Func<FailOr<int>, MatchBranchTracker, Task<int>> DirectMatchFirst(
        bool successAsync,
        bool failureAsync
    ) =>
        (source, tracker) =>
        {
            if (successAsync && failureAsync)
            {
                return source.MatchFirstAsync(
                    successAsync: value =>
                    {
                        tracker.CaptureSuccess();
                        return Task.FromResult(value + 1);
                    },
                    failureAsync: failure =>
                    {
                        tracker.CaptureFailure(failure);
                        return Task.FromResult(failure.Details.Length);
                    }
                );
            }

            if (successAsync)
            {
                return source.MatchFirstAsync(
                    successAsync: value =>
                    {
                        tracker.CaptureSuccess();
                        return Task.FromResult(value + 1);
                    },
                    failure: failure =>
                    {
                        tracker.CaptureFailure(failure);
                        return failure.Details.Length;
                    }
                );
            }

            if (failureAsync)
            {
                return source.MatchFirstAsync(
                    success: value =>
                    {
                        tracker.CaptureSuccess();
                        return value + 1;
                    },
                    failureAsync: failure =>
                    {
                        tracker.CaptureFailure(failure);
                        return Task.FromResult(failure.Details.Length);
                    }
                );
            }

            return Task.FromResult(
                source.MatchFirst(
                    success: value =>
                    {
                        tracker.CaptureSuccess();
                        return value + 1;
                    },
                    failure: failure =>
                    {
                        tracker.CaptureFailure(failure);
                        return failure.Details.Length;
                    }
                )
            );
        };

    private static Func<Task<FailOr<int>>, MatchBranchTracker, Task<int>> LiftedMatchFirst(
        bool successAsync,
        bool failureAsync
    ) =>
        (sourceTask, tracker) =>
        {
            if (successAsync && failureAsync)
            {
                return sourceTask.MatchFirstAsync(
                    successAsync: value =>
                    {
                        tracker.CaptureSuccess();
                        return Task.FromResult(value + 1);
                    },
                    failureAsync: failure =>
                    {
                        tracker.CaptureFailure(failure);
                        return Task.FromResult(failure.Details.Length);
                    }
                );
            }

            if (successAsync)
            {
                return sourceTask.MatchFirstAsync(
                    successAsync: value =>
                    {
                        tracker.CaptureSuccess();
                        return Task.FromResult(value + 1);
                    },
                    failure: failure =>
                    {
                        tracker.CaptureFailure(failure);
                        return failure.Details.Length;
                    }
                );
            }

            if (failureAsync)
            {
                return sourceTask.MatchFirstAsync(
                    success: value =>
                    {
                        tracker.CaptureSuccess();
                        return value + 1;
                    },
                    failureAsync: failure =>
                    {
                        tracker.CaptureFailure(failure);
                        return Task.FromResult(failure.Details.Length);
                    }
                );
            }

            return sourceTask.MatchFirst(
                success: value =>
                {
                    tracker.CaptureSuccess();
                    return value + 1;
                },
                failure: failure =>
                {
                    tracker.CaptureFailure(failure);
                    return failure.Details.Length;
                }
            );
        };
}

public sealed class MatchBranchTracker
{
    public int SuccessCalls { get; private set; }
    public int FailureCalls { get; private set; }
    public IReadOnlyList<Failures>? CapturedFailures { get; private set; }
    public Failures? CapturedFailure { get; private set; }

    public void CaptureSuccess() => SuccessCalls++;

    public void CaptureFailures(IReadOnlyList<Failures> failures)
    {
        FailureCalls++;
        CapturedFailures = failures;
    }

    public void CaptureFailure(Failures failure)
    {
        FailureCalls++;
        CapturedFailure = failure;
    }
}

public static class MatchAssertions
{
    public static async Task AssertSuccessBranch(MatchBranchTracker tracker)
    {
        using var _ = Assert.Multiple();

        await Assert.That(tracker.SuccessCalls).IsEqualTo(1);
        await Assert.That(tracker.FailureCalls).IsEqualTo(0);
        await Assert.That(tracker.CapturedFailures).IsNull();
        await Assert.That(tracker.CapturedFailure).IsNull();
    }

    public static async Task AssertFailureListBranch(
        MatchBranchTracker tracker,
        params Failures[] expected
    )
    {
        using var _ = Assert.Multiple();

        await Assert.That(tracker.SuccessCalls).IsEqualTo(0);
        await Assert.That(tracker.FailureCalls).IsEqualTo(1);
        await Assert.That(tracker.CapturedFailure).IsNull();
        await Assert.That(tracker.CapturedFailures).IsNotNull();
        await Assert.That(tracker.CapturedFailures!.Count).IsEqualTo(expected.Length);

        for (var i = 0; i < expected.Length; i++)
        {
            await Assert.That(tracker.CapturedFailures[i]).IsEqualTo(expected[i]);
        }
    }

    public static async Task AssertFailureFirstBranch(MatchBranchTracker tracker, Failures expected)
    {
        using var _ = Assert.Multiple();

        await Assert.That(tracker.SuccessCalls).IsEqualTo(0);
        await Assert.That(tracker.FailureCalls).IsEqualTo(1);
        await Assert.That(tracker.CapturedFailures).IsNull();
        await Assert.That(tracker.CapturedFailure).IsEqualTo(expected);
    }

    public static async Task AssertEquivalent(
        MatchBranchTracker actual,
        MatchBranchTracker expected
    )
    {
        using var _ = Assert.Multiple();

        await Assert.That(actual.SuccessCalls).IsEqualTo(expected.SuccessCalls);
        await Assert.That(actual.FailureCalls).IsEqualTo(expected.FailureCalls);

        if (expected.CapturedFailures is not null)
        {
            await Assert.That(actual.CapturedFailures).IsNotNull();
            await Assert
                .That(actual.CapturedFailures!.Count)
                .IsEqualTo(expected.CapturedFailures.Count);

            for (var i = 0; i < expected.CapturedFailures.Count; i++)
            {
                await Assert
                    .That(actual.CapturedFailures[i])
                    .IsEqualTo(expected.CapturedFailures[i]);
            }
        }
        else
        {
            await Assert.That(actual.CapturedFailures).IsNull();
        }

        if (expected.CapturedFailure is not null)
        {
            await Assert.That(actual.CapturedFailure).IsEqualTo(expected.CapturedFailure);
            return;
        }

        await Assert.That(actual.CapturedFailure).IsNull();
    }
}

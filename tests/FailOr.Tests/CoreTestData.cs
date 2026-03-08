namespace FailOr.Tests;

public static class CoreTestData
{
    public static IEnumerable<
        Func<(string Operation, Func<Failures[], FailOr<int>> Create)>
    > FailureFactoryCases()
    {
        yield return () => ("single failure", failures => FailOr<int>.Fail(failures[0]));
        yield return () =>
            ("enumerable overload", failures => FailOr<int>.Fail(failures.AsEnumerable()));
        yield return () => ("params overload", failures => FailOr<int>.Fail(failures));
    }

    public static IEnumerable<
        Func<(string Operation, Action Invoke, string ParameterName)>
    > FailureFactoryGuardCases()
    {
        yield return () =>
            ("null enumerable", () => FailOr<int>.Fail((IEnumerable<Failures>)null!), "failures");
        yield return () => ("empty enumerable", () => FailOr<int>.Fail([]), "failures");
        yield return () => ("empty params", () => FailOr<int>.Fail(), "failures");
    }
}

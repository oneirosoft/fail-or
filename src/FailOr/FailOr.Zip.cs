namespace FailOr;

/// <summary>
/// Provides factory and composition helpers for <see cref="FailOr{T}"/>.
/// </summary>
public static partial class FailOr
{
    /// <summary>
    /// Combines two successful results into a tuple or returns all failures in left-to-right order.
    /// </summary>
    /// <typeparam name="T1">The first value type.</typeparam>
    /// <typeparam name="T2">The second value type.</typeparam>
    /// <param name="first">The first result.</param>
    /// <param name="second">The second result.</param>
    /// <returns>
    /// A successful tuple when both inputs succeed; otherwise a failed result containing every input failure.
    /// </returns>
    /// <example>
    /// <code>
    /// var result = FailOr.Zip(FailOr.Success(1), FailOr.Success("two"));
    /// </code>
    /// </example>
    public static FailOr<(T1, T2)> Zip<T1, T2>(FailOr<T1> first, FailOr<T2> second) =>
        first.IsFailure || second.IsFailure
            ? Fail<(T1, T2)>(CollectFailures(first, second))
            : Success((first.UnsafeUnwrap(), second.UnsafeUnwrap()));

    /// <summary>
    /// Combines three successful results into a tuple or returns all failures in left-to-right order.
    /// </summary>
    /// <typeparam name="T1">The first value type.</typeparam>
    /// <typeparam name="T2">The second value type.</typeparam>
    /// <typeparam name="T3">The third value type.</typeparam>
    /// <param name="first">The first result.</param>
    /// <param name="second">The second result.</param>
    /// <param name="third">The third result.</param>
    /// <returns>
    /// A successful tuple when all inputs succeed; otherwise a failed result containing every input failure.
    /// </returns>
    /// <example>
    /// <code>
    /// var result = FailOr.Zip(FailOr.Success(1), FailOr.Success("two"), FailOr.Success(true));
    /// </code>
    /// </example>
    public static FailOr<(T1, T2, T3)> Zip<T1, T2, T3>(
        FailOr<T1> first,
        FailOr<T2> second,
        FailOr<T3> third
    ) =>
        first.IsFailure || second.IsFailure || third.IsFailure
            ? Fail<(T1, T2, T3)>(CollectFailures(first, second, third))
            : Success((first.UnsafeUnwrap(), second.UnsafeUnwrap(), third.UnsafeUnwrap()));

    /// <summary>
    /// Combines four successful results into a tuple or returns all failures in left-to-right order.
    /// </summary>
    /// <typeparam name="T1">The first value type.</typeparam>
    /// <typeparam name="T2">The second value type.</typeparam>
    /// <typeparam name="T3">The third value type.</typeparam>
    /// <typeparam name="T4">The fourth value type.</typeparam>
    /// <param name="first">The first result.</param>
    /// <param name="second">The second result.</param>
    /// <param name="third">The third result.</param>
    /// <param name="fourth">The fourth result.</param>
    /// <returns>
    /// A successful tuple when all inputs succeed; otherwise a failed result containing every input failure.
    /// </returns>
    /// <example>
    /// <code>
    /// var result = FailOr.Zip(FailOr.Success(1), FailOr.Success("two"), FailOr.Success(true), FailOr.Success(4m));
    /// </code>
    /// </example>
    public static FailOr<(T1, T2, T3, T4)> Zip<T1, T2, T3, T4>(
        FailOr<T1> first,
        FailOr<T2> second,
        FailOr<T3> third,
        FailOr<T4> fourth
    ) =>
        first.IsFailure || second.IsFailure || third.IsFailure || fourth.IsFailure
            ? Fail<(T1, T2, T3, T4)>(CollectFailures(first, second, third, fourth))
            : Success(
                (
                    first.UnsafeUnwrap(),
                    second.UnsafeUnwrap(),
                    third.UnsafeUnwrap(),
                    fourth.UnsafeUnwrap()
                )
            );

    /// <summary>
    /// Combines five successful results into a tuple or returns all failures in left-to-right order.
    /// </summary>
    /// <typeparam name="T1">The first value type.</typeparam>
    /// <typeparam name="T2">The second value type.</typeparam>
    /// <typeparam name="T3">The third value type.</typeparam>
    /// <typeparam name="T4">The fourth value type.</typeparam>
    /// <typeparam name="T5">The fifth value type.</typeparam>
    /// <param name="first">The first result.</param>
    /// <param name="second">The second result.</param>
    /// <param name="third">The third result.</param>
    /// <param name="fourth">The fourth result.</param>
    /// <param name="fifth">The fifth result.</param>
    /// <returns>
    /// A successful tuple when all inputs succeed; otherwise a failed result containing every input failure.
    /// </returns>
    /// <example>
    /// <code>
    /// var result = FailOr.Zip(FailOr.Success(1), FailOr.Success("two"), FailOr.Success(true), FailOr.Success(4m), FailOr.Success('5'));
    /// </code>
    /// </example>
    public static FailOr<(T1, T2, T3, T4, T5)> Zip<T1, T2, T3, T4, T5>(
        FailOr<T1> first,
        FailOr<T2> second,
        FailOr<T3> third,
        FailOr<T4> fourth,
        FailOr<T5> fifth
    ) =>
        first.IsFailure
        || second.IsFailure
        || third.IsFailure
        || fourth.IsFailure
        || fifth.IsFailure
            ? Fail<(T1, T2, T3, T4, T5)>(CollectFailures(first, second, third, fourth, fifth))
            : Success(
                (
                    first.UnsafeUnwrap(),
                    second.UnsafeUnwrap(),
                    third.UnsafeUnwrap(),
                    fourth.UnsafeUnwrap(),
                    fifth.UnsafeUnwrap()
                )
            );

    /// <summary>
    /// Combines six successful results into a tuple or returns all failures in left-to-right order.
    /// </summary>
    /// <typeparam name="T1">The first value type.</typeparam>
    /// <typeparam name="T2">The second value type.</typeparam>
    /// <typeparam name="T3">The third value type.</typeparam>
    /// <typeparam name="T4">The fourth value type.</typeparam>
    /// <typeparam name="T5">The fifth value type.</typeparam>
    /// <typeparam name="T6">The sixth value type.</typeparam>
    /// <param name="first">The first result.</param>
    /// <param name="second">The second result.</param>
    /// <param name="third">The third result.</param>
    /// <param name="fourth">The fourth result.</param>
    /// <param name="fifth">The fifth result.</param>
    /// <param name="sixth">The sixth result.</param>
    /// <returns>
    /// A successful tuple when all inputs succeed; otherwise a failed result containing every input failure.
    /// </returns>
    /// <example>
    /// <code>
    /// var result = FailOr.Zip(FailOr.Success(1), FailOr.Success("two"), FailOr.Success(true), FailOr.Success(4m), FailOr.Success('5'), FailOr.Success(6L));
    /// </code>
    /// </example>
    public static FailOr<(T1, T2, T3, T4, T5, T6)> Zip<T1, T2, T3, T4, T5, T6>(
        FailOr<T1> first,
        FailOr<T2> second,
        FailOr<T3> third,
        FailOr<T4> fourth,
        FailOr<T5> fifth,
        FailOr<T6> sixth
    ) =>
        first.IsFailure
        || second.IsFailure
        || third.IsFailure
        || fourth.IsFailure
        || fifth.IsFailure
        || sixth.IsFailure
            ? Fail<(T1, T2, T3, T4, T5, T6)>(
                CollectFailures(first, second, third, fourth, fifth, sixth)
            )
            : Success(
                (
                    first.UnsafeUnwrap(),
                    second.UnsafeUnwrap(),
                    third.UnsafeUnwrap(),
                    fourth.UnsafeUnwrap(),
                    fifth.UnsafeUnwrap(),
                    sixth.UnsafeUnwrap()
                )
            );

    /// <summary>
    /// Combines seven successful results into a tuple or returns all failures in left-to-right order.
    /// </summary>
    /// <typeparam name="T1">The first value type.</typeparam>
    /// <typeparam name="T2">The second value type.</typeparam>
    /// <typeparam name="T3">The third value type.</typeparam>
    /// <typeparam name="T4">The fourth value type.</typeparam>
    /// <typeparam name="T5">The fifth value type.</typeparam>
    /// <typeparam name="T6">The sixth value type.</typeparam>
    /// <typeparam name="T7">The seventh value type.</typeparam>
    /// <param name="first">The first result.</param>
    /// <param name="second">The second result.</param>
    /// <param name="third">The third result.</param>
    /// <param name="fourth">The fourth result.</param>
    /// <param name="fifth">The fifth result.</param>
    /// <param name="sixth">The sixth result.</param>
    /// <param name="seventh">The seventh result.</param>
    /// <returns>
    /// A successful tuple when all inputs succeed; otherwise a failed result containing every input failure.
    /// </returns>
    /// <example>
    /// <code>
    /// var result = FailOr.Zip(FailOr.Success(1), FailOr.Success("two"), FailOr.Success(true), FailOr.Success(4m), FailOr.Success('5'), FailOr.Success(6L), FailOr.Success(7u));
    /// </code>
    /// </example>
    public static FailOr<(T1, T2, T3, T4, T5, T6, T7)> Zip<T1, T2, T3, T4, T5, T6, T7>(
        FailOr<T1> first,
        FailOr<T2> second,
        FailOr<T3> third,
        FailOr<T4> fourth,
        FailOr<T5> fifth,
        FailOr<T6> sixth,
        FailOr<T7> seventh
    ) =>
        first.IsFailure
        || second.IsFailure
        || third.IsFailure
        || fourth.IsFailure
        || fifth.IsFailure
        || sixth.IsFailure
        || seventh.IsFailure
            ? Fail<(T1, T2, T3, T4, T5, T6, T7)>(
                CollectFailures(first, second, third, fourth, fifth, sixth, seventh)
            )
            : Success(
                (
                    first.UnsafeUnwrap(),
                    second.UnsafeUnwrap(),
                    third.UnsafeUnwrap(),
                    fourth.UnsafeUnwrap(),
                    fifth.UnsafeUnwrap(),
                    sixth.UnsafeUnwrap(),
                    seventh.UnsafeUnwrap()
                )
            );

    private static Failures[] CollectFailures<T1, T2>(FailOr<T1> first, FailOr<T2> second) =>
        [.. FailuresOrEmpty(first), .. FailuresOrEmpty(second)];

    private static Failures[] CollectFailures<T1, T2, T3>(
        FailOr<T1> first,
        FailOr<T2> second,
        FailOr<T3> third
    ) => [.. FailuresOrEmpty(first), .. FailuresOrEmpty(second), .. FailuresOrEmpty(third)];

    private static Failures[] CollectFailures<T1, T2, T3, T4>(
        FailOr<T1> first,
        FailOr<T2> second,
        FailOr<T3> third,
        FailOr<T4> fourth
    ) =>
        [
            .. FailuresOrEmpty(first),
            .. FailuresOrEmpty(second),
            .. FailuresOrEmpty(third),
            .. FailuresOrEmpty(fourth),
        ];

    private static Failures[] CollectFailures<T1, T2, T3, T4, T5>(
        FailOr<T1> first,
        FailOr<T2> second,
        FailOr<T3> third,
        FailOr<T4> fourth,
        FailOr<T5> fifth
    ) =>
        [
            .. FailuresOrEmpty(first),
            .. FailuresOrEmpty(second),
            .. FailuresOrEmpty(third),
            .. FailuresOrEmpty(fourth),
            .. FailuresOrEmpty(fifth),
        ];

    private static Failures[] CollectFailures<T1, T2, T3, T4, T5, T6>(
        FailOr<T1> first,
        FailOr<T2> second,
        FailOr<T3> third,
        FailOr<T4> fourth,
        FailOr<T5> fifth,
        FailOr<T6> sixth
    ) =>
        [
            .. FailuresOrEmpty(first),
            .. FailuresOrEmpty(second),
            .. FailuresOrEmpty(third),
            .. FailuresOrEmpty(fourth),
            .. FailuresOrEmpty(fifth),
            .. FailuresOrEmpty(sixth),
        ];

    private static Failures[] CollectFailures<T1, T2, T3, T4, T5, T6, T7>(
        FailOr<T1> first,
        FailOr<T2> second,
        FailOr<T3> third,
        FailOr<T4> fourth,
        FailOr<T5> fifth,
        FailOr<T6> sixth,
        FailOr<T7> seventh
    ) =>
        [
            .. FailuresOrEmpty(first),
            .. FailuresOrEmpty(second),
            .. FailuresOrEmpty(third),
            .. FailuresOrEmpty(fourth),
            .. FailuresOrEmpty(fifth),
            .. FailuresOrEmpty(sixth),
            .. FailuresOrEmpty(seventh),
        ];

    private static IReadOnlyList<Failures> FailuresOrEmpty<T>(FailOr<T> source) =>
        source.IsSuccess ? [] : source.Failures;
}

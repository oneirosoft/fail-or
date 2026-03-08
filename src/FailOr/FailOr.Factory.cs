namespace FailOr;

/// <summary>
/// Provides factory and composition helpers for <see cref="FailOr{T}"/>.
/// </summary>
public static partial class FailOr
{
    /// <summary>
    /// Creates a successful <see cref="FailOr{T}"/> result.
    /// </summary>
    /// <param name="value">The success value to wrap.</param>
    /// <returns>A successful result containing <paramref name="value"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="value"/> is <see langword="null"/> for a reference type.
    /// </exception>
    /// <example>
    /// <code>
    /// var result = FailOr.Success(42);
    /// </code>
    /// </example>
    public static FailOr<T> Success<T>(T value) => FailOr<T>.Success(value);

    /// <summary>
    /// Creates a failed <see cref="FailOr{T}"/> result from a single failure.
    /// </summary>
    /// <param name="failure">The failure to wrap.</param>
    /// <returns>A failed result containing <paramref name="failure"/>.</returns>
    /// <example>
    /// <code>
    /// var result = FailOr.Fail&lt;int&gt;(Failure.General("Invalid value"));
    /// </code>
    /// </example>
    public static FailOr<T> Fail<T>(Failure failure) => FailOr<T>.Fail(failure);

    /// <summary>
    /// Creates a failed <see cref="FailOr{T}"/> result from a sequence of failures.
    /// </summary>
    /// <param name="failures">The failures to wrap.</param>
    /// <returns>A failed result containing <paramref name="failures"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="failures"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="failures"/> is empty.
    /// </exception>
    /// <example>
    /// <code>
    /// var result = FailOr.Fail&lt;int&gt;([Failure.General("A"), Failure.General("B")]);
    /// </code>
    /// </example>
    public static FailOr<T> Fail<T>(IEnumerable<Failure> failures) => FailOr<T>.Fail(failures);

    /// <summary>
    /// Creates a failed <see cref="FailOr{T}"/> result from one or more failures.
    /// </summary>
    /// <param name="failures">The failures to wrap.</param>
    /// <returns>A failed result containing <paramref name="failures"/>.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="failures"/> is empty.
    /// </exception>
    /// <example>
    /// <code>
    /// var result = FailOr.Fail&lt;int&gt;(Failure.General("A"), Failure.General("B"));
    /// </code>
    /// </example>
    public static FailOr<T> Fail<T>(params Failure[] failures) => FailOr<T>.Fail(failures);
}

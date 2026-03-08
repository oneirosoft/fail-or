namespace FailOr;

/// <summary>
/// Provides factory and composition helpers for <see cref="FailOr{T}"/>.
/// </summary>
public static partial class FailOr
{
    /// <summary>
    /// Returns the left result when it succeeds; otherwise returns the right result.
    /// </summary>
    /// <typeparam name="T">The wrapped value type.</typeparam>
    /// <param name="left">The preferred result.</param>
    /// <param name="right">The fallback result.</param>
    /// <returns>
    /// <paramref name="left"/> when it succeeds; otherwise <paramref name="right"/>.
    /// </returns>
    /// <example>
    /// <code>
    /// var result = FailOr.Combine(FailOr.Fail&lt;int&gt;(Failure.General("left")), FailOr.Success(42));
    /// </code>
    /// </example>
    public static FailOr<T> Combine<T>(FailOr<T> left, FailOr<T> right) =>
        left.IsSuccess ? left : right;
}

namespace FailOr;

/// <summary>
/// Represents either a successful value of type <typeparamref name="T"/> or one or more failures.
/// </summary>
public readonly record struct FailOr<T>
{
    private readonly T _value;
    private readonly Failure[] _failures;
    private readonly IReadOnlyList<Failure> _failuresView;

    private FailOr(T value)
    {
        _value = value;
        _failures = [];
        _failuresView = Array.AsReadOnly(_failures);
    }

    private FailOr(Failure[] failures)
    {
        _value = default!;
        _failures = failures;
        _failuresView = Array.AsReadOnly(_failures);
    }

    /// <summary>
    /// Gets a value indicating whether the result contains a success value.
    /// </summary>
    public bool IsSuccess => !IsFailure;

    /// <summary>
    /// Gets a value indicating whether the result contains one or more failures.
    /// </summary>
    public bool IsFailure => _failures.Length > 0;

    /// <summary>
    /// Returns the success value when the result is successful.
    /// </summary>
    /// <returns>The wrapped success value.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the result represents one or more failures.
    /// </exception>
    /// <example>
    /// <code>
    /// var value = FailOr.Success(42).UnsafeUnwrap();
    /// </code>
    /// </example>
    public T UnsafeUnwrap() =>
        IsFailure
            ? throw new InvalidOperationException("A failed FailOr does not contain a value.")
            : _value;

    /// <summary>
    /// Gets the read-only collection of failures for a failed result.
    /// </summary>
    public IReadOnlyList<Failure> Failures => _failuresView;

    /// <summary>
    /// Creates a successful result for <typeparamref name="T"/>.
    /// </summary>
    /// <param name="value">The success value to wrap.</param>
    /// <returns>A successful result containing <paramref name="value"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="value"/> is <see langword="null"/> for a reference type.
    /// </exception>
    /// <example>
    /// <code>
    /// var result = FailOr&lt;string&gt;.Success("ok");
    /// </code>
    /// </example>
    public static FailOr<T> Success(T value)
    {
        if (!typeof(T).IsValueType)
        {
            ArgumentNullException.ThrowIfNull(value);
        }

        return new(value);
    }

    /// <summary>
    /// Creates a failed result from a single failure.
    /// </summary>
    /// <param name="failure">The failure to wrap.</param>
    /// <returns>A failed result containing <paramref name="failure"/>.</returns>
    /// <example>
    /// <code>
    /// var result = FailOr&lt;int&gt;.Fail(Failure.General("Invalid value"));
    /// </code>
    /// </example>
    public static FailOr<T> Fail(Failure failure) => new([failure]);

    /// <summary>
    /// Creates a failed result from a sequence of failures.
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
    /// var result = FailOr&lt;int&gt;.Fail([Failure.General("A"), Failure.General("B")]);
    /// </code>
    /// </example>
    public static FailOr<T> Fail(IEnumerable<Failure> failures)
    {
        ArgumentNullException.ThrowIfNull(failures);

        var failuresArray = failures.ToArray();

        if (failuresArray.Length == 0)
        {
            throw new ArgumentException("At least one failure must be provided.", nameof(failures));
        }

        return new(failuresArray);
    }

    /// <summary>
    /// Creates a failed result from one or more failures.
    /// </summary>
    /// <param name="failures">The failures to wrap.</param>
    /// <returns>A failed result containing <paramref name="failures"/>.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="failures"/> is empty.
    /// </exception>
    /// <example>
    /// <code>
    /// var result = FailOr&lt;int&gt;.Fail(Failure.General("A"), Failure.General("B"));
    /// </code>
    /// </example>
    public static FailOr<T> Fail(params Failure[] failures) => Fail((IEnumerable<Failure>)failures);
}

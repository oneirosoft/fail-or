namespace FailOr;

/// <summary>
/// Represents either a successful value of type <typeparamref name="T"/> or one or more failures.
/// </summary>
public readonly record struct FailOr<T>
{
    private readonly T _value;
    private readonly Failures[] _failures;

    private FailOr(T value)
    {
        _value = value;
        _failures = [];
    }

    private FailOr(Failures[] failures)
    {
        _value = default!;
        _failures = failures;
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
    public IReadOnlyList<Failures> Failures => Array.AsReadOnly(_failures);

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
    /// Implicitly wraps a success value in a <see cref="FailOr{T}"/>.
    /// </summary>
    /// <param name="value">The success value to wrap.</param>
    /// <returns>A successful result containing <paramref name="value"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="value"/> is <see langword="null"/> for a reference type.
    /// </exception>
    /// <example>
    /// <code>
    /// FailOr&lt;int&gt; result = 42;
    /// </code>
    /// </example>
    public static implicit operator FailOr<T>(T value) => Success(value);

    /// <summary>
    /// Creates a failed result from a single failure.
    /// </summary>
    /// <param name="failure">The failure to wrap.</param>
    /// <returns>A failed result containing <paramref name="failure"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="failure"/> is <see langword="null"/>.
    /// </exception>
    /// <example>
    /// <code>
    /// var result = FailOr&lt;int&gt;.Fail(Failure.General("Invalid value"));
    /// </code>
    /// </example>
    public static FailOr<T> Fail(Failures failure)
    {
        ArgumentNullException.ThrowIfNull(failure);

        return new([failure]);
    }

    /// <summary>
    /// Implicitly wraps a single failure in a failed <see cref="FailOr{T}"/>.
    /// </summary>
    /// <param name="failure">The failure to wrap.</param>
    /// <returns>A failed result containing <paramref name="failure"/>.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="failure"/> is <see langword="null"/>.
    /// </exception>
    /// <example>
    /// <code>
    /// FailOr&lt;int&gt; result = Failure.General("Invalid value");
    /// </code>
    /// </example>
    public static implicit operator FailOr<T>(Failures failure) => Fail([failure]);

    /// <summary>
    /// Creates a failed result from a sequence of failures.
    /// </summary>
    /// <param name="failures">The failures to wrap.</param>
    /// <returns>A failed result containing <paramref name="failures"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="failures"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="failures"/> is empty or contains a <see langword="null"/> value.
    /// </exception>
    /// <example>
    /// <code>
    /// var result = FailOr&lt;int&gt;.Fail([Failure.General("A"), Failure.General("B")]);
    /// </code>
    /// </example>
    public static FailOr<T> Fail(IEnumerable<Failures> failures)
    {
        ArgumentNullException.ThrowIfNull(failures);

        var failuresArray = failures.ToArray();

        if (failuresArray.Length == 0)
        {
            throw new ArgumentException("At least one failure must be provided.", nameof(failures));
        }

        if (failuresArray.Any(static failure => failure is null))
        {
            throw new ArgumentException("Failures cannot contain null values.", nameof(failures));
        }

        return new(failuresArray);
    }

    /// <summary>
    /// Implicitly wraps an array of failures in a failed <see cref="FailOr{T}"/>.
    /// </summary>
    /// <param name="failures">The failures to wrap.</param>
    /// <returns>A failed result containing <paramref name="failures"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="failures"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="failures"/> is empty or contains a <see langword="null"/> value.
    /// </exception>
    /// <example>
    /// <code>
    /// Failures[] failures = [Failure.General("A"), Failure.General("B")];
    /// FailOr&lt;int&gt; result = failures;
    /// </code>
    /// </example>
    public static implicit operator FailOr<T>(Failures[] failures) => Fail(failures);

    /// <summary>
    /// Implicitly wraps a list of failures in a failed <see cref="FailOr{T}"/>.
    /// </summary>
    /// <param name="failures">The failures to wrap.</param>
    /// <returns>A failed result containing <paramref name="failures"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="failures"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="failures"/> is empty or contains a <see langword="null"/> value.
    /// </exception>
    /// <example>
    /// <code>
    /// List&lt;Failures&gt; failures = [Failure.General("A"), Failure.General("B")];
    /// FailOr&lt;int&gt; result = failures;
    /// </code>
    /// </example>
    public static implicit operator FailOr<T>(List<Failures> failures) => Fail(failures);

    /// <summary>
    /// Creates a failed result from one or more failures.
    /// </summary>
    /// <param name="failures">The failures to wrap.</param>
    /// <returns>A failed result containing <paramref name="failures"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="failures"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="failures"/> is empty or contains a <see langword="null"/> value.
    /// </exception>
    /// <example>
    /// <code>
    /// var result = FailOr&lt;int&gt;.Fail(Failure.General("A"), Failure.General("B"));
    /// </code>
    /// </example>
    public static FailOr<T> Fail(params Failures[] failures) =>
        Fail((IEnumerable<Failures>)failures);
}

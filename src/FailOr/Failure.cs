namespace FailOr;

/// <summary>
/// Represents a failure with a stable code and human-readable details.
/// </summary>
public readonly record struct Failure
{
    private Failure(string code, string details)
    {
        Code = code;
        Details = details;
    }

    /// <summary>
    /// Gets the failure code.
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// Gets the human-readable failure details.
    /// </summary>
    public string Details { get; }

    /// <summary>
    /// Creates a general-purpose failure with the supplied details.
    /// </summary>
    /// <param name="details">The human-readable failure details.</param>
    /// <returns>A failure with the <c>General</c> code.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="details"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="details"/> is empty or whitespace.
    /// </exception>
    /// <example>
    /// <code>
    /// var failure = Failure.General("Something went wrong");
    /// </code>
    /// </example>
    public static Failure General(string details)
    {
        ArgumentNullException.ThrowIfNull(details);

        if (string.IsNullOrWhiteSpace(details))
        {
            throw new ArgumentException("Failure details must be provided.", nameof(details));
        }

        return new Failure("General", details);
    }
}

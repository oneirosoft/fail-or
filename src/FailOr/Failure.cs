using System.Collections.ObjectModel;

namespace FailOr;

/// <summary>
/// Represents a failure value that can be carried by <see cref="FailOr{T}"/>.
/// </summary>
public abstract record Failures
{
    private protected Failures(string code, string details)
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
    /// Represents a general-purpose failure with optional metadata.
    /// </summary>
    public sealed record General : Failures
    {
        internal General(string code, string details, IReadOnlyDictionary<string, object?> metadata)
            : base(code, details) => Metadata = metadata;

        /// <summary>
        /// Gets the read-only metadata associated with the failure.
        /// </summary>
        public IReadOnlyDictionary<string, object?> Metadata { get; }
    }

    /// <summary>
    /// Represents a validation failure for a specific property.
    /// </summary>
    public sealed record Validation : Failures
    {
        internal Validation(string code, string details, string propertyName)
            : base(code, details) => PropertyName = propertyName;

        /// <summary>
        /// Gets the property name associated with the validation failure.
        /// </summary>
        public string PropertyName { get; }
    }

    /// <summary>
    /// Represents a failure that wraps an exception.
    /// </summary>
    public sealed record Exceptional : Failures
    {
        internal Exceptional(string code, string details, Exception exception)
            : base(code, details) => Exception = exception;

        /// <summary>
        /// Gets the exception associated with the failure.
        /// </summary>
        public Exception Exception { get; }
    }
}

/// <summary>
/// Provides factory helpers for creating <see cref="Failures"/> values.
/// </summary>
public static class Failure
{
    /// <summary>
    /// Creates a general-purpose failure with the supplied details.
    /// </summary>
    /// <param name="details">The human-readable failure details.</param>
    /// <param name="code">The optional failure code. Defaults to <c>General</c>.</param>
    /// <param name="metadata">The optional metadata to associate with the failure.</param>
    /// <returns>A <see cref="Failures.General"/> failure.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="details"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="details"/> is empty or whitespace, or when
    /// <paramref name="code"/> is provided but empty or whitespace.
    /// </exception>
    /// <example>
    /// <code>
    /// var failure = Failure.General("Something went wrong.");
    /// </code>
    /// </example>
    public static Failures.General General(
        string details,
        string? code = null,
        Dictionary<string, object?>? metadata = null
    ) => new(NormalizeCode(code, "General"), RequireDetails(details), CreateMetadata(metadata));

    /// <summary>
    /// Creates a validation failure for the supplied property and messages.
    /// </summary>
    /// <param name="propertyName">The name of the property that failed validation.</param>
    /// <param name="errors">One or more validation messages.</param>
    /// <returns>A <see cref="Failures.Validation"/> failure.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="propertyName"/> or <paramref name="errors"/> is
    /// <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="propertyName"/> is empty or whitespace, when
    /// <paramref name="errors"/> is empty, or when any validation message is empty or
    /// whitespace.
    /// </exception>
    /// <example>
    /// <code>
    /// var failure = Failure.Validation("Email", "Email is required.");
    /// </code>
    /// </example>
    public static Failures.Validation Validation(string propertyName, params string[] errors)
    {
        ArgumentNullException.ThrowIfNull(propertyName);
        ArgumentNullException.ThrowIfNull(errors);

        if (string.IsNullOrWhiteSpace(propertyName))
        {
            throw new ArgumentException("Property name must be provided.", nameof(propertyName));
        }

        if (errors.Length == 0)
        {
            throw new ArgumentException(
                "At least one validation error must be provided.",
                nameof(errors)
            );
        }

        var copy = new string[errors.Length];

        for (var index = 0; index < errors.Length; index++)
        {
            var error = errors[index];

            if (string.IsNullOrWhiteSpace(error))
            {
                throw new ArgumentException(
                    "Validation error messages must be provided.",
                    nameof(errors)
                );
            }

            copy[index] = error;
        }

        var details = string.Join("; ", copy);
        return new($"Validation.{propertyName}", details, propertyName);
    }

    /// <summary>
    /// Creates a failure that wraps an exception.
    /// </summary>
    /// <param name="exception">The exception associated with the failure.</param>
    /// <param name="details">
    /// The optional human-readable failure details. Defaults to the exception message, or the
    /// exception type name when the message is blank.
    /// </param>
    /// <param name="code">The optional failure code. Defaults to <c>Exceptional</c>.</param>
    /// <returns>An <see cref="Failures.Exceptional"/> failure.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="exception"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="details"/> is provided but empty or whitespace, or when
    /// <paramref name="code"/> is provided but empty or whitespace.
    /// </exception>
    /// <example>
    /// <code>
    /// var failure = Failure.Exceptional(new InvalidOperationException("Operation failed."));
    /// </code>
    /// </example>
    public static Failures.Exceptional Exceptional(
        Exception exception,
        string? details = null,
        string? code = null
    )
    {
        ArgumentNullException.ThrowIfNull(exception);

        var resolvedDetails = details is null
            ? DefaultExceptionalDetails(exception)
            : RequireDetails(details);

        return new(NormalizeCode(code, "Exceptional"), resolvedDetails, exception);
    }

    private static IReadOnlyDictionary<string, object?> CreateMetadata(
        Dictionary<string, object?>? metadata
    ) =>
        new ReadOnlyDictionary<string, object?>(
            metadata is null ? new Dictionary<string, object?>() : new(metadata)
        );

    private static string NormalizeCode(string? code, string defaultCode)
    {
        if (code is null)
        {
            return defaultCode;
        }

        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException("Failure code must be provided.", nameof(code));
        }

        return code;
    }

    private static string RequireDetails(string details)
    {
        ArgumentNullException.ThrowIfNull(details);

        if (string.IsNullOrWhiteSpace(details))
        {
            throw new ArgumentException("Failure details must be provided.", nameof(details));
        }

        return details;
    }

    private static string DefaultExceptionalDetails(Exception exception) =>
        string.IsNullOrWhiteSpace(exception.Message) ? exception.GetType().Name : exception.Message;
}

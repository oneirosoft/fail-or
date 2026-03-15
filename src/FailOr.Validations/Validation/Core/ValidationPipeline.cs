namespace FailOr.Validation.Core;

internal static class ValidationPipeline
{
    internal static FailOr<T> Validate<T>(T value, ValidationRule<T>[] rules)
    {
        ArgumentNullException.ThrowIfNull(value);

        List<Failures>? failures = null;

        foreach (var rule in rules)
        {
            var normalizedFailures = rule.Validate(value);

            if (normalizedFailures.Length == 0)
            {
                continue;
            }

            failures ??= [];
            failures.AddRange(normalizedFailures);
        }

        return failures is null ? FailOr.Success(value) : FailOr.Fail<T>(failures);
    }

    internal static async Task<FailOr<T>> ValidateAsync<T>(T value, AsyncValidationRule<T>[] rules)
    {
        ArgumentNullException.ThrowIfNull(value);

        List<Failures>? failures = null;

        foreach (var rule in rules)
        {
            var normalizedFailures = await rule.ValidateAsync(value).ConfigureAwait(false);

            if (normalizedFailures.Length == 0)
            {
                continue;
            }

            failures ??= [];
            failures.AddRange(normalizedFailures);
        }

        return failures is null ? FailOr.Success(value) : FailOr.Fail<T>(failures);
    }

    internal static FailOr<object?[]> ValidateAndMap<T>(T value, ValidationMapRule<T>[] rules)
    {
        ArgumentNullException.ThrowIfNull(value);

        List<Failures>? failures = null;
        var mappedValues = new object?[rules.Length];

        for (var index = 0; index < rules.Length; index++)
        {
            var rule = rules[index];
            var result = rule.Map(value);

            if (result.IsFailure)
            {
                failures ??= [];
                failures.AddRange(result.Failures);
                continue;
            }

            mappedValues[index] = result.Value;
        }

        return failures is null ? FailOr.Success(mappedValues) : FailOr.Fail<object?[]>(failures);
    }

    internal static async Task<FailOr<object?[]>> ValidateAndMapAsync<T>(
        T value,
        AsyncValidationMapRule<T>[] rules
    )
    {
        ArgumentNullException.ThrowIfNull(value);

        List<Failures>? failures = null;
        var mappedValues = new object?[rules.Length];

        for (var index = 0; index < rules.Length; index++)
        {
            var rule = rules[index];
            var result = await rule.MapAsync(value).ConfigureAwait(false);

            if (result.IsFailure)
            {
                failures ??= [];
                failures.AddRange(result.Failures);
                continue;
            }

            mappedValues[index] = result.Value;
        }

        return failures is null ? FailOr.Success(mappedValues) : FailOr.Fail<object?[]>(failures);
    }

    internal static Failures[] NormalizeFailures<T>(string propertyName, FailOr<T> result)
    {
        ArgumentNullException.ThrowIfNull(propertyName);

        if (result.IsSuccess)
        {
            return [];
        }

        var failures = new Failures[result.Failures.Count];

        for (var index = 0; index < result.Failures.Count; index++)
        {
            failures[index] = result.Failures[index] switch
            {
                Failures.Validation validation => Failure.Validation(
                    propertyName,
                    validation.Details
                ),
                _ => result.Failures[index],
            };
        }

        return failures;
    }
}

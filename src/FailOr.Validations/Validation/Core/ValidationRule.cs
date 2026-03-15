using System.Linq.Expressions;

namespace FailOr.Validation.Core;

internal readonly record struct ValidationRule<T>
{
    private ValidationRule(string propertyName, Func<T, Failures[]> validate)
    {
        PropertyName = propertyName;
        Validate = validate;
    }

    internal string PropertyName { get; }

    internal Func<T, Failures[]> Validate { get; }

    internal static ValidationRule<T> Create<TProp, TResult>(
        Expression<Func<T, TProp>> propertySelector,
        Func<TProp, FailOr<TResult>> predicate
    )
    {
        ArgumentNullException.ThrowIfNull(propertySelector);
        ArgumentNullException.ThrowIfNull(predicate);

        var propertyName = ValidationSelectors.GetPropertyName(propertySelector);
        var accessor = propertySelector.Compile();

        return new(
            propertyName,
            value => ValidationPipeline.NormalizeFailures(propertyName, predicate(accessor(value)))
        );
    }
}

internal readonly record struct AsyncValidationRule<T>
{
    private AsyncValidationRule(string propertyName, Func<T, Task<Failures[]>> validateAsync)
    {
        PropertyName = propertyName;
        ValidateAsync = validateAsync;
    }

    internal string PropertyName { get; }

    internal Func<T, Task<Failures[]>> ValidateAsync { get; }

    internal static AsyncValidationRule<T> Create<TProp, TResult>(
        Expression<Func<T, TProp>> propertySelector,
        Func<TProp, Task<FailOr<TResult>>> predicateAsync
    )
    {
        ArgumentNullException.ThrowIfNull(propertySelector);
        ArgumentNullException.ThrowIfNull(predicateAsync);

        var propertyName = ValidationSelectors.GetPropertyName(propertySelector);
        var accessor = propertySelector.Compile();

        return new(
            propertyName,
            async value =>
            {
                var resultTask =
                    predicateAsync(accessor(value))
                    ?? throw new ArgumentNullException(nameof(predicateAsync));
                return ValidationPipeline.NormalizeFailures(
                    propertyName,
                    await resultTask.ConfigureAwait(false)
                );
            }
        );
    }
}

internal readonly record struct ValidationMapRule<T>
{
    private ValidationMapRule(string propertyName, Func<T, ValidationMapResult> map)
    {
        PropertyName = propertyName;
        Map = map;
    }

    internal string PropertyName { get; }

    internal Func<T, ValidationMapResult> Map { get; }

    internal static ValidationMapRule<T> Create<TProp, TMapped>(
        Expression<Func<T, TProp>> propertySelector,
        Func<TProp, FailOr<TMapped>> mapper
    )
    {
        ArgumentNullException.ThrowIfNull(propertySelector);
        ArgumentNullException.ThrowIfNull(mapper);

        var propertyName = ValidationSelectors.GetPropertyName(propertySelector);
        var accessor = propertySelector.Compile();

        return new(
            propertyName,
            value =>
            {
                var result = mapper(accessor(value));

                return result.IsFailure
                    ? ValidationMapResult.Fail(
                        ValidationPipeline.NormalizeFailures(propertyName, result)
                    )
                    : ValidationMapResult.Success(result.UnsafeUnwrap());
            }
        );
    }
}

internal readonly record struct AsyncValidationMapRule<T>
{
    private AsyncValidationMapRule(string propertyName, Func<T, Task<ValidationMapResult>> mapAsync)
    {
        PropertyName = propertyName;
        MapAsync = mapAsync;
    }

    internal string PropertyName { get; }

    internal Func<T, Task<ValidationMapResult>> MapAsync { get; }

    internal static AsyncValidationMapRule<T> Create<TProp, TMapped>(
        Expression<Func<T, TProp>> propertySelector,
        Func<TProp, Task<FailOr<TMapped>>> mapperAsync
    )
    {
        ArgumentNullException.ThrowIfNull(propertySelector);
        ArgumentNullException.ThrowIfNull(mapperAsync);

        var propertyName = ValidationSelectors.GetPropertyName(propertySelector);
        var accessor = propertySelector.Compile();

        return new(
            propertyName,
            async value =>
            {
                var resultTask =
                    mapperAsync(accessor(value))
                    ?? throw new ArgumentNullException(nameof(mapperAsync));
                var result = await resultTask.ConfigureAwait(false);

                return result.IsFailure
                    ? ValidationMapResult.Fail(
                        ValidationPipeline.NormalizeFailures(propertyName, result)
                    )
                    : ValidationMapResult.Success(result.UnsafeUnwrap());
            }
        );
    }
}

internal readonly record struct ValidationMapResult
{
    private ValidationMapResult(object? value, Failures[] failures)
    {
        Value = value;
        Failures = failures;
    }

    internal object? Value { get; }

    internal Failures[] Failures { get; }

    internal bool IsFailure => Failures.Length > 0;

    internal static ValidationMapResult Success(object? value) => new(value, []);

    internal static ValidationMapResult Fail(Failures[] failures) => new(default, failures);
}

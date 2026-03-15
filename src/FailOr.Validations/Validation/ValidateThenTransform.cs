using System.Linq.Expressions;
using FailOr.Validation.Core;

namespace FailOr.Validation;

/// <summary>
/// Provides extension methods that validate selected properties, map their values, and transform the mapped results.
/// </summary>
/// <remarks>
/// Every mapper runs before failures are returned. Validation failures are normalized to the selected leaf property name.
/// </remarks>
public static class ValidateThenTransformExtensions
{
    /// <summary>
    /// Maps 1 selected property with mappers and transforms the mapped values when every mapper succeeds.
    /// </summary>
    /// <param name="value">The object to validate and map.</param>
    /// <param name="validator1">The first property selector and mapper.</param>
    /// <param name="transform">The final transform to run after every mapper succeeds.</param>
    /// <returns>The transformed success value when every mapper succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, any mapper, or the final transform delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = person.ValidateThenTransform(validator1, transform);</code></example>
    public static FailOr<TResult> ValidateThenTransform<T, TProp1, TMapped1, TResult>(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, FailOr<TMapped1>> mapper
        ) validator1,
        Func<TMapped1, TResult> transform
    )
    {
        ArgumentNullException.ThrowIfNull(transform);

        return ValidateThenTransformCore(
            value,
            [ValidationMapRule<T>.Create(validator1.propertySelector, validator1.mapper)],
            mappedValues => transform(GetMappedValue<TMapped1>(mappedValues, 0))
        );
    }

    /// <summary>
    /// Maps 2 selected properties with mappers and transforms the mapped values when every mapper succeeds.
    /// </summary>
    /// <param name="value">The object to validate and map.</param>
    /// <param name="validator1">The first property selector and mapper.</param>
    /// <param name="validator2">The second property selector and mapper.</param>
    /// <param name="transform">The final transform to run after every mapper succeeds.</param>
    /// <returns>The transformed success value when every mapper succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, any mapper, or the final transform delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = person.ValidateThenTransform(validator1, validator2, transform);</code></example>
    public static FailOr<TResult> ValidateThenTransform<
        T,
        TProp1,
        TMapped1,
        TProp2,
        TMapped2,
        TResult
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, FailOr<TMapped1>> mapper
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, FailOr<TMapped2>> mapper
        ) validator2,
        Func<TMapped1, TMapped2, TResult> transform
    )
    {
        ArgumentNullException.ThrowIfNull(transform);

        return ValidateThenTransformCore(
            value,
            [
                ValidationMapRule<T>.Create(validator1.propertySelector, validator1.mapper),
                ValidationMapRule<T>.Create(validator2.propertySelector, validator2.mapper),
            ],
            mappedValues =>
                transform(
                    GetMappedValue<TMapped1>(mappedValues, 0),
                    GetMappedValue<TMapped2>(mappedValues, 1)
                )
        );
    }

    /// <summary>
    /// Maps 3 selected properties with mappers and transforms the mapped values when every mapper succeeds.
    /// </summary>
    /// <param name="value">The object to validate and map.</param>
    /// <param name="validator1">The first property selector and mapper.</param>
    /// <param name="validator2">The second property selector and mapper.</param>
    /// <param name="validator3">The third property selector and mapper.</param>
    /// <param name="transform">The final transform to run after every mapper succeeds.</param>
    /// <returns>The transformed success value when every mapper succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, any mapper, or the final transform delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = person.ValidateThenTransform(validator1, validator2, transform);</code></example>
    public static FailOr<TResult> ValidateThenTransform<
        T,
        TProp1,
        TMapped1,
        TProp2,
        TMapped2,
        TProp3,
        TMapped3,
        TResult
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, FailOr<TMapped1>> mapper
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, FailOr<TMapped2>> mapper
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, FailOr<TMapped3>> mapper
        ) validator3,
        Func<TMapped1, TMapped2, TMapped3, TResult> transform
    )
    {
        ArgumentNullException.ThrowIfNull(transform);

        return ValidateThenTransformCore(
            value,
            [
                ValidationMapRule<T>.Create(validator1.propertySelector, validator1.mapper),
                ValidationMapRule<T>.Create(validator2.propertySelector, validator2.mapper),
                ValidationMapRule<T>.Create(validator3.propertySelector, validator3.mapper),
            ],
            mappedValues =>
                transform(
                    GetMappedValue<TMapped1>(mappedValues, 0),
                    GetMappedValue<TMapped2>(mappedValues, 1),
                    GetMappedValue<TMapped3>(mappedValues, 2)
                )
        );
    }

    /// <summary>
    /// Maps 4 selected properties with mappers and transforms the mapped values when every mapper succeeds.
    /// </summary>
    /// <param name="value">The object to validate and map.</param>
    /// <param name="validator1">The first property selector and mapper.</param>
    /// <param name="validator2">The second property selector and mapper.</param>
    /// <param name="validator3">The third property selector and mapper.</param>
    /// <param name="validator4">The fourth property selector and mapper.</param>
    /// <param name="transform">The final transform to run after every mapper succeeds.</param>
    /// <returns>The transformed success value when every mapper succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, any mapper, or the final transform delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = person.ValidateThenTransform(validator1, validator2, transform);</code></example>
    public static FailOr<TResult> ValidateThenTransform<
        T,
        TProp1,
        TMapped1,
        TProp2,
        TMapped2,
        TProp3,
        TMapped3,
        TProp4,
        TMapped4,
        TResult
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, FailOr<TMapped1>> mapper
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, FailOr<TMapped2>> mapper
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, FailOr<TMapped3>> mapper
        ) validator3,
        (
            Expression<Func<T, TProp4>> propertySelector,
            Func<TProp4, FailOr<TMapped4>> mapper
        ) validator4,
        Func<TMapped1, TMapped2, TMapped3, TMapped4, TResult> transform
    )
    {
        ArgumentNullException.ThrowIfNull(transform);

        return ValidateThenTransformCore(
            value,
            [
                ValidationMapRule<T>.Create(validator1.propertySelector, validator1.mapper),
                ValidationMapRule<T>.Create(validator2.propertySelector, validator2.mapper),
                ValidationMapRule<T>.Create(validator3.propertySelector, validator3.mapper),
                ValidationMapRule<T>.Create(validator4.propertySelector, validator4.mapper),
            ],
            mappedValues =>
                transform(
                    GetMappedValue<TMapped1>(mappedValues, 0),
                    GetMappedValue<TMapped2>(mappedValues, 1),
                    GetMappedValue<TMapped3>(mappedValues, 2),
                    GetMappedValue<TMapped4>(mappedValues, 3)
                )
        );
    }

    /// <summary>
    /// Maps 5 selected properties with mappers and transforms the mapped values when every mapper succeeds.
    /// </summary>
    /// <param name="value">The object to validate and map.</param>
    /// <param name="validator1">The first property selector and mapper.</param>
    /// <param name="validator2">The second property selector and mapper.</param>
    /// <param name="validator3">The third property selector and mapper.</param>
    /// <param name="validator4">The fourth property selector and mapper.</param>
    /// <param name="validator5">The fifth property selector and mapper.</param>
    /// <param name="transform">The final transform to run after every mapper succeeds.</param>
    /// <returns>The transformed success value when every mapper succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, any mapper, or the final transform delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = person.ValidateThenTransform(validator1, validator2, transform);</code></example>
    public static FailOr<TResult> ValidateThenTransform<
        T,
        TProp1,
        TMapped1,
        TProp2,
        TMapped2,
        TProp3,
        TMapped3,
        TProp4,
        TMapped4,
        TProp5,
        TMapped5,
        TResult
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, FailOr<TMapped1>> mapper
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, FailOr<TMapped2>> mapper
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, FailOr<TMapped3>> mapper
        ) validator3,
        (
            Expression<Func<T, TProp4>> propertySelector,
            Func<TProp4, FailOr<TMapped4>> mapper
        ) validator4,
        (
            Expression<Func<T, TProp5>> propertySelector,
            Func<TProp5, FailOr<TMapped5>> mapper
        ) validator5,
        Func<TMapped1, TMapped2, TMapped3, TMapped4, TMapped5, TResult> transform
    )
    {
        ArgumentNullException.ThrowIfNull(transform);

        return ValidateThenTransformCore(
            value,
            [
                ValidationMapRule<T>.Create(validator1.propertySelector, validator1.mapper),
                ValidationMapRule<T>.Create(validator2.propertySelector, validator2.mapper),
                ValidationMapRule<T>.Create(validator3.propertySelector, validator3.mapper),
                ValidationMapRule<T>.Create(validator4.propertySelector, validator4.mapper),
                ValidationMapRule<T>.Create(validator5.propertySelector, validator5.mapper),
            ],
            mappedValues =>
                transform(
                    GetMappedValue<TMapped1>(mappedValues, 0),
                    GetMappedValue<TMapped2>(mappedValues, 1),
                    GetMappedValue<TMapped3>(mappedValues, 2),
                    GetMappedValue<TMapped4>(mappedValues, 3),
                    GetMappedValue<TMapped5>(mappedValues, 4)
                )
        );
    }

    /// <summary>
    /// Maps 6 selected properties with mappers and transforms the mapped values when every mapper succeeds.
    /// </summary>
    /// <param name="value">The object to validate and map.</param>
    /// <param name="validator1">The first property selector and mapper.</param>
    /// <param name="validator2">The second property selector and mapper.</param>
    /// <param name="validator3">The third property selector and mapper.</param>
    /// <param name="validator4">The fourth property selector and mapper.</param>
    /// <param name="validator5">The fifth property selector and mapper.</param>
    /// <param name="validator6">The sixth property selector and mapper.</param>
    /// <param name="transform">The final transform to run after every mapper succeeds.</param>
    /// <returns>The transformed success value when every mapper succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, any mapper, or the final transform delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = person.ValidateThenTransform(validator1, validator2, transform);</code></example>
    public static FailOr<TResult> ValidateThenTransform<
        T,
        TProp1,
        TMapped1,
        TProp2,
        TMapped2,
        TProp3,
        TMapped3,
        TProp4,
        TMapped4,
        TProp5,
        TMapped5,
        TProp6,
        TMapped6,
        TResult
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, FailOr<TMapped1>> mapper
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, FailOr<TMapped2>> mapper
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, FailOr<TMapped3>> mapper
        ) validator3,
        (
            Expression<Func<T, TProp4>> propertySelector,
            Func<TProp4, FailOr<TMapped4>> mapper
        ) validator4,
        (
            Expression<Func<T, TProp5>> propertySelector,
            Func<TProp5, FailOr<TMapped5>> mapper
        ) validator5,
        (
            Expression<Func<T, TProp6>> propertySelector,
            Func<TProp6, FailOr<TMapped6>> mapper
        ) validator6,
        Func<TMapped1, TMapped2, TMapped3, TMapped4, TMapped5, TMapped6, TResult> transform
    )
    {
        ArgumentNullException.ThrowIfNull(transform);

        return ValidateThenTransformCore(
            value,
            [
                ValidationMapRule<T>.Create(validator1.propertySelector, validator1.mapper),
                ValidationMapRule<T>.Create(validator2.propertySelector, validator2.mapper),
                ValidationMapRule<T>.Create(validator3.propertySelector, validator3.mapper),
                ValidationMapRule<T>.Create(validator4.propertySelector, validator4.mapper),
                ValidationMapRule<T>.Create(validator5.propertySelector, validator5.mapper),
                ValidationMapRule<T>.Create(validator6.propertySelector, validator6.mapper),
            ],
            mappedValues =>
                transform(
                    GetMappedValue<TMapped1>(mappedValues, 0),
                    GetMappedValue<TMapped2>(mappedValues, 1),
                    GetMappedValue<TMapped3>(mappedValues, 2),
                    GetMappedValue<TMapped4>(mappedValues, 3),
                    GetMappedValue<TMapped5>(mappedValues, 4),
                    GetMappedValue<TMapped6>(mappedValues, 5)
                )
        );
    }

    /// <summary>
    /// Maps 7 selected properties with mappers and transforms the mapped values when every mapper succeeds.
    /// </summary>
    /// <param name="value">The object to validate and map.</param>
    /// <param name="validator1">The first property selector and mapper.</param>
    /// <param name="validator2">The second property selector and mapper.</param>
    /// <param name="validator3">The third property selector and mapper.</param>
    /// <param name="validator4">The fourth property selector and mapper.</param>
    /// <param name="validator5">The fifth property selector and mapper.</param>
    /// <param name="validator6">The sixth property selector and mapper.</param>
    /// <param name="validator7">The seventh property selector and mapper.</param>
    /// <param name="transform">The final transform to run after every mapper succeeds.</param>
    /// <returns>The transformed success value when every mapper succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, any mapper, or the final transform delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = person.ValidateThenTransform(validator1, validator2, transform);</code></example>
    public static FailOr<TResult> ValidateThenTransform<
        T,
        TProp1,
        TMapped1,
        TProp2,
        TMapped2,
        TProp3,
        TMapped3,
        TProp4,
        TMapped4,
        TProp5,
        TMapped5,
        TProp6,
        TMapped6,
        TProp7,
        TMapped7,
        TResult
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, FailOr<TMapped1>> mapper
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, FailOr<TMapped2>> mapper
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, FailOr<TMapped3>> mapper
        ) validator3,
        (
            Expression<Func<T, TProp4>> propertySelector,
            Func<TProp4, FailOr<TMapped4>> mapper
        ) validator4,
        (
            Expression<Func<T, TProp5>> propertySelector,
            Func<TProp5, FailOr<TMapped5>> mapper
        ) validator5,
        (
            Expression<Func<T, TProp6>> propertySelector,
            Func<TProp6, FailOr<TMapped6>> mapper
        ) validator6,
        (
            Expression<Func<T, TProp7>> propertySelector,
            Func<TProp7, FailOr<TMapped7>> mapper
        ) validator7,
        Func<
            TMapped1,
            TMapped2,
            TMapped3,
            TMapped4,
            TMapped5,
            TMapped6,
            TMapped7,
            TResult
        > transform
    )
    {
        ArgumentNullException.ThrowIfNull(transform);

        return ValidateThenTransformCore(
            value,
            [
                ValidationMapRule<T>.Create(validator1.propertySelector, validator1.mapper),
                ValidationMapRule<T>.Create(validator2.propertySelector, validator2.mapper),
                ValidationMapRule<T>.Create(validator3.propertySelector, validator3.mapper),
                ValidationMapRule<T>.Create(validator4.propertySelector, validator4.mapper),
                ValidationMapRule<T>.Create(validator5.propertySelector, validator5.mapper),
                ValidationMapRule<T>.Create(validator6.propertySelector, validator6.mapper),
                ValidationMapRule<T>.Create(validator7.propertySelector, validator7.mapper),
            ],
            mappedValues =>
                transform(
                    GetMappedValue<TMapped1>(mappedValues, 0),
                    GetMappedValue<TMapped2>(mappedValues, 1),
                    GetMappedValue<TMapped3>(mappedValues, 2),
                    GetMappedValue<TMapped4>(mappedValues, 3),
                    GetMappedValue<TMapped5>(mappedValues, 4),
                    GetMappedValue<TMapped6>(mappedValues, 5),
                    GetMappedValue<TMapped7>(mappedValues, 6)
                )
        );
    }

    /// <summary>
    /// Maps 8 selected properties with mappers and transforms the mapped values when every mapper succeeds.
    /// </summary>
    /// <param name="value">The object to validate and map.</param>
    /// <param name="validator1">The first property selector and mapper.</param>
    /// <param name="validator2">The second property selector and mapper.</param>
    /// <param name="validator3">The third property selector and mapper.</param>
    /// <param name="validator4">The fourth property selector and mapper.</param>
    /// <param name="validator5">The fifth property selector and mapper.</param>
    /// <param name="validator6">The sixth property selector and mapper.</param>
    /// <param name="validator7">The seventh property selector and mapper.</param>
    /// <param name="validator8">The eighth property selector and mapper.</param>
    /// <param name="transform">The final transform to run after every mapper succeeds.</param>
    /// <returns>The transformed success value when every mapper succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, any mapper, or the final transform delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = person.ValidateThenTransform(validator1, validator2, transform);</code></example>
    public static FailOr<TResult> ValidateThenTransform<
        T,
        TProp1,
        TMapped1,
        TProp2,
        TMapped2,
        TProp3,
        TMapped3,
        TProp4,
        TMapped4,
        TProp5,
        TMapped5,
        TProp6,
        TMapped6,
        TProp7,
        TMapped7,
        TProp8,
        TMapped8,
        TResult
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, FailOr<TMapped1>> mapper
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, FailOr<TMapped2>> mapper
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, FailOr<TMapped3>> mapper
        ) validator3,
        (
            Expression<Func<T, TProp4>> propertySelector,
            Func<TProp4, FailOr<TMapped4>> mapper
        ) validator4,
        (
            Expression<Func<T, TProp5>> propertySelector,
            Func<TProp5, FailOr<TMapped5>> mapper
        ) validator5,
        (
            Expression<Func<T, TProp6>> propertySelector,
            Func<TProp6, FailOr<TMapped6>> mapper
        ) validator6,
        (
            Expression<Func<T, TProp7>> propertySelector,
            Func<TProp7, FailOr<TMapped7>> mapper
        ) validator7,
        (
            Expression<Func<T, TProp8>> propertySelector,
            Func<TProp8, FailOr<TMapped8>> mapper
        ) validator8,
        Func<
            TMapped1,
            TMapped2,
            TMapped3,
            TMapped4,
            TMapped5,
            TMapped6,
            TMapped7,
            TMapped8,
            TResult
        > transform
    )
    {
        ArgumentNullException.ThrowIfNull(transform);

        return ValidateThenTransformCore(
            value,
            [
                ValidationMapRule<T>.Create(validator1.propertySelector, validator1.mapper),
                ValidationMapRule<T>.Create(validator2.propertySelector, validator2.mapper),
                ValidationMapRule<T>.Create(validator3.propertySelector, validator3.mapper),
                ValidationMapRule<T>.Create(validator4.propertySelector, validator4.mapper),
                ValidationMapRule<T>.Create(validator5.propertySelector, validator5.mapper),
                ValidationMapRule<T>.Create(validator6.propertySelector, validator6.mapper),
                ValidationMapRule<T>.Create(validator7.propertySelector, validator7.mapper),
                ValidationMapRule<T>.Create(validator8.propertySelector, validator8.mapper),
            ],
            mappedValues =>
                transform(
                    GetMappedValue<TMapped1>(mappedValues, 0),
                    GetMappedValue<TMapped2>(mappedValues, 1),
                    GetMappedValue<TMapped3>(mappedValues, 2),
                    GetMappedValue<TMapped4>(mappedValues, 3),
                    GetMappedValue<TMapped5>(mappedValues, 4),
                    GetMappedValue<TMapped6>(mappedValues, 5),
                    GetMappedValue<TMapped7>(mappedValues, 6),
                    GetMappedValue<TMapped8>(mappedValues, 7)
                )
        );
    }

    /// <summary>
    /// Maps 9 selected properties with mappers and transforms the mapped values when every mapper succeeds.
    /// </summary>
    /// <param name="value">The object to validate and map.</param>
    /// <param name="validator1">The first property selector and mapper.</param>
    /// <param name="validator2">The second property selector and mapper.</param>
    /// <param name="validator3">The third property selector and mapper.</param>
    /// <param name="validator4">The fourth property selector and mapper.</param>
    /// <param name="validator5">The fifth property selector and mapper.</param>
    /// <param name="validator6">The sixth property selector and mapper.</param>
    /// <param name="validator7">The seventh property selector and mapper.</param>
    /// <param name="validator8">The eighth property selector and mapper.</param>
    /// <param name="validator9">The ninth property selector and mapper.</param>
    /// <param name="transform">The final transform to run after every mapper succeeds.</param>
    /// <returns>The transformed success value when every mapper succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, any mapper, or the final transform delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = person.ValidateThenTransform(validator1, validator2, transform);</code></example>
    public static FailOr<TResult> ValidateThenTransform<
        T,
        TProp1,
        TMapped1,
        TProp2,
        TMapped2,
        TProp3,
        TMapped3,
        TProp4,
        TMapped4,
        TProp5,
        TMapped5,
        TProp6,
        TMapped6,
        TProp7,
        TMapped7,
        TProp8,
        TMapped8,
        TProp9,
        TMapped9,
        TResult
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, FailOr<TMapped1>> mapper
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, FailOr<TMapped2>> mapper
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, FailOr<TMapped3>> mapper
        ) validator3,
        (
            Expression<Func<T, TProp4>> propertySelector,
            Func<TProp4, FailOr<TMapped4>> mapper
        ) validator4,
        (
            Expression<Func<T, TProp5>> propertySelector,
            Func<TProp5, FailOr<TMapped5>> mapper
        ) validator5,
        (
            Expression<Func<T, TProp6>> propertySelector,
            Func<TProp6, FailOr<TMapped6>> mapper
        ) validator6,
        (
            Expression<Func<T, TProp7>> propertySelector,
            Func<TProp7, FailOr<TMapped7>> mapper
        ) validator7,
        (
            Expression<Func<T, TProp8>> propertySelector,
            Func<TProp8, FailOr<TMapped8>> mapper
        ) validator8,
        (
            Expression<Func<T, TProp9>> propertySelector,
            Func<TProp9, FailOr<TMapped9>> mapper
        ) validator9,
        Func<
            TMapped1,
            TMapped2,
            TMapped3,
            TMapped4,
            TMapped5,
            TMapped6,
            TMapped7,
            TMapped8,
            TMapped9,
            TResult
        > transform
    )
    {
        ArgumentNullException.ThrowIfNull(transform);

        return ValidateThenTransformCore(
            value,
            [
                ValidationMapRule<T>.Create(validator1.propertySelector, validator1.mapper),
                ValidationMapRule<T>.Create(validator2.propertySelector, validator2.mapper),
                ValidationMapRule<T>.Create(validator3.propertySelector, validator3.mapper),
                ValidationMapRule<T>.Create(validator4.propertySelector, validator4.mapper),
                ValidationMapRule<T>.Create(validator5.propertySelector, validator5.mapper),
                ValidationMapRule<T>.Create(validator6.propertySelector, validator6.mapper),
                ValidationMapRule<T>.Create(validator7.propertySelector, validator7.mapper),
                ValidationMapRule<T>.Create(validator8.propertySelector, validator8.mapper),
                ValidationMapRule<T>.Create(validator9.propertySelector, validator9.mapper),
            ],
            mappedValues =>
                transform(
                    GetMappedValue<TMapped1>(mappedValues, 0),
                    GetMappedValue<TMapped2>(mappedValues, 1),
                    GetMappedValue<TMapped3>(mappedValues, 2),
                    GetMappedValue<TMapped4>(mappedValues, 3),
                    GetMappedValue<TMapped5>(mappedValues, 4),
                    GetMappedValue<TMapped6>(mappedValues, 5),
                    GetMappedValue<TMapped7>(mappedValues, 6),
                    GetMappedValue<TMapped8>(mappedValues, 7),
                    GetMappedValue<TMapped9>(mappedValues, 8)
                )
        );
    }

    /// <summary>
    /// Maps 10 selected properties with mappers and transforms the mapped values when every mapper succeeds.
    /// </summary>
    /// <param name="value">The object to validate and map.</param>
    /// <param name="validator1">The first property selector and mapper.</param>
    /// <param name="validator2">The second property selector and mapper.</param>
    /// <param name="validator3">The third property selector and mapper.</param>
    /// <param name="validator4">The fourth property selector and mapper.</param>
    /// <param name="validator5">The fifth property selector and mapper.</param>
    /// <param name="validator6">The sixth property selector and mapper.</param>
    /// <param name="validator7">The seventh property selector and mapper.</param>
    /// <param name="validator8">The eighth property selector and mapper.</param>
    /// <param name="validator9">The ninth property selector and mapper.</param>
    /// <param name="validator10">The tenth property selector and mapper.</param>
    /// <param name="transform">The final transform to run after every mapper succeeds.</param>
    /// <returns>The transformed success value when every mapper succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, any mapper, or the final transform delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = person.ValidateThenTransform(validator1, validator2, transform);</code></example>
    public static FailOr<TResult> ValidateThenTransform<
        T,
        TProp1,
        TMapped1,
        TProp2,
        TMapped2,
        TProp3,
        TMapped3,
        TProp4,
        TMapped4,
        TProp5,
        TMapped5,
        TProp6,
        TMapped6,
        TProp7,
        TMapped7,
        TProp8,
        TMapped8,
        TProp9,
        TMapped9,
        TProp10,
        TMapped10,
        TResult
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, FailOr<TMapped1>> mapper
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, FailOr<TMapped2>> mapper
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, FailOr<TMapped3>> mapper
        ) validator3,
        (
            Expression<Func<T, TProp4>> propertySelector,
            Func<TProp4, FailOr<TMapped4>> mapper
        ) validator4,
        (
            Expression<Func<T, TProp5>> propertySelector,
            Func<TProp5, FailOr<TMapped5>> mapper
        ) validator5,
        (
            Expression<Func<T, TProp6>> propertySelector,
            Func<TProp6, FailOr<TMapped6>> mapper
        ) validator6,
        (
            Expression<Func<T, TProp7>> propertySelector,
            Func<TProp7, FailOr<TMapped7>> mapper
        ) validator7,
        (
            Expression<Func<T, TProp8>> propertySelector,
            Func<TProp8, FailOr<TMapped8>> mapper
        ) validator8,
        (
            Expression<Func<T, TProp9>> propertySelector,
            Func<TProp9, FailOr<TMapped9>> mapper
        ) validator9,
        (
            Expression<Func<T, TProp10>> propertySelector,
            Func<TProp10, FailOr<TMapped10>> mapper
        ) validator10,
        Func<
            TMapped1,
            TMapped2,
            TMapped3,
            TMapped4,
            TMapped5,
            TMapped6,
            TMapped7,
            TMapped8,
            TMapped9,
            TMapped10,
            TResult
        > transform
    )
    {
        ArgumentNullException.ThrowIfNull(transform);

        return ValidateThenTransformCore(
            value,
            [
                ValidationMapRule<T>.Create(validator1.propertySelector, validator1.mapper),
                ValidationMapRule<T>.Create(validator2.propertySelector, validator2.mapper),
                ValidationMapRule<T>.Create(validator3.propertySelector, validator3.mapper),
                ValidationMapRule<T>.Create(validator4.propertySelector, validator4.mapper),
                ValidationMapRule<T>.Create(validator5.propertySelector, validator5.mapper),
                ValidationMapRule<T>.Create(validator6.propertySelector, validator6.mapper),
                ValidationMapRule<T>.Create(validator7.propertySelector, validator7.mapper),
                ValidationMapRule<T>.Create(validator8.propertySelector, validator8.mapper),
                ValidationMapRule<T>.Create(validator9.propertySelector, validator9.mapper),
                ValidationMapRule<T>.Create(validator10.propertySelector, validator10.mapper),
            ],
            mappedValues =>
                transform(
                    GetMappedValue<TMapped1>(mappedValues, 0),
                    GetMappedValue<TMapped2>(mappedValues, 1),
                    GetMappedValue<TMapped3>(mappedValues, 2),
                    GetMappedValue<TMapped4>(mappedValues, 3),
                    GetMappedValue<TMapped5>(mappedValues, 4),
                    GetMappedValue<TMapped6>(mappedValues, 5),
                    GetMappedValue<TMapped7>(mappedValues, 6),
                    GetMappedValue<TMapped8>(mappedValues, 7),
                    GetMappedValue<TMapped9>(mappedValues, 8),
                    GetMappedValue<TMapped10>(mappedValues, 9)
                )
        );
    }

    /// <summary>
    /// Maps 11 selected properties with mappers and transforms the mapped values when every mapper succeeds.
    /// </summary>
    /// <param name="value">The object to validate and map.</param>
    /// <param name="validator1">The first property selector and mapper.</param>
    /// <param name="validator2">The second property selector and mapper.</param>
    /// <param name="validator3">The third property selector and mapper.</param>
    /// <param name="validator4">The fourth property selector and mapper.</param>
    /// <param name="validator5">The fifth property selector and mapper.</param>
    /// <param name="validator6">The sixth property selector and mapper.</param>
    /// <param name="validator7">The seventh property selector and mapper.</param>
    /// <param name="validator8">The eighth property selector and mapper.</param>
    /// <param name="validator9">The ninth property selector and mapper.</param>
    /// <param name="validator10">The tenth property selector and mapper.</param>
    /// <param name="validator11">The eleventh property selector and mapper.</param>
    /// <param name="transform">The final transform to run after every mapper succeeds.</param>
    /// <returns>The transformed success value when every mapper succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, any mapper, or the final transform delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = person.ValidateThenTransform(validator1, validator2, transform);</code></example>
    public static FailOr<TResult> ValidateThenTransform<
        T,
        TProp1,
        TMapped1,
        TProp2,
        TMapped2,
        TProp3,
        TMapped3,
        TProp4,
        TMapped4,
        TProp5,
        TMapped5,
        TProp6,
        TMapped6,
        TProp7,
        TMapped7,
        TProp8,
        TMapped8,
        TProp9,
        TMapped9,
        TProp10,
        TMapped10,
        TProp11,
        TMapped11,
        TResult
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, FailOr<TMapped1>> mapper
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, FailOr<TMapped2>> mapper
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, FailOr<TMapped3>> mapper
        ) validator3,
        (
            Expression<Func<T, TProp4>> propertySelector,
            Func<TProp4, FailOr<TMapped4>> mapper
        ) validator4,
        (
            Expression<Func<T, TProp5>> propertySelector,
            Func<TProp5, FailOr<TMapped5>> mapper
        ) validator5,
        (
            Expression<Func<T, TProp6>> propertySelector,
            Func<TProp6, FailOr<TMapped6>> mapper
        ) validator6,
        (
            Expression<Func<T, TProp7>> propertySelector,
            Func<TProp7, FailOr<TMapped7>> mapper
        ) validator7,
        (
            Expression<Func<T, TProp8>> propertySelector,
            Func<TProp8, FailOr<TMapped8>> mapper
        ) validator8,
        (
            Expression<Func<T, TProp9>> propertySelector,
            Func<TProp9, FailOr<TMapped9>> mapper
        ) validator9,
        (
            Expression<Func<T, TProp10>> propertySelector,
            Func<TProp10, FailOr<TMapped10>> mapper
        ) validator10,
        (
            Expression<Func<T, TProp11>> propertySelector,
            Func<TProp11, FailOr<TMapped11>> mapper
        ) validator11,
        Func<
            TMapped1,
            TMapped2,
            TMapped3,
            TMapped4,
            TMapped5,
            TMapped6,
            TMapped7,
            TMapped8,
            TMapped9,
            TMapped10,
            TMapped11,
            TResult
        > transform
    )
    {
        ArgumentNullException.ThrowIfNull(transform);

        return ValidateThenTransformCore(
            value,
            [
                ValidationMapRule<T>.Create(validator1.propertySelector, validator1.mapper),
                ValidationMapRule<T>.Create(validator2.propertySelector, validator2.mapper),
                ValidationMapRule<T>.Create(validator3.propertySelector, validator3.mapper),
                ValidationMapRule<T>.Create(validator4.propertySelector, validator4.mapper),
                ValidationMapRule<T>.Create(validator5.propertySelector, validator5.mapper),
                ValidationMapRule<T>.Create(validator6.propertySelector, validator6.mapper),
                ValidationMapRule<T>.Create(validator7.propertySelector, validator7.mapper),
                ValidationMapRule<T>.Create(validator8.propertySelector, validator8.mapper),
                ValidationMapRule<T>.Create(validator9.propertySelector, validator9.mapper),
                ValidationMapRule<T>.Create(validator10.propertySelector, validator10.mapper),
                ValidationMapRule<T>.Create(validator11.propertySelector, validator11.mapper),
            ],
            mappedValues =>
                transform(
                    GetMappedValue<TMapped1>(mappedValues, 0),
                    GetMappedValue<TMapped2>(mappedValues, 1),
                    GetMappedValue<TMapped3>(mappedValues, 2),
                    GetMappedValue<TMapped4>(mappedValues, 3),
                    GetMappedValue<TMapped5>(mappedValues, 4),
                    GetMappedValue<TMapped6>(mappedValues, 5),
                    GetMappedValue<TMapped7>(mappedValues, 6),
                    GetMappedValue<TMapped8>(mappedValues, 7),
                    GetMappedValue<TMapped9>(mappedValues, 8),
                    GetMappedValue<TMapped10>(mappedValues, 9),
                    GetMappedValue<TMapped11>(mappedValues, 10)
                )
        );
    }

    /// <summary>
    /// Maps 12 selected properties with mappers and transforms the mapped values when every mapper succeeds.
    /// </summary>
    /// <param name="value">The object to validate and map.</param>
    /// <param name="validator1">The first property selector and mapper.</param>
    /// <param name="validator2">The second property selector and mapper.</param>
    /// <param name="validator3">The third property selector and mapper.</param>
    /// <param name="validator4">The fourth property selector and mapper.</param>
    /// <param name="validator5">The fifth property selector and mapper.</param>
    /// <param name="validator6">The sixth property selector and mapper.</param>
    /// <param name="validator7">The seventh property selector and mapper.</param>
    /// <param name="validator8">The eighth property selector and mapper.</param>
    /// <param name="validator9">The ninth property selector and mapper.</param>
    /// <param name="validator10">The tenth property selector and mapper.</param>
    /// <param name="validator11">The eleventh property selector and mapper.</param>
    /// <param name="validator12">The twelfth property selector and mapper.</param>
    /// <param name="transform">The final transform to run after every mapper succeeds.</param>
    /// <returns>The transformed success value when every mapper succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, any mapper, or the final transform delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = person.ValidateThenTransform(validator1, validator2, transform);</code></example>
    public static FailOr<TResult> ValidateThenTransform<
        T,
        TProp1,
        TMapped1,
        TProp2,
        TMapped2,
        TProp3,
        TMapped3,
        TProp4,
        TMapped4,
        TProp5,
        TMapped5,
        TProp6,
        TMapped6,
        TProp7,
        TMapped7,
        TProp8,
        TMapped8,
        TProp9,
        TMapped9,
        TProp10,
        TMapped10,
        TProp11,
        TMapped11,
        TProp12,
        TMapped12,
        TResult
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, FailOr<TMapped1>> mapper
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, FailOr<TMapped2>> mapper
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, FailOr<TMapped3>> mapper
        ) validator3,
        (
            Expression<Func<T, TProp4>> propertySelector,
            Func<TProp4, FailOr<TMapped4>> mapper
        ) validator4,
        (
            Expression<Func<T, TProp5>> propertySelector,
            Func<TProp5, FailOr<TMapped5>> mapper
        ) validator5,
        (
            Expression<Func<T, TProp6>> propertySelector,
            Func<TProp6, FailOr<TMapped6>> mapper
        ) validator6,
        (
            Expression<Func<T, TProp7>> propertySelector,
            Func<TProp7, FailOr<TMapped7>> mapper
        ) validator7,
        (
            Expression<Func<T, TProp8>> propertySelector,
            Func<TProp8, FailOr<TMapped8>> mapper
        ) validator8,
        (
            Expression<Func<T, TProp9>> propertySelector,
            Func<TProp9, FailOr<TMapped9>> mapper
        ) validator9,
        (
            Expression<Func<T, TProp10>> propertySelector,
            Func<TProp10, FailOr<TMapped10>> mapper
        ) validator10,
        (
            Expression<Func<T, TProp11>> propertySelector,
            Func<TProp11, FailOr<TMapped11>> mapper
        ) validator11,
        (
            Expression<Func<T, TProp12>> propertySelector,
            Func<TProp12, FailOr<TMapped12>> mapper
        ) validator12,
        Func<
            TMapped1,
            TMapped2,
            TMapped3,
            TMapped4,
            TMapped5,
            TMapped6,
            TMapped7,
            TMapped8,
            TMapped9,
            TMapped10,
            TMapped11,
            TMapped12,
            TResult
        > transform
    )
    {
        ArgumentNullException.ThrowIfNull(transform);

        return ValidateThenTransformCore(
            value,
            [
                ValidationMapRule<T>.Create(validator1.propertySelector, validator1.mapper),
                ValidationMapRule<T>.Create(validator2.propertySelector, validator2.mapper),
                ValidationMapRule<T>.Create(validator3.propertySelector, validator3.mapper),
                ValidationMapRule<T>.Create(validator4.propertySelector, validator4.mapper),
                ValidationMapRule<T>.Create(validator5.propertySelector, validator5.mapper),
                ValidationMapRule<T>.Create(validator6.propertySelector, validator6.mapper),
                ValidationMapRule<T>.Create(validator7.propertySelector, validator7.mapper),
                ValidationMapRule<T>.Create(validator8.propertySelector, validator8.mapper),
                ValidationMapRule<T>.Create(validator9.propertySelector, validator9.mapper),
                ValidationMapRule<T>.Create(validator10.propertySelector, validator10.mapper),
                ValidationMapRule<T>.Create(validator11.propertySelector, validator11.mapper),
                ValidationMapRule<T>.Create(validator12.propertySelector, validator12.mapper),
            ],
            mappedValues =>
                transform(
                    GetMappedValue<TMapped1>(mappedValues, 0),
                    GetMappedValue<TMapped2>(mappedValues, 1),
                    GetMappedValue<TMapped3>(mappedValues, 2),
                    GetMappedValue<TMapped4>(mappedValues, 3),
                    GetMappedValue<TMapped5>(mappedValues, 4),
                    GetMappedValue<TMapped6>(mappedValues, 5),
                    GetMappedValue<TMapped7>(mappedValues, 6),
                    GetMappedValue<TMapped8>(mappedValues, 7),
                    GetMappedValue<TMapped9>(mappedValues, 8),
                    GetMappedValue<TMapped10>(mappedValues, 9),
                    GetMappedValue<TMapped11>(mappedValues, 10),
                    GetMappedValue<TMapped12>(mappedValues, 11)
                )
        );
    }

    /// <summary>
    /// Maps 13 selected properties with mappers and transforms the mapped values when every mapper succeeds.
    /// </summary>
    /// <param name="value">The object to validate and map.</param>
    /// <param name="validator1">The first property selector and mapper.</param>
    /// <param name="validator2">The second property selector and mapper.</param>
    /// <param name="validator3">The third property selector and mapper.</param>
    /// <param name="validator4">The fourth property selector and mapper.</param>
    /// <param name="validator5">The fifth property selector and mapper.</param>
    /// <param name="validator6">The sixth property selector and mapper.</param>
    /// <param name="validator7">The seventh property selector and mapper.</param>
    /// <param name="validator8">The eighth property selector and mapper.</param>
    /// <param name="validator9">The ninth property selector and mapper.</param>
    /// <param name="validator10">The tenth property selector and mapper.</param>
    /// <param name="validator11">The eleventh property selector and mapper.</param>
    /// <param name="validator12">The twelfth property selector and mapper.</param>
    /// <param name="validator13">The thirteenth property selector and mapper.</param>
    /// <param name="transform">The final transform to run after every mapper succeeds.</param>
    /// <returns>The transformed success value when every mapper succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, any mapper, or the final transform delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = person.ValidateThenTransform(validator1, validator2, transform);</code></example>
    public static FailOr<TResult> ValidateThenTransform<
        T,
        TProp1,
        TMapped1,
        TProp2,
        TMapped2,
        TProp3,
        TMapped3,
        TProp4,
        TMapped4,
        TProp5,
        TMapped5,
        TProp6,
        TMapped6,
        TProp7,
        TMapped7,
        TProp8,
        TMapped8,
        TProp9,
        TMapped9,
        TProp10,
        TMapped10,
        TProp11,
        TMapped11,
        TProp12,
        TMapped12,
        TProp13,
        TMapped13,
        TResult
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, FailOr<TMapped1>> mapper
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, FailOr<TMapped2>> mapper
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, FailOr<TMapped3>> mapper
        ) validator3,
        (
            Expression<Func<T, TProp4>> propertySelector,
            Func<TProp4, FailOr<TMapped4>> mapper
        ) validator4,
        (
            Expression<Func<T, TProp5>> propertySelector,
            Func<TProp5, FailOr<TMapped5>> mapper
        ) validator5,
        (
            Expression<Func<T, TProp6>> propertySelector,
            Func<TProp6, FailOr<TMapped6>> mapper
        ) validator6,
        (
            Expression<Func<T, TProp7>> propertySelector,
            Func<TProp7, FailOr<TMapped7>> mapper
        ) validator7,
        (
            Expression<Func<T, TProp8>> propertySelector,
            Func<TProp8, FailOr<TMapped8>> mapper
        ) validator8,
        (
            Expression<Func<T, TProp9>> propertySelector,
            Func<TProp9, FailOr<TMapped9>> mapper
        ) validator9,
        (
            Expression<Func<T, TProp10>> propertySelector,
            Func<TProp10, FailOr<TMapped10>> mapper
        ) validator10,
        (
            Expression<Func<T, TProp11>> propertySelector,
            Func<TProp11, FailOr<TMapped11>> mapper
        ) validator11,
        (
            Expression<Func<T, TProp12>> propertySelector,
            Func<TProp12, FailOr<TMapped12>> mapper
        ) validator12,
        (
            Expression<Func<T, TProp13>> propertySelector,
            Func<TProp13, FailOr<TMapped13>> mapper
        ) validator13,
        Func<
            TMapped1,
            TMapped2,
            TMapped3,
            TMapped4,
            TMapped5,
            TMapped6,
            TMapped7,
            TMapped8,
            TMapped9,
            TMapped10,
            TMapped11,
            TMapped12,
            TMapped13,
            TResult
        > transform
    )
    {
        ArgumentNullException.ThrowIfNull(transform);

        return ValidateThenTransformCore(
            value,
            [
                ValidationMapRule<T>.Create(validator1.propertySelector, validator1.mapper),
                ValidationMapRule<T>.Create(validator2.propertySelector, validator2.mapper),
                ValidationMapRule<T>.Create(validator3.propertySelector, validator3.mapper),
                ValidationMapRule<T>.Create(validator4.propertySelector, validator4.mapper),
                ValidationMapRule<T>.Create(validator5.propertySelector, validator5.mapper),
                ValidationMapRule<T>.Create(validator6.propertySelector, validator6.mapper),
                ValidationMapRule<T>.Create(validator7.propertySelector, validator7.mapper),
                ValidationMapRule<T>.Create(validator8.propertySelector, validator8.mapper),
                ValidationMapRule<T>.Create(validator9.propertySelector, validator9.mapper),
                ValidationMapRule<T>.Create(validator10.propertySelector, validator10.mapper),
                ValidationMapRule<T>.Create(validator11.propertySelector, validator11.mapper),
                ValidationMapRule<T>.Create(validator12.propertySelector, validator12.mapper),
                ValidationMapRule<T>.Create(validator13.propertySelector, validator13.mapper),
            ],
            mappedValues =>
                transform(
                    GetMappedValue<TMapped1>(mappedValues, 0),
                    GetMappedValue<TMapped2>(mappedValues, 1),
                    GetMappedValue<TMapped3>(mappedValues, 2),
                    GetMappedValue<TMapped4>(mappedValues, 3),
                    GetMappedValue<TMapped5>(mappedValues, 4),
                    GetMappedValue<TMapped6>(mappedValues, 5),
                    GetMappedValue<TMapped7>(mappedValues, 6),
                    GetMappedValue<TMapped8>(mappedValues, 7),
                    GetMappedValue<TMapped9>(mappedValues, 8),
                    GetMappedValue<TMapped10>(mappedValues, 9),
                    GetMappedValue<TMapped11>(mappedValues, 10),
                    GetMappedValue<TMapped12>(mappedValues, 11),
                    GetMappedValue<TMapped13>(mappedValues, 12)
                )
        );
    }

    /// <summary>
    /// Maps 14 selected properties with mappers and transforms the mapped values when every mapper succeeds.
    /// </summary>
    /// <param name="value">The object to validate and map.</param>
    /// <param name="validator1">The first property selector and mapper.</param>
    /// <param name="validator2">The second property selector and mapper.</param>
    /// <param name="validator3">The third property selector and mapper.</param>
    /// <param name="validator4">The fourth property selector and mapper.</param>
    /// <param name="validator5">The fifth property selector and mapper.</param>
    /// <param name="validator6">The sixth property selector and mapper.</param>
    /// <param name="validator7">The seventh property selector and mapper.</param>
    /// <param name="validator8">The eighth property selector and mapper.</param>
    /// <param name="validator9">The ninth property selector and mapper.</param>
    /// <param name="validator10">The tenth property selector and mapper.</param>
    /// <param name="validator11">The eleventh property selector and mapper.</param>
    /// <param name="validator12">The twelfth property selector and mapper.</param>
    /// <param name="validator13">The thirteenth property selector and mapper.</param>
    /// <param name="validator14">The fourteenth property selector and mapper.</param>
    /// <param name="transform">The final transform to run after every mapper succeeds.</param>
    /// <returns>The transformed success value when every mapper succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, any mapper, or the final transform delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = person.ValidateThenTransform(validator1, validator2, transform);</code></example>
    public static FailOr<TResult> ValidateThenTransform<
        T,
        TProp1,
        TMapped1,
        TProp2,
        TMapped2,
        TProp3,
        TMapped3,
        TProp4,
        TMapped4,
        TProp5,
        TMapped5,
        TProp6,
        TMapped6,
        TProp7,
        TMapped7,
        TProp8,
        TMapped8,
        TProp9,
        TMapped9,
        TProp10,
        TMapped10,
        TProp11,
        TMapped11,
        TProp12,
        TMapped12,
        TProp13,
        TMapped13,
        TProp14,
        TMapped14,
        TResult
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, FailOr<TMapped1>> mapper
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, FailOr<TMapped2>> mapper
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, FailOr<TMapped3>> mapper
        ) validator3,
        (
            Expression<Func<T, TProp4>> propertySelector,
            Func<TProp4, FailOr<TMapped4>> mapper
        ) validator4,
        (
            Expression<Func<T, TProp5>> propertySelector,
            Func<TProp5, FailOr<TMapped5>> mapper
        ) validator5,
        (
            Expression<Func<T, TProp6>> propertySelector,
            Func<TProp6, FailOr<TMapped6>> mapper
        ) validator6,
        (
            Expression<Func<T, TProp7>> propertySelector,
            Func<TProp7, FailOr<TMapped7>> mapper
        ) validator7,
        (
            Expression<Func<T, TProp8>> propertySelector,
            Func<TProp8, FailOr<TMapped8>> mapper
        ) validator8,
        (
            Expression<Func<T, TProp9>> propertySelector,
            Func<TProp9, FailOr<TMapped9>> mapper
        ) validator9,
        (
            Expression<Func<T, TProp10>> propertySelector,
            Func<TProp10, FailOr<TMapped10>> mapper
        ) validator10,
        (
            Expression<Func<T, TProp11>> propertySelector,
            Func<TProp11, FailOr<TMapped11>> mapper
        ) validator11,
        (
            Expression<Func<T, TProp12>> propertySelector,
            Func<TProp12, FailOr<TMapped12>> mapper
        ) validator12,
        (
            Expression<Func<T, TProp13>> propertySelector,
            Func<TProp13, FailOr<TMapped13>> mapper
        ) validator13,
        (
            Expression<Func<T, TProp14>> propertySelector,
            Func<TProp14, FailOr<TMapped14>> mapper
        ) validator14,
        Func<
            TMapped1,
            TMapped2,
            TMapped3,
            TMapped4,
            TMapped5,
            TMapped6,
            TMapped7,
            TMapped8,
            TMapped9,
            TMapped10,
            TMapped11,
            TMapped12,
            TMapped13,
            TMapped14,
            TResult
        > transform
    )
    {
        ArgumentNullException.ThrowIfNull(transform);

        return ValidateThenTransformCore(
            value,
            [
                ValidationMapRule<T>.Create(validator1.propertySelector, validator1.mapper),
                ValidationMapRule<T>.Create(validator2.propertySelector, validator2.mapper),
                ValidationMapRule<T>.Create(validator3.propertySelector, validator3.mapper),
                ValidationMapRule<T>.Create(validator4.propertySelector, validator4.mapper),
                ValidationMapRule<T>.Create(validator5.propertySelector, validator5.mapper),
                ValidationMapRule<T>.Create(validator6.propertySelector, validator6.mapper),
                ValidationMapRule<T>.Create(validator7.propertySelector, validator7.mapper),
                ValidationMapRule<T>.Create(validator8.propertySelector, validator8.mapper),
                ValidationMapRule<T>.Create(validator9.propertySelector, validator9.mapper),
                ValidationMapRule<T>.Create(validator10.propertySelector, validator10.mapper),
                ValidationMapRule<T>.Create(validator11.propertySelector, validator11.mapper),
                ValidationMapRule<T>.Create(validator12.propertySelector, validator12.mapper),
                ValidationMapRule<T>.Create(validator13.propertySelector, validator13.mapper),
                ValidationMapRule<T>.Create(validator14.propertySelector, validator14.mapper),
            ],
            mappedValues =>
                transform(
                    GetMappedValue<TMapped1>(mappedValues, 0),
                    GetMappedValue<TMapped2>(mappedValues, 1),
                    GetMappedValue<TMapped3>(mappedValues, 2),
                    GetMappedValue<TMapped4>(mappedValues, 3),
                    GetMappedValue<TMapped5>(mappedValues, 4),
                    GetMappedValue<TMapped6>(mappedValues, 5),
                    GetMappedValue<TMapped7>(mappedValues, 6),
                    GetMappedValue<TMapped8>(mappedValues, 7),
                    GetMappedValue<TMapped9>(mappedValues, 8),
                    GetMappedValue<TMapped10>(mappedValues, 9),
                    GetMappedValue<TMapped11>(mappedValues, 10),
                    GetMappedValue<TMapped12>(mappedValues, 11),
                    GetMappedValue<TMapped13>(mappedValues, 12),
                    GetMappedValue<TMapped14>(mappedValues, 13)
                )
        );
    }

    /// <summary>
    /// Maps 1 selected property with async mappers and transforms the mapped values when every mapper succeeds.
    /// </summary>
    /// <param name="value">The object to validate and map.</param>
    /// <param name="validator1">The first property selector and mapper.</param>
    /// <param name="transform">The final transform to run after every mapper succeeds.</param>
    /// <returns>The transformed success value when every mapper succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, any mapper, or the final transform delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = await person.ValidateThenTransformAsync(validator1, transform);</code></example>
    public static Task<FailOr<TResult>> ValidateThenTransformAsync<T, TProp1, TMapped1, TResult>(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, Task<FailOr<TMapped1>>> mapperAsync
        ) validator1,
        Func<TMapped1, TResult> transform
    )
    {
        ArgumentNullException.ThrowIfNull(transform);

        return ValidateThenTransformAsyncCore(
            value,
            [AsyncValidationMapRule<T>.Create(validator1.propertySelector, validator1.mapperAsync)],
            mappedValues => transform(GetMappedValue<TMapped1>(mappedValues, 0))
        );
    }

    /// <summary>
    /// Maps 2 selected properties with async mappers and transforms the mapped values when every mapper succeeds.
    /// </summary>
    /// <param name="value">The object to validate and map.</param>
    /// <param name="validator1">The first property selector and mapper.</param>
    /// <param name="validator2">The second property selector and mapper.</param>
    /// <param name="transform">The final transform to run after every mapper succeeds.</param>
    /// <returns>The transformed success value when every mapper succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, any mapper, or the final transform delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = await person.ValidateThenTransformAsync(validator1, validator2, transform);</code></example>
    public static Task<FailOr<TResult>> ValidateThenTransformAsync<
        T,
        TProp1,
        TMapped1,
        TProp2,
        TMapped2,
        TResult
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, Task<FailOr<TMapped1>>> mapperAsync
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, Task<FailOr<TMapped2>>> mapperAsync
        ) validator2,
        Func<TMapped1, TMapped2, TResult> transform
    )
    {
        ArgumentNullException.ThrowIfNull(transform);

        return ValidateThenTransformAsyncCore(
            value,
            [
                AsyncValidationMapRule<T>.Create(
                    validator1.propertySelector,
                    validator1.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator2.propertySelector,
                    validator2.mapperAsync
                ),
            ],
            mappedValues =>
                transform(
                    GetMappedValue<TMapped1>(mappedValues, 0),
                    GetMappedValue<TMapped2>(mappedValues, 1)
                )
        );
    }

    /// <summary>
    /// Maps 3 selected properties with async mappers and transforms the mapped values when every mapper succeeds.
    /// </summary>
    /// <param name="value">The object to validate and map.</param>
    /// <param name="validator1">The first property selector and mapper.</param>
    /// <param name="validator2">The second property selector and mapper.</param>
    /// <param name="validator3">The third property selector and mapper.</param>
    /// <param name="transform">The final transform to run after every mapper succeeds.</param>
    /// <returns>The transformed success value when every mapper succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, any mapper, or the final transform delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = await person.ValidateThenTransformAsync(validator1, validator2, transform);</code></example>
    public static Task<FailOr<TResult>> ValidateThenTransformAsync<
        T,
        TProp1,
        TMapped1,
        TProp2,
        TMapped2,
        TProp3,
        TMapped3,
        TResult
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, Task<FailOr<TMapped1>>> mapperAsync
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, Task<FailOr<TMapped2>>> mapperAsync
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, Task<FailOr<TMapped3>>> mapperAsync
        ) validator3,
        Func<TMapped1, TMapped2, TMapped3, TResult> transform
    )
    {
        ArgumentNullException.ThrowIfNull(transform);

        return ValidateThenTransformAsyncCore(
            value,
            [
                AsyncValidationMapRule<T>.Create(
                    validator1.propertySelector,
                    validator1.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator2.propertySelector,
                    validator2.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator3.propertySelector,
                    validator3.mapperAsync
                ),
            ],
            mappedValues =>
                transform(
                    GetMappedValue<TMapped1>(mappedValues, 0),
                    GetMappedValue<TMapped2>(mappedValues, 1),
                    GetMappedValue<TMapped3>(mappedValues, 2)
                )
        );
    }

    /// <summary>
    /// Maps 4 selected properties with async mappers and transforms the mapped values when every mapper succeeds.
    /// </summary>
    /// <param name="value">The object to validate and map.</param>
    /// <param name="validator1">The first property selector and mapper.</param>
    /// <param name="validator2">The second property selector and mapper.</param>
    /// <param name="validator3">The third property selector and mapper.</param>
    /// <param name="validator4">The fourth property selector and mapper.</param>
    /// <param name="transform">The final transform to run after every mapper succeeds.</param>
    /// <returns>The transformed success value when every mapper succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, any mapper, or the final transform delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = await person.ValidateThenTransformAsync(validator1, validator2, transform);</code></example>
    public static Task<FailOr<TResult>> ValidateThenTransformAsync<
        T,
        TProp1,
        TMapped1,
        TProp2,
        TMapped2,
        TProp3,
        TMapped3,
        TProp4,
        TMapped4,
        TResult
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, Task<FailOr<TMapped1>>> mapperAsync
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, Task<FailOr<TMapped2>>> mapperAsync
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, Task<FailOr<TMapped3>>> mapperAsync
        ) validator3,
        (
            Expression<Func<T, TProp4>> propertySelector,
            Func<TProp4, Task<FailOr<TMapped4>>> mapperAsync
        ) validator4,
        Func<TMapped1, TMapped2, TMapped3, TMapped4, TResult> transform
    )
    {
        ArgumentNullException.ThrowIfNull(transform);

        return ValidateThenTransformAsyncCore(
            value,
            [
                AsyncValidationMapRule<T>.Create(
                    validator1.propertySelector,
                    validator1.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator2.propertySelector,
                    validator2.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator3.propertySelector,
                    validator3.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator4.propertySelector,
                    validator4.mapperAsync
                ),
            ],
            mappedValues =>
                transform(
                    GetMappedValue<TMapped1>(mappedValues, 0),
                    GetMappedValue<TMapped2>(mappedValues, 1),
                    GetMappedValue<TMapped3>(mappedValues, 2),
                    GetMappedValue<TMapped4>(mappedValues, 3)
                )
        );
    }

    /// <summary>
    /// Maps 5 selected properties with async mappers and transforms the mapped values when every mapper succeeds.
    /// </summary>
    /// <param name="value">The object to validate and map.</param>
    /// <param name="validator1">The first property selector and mapper.</param>
    /// <param name="validator2">The second property selector and mapper.</param>
    /// <param name="validator3">The third property selector and mapper.</param>
    /// <param name="validator4">The fourth property selector and mapper.</param>
    /// <param name="validator5">The fifth property selector and mapper.</param>
    /// <param name="transform">The final transform to run after every mapper succeeds.</param>
    /// <returns>The transformed success value when every mapper succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, any mapper, or the final transform delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = await person.ValidateThenTransformAsync(validator1, validator2, transform);</code></example>
    public static Task<FailOr<TResult>> ValidateThenTransformAsync<
        T,
        TProp1,
        TMapped1,
        TProp2,
        TMapped2,
        TProp3,
        TMapped3,
        TProp4,
        TMapped4,
        TProp5,
        TMapped5,
        TResult
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, Task<FailOr<TMapped1>>> mapperAsync
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, Task<FailOr<TMapped2>>> mapperAsync
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, Task<FailOr<TMapped3>>> mapperAsync
        ) validator3,
        (
            Expression<Func<T, TProp4>> propertySelector,
            Func<TProp4, Task<FailOr<TMapped4>>> mapperAsync
        ) validator4,
        (
            Expression<Func<T, TProp5>> propertySelector,
            Func<TProp5, Task<FailOr<TMapped5>>> mapperAsync
        ) validator5,
        Func<TMapped1, TMapped2, TMapped3, TMapped4, TMapped5, TResult> transform
    )
    {
        ArgumentNullException.ThrowIfNull(transform);

        return ValidateThenTransformAsyncCore(
            value,
            [
                AsyncValidationMapRule<T>.Create(
                    validator1.propertySelector,
                    validator1.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator2.propertySelector,
                    validator2.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator3.propertySelector,
                    validator3.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator4.propertySelector,
                    validator4.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator5.propertySelector,
                    validator5.mapperAsync
                ),
            ],
            mappedValues =>
                transform(
                    GetMappedValue<TMapped1>(mappedValues, 0),
                    GetMappedValue<TMapped2>(mappedValues, 1),
                    GetMappedValue<TMapped3>(mappedValues, 2),
                    GetMappedValue<TMapped4>(mappedValues, 3),
                    GetMappedValue<TMapped5>(mappedValues, 4)
                )
        );
    }

    /// <summary>
    /// Maps 6 selected properties with async mappers and transforms the mapped values when every mapper succeeds.
    /// </summary>
    /// <param name="value">The object to validate and map.</param>
    /// <param name="validator1">The first property selector and mapper.</param>
    /// <param name="validator2">The second property selector and mapper.</param>
    /// <param name="validator3">The third property selector and mapper.</param>
    /// <param name="validator4">The fourth property selector and mapper.</param>
    /// <param name="validator5">The fifth property selector and mapper.</param>
    /// <param name="validator6">The sixth property selector and mapper.</param>
    /// <param name="transform">The final transform to run after every mapper succeeds.</param>
    /// <returns>The transformed success value when every mapper succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, any mapper, or the final transform delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = await person.ValidateThenTransformAsync(validator1, validator2, transform);</code></example>
    public static Task<FailOr<TResult>> ValidateThenTransformAsync<
        T,
        TProp1,
        TMapped1,
        TProp2,
        TMapped2,
        TProp3,
        TMapped3,
        TProp4,
        TMapped4,
        TProp5,
        TMapped5,
        TProp6,
        TMapped6,
        TResult
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, Task<FailOr<TMapped1>>> mapperAsync
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, Task<FailOr<TMapped2>>> mapperAsync
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, Task<FailOr<TMapped3>>> mapperAsync
        ) validator3,
        (
            Expression<Func<T, TProp4>> propertySelector,
            Func<TProp4, Task<FailOr<TMapped4>>> mapperAsync
        ) validator4,
        (
            Expression<Func<T, TProp5>> propertySelector,
            Func<TProp5, Task<FailOr<TMapped5>>> mapperAsync
        ) validator5,
        (
            Expression<Func<T, TProp6>> propertySelector,
            Func<TProp6, Task<FailOr<TMapped6>>> mapperAsync
        ) validator6,
        Func<TMapped1, TMapped2, TMapped3, TMapped4, TMapped5, TMapped6, TResult> transform
    )
    {
        ArgumentNullException.ThrowIfNull(transform);

        return ValidateThenTransformAsyncCore(
            value,
            [
                AsyncValidationMapRule<T>.Create(
                    validator1.propertySelector,
                    validator1.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator2.propertySelector,
                    validator2.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator3.propertySelector,
                    validator3.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator4.propertySelector,
                    validator4.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator5.propertySelector,
                    validator5.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator6.propertySelector,
                    validator6.mapperAsync
                ),
            ],
            mappedValues =>
                transform(
                    GetMappedValue<TMapped1>(mappedValues, 0),
                    GetMappedValue<TMapped2>(mappedValues, 1),
                    GetMappedValue<TMapped3>(mappedValues, 2),
                    GetMappedValue<TMapped4>(mappedValues, 3),
                    GetMappedValue<TMapped5>(mappedValues, 4),
                    GetMappedValue<TMapped6>(mappedValues, 5)
                )
        );
    }

    /// <summary>
    /// Maps 7 selected properties with async mappers and transforms the mapped values when every mapper succeeds.
    /// </summary>
    /// <param name="value">The object to validate and map.</param>
    /// <param name="validator1">The first property selector and mapper.</param>
    /// <param name="validator2">The second property selector and mapper.</param>
    /// <param name="validator3">The third property selector and mapper.</param>
    /// <param name="validator4">The fourth property selector and mapper.</param>
    /// <param name="validator5">The fifth property selector and mapper.</param>
    /// <param name="validator6">The sixth property selector and mapper.</param>
    /// <param name="validator7">The seventh property selector and mapper.</param>
    /// <param name="transform">The final transform to run after every mapper succeeds.</param>
    /// <returns>The transformed success value when every mapper succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, any mapper, or the final transform delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = await person.ValidateThenTransformAsync(validator1, validator2, transform);</code></example>
    public static Task<FailOr<TResult>> ValidateThenTransformAsync<
        T,
        TProp1,
        TMapped1,
        TProp2,
        TMapped2,
        TProp3,
        TMapped3,
        TProp4,
        TMapped4,
        TProp5,
        TMapped5,
        TProp6,
        TMapped6,
        TProp7,
        TMapped7,
        TResult
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, Task<FailOr<TMapped1>>> mapperAsync
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, Task<FailOr<TMapped2>>> mapperAsync
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, Task<FailOr<TMapped3>>> mapperAsync
        ) validator3,
        (
            Expression<Func<T, TProp4>> propertySelector,
            Func<TProp4, Task<FailOr<TMapped4>>> mapperAsync
        ) validator4,
        (
            Expression<Func<T, TProp5>> propertySelector,
            Func<TProp5, Task<FailOr<TMapped5>>> mapperAsync
        ) validator5,
        (
            Expression<Func<T, TProp6>> propertySelector,
            Func<TProp6, Task<FailOr<TMapped6>>> mapperAsync
        ) validator6,
        (
            Expression<Func<T, TProp7>> propertySelector,
            Func<TProp7, Task<FailOr<TMapped7>>> mapperAsync
        ) validator7,
        Func<
            TMapped1,
            TMapped2,
            TMapped3,
            TMapped4,
            TMapped5,
            TMapped6,
            TMapped7,
            TResult
        > transform
    )
    {
        ArgumentNullException.ThrowIfNull(transform);

        return ValidateThenTransformAsyncCore(
            value,
            [
                AsyncValidationMapRule<T>.Create(
                    validator1.propertySelector,
                    validator1.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator2.propertySelector,
                    validator2.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator3.propertySelector,
                    validator3.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator4.propertySelector,
                    validator4.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator5.propertySelector,
                    validator5.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator6.propertySelector,
                    validator6.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator7.propertySelector,
                    validator7.mapperAsync
                ),
            ],
            mappedValues =>
                transform(
                    GetMappedValue<TMapped1>(mappedValues, 0),
                    GetMappedValue<TMapped2>(mappedValues, 1),
                    GetMappedValue<TMapped3>(mappedValues, 2),
                    GetMappedValue<TMapped4>(mappedValues, 3),
                    GetMappedValue<TMapped5>(mappedValues, 4),
                    GetMappedValue<TMapped6>(mappedValues, 5),
                    GetMappedValue<TMapped7>(mappedValues, 6)
                )
        );
    }

    /// <summary>
    /// Maps 8 selected properties with async mappers and transforms the mapped values when every mapper succeeds.
    /// </summary>
    /// <param name="value">The object to validate and map.</param>
    /// <param name="validator1">The first property selector and mapper.</param>
    /// <param name="validator2">The second property selector and mapper.</param>
    /// <param name="validator3">The third property selector and mapper.</param>
    /// <param name="validator4">The fourth property selector and mapper.</param>
    /// <param name="validator5">The fifth property selector and mapper.</param>
    /// <param name="validator6">The sixth property selector and mapper.</param>
    /// <param name="validator7">The seventh property selector and mapper.</param>
    /// <param name="validator8">The eighth property selector and mapper.</param>
    /// <param name="transform">The final transform to run after every mapper succeeds.</param>
    /// <returns>The transformed success value when every mapper succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, any mapper, or the final transform delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = await person.ValidateThenTransformAsync(validator1, validator2, transform);</code></example>
    public static Task<FailOr<TResult>> ValidateThenTransformAsync<
        T,
        TProp1,
        TMapped1,
        TProp2,
        TMapped2,
        TProp3,
        TMapped3,
        TProp4,
        TMapped4,
        TProp5,
        TMapped5,
        TProp6,
        TMapped6,
        TProp7,
        TMapped7,
        TProp8,
        TMapped8,
        TResult
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, Task<FailOr<TMapped1>>> mapperAsync
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, Task<FailOr<TMapped2>>> mapperAsync
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, Task<FailOr<TMapped3>>> mapperAsync
        ) validator3,
        (
            Expression<Func<T, TProp4>> propertySelector,
            Func<TProp4, Task<FailOr<TMapped4>>> mapperAsync
        ) validator4,
        (
            Expression<Func<T, TProp5>> propertySelector,
            Func<TProp5, Task<FailOr<TMapped5>>> mapperAsync
        ) validator5,
        (
            Expression<Func<T, TProp6>> propertySelector,
            Func<TProp6, Task<FailOr<TMapped6>>> mapperAsync
        ) validator6,
        (
            Expression<Func<T, TProp7>> propertySelector,
            Func<TProp7, Task<FailOr<TMapped7>>> mapperAsync
        ) validator7,
        (
            Expression<Func<T, TProp8>> propertySelector,
            Func<TProp8, Task<FailOr<TMapped8>>> mapperAsync
        ) validator8,
        Func<
            TMapped1,
            TMapped2,
            TMapped3,
            TMapped4,
            TMapped5,
            TMapped6,
            TMapped7,
            TMapped8,
            TResult
        > transform
    )
    {
        ArgumentNullException.ThrowIfNull(transform);

        return ValidateThenTransformAsyncCore(
            value,
            [
                AsyncValidationMapRule<T>.Create(
                    validator1.propertySelector,
                    validator1.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator2.propertySelector,
                    validator2.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator3.propertySelector,
                    validator3.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator4.propertySelector,
                    validator4.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator5.propertySelector,
                    validator5.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator6.propertySelector,
                    validator6.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator7.propertySelector,
                    validator7.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator8.propertySelector,
                    validator8.mapperAsync
                ),
            ],
            mappedValues =>
                transform(
                    GetMappedValue<TMapped1>(mappedValues, 0),
                    GetMappedValue<TMapped2>(mappedValues, 1),
                    GetMappedValue<TMapped3>(mappedValues, 2),
                    GetMappedValue<TMapped4>(mappedValues, 3),
                    GetMappedValue<TMapped5>(mappedValues, 4),
                    GetMappedValue<TMapped6>(mappedValues, 5),
                    GetMappedValue<TMapped7>(mappedValues, 6),
                    GetMappedValue<TMapped8>(mappedValues, 7)
                )
        );
    }

    /// <summary>
    /// Maps 9 selected properties with async mappers and transforms the mapped values when every mapper succeeds.
    /// </summary>
    /// <param name="value">The object to validate and map.</param>
    /// <param name="validator1">The first property selector and mapper.</param>
    /// <param name="validator2">The second property selector and mapper.</param>
    /// <param name="validator3">The third property selector and mapper.</param>
    /// <param name="validator4">The fourth property selector and mapper.</param>
    /// <param name="validator5">The fifth property selector and mapper.</param>
    /// <param name="validator6">The sixth property selector and mapper.</param>
    /// <param name="validator7">The seventh property selector and mapper.</param>
    /// <param name="validator8">The eighth property selector and mapper.</param>
    /// <param name="validator9">The ninth property selector and mapper.</param>
    /// <param name="transform">The final transform to run after every mapper succeeds.</param>
    /// <returns>The transformed success value when every mapper succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, any mapper, or the final transform delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = await person.ValidateThenTransformAsync(validator1, validator2, transform);</code></example>
    public static Task<FailOr<TResult>> ValidateThenTransformAsync<
        T,
        TProp1,
        TMapped1,
        TProp2,
        TMapped2,
        TProp3,
        TMapped3,
        TProp4,
        TMapped4,
        TProp5,
        TMapped5,
        TProp6,
        TMapped6,
        TProp7,
        TMapped7,
        TProp8,
        TMapped8,
        TProp9,
        TMapped9,
        TResult
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, Task<FailOr<TMapped1>>> mapperAsync
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, Task<FailOr<TMapped2>>> mapperAsync
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, Task<FailOr<TMapped3>>> mapperAsync
        ) validator3,
        (
            Expression<Func<T, TProp4>> propertySelector,
            Func<TProp4, Task<FailOr<TMapped4>>> mapperAsync
        ) validator4,
        (
            Expression<Func<T, TProp5>> propertySelector,
            Func<TProp5, Task<FailOr<TMapped5>>> mapperAsync
        ) validator5,
        (
            Expression<Func<T, TProp6>> propertySelector,
            Func<TProp6, Task<FailOr<TMapped6>>> mapperAsync
        ) validator6,
        (
            Expression<Func<T, TProp7>> propertySelector,
            Func<TProp7, Task<FailOr<TMapped7>>> mapperAsync
        ) validator7,
        (
            Expression<Func<T, TProp8>> propertySelector,
            Func<TProp8, Task<FailOr<TMapped8>>> mapperAsync
        ) validator8,
        (
            Expression<Func<T, TProp9>> propertySelector,
            Func<TProp9, Task<FailOr<TMapped9>>> mapperAsync
        ) validator9,
        Func<
            TMapped1,
            TMapped2,
            TMapped3,
            TMapped4,
            TMapped5,
            TMapped6,
            TMapped7,
            TMapped8,
            TMapped9,
            TResult
        > transform
    )
    {
        ArgumentNullException.ThrowIfNull(transform);

        return ValidateThenTransformAsyncCore(
            value,
            [
                AsyncValidationMapRule<T>.Create(
                    validator1.propertySelector,
                    validator1.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator2.propertySelector,
                    validator2.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator3.propertySelector,
                    validator3.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator4.propertySelector,
                    validator4.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator5.propertySelector,
                    validator5.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator6.propertySelector,
                    validator6.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator7.propertySelector,
                    validator7.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator8.propertySelector,
                    validator8.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator9.propertySelector,
                    validator9.mapperAsync
                ),
            ],
            mappedValues =>
                transform(
                    GetMappedValue<TMapped1>(mappedValues, 0),
                    GetMappedValue<TMapped2>(mappedValues, 1),
                    GetMappedValue<TMapped3>(mappedValues, 2),
                    GetMappedValue<TMapped4>(mappedValues, 3),
                    GetMappedValue<TMapped5>(mappedValues, 4),
                    GetMappedValue<TMapped6>(mappedValues, 5),
                    GetMappedValue<TMapped7>(mappedValues, 6),
                    GetMappedValue<TMapped8>(mappedValues, 7),
                    GetMappedValue<TMapped9>(mappedValues, 8)
                )
        );
    }

    /// <summary>
    /// Maps 10 selected properties with async mappers and transforms the mapped values when every mapper succeeds.
    /// </summary>
    /// <param name="value">The object to validate and map.</param>
    /// <param name="validator1">The first property selector and mapper.</param>
    /// <param name="validator2">The second property selector and mapper.</param>
    /// <param name="validator3">The third property selector and mapper.</param>
    /// <param name="validator4">The fourth property selector and mapper.</param>
    /// <param name="validator5">The fifth property selector and mapper.</param>
    /// <param name="validator6">The sixth property selector and mapper.</param>
    /// <param name="validator7">The seventh property selector and mapper.</param>
    /// <param name="validator8">The eighth property selector and mapper.</param>
    /// <param name="validator9">The ninth property selector and mapper.</param>
    /// <param name="validator10">The tenth property selector and mapper.</param>
    /// <param name="transform">The final transform to run after every mapper succeeds.</param>
    /// <returns>The transformed success value when every mapper succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, any mapper, or the final transform delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = await person.ValidateThenTransformAsync(validator1, validator2, transform);</code></example>
    public static Task<FailOr<TResult>> ValidateThenTransformAsync<
        T,
        TProp1,
        TMapped1,
        TProp2,
        TMapped2,
        TProp3,
        TMapped3,
        TProp4,
        TMapped4,
        TProp5,
        TMapped5,
        TProp6,
        TMapped6,
        TProp7,
        TMapped7,
        TProp8,
        TMapped8,
        TProp9,
        TMapped9,
        TProp10,
        TMapped10,
        TResult
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, Task<FailOr<TMapped1>>> mapperAsync
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, Task<FailOr<TMapped2>>> mapperAsync
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, Task<FailOr<TMapped3>>> mapperAsync
        ) validator3,
        (
            Expression<Func<T, TProp4>> propertySelector,
            Func<TProp4, Task<FailOr<TMapped4>>> mapperAsync
        ) validator4,
        (
            Expression<Func<T, TProp5>> propertySelector,
            Func<TProp5, Task<FailOr<TMapped5>>> mapperAsync
        ) validator5,
        (
            Expression<Func<T, TProp6>> propertySelector,
            Func<TProp6, Task<FailOr<TMapped6>>> mapperAsync
        ) validator6,
        (
            Expression<Func<T, TProp7>> propertySelector,
            Func<TProp7, Task<FailOr<TMapped7>>> mapperAsync
        ) validator7,
        (
            Expression<Func<T, TProp8>> propertySelector,
            Func<TProp8, Task<FailOr<TMapped8>>> mapperAsync
        ) validator8,
        (
            Expression<Func<T, TProp9>> propertySelector,
            Func<TProp9, Task<FailOr<TMapped9>>> mapperAsync
        ) validator9,
        (
            Expression<Func<T, TProp10>> propertySelector,
            Func<TProp10, Task<FailOr<TMapped10>>> mapperAsync
        ) validator10,
        Func<
            TMapped1,
            TMapped2,
            TMapped3,
            TMapped4,
            TMapped5,
            TMapped6,
            TMapped7,
            TMapped8,
            TMapped9,
            TMapped10,
            TResult
        > transform
    )
    {
        ArgumentNullException.ThrowIfNull(transform);

        return ValidateThenTransformAsyncCore(
            value,
            [
                AsyncValidationMapRule<T>.Create(
                    validator1.propertySelector,
                    validator1.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator2.propertySelector,
                    validator2.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator3.propertySelector,
                    validator3.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator4.propertySelector,
                    validator4.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator5.propertySelector,
                    validator5.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator6.propertySelector,
                    validator6.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator7.propertySelector,
                    validator7.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator8.propertySelector,
                    validator8.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator9.propertySelector,
                    validator9.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator10.propertySelector,
                    validator10.mapperAsync
                ),
            ],
            mappedValues =>
                transform(
                    GetMappedValue<TMapped1>(mappedValues, 0),
                    GetMappedValue<TMapped2>(mappedValues, 1),
                    GetMappedValue<TMapped3>(mappedValues, 2),
                    GetMappedValue<TMapped4>(mappedValues, 3),
                    GetMappedValue<TMapped5>(mappedValues, 4),
                    GetMappedValue<TMapped6>(mappedValues, 5),
                    GetMappedValue<TMapped7>(mappedValues, 6),
                    GetMappedValue<TMapped8>(mappedValues, 7),
                    GetMappedValue<TMapped9>(mappedValues, 8),
                    GetMappedValue<TMapped10>(mappedValues, 9)
                )
        );
    }

    /// <summary>
    /// Maps 11 selected properties with async mappers and transforms the mapped values when every mapper succeeds.
    /// </summary>
    /// <param name="value">The object to validate and map.</param>
    /// <param name="validator1">The first property selector and mapper.</param>
    /// <param name="validator2">The second property selector and mapper.</param>
    /// <param name="validator3">The third property selector and mapper.</param>
    /// <param name="validator4">The fourth property selector and mapper.</param>
    /// <param name="validator5">The fifth property selector and mapper.</param>
    /// <param name="validator6">The sixth property selector and mapper.</param>
    /// <param name="validator7">The seventh property selector and mapper.</param>
    /// <param name="validator8">The eighth property selector and mapper.</param>
    /// <param name="validator9">The ninth property selector and mapper.</param>
    /// <param name="validator10">The tenth property selector and mapper.</param>
    /// <param name="validator11">The eleventh property selector and mapper.</param>
    /// <param name="transform">The final transform to run after every mapper succeeds.</param>
    /// <returns>The transformed success value when every mapper succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, any mapper, or the final transform delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = await person.ValidateThenTransformAsync(validator1, validator2, transform);</code></example>
    public static Task<FailOr<TResult>> ValidateThenTransformAsync<
        T,
        TProp1,
        TMapped1,
        TProp2,
        TMapped2,
        TProp3,
        TMapped3,
        TProp4,
        TMapped4,
        TProp5,
        TMapped5,
        TProp6,
        TMapped6,
        TProp7,
        TMapped7,
        TProp8,
        TMapped8,
        TProp9,
        TMapped9,
        TProp10,
        TMapped10,
        TProp11,
        TMapped11,
        TResult
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, Task<FailOr<TMapped1>>> mapperAsync
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, Task<FailOr<TMapped2>>> mapperAsync
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, Task<FailOr<TMapped3>>> mapperAsync
        ) validator3,
        (
            Expression<Func<T, TProp4>> propertySelector,
            Func<TProp4, Task<FailOr<TMapped4>>> mapperAsync
        ) validator4,
        (
            Expression<Func<T, TProp5>> propertySelector,
            Func<TProp5, Task<FailOr<TMapped5>>> mapperAsync
        ) validator5,
        (
            Expression<Func<T, TProp6>> propertySelector,
            Func<TProp6, Task<FailOr<TMapped6>>> mapperAsync
        ) validator6,
        (
            Expression<Func<T, TProp7>> propertySelector,
            Func<TProp7, Task<FailOr<TMapped7>>> mapperAsync
        ) validator7,
        (
            Expression<Func<T, TProp8>> propertySelector,
            Func<TProp8, Task<FailOr<TMapped8>>> mapperAsync
        ) validator8,
        (
            Expression<Func<T, TProp9>> propertySelector,
            Func<TProp9, Task<FailOr<TMapped9>>> mapperAsync
        ) validator9,
        (
            Expression<Func<T, TProp10>> propertySelector,
            Func<TProp10, Task<FailOr<TMapped10>>> mapperAsync
        ) validator10,
        (
            Expression<Func<T, TProp11>> propertySelector,
            Func<TProp11, Task<FailOr<TMapped11>>> mapperAsync
        ) validator11,
        Func<
            TMapped1,
            TMapped2,
            TMapped3,
            TMapped4,
            TMapped5,
            TMapped6,
            TMapped7,
            TMapped8,
            TMapped9,
            TMapped10,
            TMapped11,
            TResult
        > transform
    )
    {
        ArgumentNullException.ThrowIfNull(transform);

        return ValidateThenTransformAsyncCore(
            value,
            [
                AsyncValidationMapRule<T>.Create(
                    validator1.propertySelector,
                    validator1.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator2.propertySelector,
                    validator2.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator3.propertySelector,
                    validator3.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator4.propertySelector,
                    validator4.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator5.propertySelector,
                    validator5.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator6.propertySelector,
                    validator6.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator7.propertySelector,
                    validator7.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator8.propertySelector,
                    validator8.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator9.propertySelector,
                    validator9.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator10.propertySelector,
                    validator10.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator11.propertySelector,
                    validator11.mapperAsync
                ),
            ],
            mappedValues =>
                transform(
                    GetMappedValue<TMapped1>(mappedValues, 0),
                    GetMappedValue<TMapped2>(mappedValues, 1),
                    GetMappedValue<TMapped3>(mappedValues, 2),
                    GetMappedValue<TMapped4>(mappedValues, 3),
                    GetMappedValue<TMapped5>(mappedValues, 4),
                    GetMappedValue<TMapped6>(mappedValues, 5),
                    GetMappedValue<TMapped7>(mappedValues, 6),
                    GetMappedValue<TMapped8>(mappedValues, 7),
                    GetMappedValue<TMapped9>(mappedValues, 8),
                    GetMappedValue<TMapped10>(mappedValues, 9),
                    GetMappedValue<TMapped11>(mappedValues, 10)
                )
        );
    }

    /// <summary>
    /// Maps 12 selected properties with async mappers and transforms the mapped values when every mapper succeeds.
    /// </summary>
    /// <param name="value">The object to validate and map.</param>
    /// <param name="validator1">The first property selector and mapper.</param>
    /// <param name="validator2">The second property selector and mapper.</param>
    /// <param name="validator3">The third property selector and mapper.</param>
    /// <param name="validator4">The fourth property selector and mapper.</param>
    /// <param name="validator5">The fifth property selector and mapper.</param>
    /// <param name="validator6">The sixth property selector and mapper.</param>
    /// <param name="validator7">The seventh property selector and mapper.</param>
    /// <param name="validator8">The eighth property selector and mapper.</param>
    /// <param name="validator9">The ninth property selector and mapper.</param>
    /// <param name="validator10">The tenth property selector and mapper.</param>
    /// <param name="validator11">The eleventh property selector and mapper.</param>
    /// <param name="validator12">The twelfth property selector and mapper.</param>
    /// <param name="transform">The final transform to run after every mapper succeeds.</param>
    /// <returns>The transformed success value when every mapper succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, any mapper, or the final transform delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = await person.ValidateThenTransformAsync(validator1, validator2, transform);</code></example>
    public static Task<FailOr<TResult>> ValidateThenTransformAsync<
        T,
        TProp1,
        TMapped1,
        TProp2,
        TMapped2,
        TProp3,
        TMapped3,
        TProp4,
        TMapped4,
        TProp5,
        TMapped5,
        TProp6,
        TMapped6,
        TProp7,
        TMapped7,
        TProp8,
        TMapped8,
        TProp9,
        TMapped9,
        TProp10,
        TMapped10,
        TProp11,
        TMapped11,
        TProp12,
        TMapped12,
        TResult
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, Task<FailOr<TMapped1>>> mapperAsync
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, Task<FailOr<TMapped2>>> mapperAsync
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, Task<FailOr<TMapped3>>> mapperAsync
        ) validator3,
        (
            Expression<Func<T, TProp4>> propertySelector,
            Func<TProp4, Task<FailOr<TMapped4>>> mapperAsync
        ) validator4,
        (
            Expression<Func<T, TProp5>> propertySelector,
            Func<TProp5, Task<FailOr<TMapped5>>> mapperAsync
        ) validator5,
        (
            Expression<Func<T, TProp6>> propertySelector,
            Func<TProp6, Task<FailOr<TMapped6>>> mapperAsync
        ) validator6,
        (
            Expression<Func<T, TProp7>> propertySelector,
            Func<TProp7, Task<FailOr<TMapped7>>> mapperAsync
        ) validator7,
        (
            Expression<Func<T, TProp8>> propertySelector,
            Func<TProp8, Task<FailOr<TMapped8>>> mapperAsync
        ) validator8,
        (
            Expression<Func<T, TProp9>> propertySelector,
            Func<TProp9, Task<FailOr<TMapped9>>> mapperAsync
        ) validator9,
        (
            Expression<Func<T, TProp10>> propertySelector,
            Func<TProp10, Task<FailOr<TMapped10>>> mapperAsync
        ) validator10,
        (
            Expression<Func<T, TProp11>> propertySelector,
            Func<TProp11, Task<FailOr<TMapped11>>> mapperAsync
        ) validator11,
        (
            Expression<Func<T, TProp12>> propertySelector,
            Func<TProp12, Task<FailOr<TMapped12>>> mapperAsync
        ) validator12,
        Func<
            TMapped1,
            TMapped2,
            TMapped3,
            TMapped4,
            TMapped5,
            TMapped6,
            TMapped7,
            TMapped8,
            TMapped9,
            TMapped10,
            TMapped11,
            TMapped12,
            TResult
        > transform
    )
    {
        ArgumentNullException.ThrowIfNull(transform);

        return ValidateThenTransformAsyncCore(
            value,
            [
                AsyncValidationMapRule<T>.Create(
                    validator1.propertySelector,
                    validator1.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator2.propertySelector,
                    validator2.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator3.propertySelector,
                    validator3.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator4.propertySelector,
                    validator4.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator5.propertySelector,
                    validator5.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator6.propertySelector,
                    validator6.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator7.propertySelector,
                    validator7.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator8.propertySelector,
                    validator8.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator9.propertySelector,
                    validator9.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator10.propertySelector,
                    validator10.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator11.propertySelector,
                    validator11.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator12.propertySelector,
                    validator12.mapperAsync
                ),
            ],
            mappedValues =>
                transform(
                    GetMappedValue<TMapped1>(mappedValues, 0),
                    GetMappedValue<TMapped2>(mappedValues, 1),
                    GetMappedValue<TMapped3>(mappedValues, 2),
                    GetMappedValue<TMapped4>(mappedValues, 3),
                    GetMappedValue<TMapped5>(mappedValues, 4),
                    GetMappedValue<TMapped6>(mappedValues, 5),
                    GetMappedValue<TMapped7>(mappedValues, 6),
                    GetMappedValue<TMapped8>(mappedValues, 7),
                    GetMappedValue<TMapped9>(mappedValues, 8),
                    GetMappedValue<TMapped10>(mappedValues, 9),
                    GetMappedValue<TMapped11>(mappedValues, 10),
                    GetMappedValue<TMapped12>(mappedValues, 11)
                )
        );
    }

    /// <summary>
    /// Maps 13 selected properties with async mappers and transforms the mapped values when every mapper succeeds.
    /// </summary>
    /// <param name="value">The object to validate and map.</param>
    /// <param name="validator1">The first property selector and mapper.</param>
    /// <param name="validator2">The second property selector and mapper.</param>
    /// <param name="validator3">The third property selector and mapper.</param>
    /// <param name="validator4">The fourth property selector and mapper.</param>
    /// <param name="validator5">The fifth property selector and mapper.</param>
    /// <param name="validator6">The sixth property selector and mapper.</param>
    /// <param name="validator7">The seventh property selector and mapper.</param>
    /// <param name="validator8">The eighth property selector and mapper.</param>
    /// <param name="validator9">The ninth property selector and mapper.</param>
    /// <param name="validator10">The tenth property selector and mapper.</param>
    /// <param name="validator11">The eleventh property selector and mapper.</param>
    /// <param name="validator12">The twelfth property selector and mapper.</param>
    /// <param name="validator13">The thirteenth property selector and mapper.</param>
    /// <param name="transform">The final transform to run after every mapper succeeds.</param>
    /// <returns>The transformed success value when every mapper succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, any mapper, or the final transform delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = await person.ValidateThenTransformAsync(validator1, validator2, transform);</code></example>
    public static Task<FailOr<TResult>> ValidateThenTransformAsync<
        T,
        TProp1,
        TMapped1,
        TProp2,
        TMapped2,
        TProp3,
        TMapped3,
        TProp4,
        TMapped4,
        TProp5,
        TMapped5,
        TProp6,
        TMapped6,
        TProp7,
        TMapped7,
        TProp8,
        TMapped8,
        TProp9,
        TMapped9,
        TProp10,
        TMapped10,
        TProp11,
        TMapped11,
        TProp12,
        TMapped12,
        TProp13,
        TMapped13,
        TResult
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, Task<FailOr<TMapped1>>> mapperAsync
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, Task<FailOr<TMapped2>>> mapperAsync
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, Task<FailOr<TMapped3>>> mapperAsync
        ) validator3,
        (
            Expression<Func<T, TProp4>> propertySelector,
            Func<TProp4, Task<FailOr<TMapped4>>> mapperAsync
        ) validator4,
        (
            Expression<Func<T, TProp5>> propertySelector,
            Func<TProp5, Task<FailOr<TMapped5>>> mapperAsync
        ) validator5,
        (
            Expression<Func<T, TProp6>> propertySelector,
            Func<TProp6, Task<FailOr<TMapped6>>> mapperAsync
        ) validator6,
        (
            Expression<Func<T, TProp7>> propertySelector,
            Func<TProp7, Task<FailOr<TMapped7>>> mapperAsync
        ) validator7,
        (
            Expression<Func<T, TProp8>> propertySelector,
            Func<TProp8, Task<FailOr<TMapped8>>> mapperAsync
        ) validator8,
        (
            Expression<Func<T, TProp9>> propertySelector,
            Func<TProp9, Task<FailOr<TMapped9>>> mapperAsync
        ) validator9,
        (
            Expression<Func<T, TProp10>> propertySelector,
            Func<TProp10, Task<FailOr<TMapped10>>> mapperAsync
        ) validator10,
        (
            Expression<Func<T, TProp11>> propertySelector,
            Func<TProp11, Task<FailOr<TMapped11>>> mapperAsync
        ) validator11,
        (
            Expression<Func<T, TProp12>> propertySelector,
            Func<TProp12, Task<FailOr<TMapped12>>> mapperAsync
        ) validator12,
        (
            Expression<Func<T, TProp13>> propertySelector,
            Func<TProp13, Task<FailOr<TMapped13>>> mapperAsync
        ) validator13,
        Func<
            TMapped1,
            TMapped2,
            TMapped3,
            TMapped4,
            TMapped5,
            TMapped6,
            TMapped7,
            TMapped8,
            TMapped9,
            TMapped10,
            TMapped11,
            TMapped12,
            TMapped13,
            TResult
        > transform
    )
    {
        ArgumentNullException.ThrowIfNull(transform);

        return ValidateThenTransformAsyncCore(
            value,
            [
                AsyncValidationMapRule<T>.Create(
                    validator1.propertySelector,
                    validator1.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator2.propertySelector,
                    validator2.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator3.propertySelector,
                    validator3.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator4.propertySelector,
                    validator4.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator5.propertySelector,
                    validator5.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator6.propertySelector,
                    validator6.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator7.propertySelector,
                    validator7.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator8.propertySelector,
                    validator8.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator9.propertySelector,
                    validator9.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator10.propertySelector,
                    validator10.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator11.propertySelector,
                    validator11.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator12.propertySelector,
                    validator12.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator13.propertySelector,
                    validator13.mapperAsync
                ),
            ],
            mappedValues =>
                transform(
                    GetMappedValue<TMapped1>(mappedValues, 0),
                    GetMappedValue<TMapped2>(mappedValues, 1),
                    GetMappedValue<TMapped3>(mappedValues, 2),
                    GetMappedValue<TMapped4>(mappedValues, 3),
                    GetMappedValue<TMapped5>(mappedValues, 4),
                    GetMappedValue<TMapped6>(mappedValues, 5),
                    GetMappedValue<TMapped7>(mappedValues, 6),
                    GetMappedValue<TMapped8>(mappedValues, 7),
                    GetMappedValue<TMapped9>(mappedValues, 8),
                    GetMappedValue<TMapped10>(mappedValues, 9),
                    GetMappedValue<TMapped11>(mappedValues, 10),
                    GetMappedValue<TMapped12>(mappedValues, 11),
                    GetMappedValue<TMapped13>(mappedValues, 12)
                )
        );
    }

    /// <summary>
    /// Maps 14 selected properties with async mappers and transforms the mapped values when every mapper succeeds.
    /// </summary>
    /// <param name="value">The object to validate and map.</param>
    /// <param name="validator1">The first property selector and mapper.</param>
    /// <param name="validator2">The second property selector and mapper.</param>
    /// <param name="validator3">The third property selector and mapper.</param>
    /// <param name="validator4">The fourth property selector and mapper.</param>
    /// <param name="validator5">The fifth property selector and mapper.</param>
    /// <param name="validator6">The sixth property selector and mapper.</param>
    /// <param name="validator7">The seventh property selector and mapper.</param>
    /// <param name="validator8">The eighth property selector and mapper.</param>
    /// <param name="validator9">The ninth property selector and mapper.</param>
    /// <param name="validator10">The tenth property selector and mapper.</param>
    /// <param name="validator11">The eleventh property selector and mapper.</param>
    /// <param name="validator12">The twelfth property selector and mapper.</param>
    /// <param name="validator13">The thirteenth property selector and mapper.</param>
    /// <param name="validator14">The fourteenth property selector and mapper.</param>
    /// <param name="transform">The final transform to run after every mapper succeeds.</param>
    /// <returns>The transformed success value when every mapper succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, any mapper, or the final transform delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = await person.ValidateThenTransformAsync(validator1, validator2, transform);</code></example>
    public static Task<FailOr<TResult>> ValidateThenTransformAsync<
        T,
        TProp1,
        TMapped1,
        TProp2,
        TMapped2,
        TProp3,
        TMapped3,
        TProp4,
        TMapped4,
        TProp5,
        TMapped5,
        TProp6,
        TMapped6,
        TProp7,
        TMapped7,
        TProp8,
        TMapped8,
        TProp9,
        TMapped9,
        TProp10,
        TMapped10,
        TProp11,
        TMapped11,
        TProp12,
        TMapped12,
        TProp13,
        TMapped13,
        TProp14,
        TMapped14,
        TResult
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, Task<FailOr<TMapped1>>> mapperAsync
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, Task<FailOr<TMapped2>>> mapperAsync
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, Task<FailOr<TMapped3>>> mapperAsync
        ) validator3,
        (
            Expression<Func<T, TProp4>> propertySelector,
            Func<TProp4, Task<FailOr<TMapped4>>> mapperAsync
        ) validator4,
        (
            Expression<Func<T, TProp5>> propertySelector,
            Func<TProp5, Task<FailOr<TMapped5>>> mapperAsync
        ) validator5,
        (
            Expression<Func<T, TProp6>> propertySelector,
            Func<TProp6, Task<FailOr<TMapped6>>> mapperAsync
        ) validator6,
        (
            Expression<Func<T, TProp7>> propertySelector,
            Func<TProp7, Task<FailOr<TMapped7>>> mapperAsync
        ) validator7,
        (
            Expression<Func<T, TProp8>> propertySelector,
            Func<TProp8, Task<FailOr<TMapped8>>> mapperAsync
        ) validator8,
        (
            Expression<Func<T, TProp9>> propertySelector,
            Func<TProp9, Task<FailOr<TMapped9>>> mapperAsync
        ) validator9,
        (
            Expression<Func<T, TProp10>> propertySelector,
            Func<TProp10, Task<FailOr<TMapped10>>> mapperAsync
        ) validator10,
        (
            Expression<Func<T, TProp11>> propertySelector,
            Func<TProp11, Task<FailOr<TMapped11>>> mapperAsync
        ) validator11,
        (
            Expression<Func<T, TProp12>> propertySelector,
            Func<TProp12, Task<FailOr<TMapped12>>> mapperAsync
        ) validator12,
        (
            Expression<Func<T, TProp13>> propertySelector,
            Func<TProp13, Task<FailOr<TMapped13>>> mapperAsync
        ) validator13,
        (
            Expression<Func<T, TProp14>> propertySelector,
            Func<TProp14, Task<FailOr<TMapped14>>> mapperAsync
        ) validator14,
        Func<
            TMapped1,
            TMapped2,
            TMapped3,
            TMapped4,
            TMapped5,
            TMapped6,
            TMapped7,
            TMapped8,
            TMapped9,
            TMapped10,
            TMapped11,
            TMapped12,
            TMapped13,
            TMapped14,
            TResult
        > transform
    )
    {
        ArgumentNullException.ThrowIfNull(transform);

        return ValidateThenTransformAsyncCore(
            value,
            [
                AsyncValidationMapRule<T>.Create(
                    validator1.propertySelector,
                    validator1.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator2.propertySelector,
                    validator2.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator3.propertySelector,
                    validator3.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator4.propertySelector,
                    validator4.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator5.propertySelector,
                    validator5.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator6.propertySelector,
                    validator6.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator7.propertySelector,
                    validator7.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator8.propertySelector,
                    validator8.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator9.propertySelector,
                    validator9.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator10.propertySelector,
                    validator10.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator11.propertySelector,
                    validator11.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator12.propertySelector,
                    validator12.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator13.propertySelector,
                    validator13.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator14.propertySelector,
                    validator14.mapperAsync
                ),
            ],
            mappedValues =>
                transform(
                    GetMappedValue<TMapped1>(mappedValues, 0),
                    GetMappedValue<TMapped2>(mappedValues, 1),
                    GetMappedValue<TMapped3>(mappedValues, 2),
                    GetMappedValue<TMapped4>(mappedValues, 3),
                    GetMappedValue<TMapped5>(mappedValues, 4),
                    GetMappedValue<TMapped6>(mappedValues, 5),
                    GetMappedValue<TMapped7>(mappedValues, 6),
                    GetMappedValue<TMapped8>(mappedValues, 7),
                    GetMappedValue<TMapped9>(mappedValues, 8),
                    GetMappedValue<TMapped10>(mappedValues, 9),
                    GetMappedValue<TMapped11>(mappedValues, 10),
                    GetMappedValue<TMapped12>(mappedValues, 11),
                    GetMappedValue<TMapped13>(mappedValues, 12),
                    GetMappedValue<TMapped14>(mappedValues, 13)
                )
        );
    }

    /// <summary>
    /// Maps 1 selected property with async mappers and asynchronously transforms the mapped values when every mapper succeeds.
    /// </summary>
    /// <param name="value">The object to validate and map.</param>
    /// <param name="validator1">The first property selector and mapper.</param>
    /// <param name="transformAsync">The final transform to run after every mapper succeeds.</param>
    /// <returns>The transformed success value when every mapper succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, any mapper, or the final transform delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = await person.ValidateThenTransformAsync(validator1, transformAsync);</code></example>
    public static Task<FailOr<TResult>> ValidateThenTransformAsync<T, TProp1, TMapped1, TResult>(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, Task<FailOr<TMapped1>>> mapperAsync
        ) validator1,
        Func<TMapped1, Task<TResult>> transformAsync
    )
    {
        ArgumentNullException.ThrowIfNull(transformAsync);

        return ValidateThenTransformAsyncCore(
            value,
            [AsyncValidationMapRule<T>.Create(validator1.propertySelector, validator1.mapperAsync)],
            async mappedValues =>
            {
                var transformTask =
                    transformAsync(GetMappedValue<TMapped1>(mappedValues, 0))
                    ?? throw new ArgumentNullException(nameof(transformAsync));
                return await transformTask.ConfigureAwait(false);
            }
        );
    }

    /// <summary>
    /// Maps 2 selected properties with async mappers and asynchronously transforms the mapped values when every mapper succeeds.
    /// </summary>
    /// <param name="value">The object to validate and map.</param>
    /// <param name="validator1">The first property selector and mapper.</param>
    /// <param name="validator2">The second property selector and mapper.</param>
    /// <param name="transformAsync">The final transform to run after every mapper succeeds.</param>
    /// <returns>The transformed success value when every mapper succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, any mapper, or the final transform delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = await person.ValidateThenTransformAsync(validator1, validator2, transformAsync);</code></example>
    public static Task<FailOr<TResult>> ValidateThenTransformAsync<
        T,
        TProp1,
        TMapped1,
        TProp2,
        TMapped2,
        TResult
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, Task<FailOr<TMapped1>>> mapperAsync
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, Task<FailOr<TMapped2>>> mapperAsync
        ) validator2,
        Func<TMapped1, TMapped2, Task<TResult>> transformAsync
    )
    {
        ArgumentNullException.ThrowIfNull(transformAsync);

        return ValidateThenTransformAsyncCore(
            value,
            [
                AsyncValidationMapRule<T>.Create(
                    validator1.propertySelector,
                    validator1.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator2.propertySelector,
                    validator2.mapperAsync
                ),
            ],
            async mappedValues =>
            {
                var transformTask =
                    transformAsync(
                        GetMappedValue<TMapped1>(mappedValues, 0),
                        GetMappedValue<TMapped2>(mappedValues, 1)
                    ) ?? throw new ArgumentNullException(nameof(transformAsync));
                return await transformTask.ConfigureAwait(false);
            }
        );
    }

    /// <summary>
    /// Maps 3 selected properties with async mappers and asynchronously transforms the mapped values when every mapper succeeds.
    /// </summary>
    /// <param name="value">The object to validate and map.</param>
    /// <param name="validator1">The first property selector and mapper.</param>
    /// <param name="validator2">The second property selector and mapper.</param>
    /// <param name="validator3">The third property selector and mapper.</param>
    /// <param name="transformAsync">The final transform to run after every mapper succeeds.</param>
    /// <returns>The transformed success value when every mapper succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, any mapper, or the final transform delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = await person.ValidateThenTransformAsync(validator1, validator2, transformAsync);</code></example>
    public static Task<FailOr<TResult>> ValidateThenTransformAsync<
        T,
        TProp1,
        TMapped1,
        TProp2,
        TMapped2,
        TProp3,
        TMapped3,
        TResult
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, Task<FailOr<TMapped1>>> mapperAsync
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, Task<FailOr<TMapped2>>> mapperAsync
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, Task<FailOr<TMapped3>>> mapperAsync
        ) validator3,
        Func<TMapped1, TMapped2, TMapped3, Task<TResult>> transformAsync
    )
    {
        ArgumentNullException.ThrowIfNull(transformAsync);

        return ValidateThenTransformAsyncCore(
            value,
            [
                AsyncValidationMapRule<T>.Create(
                    validator1.propertySelector,
                    validator1.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator2.propertySelector,
                    validator2.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator3.propertySelector,
                    validator3.mapperAsync
                ),
            ],
            async mappedValues =>
            {
                var transformTask =
                    transformAsync(
                        GetMappedValue<TMapped1>(mappedValues, 0),
                        GetMappedValue<TMapped2>(mappedValues, 1),
                        GetMappedValue<TMapped3>(mappedValues, 2)
                    ) ?? throw new ArgumentNullException(nameof(transformAsync));
                return await transformTask.ConfigureAwait(false);
            }
        );
    }

    /// <summary>
    /// Maps 4 selected properties with async mappers and asynchronously transforms the mapped values when every mapper succeeds.
    /// </summary>
    /// <param name="value">The object to validate and map.</param>
    /// <param name="validator1">The first property selector and mapper.</param>
    /// <param name="validator2">The second property selector and mapper.</param>
    /// <param name="validator3">The third property selector and mapper.</param>
    /// <param name="validator4">The fourth property selector and mapper.</param>
    /// <param name="transformAsync">The final transform to run after every mapper succeeds.</param>
    /// <returns>The transformed success value when every mapper succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, any mapper, or the final transform delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = await person.ValidateThenTransformAsync(validator1, validator2, transformAsync);</code></example>
    public static Task<FailOr<TResult>> ValidateThenTransformAsync<
        T,
        TProp1,
        TMapped1,
        TProp2,
        TMapped2,
        TProp3,
        TMapped3,
        TProp4,
        TMapped4,
        TResult
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, Task<FailOr<TMapped1>>> mapperAsync
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, Task<FailOr<TMapped2>>> mapperAsync
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, Task<FailOr<TMapped3>>> mapperAsync
        ) validator3,
        (
            Expression<Func<T, TProp4>> propertySelector,
            Func<TProp4, Task<FailOr<TMapped4>>> mapperAsync
        ) validator4,
        Func<TMapped1, TMapped2, TMapped3, TMapped4, Task<TResult>> transformAsync
    )
    {
        ArgumentNullException.ThrowIfNull(transformAsync);

        return ValidateThenTransformAsyncCore(
            value,
            [
                AsyncValidationMapRule<T>.Create(
                    validator1.propertySelector,
                    validator1.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator2.propertySelector,
                    validator2.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator3.propertySelector,
                    validator3.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator4.propertySelector,
                    validator4.mapperAsync
                ),
            ],
            async mappedValues =>
            {
                var transformTask =
                    transformAsync(
                        GetMappedValue<TMapped1>(mappedValues, 0),
                        GetMappedValue<TMapped2>(mappedValues, 1),
                        GetMappedValue<TMapped3>(mappedValues, 2),
                        GetMappedValue<TMapped4>(mappedValues, 3)
                    ) ?? throw new ArgumentNullException(nameof(transformAsync));
                return await transformTask.ConfigureAwait(false);
            }
        );
    }

    /// <summary>
    /// Maps 5 selected properties with async mappers and asynchronously transforms the mapped values when every mapper succeeds.
    /// </summary>
    /// <param name="value">The object to validate and map.</param>
    /// <param name="validator1">The first property selector and mapper.</param>
    /// <param name="validator2">The second property selector and mapper.</param>
    /// <param name="validator3">The third property selector and mapper.</param>
    /// <param name="validator4">The fourth property selector and mapper.</param>
    /// <param name="validator5">The fifth property selector and mapper.</param>
    /// <param name="transformAsync">The final transform to run after every mapper succeeds.</param>
    /// <returns>The transformed success value when every mapper succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, any mapper, or the final transform delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = await person.ValidateThenTransformAsync(validator1, validator2, transformAsync);</code></example>
    public static Task<FailOr<TResult>> ValidateThenTransformAsync<
        T,
        TProp1,
        TMapped1,
        TProp2,
        TMapped2,
        TProp3,
        TMapped3,
        TProp4,
        TMapped4,
        TProp5,
        TMapped5,
        TResult
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, Task<FailOr<TMapped1>>> mapperAsync
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, Task<FailOr<TMapped2>>> mapperAsync
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, Task<FailOr<TMapped3>>> mapperAsync
        ) validator3,
        (
            Expression<Func<T, TProp4>> propertySelector,
            Func<TProp4, Task<FailOr<TMapped4>>> mapperAsync
        ) validator4,
        (
            Expression<Func<T, TProp5>> propertySelector,
            Func<TProp5, Task<FailOr<TMapped5>>> mapperAsync
        ) validator5,
        Func<TMapped1, TMapped2, TMapped3, TMapped4, TMapped5, Task<TResult>> transformAsync
    )
    {
        ArgumentNullException.ThrowIfNull(transformAsync);

        return ValidateThenTransformAsyncCore(
            value,
            [
                AsyncValidationMapRule<T>.Create(
                    validator1.propertySelector,
                    validator1.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator2.propertySelector,
                    validator2.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator3.propertySelector,
                    validator3.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator4.propertySelector,
                    validator4.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator5.propertySelector,
                    validator5.mapperAsync
                ),
            ],
            async mappedValues =>
            {
                var transformTask =
                    transformAsync(
                        GetMappedValue<TMapped1>(mappedValues, 0),
                        GetMappedValue<TMapped2>(mappedValues, 1),
                        GetMappedValue<TMapped3>(mappedValues, 2),
                        GetMappedValue<TMapped4>(mappedValues, 3),
                        GetMappedValue<TMapped5>(mappedValues, 4)
                    ) ?? throw new ArgumentNullException(nameof(transformAsync));
                return await transformTask.ConfigureAwait(false);
            }
        );
    }

    /// <summary>
    /// Maps 6 selected properties with async mappers and asynchronously transforms the mapped values when every mapper succeeds.
    /// </summary>
    /// <param name="value">The object to validate and map.</param>
    /// <param name="validator1">The first property selector and mapper.</param>
    /// <param name="validator2">The second property selector and mapper.</param>
    /// <param name="validator3">The third property selector and mapper.</param>
    /// <param name="validator4">The fourth property selector and mapper.</param>
    /// <param name="validator5">The fifth property selector and mapper.</param>
    /// <param name="validator6">The sixth property selector and mapper.</param>
    /// <param name="transformAsync">The final transform to run after every mapper succeeds.</param>
    /// <returns>The transformed success value when every mapper succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, any mapper, or the final transform delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = await person.ValidateThenTransformAsync(validator1, validator2, transformAsync);</code></example>
    public static Task<FailOr<TResult>> ValidateThenTransformAsync<
        T,
        TProp1,
        TMapped1,
        TProp2,
        TMapped2,
        TProp3,
        TMapped3,
        TProp4,
        TMapped4,
        TProp5,
        TMapped5,
        TProp6,
        TMapped6,
        TResult
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, Task<FailOr<TMapped1>>> mapperAsync
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, Task<FailOr<TMapped2>>> mapperAsync
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, Task<FailOr<TMapped3>>> mapperAsync
        ) validator3,
        (
            Expression<Func<T, TProp4>> propertySelector,
            Func<TProp4, Task<FailOr<TMapped4>>> mapperAsync
        ) validator4,
        (
            Expression<Func<T, TProp5>> propertySelector,
            Func<TProp5, Task<FailOr<TMapped5>>> mapperAsync
        ) validator5,
        (
            Expression<Func<T, TProp6>> propertySelector,
            Func<TProp6, Task<FailOr<TMapped6>>> mapperAsync
        ) validator6,
        Func<
            TMapped1,
            TMapped2,
            TMapped3,
            TMapped4,
            TMapped5,
            TMapped6,
            Task<TResult>
        > transformAsync
    )
    {
        ArgumentNullException.ThrowIfNull(transformAsync);

        return ValidateThenTransformAsyncCore(
            value,
            [
                AsyncValidationMapRule<T>.Create(
                    validator1.propertySelector,
                    validator1.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator2.propertySelector,
                    validator2.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator3.propertySelector,
                    validator3.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator4.propertySelector,
                    validator4.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator5.propertySelector,
                    validator5.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator6.propertySelector,
                    validator6.mapperAsync
                ),
            ],
            async mappedValues =>
            {
                var transformTask =
                    transformAsync(
                        GetMappedValue<TMapped1>(mappedValues, 0),
                        GetMappedValue<TMapped2>(mappedValues, 1),
                        GetMappedValue<TMapped3>(mappedValues, 2),
                        GetMappedValue<TMapped4>(mappedValues, 3),
                        GetMappedValue<TMapped5>(mappedValues, 4),
                        GetMappedValue<TMapped6>(mappedValues, 5)
                    ) ?? throw new ArgumentNullException(nameof(transformAsync));
                return await transformTask.ConfigureAwait(false);
            }
        );
    }

    /// <summary>
    /// Maps 7 selected properties with async mappers and asynchronously transforms the mapped values when every mapper succeeds.
    /// </summary>
    /// <param name="value">The object to validate and map.</param>
    /// <param name="validator1">The first property selector and mapper.</param>
    /// <param name="validator2">The second property selector and mapper.</param>
    /// <param name="validator3">The third property selector and mapper.</param>
    /// <param name="validator4">The fourth property selector and mapper.</param>
    /// <param name="validator5">The fifth property selector and mapper.</param>
    /// <param name="validator6">The sixth property selector and mapper.</param>
    /// <param name="validator7">The seventh property selector and mapper.</param>
    /// <param name="transformAsync">The final transform to run after every mapper succeeds.</param>
    /// <returns>The transformed success value when every mapper succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, any mapper, or the final transform delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = await person.ValidateThenTransformAsync(validator1, validator2, transformAsync);</code></example>
    public static Task<FailOr<TResult>> ValidateThenTransformAsync<
        T,
        TProp1,
        TMapped1,
        TProp2,
        TMapped2,
        TProp3,
        TMapped3,
        TProp4,
        TMapped4,
        TProp5,
        TMapped5,
        TProp6,
        TMapped6,
        TProp7,
        TMapped7,
        TResult
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, Task<FailOr<TMapped1>>> mapperAsync
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, Task<FailOr<TMapped2>>> mapperAsync
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, Task<FailOr<TMapped3>>> mapperAsync
        ) validator3,
        (
            Expression<Func<T, TProp4>> propertySelector,
            Func<TProp4, Task<FailOr<TMapped4>>> mapperAsync
        ) validator4,
        (
            Expression<Func<T, TProp5>> propertySelector,
            Func<TProp5, Task<FailOr<TMapped5>>> mapperAsync
        ) validator5,
        (
            Expression<Func<T, TProp6>> propertySelector,
            Func<TProp6, Task<FailOr<TMapped6>>> mapperAsync
        ) validator6,
        (
            Expression<Func<T, TProp7>> propertySelector,
            Func<TProp7, Task<FailOr<TMapped7>>> mapperAsync
        ) validator7,
        Func<
            TMapped1,
            TMapped2,
            TMapped3,
            TMapped4,
            TMapped5,
            TMapped6,
            TMapped7,
            Task<TResult>
        > transformAsync
    )
    {
        ArgumentNullException.ThrowIfNull(transformAsync);

        return ValidateThenTransformAsyncCore(
            value,
            [
                AsyncValidationMapRule<T>.Create(
                    validator1.propertySelector,
                    validator1.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator2.propertySelector,
                    validator2.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator3.propertySelector,
                    validator3.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator4.propertySelector,
                    validator4.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator5.propertySelector,
                    validator5.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator6.propertySelector,
                    validator6.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator7.propertySelector,
                    validator7.mapperAsync
                ),
            ],
            async mappedValues =>
            {
                var transformTask =
                    transformAsync(
                        GetMappedValue<TMapped1>(mappedValues, 0),
                        GetMappedValue<TMapped2>(mappedValues, 1),
                        GetMappedValue<TMapped3>(mappedValues, 2),
                        GetMappedValue<TMapped4>(mappedValues, 3),
                        GetMappedValue<TMapped5>(mappedValues, 4),
                        GetMappedValue<TMapped6>(mappedValues, 5),
                        GetMappedValue<TMapped7>(mappedValues, 6)
                    ) ?? throw new ArgumentNullException(nameof(transformAsync));
                return await transformTask.ConfigureAwait(false);
            }
        );
    }

    /// <summary>
    /// Maps 8 selected properties with async mappers and asynchronously transforms the mapped values when every mapper succeeds.
    /// </summary>
    /// <param name="value">The object to validate and map.</param>
    /// <param name="validator1">The first property selector and mapper.</param>
    /// <param name="validator2">The second property selector and mapper.</param>
    /// <param name="validator3">The third property selector and mapper.</param>
    /// <param name="validator4">The fourth property selector and mapper.</param>
    /// <param name="validator5">The fifth property selector and mapper.</param>
    /// <param name="validator6">The sixth property selector and mapper.</param>
    /// <param name="validator7">The seventh property selector and mapper.</param>
    /// <param name="validator8">The eighth property selector and mapper.</param>
    /// <param name="transformAsync">The final transform to run after every mapper succeeds.</param>
    /// <returns>The transformed success value when every mapper succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, any mapper, or the final transform delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = await person.ValidateThenTransformAsync(validator1, validator2, transformAsync);</code></example>
    public static Task<FailOr<TResult>> ValidateThenTransformAsync<
        T,
        TProp1,
        TMapped1,
        TProp2,
        TMapped2,
        TProp3,
        TMapped3,
        TProp4,
        TMapped4,
        TProp5,
        TMapped5,
        TProp6,
        TMapped6,
        TProp7,
        TMapped7,
        TProp8,
        TMapped8,
        TResult
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, Task<FailOr<TMapped1>>> mapperAsync
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, Task<FailOr<TMapped2>>> mapperAsync
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, Task<FailOr<TMapped3>>> mapperAsync
        ) validator3,
        (
            Expression<Func<T, TProp4>> propertySelector,
            Func<TProp4, Task<FailOr<TMapped4>>> mapperAsync
        ) validator4,
        (
            Expression<Func<T, TProp5>> propertySelector,
            Func<TProp5, Task<FailOr<TMapped5>>> mapperAsync
        ) validator5,
        (
            Expression<Func<T, TProp6>> propertySelector,
            Func<TProp6, Task<FailOr<TMapped6>>> mapperAsync
        ) validator6,
        (
            Expression<Func<T, TProp7>> propertySelector,
            Func<TProp7, Task<FailOr<TMapped7>>> mapperAsync
        ) validator7,
        (
            Expression<Func<T, TProp8>> propertySelector,
            Func<TProp8, Task<FailOr<TMapped8>>> mapperAsync
        ) validator8,
        Func<
            TMapped1,
            TMapped2,
            TMapped3,
            TMapped4,
            TMapped5,
            TMapped6,
            TMapped7,
            TMapped8,
            Task<TResult>
        > transformAsync
    )
    {
        ArgumentNullException.ThrowIfNull(transformAsync);

        return ValidateThenTransformAsyncCore(
            value,
            [
                AsyncValidationMapRule<T>.Create(
                    validator1.propertySelector,
                    validator1.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator2.propertySelector,
                    validator2.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator3.propertySelector,
                    validator3.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator4.propertySelector,
                    validator4.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator5.propertySelector,
                    validator5.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator6.propertySelector,
                    validator6.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator7.propertySelector,
                    validator7.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator8.propertySelector,
                    validator8.mapperAsync
                ),
            ],
            async mappedValues =>
            {
                var transformTask =
                    transformAsync(
                        GetMappedValue<TMapped1>(mappedValues, 0),
                        GetMappedValue<TMapped2>(mappedValues, 1),
                        GetMappedValue<TMapped3>(mappedValues, 2),
                        GetMappedValue<TMapped4>(mappedValues, 3),
                        GetMappedValue<TMapped5>(mappedValues, 4),
                        GetMappedValue<TMapped6>(mappedValues, 5),
                        GetMappedValue<TMapped7>(mappedValues, 6),
                        GetMappedValue<TMapped8>(mappedValues, 7)
                    ) ?? throw new ArgumentNullException(nameof(transformAsync));
                return await transformTask.ConfigureAwait(false);
            }
        );
    }

    /// <summary>
    /// Maps 9 selected properties with async mappers and asynchronously transforms the mapped values when every mapper succeeds.
    /// </summary>
    /// <param name="value">The object to validate and map.</param>
    /// <param name="validator1">The first property selector and mapper.</param>
    /// <param name="validator2">The second property selector and mapper.</param>
    /// <param name="validator3">The third property selector and mapper.</param>
    /// <param name="validator4">The fourth property selector and mapper.</param>
    /// <param name="validator5">The fifth property selector and mapper.</param>
    /// <param name="validator6">The sixth property selector and mapper.</param>
    /// <param name="validator7">The seventh property selector and mapper.</param>
    /// <param name="validator8">The eighth property selector and mapper.</param>
    /// <param name="validator9">The ninth property selector and mapper.</param>
    /// <param name="transformAsync">The final transform to run after every mapper succeeds.</param>
    /// <returns>The transformed success value when every mapper succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, any mapper, or the final transform delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = await person.ValidateThenTransformAsync(validator1, validator2, transformAsync);</code></example>
    public static Task<FailOr<TResult>> ValidateThenTransformAsync<
        T,
        TProp1,
        TMapped1,
        TProp2,
        TMapped2,
        TProp3,
        TMapped3,
        TProp4,
        TMapped4,
        TProp5,
        TMapped5,
        TProp6,
        TMapped6,
        TProp7,
        TMapped7,
        TProp8,
        TMapped8,
        TProp9,
        TMapped9,
        TResult
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, Task<FailOr<TMapped1>>> mapperAsync
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, Task<FailOr<TMapped2>>> mapperAsync
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, Task<FailOr<TMapped3>>> mapperAsync
        ) validator3,
        (
            Expression<Func<T, TProp4>> propertySelector,
            Func<TProp4, Task<FailOr<TMapped4>>> mapperAsync
        ) validator4,
        (
            Expression<Func<T, TProp5>> propertySelector,
            Func<TProp5, Task<FailOr<TMapped5>>> mapperAsync
        ) validator5,
        (
            Expression<Func<T, TProp6>> propertySelector,
            Func<TProp6, Task<FailOr<TMapped6>>> mapperAsync
        ) validator6,
        (
            Expression<Func<T, TProp7>> propertySelector,
            Func<TProp7, Task<FailOr<TMapped7>>> mapperAsync
        ) validator7,
        (
            Expression<Func<T, TProp8>> propertySelector,
            Func<TProp8, Task<FailOr<TMapped8>>> mapperAsync
        ) validator8,
        (
            Expression<Func<T, TProp9>> propertySelector,
            Func<TProp9, Task<FailOr<TMapped9>>> mapperAsync
        ) validator9,
        Func<
            TMapped1,
            TMapped2,
            TMapped3,
            TMapped4,
            TMapped5,
            TMapped6,
            TMapped7,
            TMapped8,
            TMapped9,
            Task<TResult>
        > transformAsync
    )
    {
        ArgumentNullException.ThrowIfNull(transformAsync);

        return ValidateThenTransformAsyncCore(
            value,
            [
                AsyncValidationMapRule<T>.Create(
                    validator1.propertySelector,
                    validator1.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator2.propertySelector,
                    validator2.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator3.propertySelector,
                    validator3.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator4.propertySelector,
                    validator4.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator5.propertySelector,
                    validator5.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator6.propertySelector,
                    validator6.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator7.propertySelector,
                    validator7.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator8.propertySelector,
                    validator8.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator9.propertySelector,
                    validator9.mapperAsync
                ),
            ],
            async mappedValues =>
            {
                var transformTask =
                    transformAsync(
                        GetMappedValue<TMapped1>(mappedValues, 0),
                        GetMappedValue<TMapped2>(mappedValues, 1),
                        GetMappedValue<TMapped3>(mappedValues, 2),
                        GetMappedValue<TMapped4>(mappedValues, 3),
                        GetMappedValue<TMapped5>(mappedValues, 4),
                        GetMappedValue<TMapped6>(mappedValues, 5),
                        GetMappedValue<TMapped7>(mappedValues, 6),
                        GetMappedValue<TMapped8>(mappedValues, 7),
                        GetMappedValue<TMapped9>(mappedValues, 8)
                    ) ?? throw new ArgumentNullException(nameof(transformAsync));
                return await transformTask.ConfigureAwait(false);
            }
        );
    }

    /// <summary>
    /// Maps 10 selected properties with async mappers and asynchronously transforms the mapped values when every mapper succeeds.
    /// </summary>
    /// <param name="value">The object to validate and map.</param>
    /// <param name="validator1">The first property selector and mapper.</param>
    /// <param name="validator2">The second property selector and mapper.</param>
    /// <param name="validator3">The third property selector and mapper.</param>
    /// <param name="validator4">The fourth property selector and mapper.</param>
    /// <param name="validator5">The fifth property selector and mapper.</param>
    /// <param name="validator6">The sixth property selector and mapper.</param>
    /// <param name="validator7">The seventh property selector and mapper.</param>
    /// <param name="validator8">The eighth property selector and mapper.</param>
    /// <param name="validator9">The ninth property selector and mapper.</param>
    /// <param name="validator10">The tenth property selector and mapper.</param>
    /// <param name="transformAsync">The final transform to run after every mapper succeeds.</param>
    /// <returns>The transformed success value when every mapper succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, any mapper, or the final transform delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = await person.ValidateThenTransformAsync(validator1, validator2, transformAsync);</code></example>
    public static Task<FailOr<TResult>> ValidateThenTransformAsync<
        T,
        TProp1,
        TMapped1,
        TProp2,
        TMapped2,
        TProp3,
        TMapped3,
        TProp4,
        TMapped4,
        TProp5,
        TMapped5,
        TProp6,
        TMapped6,
        TProp7,
        TMapped7,
        TProp8,
        TMapped8,
        TProp9,
        TMapped9,
        TProp10,
        TMapped10,
        TResult
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, Task<FailOr<TMapped1>>> mapperAsync
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, Task<FailOr<TMapped2>>> mapperAsync
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, Task<FailOr<TMapped3>>> mapperAsync
        ) validator3,
        (
            Expression<Func<T, TProp4>> propertySelector,
            Func<TProp4, Task<FailOr<TMapped4>>> mapperAsync
        ) validator4,
        (
            Expression<Func<T, TProp5>> propertySelector,
            Func<TProp5, Task<FailOr<TMapped5>>> mapperAsync
        ) validator5,
        (
            Expression<Func<T, TProp6>> propertySelector,
            Func<TProp6, Task<FailOr<TMapped6>>> mapperAsync
        ) validator6,
        (
            Expression<Func<T, TProp7>> propertySelector,
            Func<TProp7, Task<FailOr<TMapped7>>> mapperAsync
        ) validator7,
        (
            Expression<Func<T, TProp8>> propertySelector,
            Func<TProp8, Task<FailOr<TMapped8>>> mapperAsync
        ) validator8,
        (
            Expression<Func<T, TProp9>> propertySelector,
            Func<TProp9, Task<FailOr<TMapped9>>> mapperAsync
        ) validator9,
        (
            Expression<Func<T, TProp10>> propertySelector,
            Func<TProp10, Task<FailOr<TMapped10>>> mapperAsync
        ) validator10,
        Func<
            TMapped1,
            TMapped2,
            TMapped3,
            TMapped4,
            TMapped5,
            TMapped6,
            TMapped7,
            TMapped8,
            TMapped9,
            TMapped10,
            Task<TResult>
        > transformAsync
    )
    {
        ArgumentNullException.ThrowIfNull(transformAsync);

        return ValidateThenTransformAsyncCore(
            value,
            [
                AsyncValidationMapRule<T>.Create(
                    validator1.propertySelector,
                    validator1.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator2.propertySelector,
                    validator2.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator3.propertySelector,
                    validator3.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator4.propertySelector,
                    validator4.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator5.propertySelector,
                    validator5.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator6.propertySelector,
                    validator6.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator7.propertySelector,
                    validator7.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator8.propertySelector,
                    validator8.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator9.propertySelector,
                    validator9.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator10.propertySelector,
                    validator10.mapperAsync
                ),
            ],
            async mappedValues =>
            {
                var transformTask =
                    transformAsync(
                        GetMappedValue<TMapped1>(mappedValues, 0),
                        GetMappedValue<TMapped2>(mappedValues, 1),
                        GetMappedValue<TMapped3>(mappedValues, 2),
                        GetMappedValue<TMapped4>(mappedValues, 3),
                        GetMappedValue<TMapped5>(mappedValues, 4),
                        GetMappedValue<TMapped6>(mappedValues, 5),
                        GetMappedValue<TMapped7>(mappedValues, 6),
                        GetMappedValue<TMapped8>(mappedValues, 7),
                        GetMappedValue<TMapped9>(mappedValues, 8),
                        GetMappedValue<TMapped10>(mappedValues, 9)
                    ) ?? throw new ArgumentNullException(nameof(transformAsync));
                return await transformTask.ConfigureAwait(false);
            }
        );
    }

    /// <summary>
    /// Maps 11 selected properties with async mappers and asynchronously transforms the mapped values when every mapper succeeds.
    /// </summary>
    /// <param name="value">The object to validate and map.</param>
    /// <param name="validator1">The first property selector and mapper.</param>
    /// <param name="validator2">The second property selector and mapper.</param>
    /// <param name="validator3">The third property selector and mapper.</param>
    /// <param name="validator4">The fourth property selector and mapper.</param>
    /// <param name="validator5">The fifth property selector and mapper.</param>
    /// <param name="validator6">The sixth property selector and mapper.</param>
    /// <param name="validator7">The seventh property selector and mapper.</param>
    /// <param name="validator8">The eighth property selector and mapper.</param>
    /// <param name="validator9">The ninth property selector and mapper.</param>
    /// <param name="validator10">The tenth property selector and mapper.</param>
    /// <param name="validator11">The eleventh property selector and mapper.</param>
    /// <param name="transformAsync">The final transform to run after every mapper succeeds.</param>
    /// <returns>The transformed success value when every mapper succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, any mapper, or the final transform delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = await person.ValidateThenTransformAsync(validator1, validator2, transformAsync);</code></example>
    public static Task<FailOr<TResult>> ValidateThenTransformAsync<
        T,
        TProp1,
        TMapped1,
        TProp2,
        TMapped2,
        TProp3,
        TMapped3,
        TProp4,
        TMapped4,
        TProp5,
        TMapped5,
        TProp6,
        TMapped6,
        TProp7,
        TMapped7,
        TProp8,
        TMapped8,
        TProp9,
        TMapped9,
        TProp10,
        TMapped10,
        TProp11,
        TMapped11,
        TResult
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, Task<FailOr<TMapped1>>> mapperAsync
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, Task<FailOr<TMapped2>>> mapperAsync
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, Task<FailOr<TMapped3>>> mapperAsync
        ) validator3,
        (
            Expression<Func<T, TProp4>> propertySelector,
            Func<TProp4, Task<FailOr<TMapped4>>> mapperAsync
        ) validator4,
        (
            Expression<Func<T, TProp5>> propertySelector,
            Func<TProp5, Task<FailOr<TMapped5>>> mapperAsync
        ) validator5,
        (
            Expression<Func<T, TProp6>> propertySelector,
            Func<TProp6, Task<FailOr<TMapped6>>> mapperAsync
        ) validator6,
        (
            Expression<Func<T, TProp7>> propertySelector,
            Func<TProp7, Task<FailOr<TMapped7>>> mapperAsync
        ) validator7,
        (
            Expression<Func<T, TProp8>> propertySelector,
            Func<TProp8, Task<FailOr<TMapped8>>> mapperAsync
        ) validator8,
        (
            Expression<Func<T, TProp9>> propertySelector,
            Func<TProp9, Task<FailOr<TMapped9>>> mapperAsync
        ) validator9,
        (
            Expression<Func<T, TProp10>> propertySelector,
            Func<TProp10, Task<FailOr<TMapped10>>> mapperAsync
        ) validator10,
        (
            Expression<Func<T, TProp11>> propertySelector,
            Func<TProp11, Task<FailOr<TMapped11>>> mapperAsync
        ) validator11,
        Func<
            TMapped1,
            TMapped2,
            TMapped3,
            TMapped4,
            TMapped5,
            TMapped6,
            TMapped7,
            TMapped8,
            TMapped9,
            TMapped10,
            TMapped11,
            Task<TResult>
        > transformAsync
    )
    {
        ArgumentNullException.ThrowIfNull(transformAsync);

        return ValidateThenTransformAsyncCore(
            value,
            [
                AsyncValidationMapRule<T>.Create(
                    validator1.propertySelector,
                    validator1.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator2.propertySelector,
                    validator2.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator3.propertySelector,
                    validator3.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator4.propertySelector,
                    validator4.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator5.propertySelector,
                    validator5.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator6.propertySelector,
                    validator6.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator7.propertySelector,
                    validator7.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator8.propertySelector,
                    validator8.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator9.propertySelector,
                    validator9.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator10.propertySelector,
                    validator10.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator11.propertySelector,
                    validator11.mapperAsync
                ),
            ],
            async mappedValues =>
            {
                var transformTask =
                    transformAsync(
                        GetMappedValue<TMapped1>(mappedValues, 0),
                        GetMappedValue<TMapped2>(mappedValues, 1),
                        GetMappedValue<TMapped3>(mappedValues, 2),
                        GetMappedValue<TMapped4>(mappedValues, 3),
                        GetMappedValue<TMapped5>(mappedValues, 4),
                        GetMappedValue<TMapped6>(mappedValues, 5),
                        GetMappedValue<TMapped7>(mappedValues, 6),
                        GetMappedValue<TMapped8>(mappedValues, 7),
                        GetMappedValue<TMapped9>(mappedValues, 8),
                        GetMappedValue<TMapped10>(mappedValues, 9),
                        GetMappedValue<TMapped11>(mappedValues, 10)
                    ) ?? throw new ArgumentNullException(nameof(transformAsync));
                return await transformTask.ConfigureAwait(false);
            }
        );
    }

    /// <summary>
    /// Maps 12 selected properties with async mappers and asynchronously transforms the mapped values when every mapper succeeds.
    /// </summary>
    /// <param name="value">The object to validate and map.</param>
    /// <param name="validator1">The first property selector and mapper.</param>
    /// <param name="validator2">The second property selector and mapper.</param>
    /// <param name="validator3">The third property selector and mapper.</param>
    /// <param name="validator4">The fourth property selector and mapper.</param>
    /// <param name="validator5">The fifth property selector and mapper.</param>
    /// <param name="validator6">The sixth property selector and mapper.</param>
    /// <param name="validator7">The seventh property selector and mapper.</param>
    /// <param name="validator8">The eighth property selector and mapper.</param>
    /// <param name="validator9">The ninth property selector and mapper.</param>
    /// <param name="validator10">The tenth property selector and mapper.</param>
    /// <param name="validator11">The eleventh property selector and mapper.</param>
    /// <param name="validator12">The twelfth property selector and mapper.</param>
    /// <param name="transformAsync">The final transform to run after every mapper succeeds.</param>
    /// <returns>The transformed success value when every mapper succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, any mapper, or the final transform delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = await person.ValidateThenTransformAsync(validator1, validator2, transformAsync);</code></example>
    public static Task<FailOr<TResult>> ValidateThenTransformAsync<
        T,
        TProp1,
        TMapped1,
        TProp2,
        TMapped2,
        TProp3,
        TMapped3,
        TProp4,
        TMapped4,
        TProp5,
        TMapped5,
        TProp6,
        TMapped6,
        TProp7,
        TMapped7,
        TProp8,
        TMapped8,
        TProp9,
        TMapped9,
        TProp10,
        TMapped10,
        TProp11,
        TMapped11,
        TProp12,
        TMapped12,
        TResult
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, Task<FailOr<TMapped1>>> mapperAsync
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, Task<FailOr<TMapped2>>> mapperAsync
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, Task<FailOr<TMapped3>>> mapperAsync
        ) validator3,
        (
            Expression<Func<T, TProp4>> propertySelector,
            Func<TProp4, Task<FailOr<TMapped4>>> mapperAsync
        ) validator4,
        (
            Expression<Func<T, TProp5>> propertySelector,
            Func<TProp5, Task<FailOr<TMapped5>>> mapperAsync
        ) validator5,
        (
            Expression<Func<T, TProp6>> propertySelector,
            Func<TProp6, Task<FailOr<TMapped6>>> mapperAsync
        ) validator6,
        (
            Expression<Func<T, TProp7>> propertySelector,
            Func<TProp7, Task<FailOr<TMapped7>>> mapperAsync
        ) validator7,
        (
            Expression<Func<T, TProp8>> propertySelector,
            Func<TProp8, Task<FailOr<TMapped8>>> mapperAsync
        ) validator8,
        (
            Expression<Func<T, TProp9>> propertySelector,
            Func<TProp9, Task<FailOr<TMapped9>>> mapperAsync
        ) validator9,
        (
            Expression<Func<T, TProp10>> propertySelector,
            Func<TProp10, Task<FailOr<TMapped10>>> mapperAsync
        ) validator10,
        (
            Expression<Func<T, TProp11>> propertySelector,
            Func<TProp11, Task<FailOr<TMapped11>>> mapperAsync
        ) validator11,
        (
            Expression<Func<T, TProp12>> propertySelector,
            Func<TProp12, Task<FailOr<TMapped12>>> mapperAsync
        ) validator12,
        Func<
            TMapped1,
            TMapped2,
            TMapped3,
            TMapped4,
            TMapped5,
            TMapped6,
            TMapped7,
            TMapped8,
            TMapped9,
            TMapped10,
            TMapped11,
            TMapped12,
            Task<TResult>
        > transformAsync
    )
    {
        ArgumentNullException.ThrowIfNull(transformAsync);

        return ValidateThenTransformAsyncCore(
            value,
            [
                AsyncValidationMapRule<T>.Create(
                    validator1.propertySelector,
                    validator1.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator2.propertySelector,
                    validator2.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator3.propertySelector,
                    validator3.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator4.propertySelector,
                    validator4.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator5.propertySelector,
                    validator5.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator6.propertySelector,
                    validator6.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator7.propertySelector,
                    validator7.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator8.propertySelector,
                    validator8.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator9.propertySelector,
                    validator9.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator10.propertySelector,
                    validator10.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator11.propertySelector,
                    validator11.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator12.propertySelector,
                    validator12.mapperAsync
                ),
            ],
            async mappedValues =>
            {
                var transformTask =
                    transformAsync(
                        GetMappedValue<TMapped1>(mappedValues, 0),
                        GetMappedValue<TMapped2>(mappedValues, 1),
                        GetMappedValue<TMapped3>(mappedValues, 2),
                        GetMappedValue<TMapped4>(mappedValues, 3),
                        GetMappedValue<TMapped5>(mappedValues, 4),
                        GetMappedValue<TMapped6>(mappedValues, 5),
                        GetMappedValue<TMapped7>(mappedValues, 6),
                        GetMappedValue<TMapped8>(mappedValues, 7),
                        GetMappedValue<TMapped9>(mappedValues, 8),
                        GetMappedValue<TMapped10>(mappedValues, 9),
                        GetMappedValue<TMapped11>(mappedValues, 10),
                        GetMappedValue<TMapped12>(mappedValues, 11)
                    ) ?? throw new ArgumentNullException(nameof(transformAsync));
                return await transformTask.ConfigureAwait(false);
            }
        );
    }

    /// <summary>
    /// Maps 13 selected properties with async mappers and asynchronously transforms the mapped values when every mapper succeeds.
    /// </summary>
    /// <param name="value">The object to validate and map.</param>
    /// <param name="validator1">The first property selector and mapper.</param>
    /// <param name="validator2">The second property selector and mapper.</param>
    /// <param name="validator3">The third property selector and mapper.</param>
    /// <param name="validator4">The fourth property selector and mapper.</param>
    /// <param name="validator5">The fifth property selector and mapper.</param>
    /// <param name="validator6">The sixth property selector and mapper.</param>
    /// <param name="validator7">The seventh property selector and mapper.</param>
    /// <param name="validator8">The eighth property selector and mapper.</param>
    /// <param name="validator9">The ninth property selector and mapper.</param>
    /// <param name="validator10">The tenth property selector and mapper.</param>
    /// <param name="validator11">The eleventh property selector and mapper.</param>
    /// <param name="validator12">The twelfth property selector and mapper.</param>
    /// <param name="validator13">The thirteenth property selector and mapper.</param>
    /// <param name="transformAsync">The final transform to run after every mapper succeeds.</param>
    /// <returns>The transformed success value when every mapper succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, any mapper, or the final transform delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = await person.ValidateThenTransformAsync(validator1, validator2, transformAsync);</code></example>
    public static Task<FailOr<TResult>> ValidateThenTransformAsync<
        T,
        TProp1,
        TMapped1,
        TProp2,
        TMapped2,
        TProp3,
        TMapped3,
        TProp4,
        TMapped4,
        TProp5,
        TMapped5,
        TProp6,
        TMapped6,
        TProp7,
        TMapped7,
        TProp8,
        TMapped8,
        TProp9,
        TMapped9,
        TProp10,
        TMapped10,
        TProp11,
        TMapped11,
        TProp12,
        TMapped12,
        TProp13,
        TMapped13,
        TResult
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, Task<FailOr<TMapped1>>> mapperAsync
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, Task<FailOr<TMapped2>>> mapperAsync
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, Task<FailOr<TMapped3>>> mapperAsync
        ) validator3,
        (
            Expression<Func<T, TProp4>> propertySelector,
            Func<TProp4, Task<FailOr<TMapped4>>> mapperAsync
        ) validator4,
        (
            Expression<Func<T, TProp5>> propertySelector,
            Func<TProp5, Task<FailOr<TMapped5>>> mapperAsync
        ) validator5,
        (
            Expression<Func<T, TProp6>> propertySelector,
            Func<TProp6, Task<FailOr<TMapped6>>> mapperAsync
        ) validator6,
        (
            Expression<Func<T, TProp7>> propertySelector,
            Func<TProp7, Task<FailOr<TMapped7>>> mapperAsync
        ) validator7,
        (
            Expression<Func<T, TProp8>> propertySelector,
            Func<TProp8, Task<FailOr<TMapped8>>> mapperAsync
        ) validator8,
        (
            Expression<Func<T, TProp9>> propertySelector,
            Func<TProp9, Task<FailOr<TMapped9>>> mapperAsync
        ) validator9,
        (
            Expression<Func<T, TProp10>> propertySelector,
            Func<TProp10, Task<FailOr<TMapped10>>> mapperAsync
        ) validator10,
        (
            Expression<Func<T, TProp11>> propertySelector,
            Func<TProp11, Task<FailOr<TMapped11>>> mapperAsync
        ) validator11,
        (
            Expression<Func<T, TProp12>> propertySelector,
            Func<TProp12, Task<FailOr<TMapped12>>> mapperAsync
        ) validator12,
        (
            Expression<Func<T, TProp13>> propertySelector,
            Func<TProp13, Task<FailOr<TMapped13>>> mapperAsync
        ) validator13,
        Func<
            TMapped1,
            TMapped2,
            TMapped3,
            TMapped4,
            TMapped5,
            TMapped6,
            TMapped7,
            TMapped8,
            TMapped9,
            TMapped10,
            TMapped11,
            TMapped12,
            TMapped13,
            Task<TResult>
        > transformAsync
    )
    {
        ArgumentNullException.ThrowIfNull(transformAsync);

        return ValidateThenTransformAsyncCore(
            value,
            [
                AsyncValidationMapRule<T>.Create(
                    validator1.propertySelector,
                    validator1.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator2.propertySelector,
                    validator2.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator3.propertySelector,
                    validator3.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator4.propertySelector,
                    validator4.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator5.propertySelector,
                    validator5.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator6.propertySelector,
                    validator6.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator7.propertySelector,
                    validator7.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator8.propertySelector,
                    validator8.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator9.propertySelector,
                    validator9.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator10.propertySelector,
                    validator10.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator11.propertySelector,
                    validator11.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator12.propertySelector,
                    validator12.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator13.propertySelector,
                    validator13.mapperAsync
                ),
            ],
            async mappedValues =>
            {
                var transformTask =
                    transformAsync(
                        GetMappedValue<TMapped1>(mappedValues, 0),
                        GetMappedValue<TMapped2>(mappedValues, 1),
                        GetMappedValue<TMapped3>(mappedValues, 2),
                        GetMappedValue<TMapped4>(mappedValues, 3),
                        GetMappedValue<TMapped5>(mappedValues, 4),
                        GetMappedValue<TMapped6>(mappedValues, 5),
                        GetMappedValue<TMapped7>(mappedValues, 6),
                        GetMappedValue<TMapped8>(mappedValues, 7),
                        GetMappedValue<TMapped9>(mappedValues, 8),
                        GetMappedValue<TMapped10>(mappedValues, 9),
                        GetMappedValue<TMapped11>(mappedValues, 10),
                        GetMappedValue<TMapped12>(mappedValues, 11),
                        GetMappedValue<TMapped13>(mappedValues, 12)
                    ) ?? throw new ArgumentNullException(nameof(transformAsync));
                return await transformTask.ConfigureAwait(false);
            }
        );
    }

    /// <summary>
    /// Maps 14 selected properties with async mappers and asynchronously transforms the mapped values when every mapper succeeds.
    /// </summary>
    /// <param name="value">The object to validate and map.</param>
    /// <param name="validator1">The first property selector and mapper.</param>
    /// <param name="validator2">The second property selector and mapper.</param>
    /// <param name="validator3">The third property selector and mapper.</param>
    /// <param name="validator4">The fourth property selector and mapper.</param>
    /// <param name="validator5">The fifth property selector and mapper.</param>
    /// <param name="validator6">The sixth property selector and mapper.</param>
    /// <param name="validator7">The seventh property selector and mapper.</param>
    /// <param name="validator8">The eighth property selector and mapper.</param>
    /// <param name="validator9">The ninth property selector and mapper.</param>
    /// <param name="validator10">The tenth property selector and mapper.</param>
    /// <param name="validator11">The eleventh property selector and mapper.</param>
    /// <param name="validator12">The twelfth property selector and mapper.</param>
    /// <param name="validator13">The thirteenth property selector and mapper.</param>
    /// <param name="validator14">The fourteenth property selector and mapper.</param>
    /// <param name="transformAsync">The final transform to run after every mapper succeeds.</param>
    /// <returns>The transformed success value when every mapper succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, any mapper, or the final transform delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = await person.ValidateThenTransformAsync(validator1, validator2, transformAsync);</code></example>
    public static Task<FailOr<TResult>> ValidateThenTransformAsync<
        T,
        TProp1,
        TMapped1,
        TProp2,
        TMapped2,
        TProp3,
        TMapped3,
        TProp4,
        TMapped4,
        TProp5,
        TMapped5,
        TProp6,
        TMapped6,
        TProp7,
        TMapped7,
        TProp8,
        TMapped8,
        TProp9,
        TMapped9,
        TProp10,
        TMapped10,
        TProp11,
        TMapped11,
        TProp12,
        TMapped12,
        TProp13,
        TMapped13,
        TProp14,
        TMapped14,
        TResult
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, Task<FailOr<TMapped1>>> mapperAsync
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, Task<FailOr<TMapped2>>> mapperAsync
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, Task<FailOr<TMapped3>>> mapperAsync
        ) validator3,
        (
            Expression<Func<T, TProp4>> propertySelector,
            Func<TProp4, Task<FailOr<TMapped4>>> mapperAsync
        ) validator4,
        (
            Expression<Func<T, TProp5>> propertySelector,
            Func<TProp5, Task<FailOr<TMapped5>>> mapperAsync
        ) validator5,
        (
            Expression<Func<T, TProp6>> propertySelector,
            Func<TProp6, Task<FailOr<TMapped6>>> mapperAsync
        ) validator6,
        (
            Expression<Func<T, TProp7>> propertySelector,
            Func<TProp7, Task<FailOr<TMapped7>>> mapperAsync
        ) validator7,
        (
            Expression<Func<T, TProp8>> propertySelector,
            Func<TProp8, Task<FailOr<TMapped8>>> mapperAsync
        ) validator8,
        (
            Expression<Func<T, TProp9>> propertySelector,
            Func<TProp9, Task<FailOr<TMapped9>>> mapperAsync
        ) validator9,
        (
            Expression<Func<T, TProp10>> propertySelector,
            Func<TProp10, Task<FailOr<TMapped10>>> mapperAsync
        ) validator10,
        (
            Expression<Func<T, TProp11>> propertySelector,
            Func<TProp11, Task<FailOr<TMapped11>>> mapperAsync
        ) validator11,
        (
            Expression<Func<T, TProp12>> propertySelector,
            Func<TProp12, Task<FailOr<TMapped12>>> mapperAsync
        ) validator12,
        (
            Expression<Func<T, TProp13>> propertySelector,
            Func<TProp13, Task<FailOr<TMapped13>>> mapperAsync
        ) validator13,
        (
            Expression<Func<T, TProp14>> propertySelector,
            Func<TProp14, Task<FailOr<TMapped14>>> mapperAsync
        ) validator14,
        Func<
            TMapped1,
            TMapped2,
            TMapped3,
            TMapped4,
            TMapped5,
            TMapped6,
            TMapped7,
            TMapped8,
            TMapped9,
            TMapped10,
            TMapped11,
            TMapped12,
            TMapped13,
            TMapped14,
            Task<TResult>
        > transformAsync
    )
    {
        ArgumentNullException.ThrowIfNull(transformAsync);

        return ValidateThenTransformAsyncCore(
            value,
            [
                AsyncValidationMapRule<T>.Create(
                    validator1.propertySelector,
                    validator1.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator2.propertySelector,
                    validator2.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator3.propertySelector,
                    validator3.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator4.propertySelector,
                    validator4.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator5.propertySelector,
                    validator5.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator6.propertySelector,
                    validator6.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator7.propertySelector,
                    validator7.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator8.propertySelector,
                    validator8.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator9.propertySelector,
                    validator9.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator10.propertySelector,
                    validator10.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator11.propertySelector,
                    validator11.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator12.propertySelector,
                    validator12.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator13.propertySelector,
                    validator13.mapperAsync
                ),
                AsyncValidationMapRule<T>.Create(
                    validator14.propertySelector,
                    validator14.mapperAsync
                ),
            ],
            async mappedValues =>
            {
                var transformTask =
                    transformAsync(
                        GetMappedValue<TMapped1>(mappedValues, 0),
                        GetMappedValue<TMapped2>(mappedValues, 1),
                        GetMappedValue<TMapped3>(mappedValues, 2),
                        GetMappedValue<TMapped4>(mappedValues, 3),
                        GetMappedValue<TMapped5>(mappedValues, 4),
                        GetMappedValue<TMapped6>(mappedValues, 5),
                        GetMappedValue<TMapped7>(mappedValues, 6),
                        GetMappedValue<TMapped8>(mappedValues, 7),
                        GetMappedValue<TMapped9>(mappedValues, 8),
                        GetMappedValue<TMapped10>(mappedValues, 9),
                        GetMappedValue<TMapped11>(mappedValues, 10),
                        GetMappedValue<TMapped12>(mappedValues, 11),
                        GetMappedValue<TMapped13>(mappedValues, 12),
                        GetMappedValue<TMapped14>(mappedValues, 13)
                    ) ?? throw new ArgumentNullException(nameof(transformAsync));
                return await transformTask.ConfigureAwait(false);
            }
        );
    }

    private static FailOr<TResult> ValidateThenTransformCore<T, TResult>(
        T value,
        ValidationMapRule<T>[] rules,
        Func<object?[], TResult> transform
    )
    {
        ArgumentNullException.ThrowIfNull(transform);

        var mappedResults = ValidationPipeline.ValidateAndMap(value, rules);

        if (mappedResults.IsFailure)
        {
            return FailOr.Fail<TResult>(mappedResults.Failures);
        }

        return FailOr.Success(transform(mappedResults.UnsafeUnwrap()));
    }

    private static Task<FailOr<TResult>> ValidateThenTransformAsyncCore<T, TResult>(
        T value,
        AsyncValidationMapRule<T>[] rules,
        Func<object?[], TResult> transform
    ) =>
        ValidateThenTransformAsyncCore(
            value,
            rules,
            mappedValues => Task.FromResult(transform(mappedValues))
        );

    private static async Task<FailOr<TResult>> ValidateThenTransformAsyncCore<T, TResult>(
        T value,
        AsyncValidationMapRule<T>[] rules,
        Func<object?[], Task<TResult>> transformAsync
    )
    {
        ArgumentNullException.ThrowIfNull(transformAsync);

        var mappedResults = await ValidationPipeline
            .ValidateAndMapAsync(value, rules)
            .ConfigureAwait(false);

        if (mappedResults.IsFailure)
        {
            return FailOr.Fail<TResult>(mappedResults.Failures);
        }

        var transformTask =
            transformAsync(mappedResults.UnsafeUnwrap())
            ?? throw new ArgumentNullException(nameof(transformAsync));

        return FailOr.Success(await transformTask.ConfigureAwait(false));
    }

    private static TItem GetMappedValue<TItem>(IReadOnlyList<object?> mappedValues, int index)
    {
        var value = mappedValues[index];
        return value is null ? default! : (TItem)value;
    }
}

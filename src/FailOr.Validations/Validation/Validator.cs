using System.Linq.Expressions;
using FailOr.Validation.Core;

namespace FailOr.Validation;

/// <summary>
/// Provides extension methods for property-based validation that return the original input value on success.
/// </summary>
/// <remarks>
/// Validation failures are normalized to the selected leaf property name, while non-validation failures are preserved unchanged.
/// </remarks>
public static class Validator
{
    /// <summary>
    /// Validates 1 selected property and returns the original value when every rule succeeds.
    /// </summary>
    /// <param name="value">The object to validate.</param>
    /// <param name="validator1">The first property selector and validation rule.</param>
    /// <returns>The original <paramref name="value"/> when every validator succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, or any validation delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = person.Validate(validator1);</code></example>
    public static FailOr<T> Validate<T, TProp1, TResult1>(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, FailOr<TResult1>> predicate
        ) validator1
    )
    {
        return ValidationPipeline.Validate(
            value,
            [ValidationRule<T>.Create(validator1.propertySelector, validator1.predicate)]
        );
    }

    /// <summary>
    /// Validates 2 selected properties and returns the original value when every rule succeeds.
    /// </summary>
    /// <param name="value">The object to validate.</param>
    /// <param name="validator1">The first property selector and validation rule.</param>
    /// <param name="validator2">The second property selector and validation rule.</param>
    /// <returns>The original <paramref name="value"/> when every validator succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, or any validation delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = person.Validate(validator1, validator2);</code></example>
    public static FailOr<T> Validate<T, TProp1, TResult1, TProp2, TResult2>(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, FailOr<TResult1>> predicate
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, FailOr<TResult2>> predicate
        ) validator2
    )
    {
        return ValidationPipeline.Validate(
            value,
            [
                ValidationRule<T>.Create(validator1.propertySelector, validator1.predicate),
                ValidationRule<T>.Create(validator2.propertySelector, validator2.predicate),
            ]
        );
    }

    /// <summary>
    /// Validates 3 selected properties and returns the original value when every rule succeeds.
    /// </summary>
    /// <param name="value">The object to validate.</param>
    /// <param name="validator1">The first property selector and validation rule.</param>
    /// <param name="validator2">The second property selector and validation rule.</param>
    /// <param name="validator3">The third property selector and validation rule.</param>
    /// <returns>The original <paramref name="value"/> when every validator succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, or any validation delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = person.Validate(validator1, validator2);</code></example>
    public static FailOr<T> Validate<T, TProp1, TResult1, TProp2, TResult2, TProp3, TResult3>(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, FailOr<TResult1>> predicate
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, FailOr<TResult2>> predicate
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, FailOr<TResult3>> predicate
        ) validator3
    )
    {
        return ValidationPipeline.Validate(
            value,
            [
                ValidationRule<T>.Create(validator1.propertySelector, validator1.predicate),
                ValidationRule<T>.Create(validator2.propertySelector, validator2.predicate),
                ValidationRule<T>.Create(validator3.propertySelector, validator3.predicate),
            ]
        );
    }

    /// <summary>
    /// Validates 4 selected properties and returns the original value when every rule succeeds.
    /// </summary>
    /// <param name="value">The object to validate.</param>
    /// <param name="validator1">The first property selector and validation rule.</param>
    /// <param name="validator2">The second property selector and validation rule.</param>
    /// <param name="validator3">The third property selector and validation rule.</param>
    /// <param name="validator4">The fourth property selector and validation rule.</param>
    /// <returns>The original <paramref name="value"/> when every validator succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, or any validation delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = person.Validate(validator1, validator2);</code></example>
    public static FailOr<T> Validate<
        T,
        TProp1,
        TResult1,
        TProp2,
        TResult2,
        TProp3,
        TResult3,
        TProp4,
        TResult4
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, FailOr<TResult1>> predicate
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, FailOr<TResult2>> predicate
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, FailOr<TResult3>> predicate
        ) validator3,
        (
            Expression<Func<T, TProp4>> propertySelector,
            Func<TProp4, FailOr<TResult4>> predicate
        ) validator4
    )
    {
        return ValidationPipeline.Validate(
            value,
            [
                ValidationRule<T>.Create(validator1.propertySelector, validator1.predicate),
                ValidationRule<T>.Create(validator2.propertySelector, validator2.predicate),
                ValidationRule<T>.Create(validator3.propertySelector, validator3.predicate),
                ValidationRule<T>.Create(validator4.propertySelector, validator4.predicate),
            ]
        );
    }

    /// <summary>
    /// Validates 5 selected properties and returns the original value when every rule succeeds.
    /// </summary>
    /// <param name="value">The object to validate.</param>
    /// <param name="validator1">The first property selector and validation rule.</param>
    /// <param name="validator2">The second property selector and validation rule.</param>
    /// <param name="validator3">The third property selector and validation rule.</param>
    /// <param name="validator4">The fourth property selector and validation rule.</param>
    /// <param name="validator5">The fifth property selector and validation rule.</param>
    /// <returns>The original <paramref name="value"/> when every validator succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, or any validation delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = person.Validate(validator1, validator2);</code></example>
    public static FailOr<T> Validate<
        T,
        TProp1,
        TResult1,
        TProp2,
        TResult2,
        TProp3,
        TResult3,
        TProp4,
        TResult4,
        TProp5,
        TResult5
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, FailOr<TResult1>> predicate
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, FailOr<TResult2>> predicate
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, FailOr<TResult3>> predicate
        ) validator3,
        (
            Expression<Func<T, TProp4>> propertySelector,
            Func<TProp4, FailOr<TResult4>> predicate
        ) validator4,
        (
            Expression<Func<T, TProp5>> propertySelector,
            Func<TProp5, FailOr<TResult5>> predicate
        ) validator5
    )
    {
        return ValidationPipeline.Validate(
            value,
            [
                ValidationRule<T>.Create(validator1.propertySelector, validator1.predicate),
                ValidationRule<T>.Create(validator2.propertySelector, validator2.predicate),
                ValidationRule<T>.Create(validator3.propertySelector, validator3.predicate),
                ValidationRule<T>.Create(validator4.propertySelector, validator4.predicate),
                ValidationRule<T>.Create(validator5.propertySelector, validator5.predicate),
            ]
        );
    }

    /// <summary>
    /// Validates 6 selected properties and returns the original value when every rule succeeds.
    /// </summary>
    /// <param name="value">The object to validate.</param>
    /// <param name="validator1">The first property selector and validation rule.</param>
    /// <param name="validator2">The second property selector and validation rule.</param>
    /// <param name="validator3">The third property selector and validation rule.</param>
    /// <param name="validator4">The fourth property selector and validation rule.</param>
    /// <param name="validator5">The fifth property selector and validation rule.</param>
    /// <param name="validator6">The sixth property selector and validation rule.</param>
    /// <returns>The original <paramref name="value"/> when every validator succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, or any validation delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = person.Validate(validator1, validator2);</code></example>
    public static FailOr<T> Validate<
        T,
        TProp1,
        TResult1,
        TProp2,
        TResult2,
        TProp3,
        TResult3,
        TProp4,
        TResult4,
        TProp5,
        TResult5,
        TProp6,
        TResult6
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, FailOr<TResult1>> predicate
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, FailOr<TResult2>> predicate
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, FailOr<TResult3>> predicate
        ) validator3,
        (
            Expression<Func<T, TProp4>> propertySelector,
            Func<TProp4, FailOr<TResult4>> predicate
        ) validator4,
        (
            Expression<Func<T, TProp5>> propertySelector,
            Func<TProp5, FailOr<TResult5>> predicate
        ) validator5,
        (
            Expression<Func<T, TProp6>> propertySelector,
            Func<TProp6, FailOr<TResult6>> predicate
        ) validator6
    )
    {
        return ValidationPipeline.Validate(
            value,
            [
                ValidationRule<T>.Create(validator1.propertySelector, validator1.predicate),
                ValidationRule<T>.Create(validator2.propertySelector, validator2.predicate),
                ValidationRule<T>.Create(validator3.propertySelector, validator3.predicate),
                ValidationRule<T>.Create(validator4.propertySelector, validator4.predicate),
                ValidationRule<T>.Create(validator5.propertySelector, validator5.predicate),
                ValidationRule<T>.Create(validator6.propertySelector, validator6.predicate),
            ]
        );
    }

    /// <summary>
    /// Validates 7 selected properties and returns the original value when every rule succeeds.
    /// </summary>
    /// <param name="value">The object to validate.</param>
    /// <param name="validator1">The first property selector and validation rule.</param>
    /// <param name="validator2">The second property selector and validation rule.</param>
    /// <param name="validator3">The third property selector and validation rule.</param>
    /// <param name="validator4">The fourth property selector and validation rule.</param>
    /// <param name="validator5">The fifth property selector and validation rule.</param>
    /// <param name="validator6">The sixth property selector and validation rule.</param>
    /// <param name="validator7">The seventh property selector and validation rule.</param>
    /// <returns>The original <paramref name="value"/> when every validator succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, or any validation delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = person.Validate(validator1, validator2);</code></example>
    public static FailOr<T> Validate<
        T,
        TProp1,
        TResult1,
        TProp2,
        TResult2,
        TProp3,
        TResult3,
        TProp4,
        TResult4,
        TProp5,
        TResult5,
        TProp6,
        TResult6,
        TProp7,
        TResult7
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, FailOr<TResult1>> predicate
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, FailOr<TResult2>> predicate
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, FailOr<TResult3>> predicate
        ) validator3,
        (
            Expression<Func<T, TProp4>> propertySelector,
            Func<TProp4, FailOr<TResult4>> predicate
        ) validator4,
        (
            Expression<Func<T, TProp5>> propertySelector,
            Func<TProp5, FailOr<TResult5>> predicate
        ) validator5,
        (
            Expression<Func<T, TProp6>> propertySelector,
            Func<TProp6, FailOr<TResult6>> predicate
        ) validator6,
        (
            Expression<Func<T, TProp7>> propertySelector,
            Func<TProp7, FailOr<TResult7>> predicate
        ) validator7
    )
    {
        return ValidationPipeline.Validate(
            value,
            [
                ValidationRule<T>.Create(validator1.propertySelector, validator1.predicate),
                ValidationRule<T>.Create(validator2.propertySelector, validator2.predicate),
                ValidationRule<T>.Create(validator3.propertySelector, validator3.predicate),
                ValidationRule<T>.Create(validator4.propertySelector, validator4.predicate),
                ValidationRule<T>.Create(validator5.propertySelector, validator5.predicate),
                ValidationRule<T>.Create(validator6.propertySelector, validator6.predicate),
                ValidationRule<T>.Create(validator7.propertySelector, validator7.predicate),
            ]
        );
    }

    /// <summary>
    /// Validates 8 selected properties and returns the original value when every rule succeeds.
    /// </summary>
    /// <param name="value">The object to validate.</param>
    /// <param name="validator1">The first property selector and validation rule.</param>
    /// <param name="validator2">The second property selector and validation rule.</param>
    /// <param name="validator3">The third property selector and validation rule.</param>
    /// <param name="validator4">The fourth property selector and validation rule.</param>
    /// <param name="validator5">The fifth property selector and validation rule.</param>
    /// <param name="validator6">The sixth property selector and validation rule.</param>
    /// <param name="validator7">The seventh property selector and validation rule.</param>
    /// <param name="validator8">The eighth property selector and validation rule.</param>
    /// <returns>The original <paramref name="value"/> when every validator succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, or any validation delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = person.Validate(validator1, validator2);</code></example>
    public static FailOr<T> Validate<
        T,
        TProp1,
        TResult1,
        TProp2,
        TResult2,
        TProp3,
        TResult3,
        TProp4,
        TResult4,
        TProp5,
        TResult5,
        TProp6,
        TResult6,
        TProp7,
        TResult7,
        TProp8,
        TResult8
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, FailOr<TResult1>> predicate
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, FailOr<TResult2>> predicate
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, FailOr<TResult3>> predicate
        ) validator3,
        (
            Expression<Func<T, TProp4>> propertySelector,
            Func<TProp4, FailOr<TResult4>> predicate
        ) validator4,
        (
            Expression<Func<T, TProp5>> propertySelector,
            Func<TProp5, FailOr<TResult5>> predicate
        ) validator5,
        (
            Expression<Func<T, TProp6>> propertySelector,
            Func<TProp6, FailOr<TResult6>> predicate
        ) validator6,
        (
            Expression<Func<T, TProp7>> propertySelector,
            Func<TProp7, FailOr<TResult7>> predicate
        ) validator7,
        (
            Expression<Func<T, TProp8>> propertySelector,
            Func<TProp8, FailOr<TResult8>> predicate
        ) validator8
    )
    {
        return ValidationPipeline.Validate(
            value,
            [
                ValidationRule<T>.Create(validator1.propertySelector, validator1.predicate),
                ValidationRule<T>.Create(validator2.propertySelector, validator2.predicate),
                ValidationRule<T>.Create(validator3.propertySelector, validator3.predicate),
                ValidationRule<T>.Create(validator4.propertySelector, validator4.predicate),
                ValidationRule<T>.Create(validator5.propertySelector, validator5.predicate),
                ValidationRule<T>.Create(validator6.propertySelector, validator6.predicate),
                ValidationRule<T>.Create(validator7.propertySelector, validator7.predicate),
                ValidationRule<T>.Create(validator8.propertySelector, validator8.predicate),
            ]
        );
    }

    /// <summary>
    /// Validates 9 selected properties and returns the original value when every rule succeeds.
    /// </summary>
    /// <param name="value">The object to validate.</param>
    /// <param name="validator1">The first property selector and validation rule.</param>
    /// <param name="validator2">The second property selector and validation rule.</param>
    /// <param name="validator3">The third property selector and validation rule.</param>
    /// <param name="validator4">The fourth property selector and validation rule.</param>
    /// <param name="validator5">The fifth property selector and validation rule.</param>
    /// <param name="validator6">The sixth property selector and validation rule.</param>
    /// <param name="validator7">The seventh property selector and validation rule.</param>
    /// <param name="validator8">The eighth property selector and validation rule.</param>
    /// <param name="validator9">The ninth property selector and validation rule.</param>
    /// <returns>The original <paramref name="value"/> when every validator succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, or any validation delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = person.Validate(validator1, validator2);</code></example>
    public static FailOr<T> Validate<
        T,
        TProp1,
        TResult1,
        TProp2,
        TResult2,
        TProp3,
        TResult3,
        TProp4,
        TResult4,
        TProp5,
        TResult5,
        TProp6,
        TResult6,
        TProp7,
        TResult7,
        TProp8,
        TResult8,
        TProp9,
        TResult9
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, FailOr<TResult1>> predicate
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, FailOr<TResult2>> predicate
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, FailOr<TResult3>> predicate
        ) validator3,
        (
            Expression<Func<T, TProp4>> propertySelector,
            Func<TProp4, FailOr<TResult4>> predicate
        ) validator4,
        (
            Expression<Func<T, TProp5>> propertySelector,
            Func<TProp5, FailOr<TResult5>> predicate
        ) validator5,
        (
            Expression<Func<T, TProp6>> propertySelector,
            Func<TProp6, FailOr<TResult6>> predicate
        ) validator6,
        (
            Expression<Func<T, TProp7>> propertySelector,
            Func<TProp7, FailOr<TResult7>> predicate
        ) validator7,
        (
            Expression<Func<T, TProp8>> propertySelector,
            Func<TProp8, FailOr<TResult8>> predicate
        ) validator8,
        (
            Expression<Func<T, TProp9>> propertySelector,
            Func<TProp9, FailOr<TResult9>> predicate
        ) validator9
    )
    {
        return ValidationPipeline.Validate(
            value,
            [
                ValidationRule<T>.Create(validator1.propertySelector, validator1.predicate),
                ValidationRule<T>.Create(validator2.propertySelector, validator2.predicate),
                ValidationRule<T>.Create(validator3.propertySelector, validator3.predicate),
                ValidationRule<T>.Create(validator4.propertySelector, validator4.predicate),
                ValidationRule<T>.Create(validator5.propertySelector, validator5.predicate),
                ValidationRule<T>.Create(validator6.propertySelector, validator6.predicate),
                ValidationRule<T>.Create(validator7.propertySelector, validator7.predicate),
                ValidationRule<T>.Create(validator8.propertySelector, validator8.predicate),
                ValidationRule<T>.Create(validator9.propertySelector, validator9.predicate),
            ]
        );
    }

    /// <summary>
    /// Validates 10 selected properties and returns the original value when every rule succeeds.
    /// </summary>
    /// <param name="value">The object to validate.</param>
    /// <param name="validator1">The first property selector and validation rule.</param>
    /// <param name="validator2">The second property selector and validation rule.</param>
    /// <param name="validator3">The third property selector and validation rule.</param>
    /// <param name="validator4">The fourth property selector and validation rule.</param>
    /// <param name="validator5">The fifth property selector and validation rule.</param>
    /// <param name="validator6">The sixth property selector and validation rule.</param>
    /// <param name="validator7">The seventh property selector and validation rule.</param>
    /// <param name="validator8">The eighth property selector and validation rule.</param>
    /// <param name="validator9">The ninth property selector and validation rule.</param>
    /// <param name="validator10">The tenth property selector and validation rule.</param>
    /// <returns>The original <paramref name="value"/> when every validator succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, or any validation delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = person.Validate(validator1, validator2);</code></example>
    public static FailOr<T> Validate<
        T,
        TProp1,
        TResult1,
        TProp2,
        TResult2,
        TProp3,
        TResult3,
        TProp4,
        TResult4,
        TProp5,
        TResult5,
        TProp6,
        TResult6,
        TProp7,
        TResult7,
        TProp8,
        TResult8,
        TProp9,
        TResult9,
        TProp10,
        TResult10
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, FailOr<TResult1>> predicate
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, FailOr<TResult2>> predicate
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, FailOr<TResult3>> predicate
        ) validator3,
        (
            Expression<Func<T, TProp4>> propertySelector,
            Func<TProp4, FailOr<TResult4>> predicate
        ) validator4,
        (
            Expression<Func<T, TProp5>> propertySelector,
            Func<TProp5, FailOr<TResult5>> predicate
        ) validator5,
        (
            Expression<Func<T, TProp6>> propertySelector,
            Func<TProp6, FailOr<TResult6>> predicate
        ) validator6,
        (
            Expression<Func<T, TProp7>> propertySelector,
            Func<TProp7, FailOr<TResult7>> predicate
        ) validator7,
        (
            Expression<Func<T, TProp8>> propertySelector,
            Func<TProp8, FailOr<TResult8>> predicate
        ) validator8,
        (
            Expression<Func<T, TProp9>> propertySelector,
            Func<TProp9, FailOr<TResult9>> predicate
        ) validator9,
        (
            Expression<Func<T, TProp10>> propertySelector,
            Func<TProp10, FailOr<TResult10>> predicate
        ) validator10
    )
    {
        return ValidationPipeline.Validate(
            value,
            [
                ValidationRule<T>.Create(validator1.propertySelector, validator1.predicate),
                ValidationRule<T>.Create(validator2.propertySelector, validator2.predicate),
                ValidationRule<T>.Create(validator3.propertySelector, validator3.predicate),
                ValidationRule<T>.Create(validator4.propertySelector, validator4.predicate),
                ValidationRule<T>.Create(validator5.propertySelector, validator5.predicate),
                ValidationRule<T>.Create(validator6.propertySelector, validator6.predicate),
                ValidationRule<T>.Create(validator7.propertySelector, validator7.predicate),
                ValidationRule<T>.Create(validator8.propertySelector, validator8.predicate),
                ValidationRule<T>.Create(validator9.propertySelector, validator9.predicate),
                ValidationRule<T>.Create(validator10.propertySelector, validator10.predicate),
            ]
        );
    }

    /// <summary>
    /// Validates 11 selected properties and returns the original value when every rule succeeds.
    /// </summary>
    /// <param name="value">The object to validate.</param>
    /// <param name="validator1">The first property selector and validation rule.</param>
    /// <param name="validator2">The second property selector and validation rule.</param>
    /// <param name="validator3">The third property selector and validation rule.</param>
    /// <param name="validator4">The fourth property selector and validation rule.</param>
    /// <param name="validator5">The fifth property selector and validation rule.</param>
    /// <param name="validator6">The sixth property selector and validation rule.</param>
    /// <param name="validator7">The seventh property selector and validation rule.</param>
    /// <param name="validator8">The eighth property selector and validation rule.</param>
    /// <param name="validator9">The ninth property selector and validation rule.</param>
    /// <param name="validator10">The tenth property selector and validation rule.</param>
    /// <param name="validator11">The eleventh property selector and validation rule.</param>
    /// <returns>The original <paramref name="value"/> when every validator succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, or any validation delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = person.Validate(validator1, validator2);</code></example>
    public static FailOr<T> Validate<
        T,
        TProp1,
        TResult1,
        TProp2,
        TResult2,
        TProp3,
        TResult3,
        TProp4,
        TResult4,
        TProp5,
        TResult5,
        TProp6,
        TResult6,
        TProp7,
        TResult7,
        TProp8,
        TResult8,
        TProp9,
        TResult9,
        TProp10,
        TResult10,
        TProp11,
        TResult11
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, FailOr<TResult1>> predicate
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, FailOr<TResult2>> predicate
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, FailOr<TResult3>> predicate
        ) validator3,
        (
            Expression<Func<T, TProp4>> propertySelector,
            Func<TProp4, FailOr<TResult4>> predicate
        ) validator4,
        (
            Expression<Func<T, TProp5>> propertySelector,
            Func<TProp5, FailOr<TResult5>> predicate
        ) validator5,
        (
            Expression<Func<T, TProp6>> propertySelector,
            Func<TProp6, FailOr<TResult6>> predicate
        ) validator6,
        (
            Expression<Func<T, TProp7>> propertySelector,
            Func<TProp7, FailOr<TResult7>> predicate
        ) validator7,
        (
            Expression<Func<T, TProp8>> propertySelector,
            Func<TProp8, FailOr<TResult8>> predicate
        ) validator8,
        (
            Expression<Func<T, TProp9>> propertySelector,
            Func<TProp9, FailOr<TResult9>> predicate
        ) validator9,
        (
            Expression<Func<T, TProp10>> propertySelector,
            Func<TProp10, FailOr<TResult10>> predicate
        ) validator10,
        (
            Expression<Func<T, TProp11>> propertySelector,
            Func<TProp11, FailOr<TResult11>> predicate
        ) validator11
    )
    {
        return ValidationPipeline.Validate(
            value,
            [
                ValidationRule<T>.Create(validator1.propertySelector, validator1.predicate),
                ValidationRule<T>.Create(validator2.propertySelector, validator2.predicate),
                ValidationRule<T>.Create(validator3.propertySelector, validator3.predicate),
                ValidationRule<T>.Create(validator4.propertySelector, validator4.predicate),
                ValidationRule<T>.Create(validator5.propertySelector, validator5.predicate),
                ValidationRule<T>.Create(validator6.propertySelector, validator6.predicate),
                ValidationRule<T>.Create(validator7.propertySelector, validator7.predicate),
                ValidationRule<T>.Create(validator8.propertySelector, validator8.predicate),
                ValidationRule<T>.Create(validator9.propertySelector, validator9.predicate),
                ValidationRule<T>.Create(validator10.propertySelector, validator10.predicate),
                ValidationRule<T>.Create(validator11.propertySelector, validator11.predicate),
            ]
        );
    }

    /// <summary>
    /// Validates 12 selected properties and returns the original value when every rule succeeds.
    /// </summary>
    /// <param name="value">The object to validate.</param>
    /// <param name="validator1">The first property selector and validation rule.</param>
    /// <param name="validator2">The second property selector and validation rule.</param>
    /// <param name="validator3">The third property selector and validation rule.</param>
    /// <param name="validator4">The fourth property selector and validation rule.</param>
    /// <param name="validator5">The fifth property selector and validation rule.</param>
    /// <param name="validator6">The sixth property selector and validation rule.</param>
    /// <param name="validator7">The seventh property selector and validation rule.</param>
    /// <param name="validator8">The eighth property selector and validation rule.</param>
    /// <param name="validator9">The ninth property selector and validation rule.</param>
    /// <param name="validator10">The tenth property selector and validation rule.</param>
    /// <param name="validator11">The eleventh property selector and validation rule.</param>
    /// <param name="validator12">The twelfth property selector and validation rule.</param>
    /// <returns>The original <paramref name="value"/> when every validator succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, or any validation delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = person.Validate(validator1, validator2);</code></example>
    public static FailOr<T> Validate<
        T,
        TProp1,
        TResult1,
        TProp2,
        TResult2,
        TProp3,
        TResult3,
        TProp4,
        TResult4,
        TProp5,
        TResult5,
        TProp6,
        TResult6,
        TProp7,
        TResult7,
        TProp8,
        TResult8,
        TProp9,
        TResult9,
        TProp10,
        TResult10,
        TProp11,
        TResult11,
        TProp12,
        TResult12
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, FailOr<TResult1>> predicate
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, FailOr<TResult2>> predicate
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, FailOr<TResult3>> predicate
        ) validator3,
        (
            Expression<Func<T, TProp4>> propertySelector,
            Func<TProp4, FailOr<TResult4>> predicate
        ) validator4,
        (
            Expression<Func<T, TProp5>> propertySelector,
            Func<TProp5, FailOr<TResult5>> predicate
        ) validator5,
        (
            Expression<Func<T, TProp6>> propertySelector,
            Func<TProp6, FailOr<TResult6>> predicate
        ) validator6,
        (
            Expression<Func<T, TProp7>> propertySelector,
            Func<TProp7, FailOr<TResult7>> predicate
        ) validator7,
        (
            Expression<Func<T, TProp8>> propertySelector,
            Func<TProp8, FailOr<TResult8>> predicate
        ) validator8,
        (
            Expression<Func<T, TProp9>> propertySelector,
            Func<TProp9, FailOr<TResult9>> predicate
        ) validator9,
        (
            Expression<Func<T, TProp10>> propertySelector,
            Func<TProp10, FailOr<TResult10>> predicate
        ) validator10,
        (
            Expression<Func<T, TProp11>> propertySelector,
            Func<TProp11, FailOr<TResult11>> predicate
        ) validator11,
        (
            Expression<Func<T, TProp12>> propertySelector,
            Func<TProp12, FailOr<TResult12>> predicate
        ) validator12
    )
    {
        return ValidationPipeline.Validate(
            value,
            [
                ValidationRule<T>.Create(validator1.propertySelector, validator1.predicate),
                ValidationRule<T>.Create(validator2.propertySelector, validator2.predicate),
                ValidationRule<T>.Create(validator3.propertySelector, validator3.predicate),
                ValidationRule<T>.Create(validator4.propertySelector, validator4.predicate),
                ValidationRule<T>.Create(validator5.propertySelector, validator5.predicate),
                ValidationRule<T>.Create(validator6.propertySelector, validator6.predicate),
                ValidationRule<T>.Create(validator7.propertySelector, validator7.predicate),
                ValidationRule<T>.Create(validator8.propertySelector, validator8.predicate),
                ValidationRule<T>.Create(validator9.propertySelector, validator9.predicate),
                ValidationRule<T>.Create(validator10.propertySelector, validator10.predicate),
                ValidationRule<T>.Create(validator11.propertySelector, validator11.predicate),
                ValidationRule<T>.Create(validator12.propertySelector, validator12.predicate),
            ]
        );
    }

    /// <summary>
    /// Validates 13 selected properties and returns the original value when every rule succeeds.
    /// </summary>
    /// <param name="value">The object to validate.</param>
    /// <param name="validator1">The first property selector and validation rule.</param>
    /// <param name="validator2">The second property selector and validation rule.</param>
    /// <param name="validator3">The third property selector and validation rule.</param>
    /// <param name="validator4">The fourth property selector and validation rule.</param>
    /// <param name="validator5">The fifth property selector and validation rule.</param>
    /// <param name="validator6">The sixth property selector and validation rule.</param>
    /// <param name="validator7">The seventh property selector and validation rule.</param>
    /// <param name="validator8">The eighth property selector and validation rule.</param>
    /// <param name="validator9">The ninth property selector and validation rule.</param>
    /// <param name="validator10">The tenth property selector and validation rule.</param>
    /// <param name="validator11">The eleventh property selector and validation rule.</param>
    /// <param name="validator12">The twelfth property selector and validation rule.</param>
    /// <param name="validator13">The thirteenth property selector and validation rule.</param>
    /// <returns>The original <paramref name="value"/> when every validator succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, or any validation delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = person.Validate(validator1, validator2);</code></example>
    public static FailOr<T> Validate<
        T,
        TProp1,
        TResult1,
        TProp2,
        TResult2,
        TProp3,
        TResult3,
        TProp4,
        TResult4,
        TProp5,
        TResult5,
        TProp6,
        TResult6,
        TProp7,
        TResult7,
        TProp8,
        TResult8,
        TProp9,
        TResult9,
        TProp10,
        TResult10,
        TProp11,
        TResult11,
        TProp12,
        TResult12,
        TProp13,
        TResult13
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, FailOr<TResult1>> predicate
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, FailOr<TResult2>> predicate
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, FailOr<TResult3>> predicate
        ) validator3,
        (
            Expression<Func<T, TProp4>> propertySelector,
            Func<TProp4, FailOr<TResult4>> predicate
        ) validator4,
        (
            Expression<Func<T, TProp5>> propertySelector,
            Func<TProp5, FailOr<TResult5>> predicate
        ) validator5,
        (
            Expression<Func<T, TProp6>> propertySelector,
            Func<TProp6, FailOr<TResult6>> predicate
        ) validator6,
        (
            Expression<Func<T, TProp7>> propertySelector,
            Func<TProp7, FailOr<TResult7>> predicate
        ) validator7,
        (
            Expression<Func<T, TProp8>> propertySelector,
            Func<TProp8, FailOr<TResult8>> predicate
        ) validator8,
        (
            Expression<Func<T, TProp9>> propertySelector,
            Func<TProp9, FailOr<TResult9>> predicate
        ) validator9,
        (
            Expression<Func<T, TProp10>> propertySelector,
            Func<TProp10, FailOr<TResult10>> predicate
        ) validator10,
        (
            Expression<Func<T, TProp11>> propertySelector,
            Func<TProp11, FailOr<TResult11>> predicate
        ) validator11,
        (
            Expression<Func<T, TProp12>> propertySelector,
            Func<TProp12, FailOr<TResult12>> predicate
        ) validator12,
        (
            Expression<Func<T, TProp13>> propertySelector,
            Func<TProp13, FailOr<TResult13>> predicate
        ) validator13
    )
    {
        return ValidationPipeline.Validate(
            value,
            [
                ValidationRule<T>.Create(validator1.propertySelector, validator1.predicate),
                ValidationRule<T>.Create(validator2.propertySelector, validator2.predicate),
                ValidationRule<T>.Create(validator3.propertySelector, validator3.predicate),
                ValidationRule<T>.Create(validator4.propertySelector, validator4.predicate),
                ValidationRule<T>.Create(validator5.propertySelector, validator5.predicate),
                ValidationRule<T>.Create(validator6.propertySelector, validator6.predicate),
                ValidationRule<T>.Create(validator7.propertySelector, validator7.predicate),
                ValidationRule<T>.Create(validator8.propertySelector, validator8.predicate),
                ValidationRule<T>.Create(validator9.propertySelector, validator9.predicate),
                ValidationRule<T>.Create(validator10.propertySelector, validator10.predicate),
                ValidationRule<T>.Create(validator11.propertySelector, validator11.predicate),
                ValidationRule<T>.Create(validator12.propertySelector, validator12.predicate),
                ValidationRule<T>.Create(validator13.propertySelector, validator13.predicate),
            ]
        );
    }

    /// <summary>
    /// Validates 14 selected properties and returns the original value when every rule succeeds.
    /// </summary>
    /// <param name="value">The object to validate.</param>
    /// <param name="validator1">The first property selector and validation rule.</param>
    /// <param name="validator2">The second property selector and validation rule.</param>
    /// <param name="validator3">The third property selector and validation rule.</param>
    /// <param name="validator4">The fourth property selector and validation rule.</param>
    /// <param name="validator5">The fifth property selector and validation rule.</param>
    /// <param name="validator6">The sixth property selector and validation rule.</param>
    /// <param name="validator7">The seventh property selector and validation rule.</param>
    /// <param name="validator8">The eighth property selector and validation rule.</param>
    /// <param name="validator9">The ninth property selector and validation rule.</param>
    /// <param name="validator10">The tenth property selector and validation rule.</param>
    /// <param name="validator11">The eleventh property selector and validation rule.</param>
    /// <param name="validator12">The twelfth property selector and validation rule.</param>
    /// <param name="validator13">The thirteenth property selector and validation rule.</param>
    /// <param name="validator14">The fourteenth property selector and validation rule.</param>
    /// <returns>The original <paramref name="value"/> when every validator succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, or any validation delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = person.Validate(validator1, validator2);</code></example>
    public static FailOr<T> Validate<
        T,
        TProp1,
        TResult1,
        TProp2,
        TResult2,
        TProp3,
        TResult3,
        TProp4,
        TResult4,
        TProp5,
        TResult5,
        TProp6,
        TResult6,
        TProp7,
        TResult7,
        TProp8,
        TResult8,
        TProp9,
        TResult9,
        TProp10,
        TResult10,
        TProp11,
        TResult11,
        TProp12,
        TResult12,
        TProp13,
        TResult13,
        TProp14,
        TResult14
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, FailOr<TResult1>> predicate
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, FailOr<TResult2>> predicate
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, FailOr<TResult3>> predicate
        ) validator3,
        (
            Expression<Func<T, TProp4>> propertySelector,
            Func<TProp4, FailOr<TResult4>> predicate
        ) validator4,
        (
            Expression<Func<T, TProp5>> propertySelector,
            Func<TProp5, FailOr<TResult5>> predicate
        ) validator5,
        (
            Expression<Func<T, TProp6>> propertySelector,
            Func<TProp6, FailOr<TResult6>> predicate
        ) validator6,
        (
            Expression<Func<T, TProp7>> propertySelector,
            Func<TProp7, FailOr<TResult7>> predicate
        ) validator7,
        (
            Expression<Func<T, TProp8>> propertySelector,
            Func<TProp8, FailOr<TResult8>> predicate
        ) validator8,
        (
            Expression<Func<T, TProp9>> propertySelector,
            Func<TProp9, FailOr<TResult9>> predicate
        ) validator9,
        (
            Expression<Func<T, TProp10>> propertySelector,
            Func<TProp10, FailOr<TResult10>> predicate
        ) validator10,
        (
            Expression<Func<T, TProp11>> propertySelector,
            Func<TProp11, FailOr<TResult11>> predicate
        ) validator11,
        (
            Expression<Func<T, TProp12>> propertySelector,
            Func<TProp12, FailOr<TResult12>> predicate
        ) validator12,
        (
            Expression<Func<T, TProp13>> propertySelector,
            Func<TProp13, FailOr<TResult13>> predicate
        ) validator13,
        (
            Expression<Func<T, TProp14>> propertySelector,
            Func<TProp14, FailOr<TResult14>> predicate
        ) validator14
    )
    {
        return ValidationPipeline.Validate(
            value,
            [
                ValidationRule<T>.Create(validator1.propertySelector, validator1.predicate),
                ValidationRule<T>.Create(validator2.propertySelector, validator2.predicate),
                ValidationRule<T>.Create(validator3.propertySelector, validator3.predicate),
                ValidationRule<T>.Create(validator4.propertySelector, validator4.predicate),
                ValidationRule<T>.Create(validator5.propertySelector, validator5.predicate),
                ValidationRule<T>.Create(validator6.propertySelector, validator6.predicate),
                ValidationRule<T>.Create(validator7.propertySelector, validator7.predicate),
                ValidationRule<T>.Create(validator8.propertySelector, validator8.predicate),
                ValidationRule<T>.Create(validator9.propertySelector, validator9.predicate),
                ValidationRule<T>.Create(validator10.propertySelector, validator10.predicate),
                ValidationRule<T>.Create(validator11.propertySelector, validator11.predicate),
                ValidationRule<T>.Create(validator12.propertySelector, validator12.predicate),
                ValidationRule<T>.Create(validator13.propertySelector, validator13.predicate),
                ValidationRule<T>.Create(validator14.propertySelector, validator14.predicate),
            ]
        );
    }

    /// <summary>
    /// Validates 1 selected property and returns the original value when every rule succeeds.
    /// </summary>
    /// <param name="value">The object to validate.</param>
    /// <param name="validator1">The first property selector and validation rule.</param>
    /// <returns>The original <paramref name="value"/> when every validator succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, or any validation delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = await person.ValidateAsync(validator1);</code></example>
    public static Task<FailOr<T>> ValidateAsync<T, TProp1, TResult1>(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, Task<FailOr<TResult1>>> predicateAsync
        ) validator1
    )
    {
        return ValidationPipeline.ValidateAsync(
            value,
            [AsyncValidationRule<T>.Create(validator1.propertySelector, validator1.predicateAsync)]
        );
    }

    /// <summary>
    /// Validates 2 selected properties and returns the original value when every rule succeeds.
    /// </summary>
    /// <param name="value">The object to validate.</param>
    /// <param name="validator1">The first property selector and validation rule.</param>
    /// <param name="validator2">The second property selector and validation rule.</param>
    /// <returns>The original <paramref name="value"/> when every validator succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, or any validation delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = await person.ValidateAsync(validator1, validator2);</code></example>
    public static Task<FailOr<T>> ValidateAsync<T, TProp1, TResult1, TProp2, TResult2>(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, Task<FailOr<TResult1>>> predicateAsync
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, Task<FailOr<TResult2>>> predicateAsync
        ) validator2
    )
    {
        return ValidationPipeline.ValidateAsync(
            value,
            [
                AsyncValidationRule<T>.Create(
                    validator1.propertySelector,
                    validator1.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator2.propertySelector,
                    validator2.predicateAsync
                ),
            ]
        );
    }

    /// <summary>
    /// Validates 3 selected properties and returns the original value when every rule succeeds.
    /// </summary>
    /// <param name="value">The object to validate.</param>
    /// <param name="validator1">The first property selector and validation rule.</param>
    /// <param name="validator2">The second property selector and validation rule.</param>
    /// <param name="validator3">The third property selector and validation rule.</param>
    /// <returns>The original <paramref name="value"/> when every validator succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, or any validation delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = await person.ValidateAsync(validator1, validator2);</code></example>
    public static Task<FailOr<T>> ValidateAsync<
        T,
        TProp1,
        TResult1,
        TProp2,
        TResult2,
        TProp3,
        TResult3
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, Task<FailOr<TResult1>>> predicateAsync
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, Task<FailOr<TResult2>>> predicateAsync
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, Task<FailOr<TResult3>>> predicateAsync
        ) validator3
    )
    {
        return ValidationPipeline.ValidateAsync(
            value,
            [
                AsyncValidationRule<T>.Create(
                    validator1.propertySelector,
                    validator1.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator2.propertySelector,
                    validator2.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator3.propertySelector,
                    validator3.predicateAsync
                ),
            ]
        );
    }

    /// <summary>
    /// Validates 4 selected properties and returns the original value when every rule succeeds.
    /// </summary>
    /// <param name="value">The object to validate.</param>
    /// <param name="validator1">The first property selector and validation rule.</param>
    /// <param name="validator2">The second property selector and validation rule.</param>
    /// <param name="validator3">The third property selector and validation rule.</param>
    /// <param name="validator4">The fourth property selector and validation rule.</param>
    /// <returns>The original <paramref name="value"/> when every validator succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, or any validation delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = await person.ValidateAsync(validator1, validator2);</code></example>
    public static Task<FailOr<T>> ValidateAsync<
        T,
        TProp1,
        TResult1,
        TProp2,
        TResult2,
        TProp3,
        TResult3,
        TProp4,
        TResult4
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, Task<FailOr<TResult1>>> predicateAsync
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, Task<FailOr<TResult2>>> predicateAsync
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, Task<FailOr<TResult3>>> predicateAsync
        ) validator3,
        (
            Expression<Func<T, TProp4>> propertySelector,
            Func<TProp4, Task<FailOr<TResult4>>> predicateAsync
        ) validator4
    )
    {
        return ValidationPipeline.ValidateAsync(
            value,
            [
                AsyncValidationRule<T>.Create(
                    validator1.propertySelector,
                    validator1.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator2.propertySelector,
                    validator2.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator3.propertySelector,
                    validator3.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator4.propertySelector,
                    validator4.predicateAsync
                ),
            ]
        );
    }

    /// <summary>
    /// Validates 5 selected properties and returns the original value when every rule succeeds.
    /// </summary>
    /// <param name="value">The object to validate.</param>
    /// <param name="validator1">The first property selector and validation rule.</param>
    /// <param name="validator2">The second property selector and validation rule.</param>
    /// <param name="validator3">The third property selector and validation rule.</param>
    /// <param name="validator4">The fourth property selector and validation rule.</param>
    /// <param name="validator5">The fifth property selector and validation rule.</param>
    /// <returns>The original <paramref name="value"/> when every validator succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, or any validation delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = await person.ValidateAsync(validator1, validator2);</code></example>
    public static Task<FailOr<T>> ValidateAsync<
        T,
        TProp1,
        TResult1,
        TProp2,
        TResult2,
        TProp3,
        TResult3,
        TProp4,
        TResult4,
        TProp5,
        TResult5
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, Task<FailOr<TResult1>>> predicateAsync
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, Task<FailOr<TResult2>>> predicateAsync
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, Task<FailOr<TResult3>>> predicateAsync
        ) validator3,
        (
            Expression<Func<T, TProp4>> propertySelector,
            Func<TProp4, Task<FailOr<TResult4>>> predicateAsync
        ) validator4,
        (
            Expression<Func<T, TProp5>> propertySelector,
            Func<TProp5, Task<FailOr<TResult5>>> predicateAsync
        ) validator5
    )
    {
        return ValidationPipeline.ValidateAsync(
            value,
            [
                AsyncValidationRule<T>.Create(
                    validator1.propertySelector,
                    validator1.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator2.propertySelector,
                    validator2.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator3.propertySelector,
                    validator3.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator4.propertySelector,
                    validator4.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator5.propertySelector,
                    validator5.predicateAsync
                ),
            ]
        );
    }

    /// <summary>
    /// Validates 6 selected properties and returns the original value when every rule succeeds.
    /// </summary>
    /// <param name="value">The object to validate.</param>
    /// <param name="validator1">The first property selector and validation rule.</param>
    /// <param name="validator2">The second property selector and validation rule.</param>
    /// <param name="validator3">The third property selector and validation rule.</param>
    /// <param name="validator4">The fourth property selector and validation rule.</param>
    /// <param name="validator5">The fifth property selector and validation rule.</param>
    /// <param name="validator6">The sixth property selector and validation rule.</param>
    /// <returns>The original <paramref name="value"/> when every validator succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, or any validation delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = await person.ValidateAsync(validator1, validator2);</code></example>
    public static Task<FailOr<T>> ValidateAsync<
        T,
        TProp1,
        TResult1,
        TProp2,
        TResult2,
        TProp3,
        TResult3,
        TProp4,
        TResult4,
        TProp5,
        TResult5,
        TProp6,
        TResult6
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, Task<FailOr<TResult1>>> predicateAsync
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, Task<FailOr<TResult2>>> predicateAsync
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, Task<FailOr<TResult3>>> predicateAsync
        ) validator3,
        (
            Expression<Func<T, TProp4>> propertySelector,
            Func<TProp4, Task<FailOr<TResult4>>> predicateAsync
        ) validator4,
        (
            Expression<Func<T, TProp5>> propertySelector,
            Func<TProp5, Task<FailOr<TResult5>>> predicateAsync
        ) validator5,
        (
            Expression<Func<T, TProp6>> propertySelector,
            Func<TProp6, Task<FailOr<TResult6>>> predicateAsync
        ) validator6
    )
    {
        return ValidationPipeline.ValidateAsync(
            value,
            [
                AsyncValidationRule<T>.Create(
                    validator1.propertySelector,
                    validator1.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator2.propertySelector,
                    validator2.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator3.propertySelector,
                    validator3.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator4.propertySelector,
                    validator4.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator5.propertySelector,
                    validator5.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator6.propertySelector,
                    validator6.predicateAsync
                ),
            ]
        );
    }

    /// <summary>
    /// Validates 7 selected properties and returns the original value when every rule succeeds.
    /// </summary>
    /// <param name="value">The object to validate.</param>
    /// <param name="validator1">The first property selector and validation rule.</param>
    /// <param name="validator2">The second property selector and validation rule.</param>
    /// <param name="validator3">The third property selector and validation rule.</param>
    /// <param name="validator4">The fourth property selector and validation rule.</param>
    /// <param name="validator5">The fifth property selector and validation rule.</param>
    /// <param name="validator6">The sixth property selector and validation rule.</param>
    /// <param name="validator7">The seventh property selector and validation rule.</param>
    /// <returns>The original <paramref name="value"/> when every validator succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, or any validation delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = await person.ValidateAsync(validator1, validator2);</code></example>
    public static Task<FailOr<T>> ValidateAsync<
        T,
        TProp1,
        TResult1,
        TProp2,
        TResult2,
        TProp3,
        TResult3,
        TProp4,
        TResult4,
        TProp5,
        TResult5,
        TProp6,
        TResult6,
        TProp7,
        TResult7
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, Task<FailOr<TResult1>>> predicateAsync
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, Task<FailOr<TResult2>>> predicateAsync
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, Task<FailOr<TResult3>>> predicateAsync
        ) validator3,
        (
            Expression<Func<T, TProp4>> propertySelector,
            Func<TProp4, Task<FailOr<TResult4>>> predicateAsync
        ) validator4,
        (
            Expression<Func<T, TProp5>> propertySelector,
            Func<TProp5, Task<FailOr<TResult5>>> predicateAsync
        ) validator5,
        (
            Expression<Func<T, TProp6>> propertySelector,
            Func<TProp6, Task<FailOr<TResult6>>> predicateAsync
        ) validator6,
        (
            Expression<Func<T, TProp7>> propertySelector,
            Func<TProp7, Task<FailOr<TResult7>>> predicateAsync
        ) validator7
    )
    {
        return ValidationPipeline.ValidateAsync(
            value,
            [
                AsyncValidationRule<T>.Create(
                    validator1.propertySelector,
                    validator1.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator2.propertySelector,
                    validator2.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator3.propertySelector,
                    validator3.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator4.propertySelector,
                    validator4.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator5.propertySelector,
                    validator5.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator6.propertySelector,
                    validator6.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator7.propertySelector,
                    validator7.predicateAsync
                ),
            ]
        );
    }

    /// <summary>
    /// Validates 8 selected properties and returns the original value when every rule succeeds.
    /// </summary>
    /// <param name="value">The object to validate.</param>
    /// <param name="validator1">The first property selector and validation rule.</param>
    /// <param name="validator2">The second property selector and validation rule.</param>
    /// <param name="validator3">The third property selector and validation rule.</param>
    /// <param name="validator4">The fourth property selector and validation rule.</param>
    /// <param name="validator5">The fifth property selector and validation rule.</param>
    /// <param name="validator6">The sixth property selector and validation rule.</param>
    /// <param name="validator7">The seventh property selector and validation rule.</param>
    /// <param name="validator8">The eighth property selector and validation rule.</param>
    /// <returns>The original <paramref name="value"/> when every validator succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, or any validation delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = await person.ValidateAsync(validator1, validator2);</code></example>
    public static Task<FailOr<T>> ValidateAsync<
        T,
        TProp1,
        TResult1,
        TProp2,
        TResult2,
        TProp3,
        TResult3,
        TProp4,
        TResult4,
        TProp5,
        TResult5,
        TProp6,
        TResult6,
        TProp7,
        TResult7,
        TProp8,
        TResult8
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, Task<FailOr<TResult1>>> predicateAsync
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, Task<FailOr<TResult2>>> predicateAsync
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, Task<FailOr<TResult3>>> predicateAsync
        ) validator3,
        (
            Expression<Func<T, TProp4>> propertySelector,
            Func<TProp4, Task<FailOr<TResult4>>> predicateAsync
        ) validator4,
        (
            Expression<Func<T, TProp5>> propertySelector,
            Func<TProp5, Task<FailOr<TResult5>>> predicateAsync
        ) validator5,
        (
            Expression<Func<T, TProp6>> propertySelector,
            Func<TProp6, Task<FailOr<TResult6>>> predicateAsync
        ) validator6,
        (
            Expression<Func<T, TProp7>> propertySelector,
            Func<TProp7, Task<FailOr<TResult7>>> predicateAsync
        ) validator7,
        (
            Expression<Func<T, TProp8>> propertySelector,
            Func<TProp8, Task<FailOr<TResult8>>> predicateAsync
        ) validator8
    )
    {
        return ValidationPipeline.ValidateAsync(
            value,
            [
                AsyncValidationRule<T>.Create(
                    validator1.propertySelector,
                    validator1.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator2.propertySelector,
                    validator2.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator3.propertySelector,
                    validator3.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator4.propertySelector,
                    validator4.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator5.propertySelector,
                    validator5.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator6.propertySelector,
                    validator6.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator7.propertySelector,
                    validator7.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator8.propertySelector,
                    validator8.predicateAsync
                ),
            ]
        );
    }

    /// <summary>
    /// Validates 9 selected properties and returns the original value when every rule succeeds.
    /// </summary>
    /// <param name="value">The object to validate.</param>
    /// <param name="validator1">The first property selector and validation rule.</param>
    /// <param name="validator2">The second property selector and validation rule.</param>
    /// <param name="validator3">The third property selector and validation rule.</param>
    /// <param name="validator4">The fourth property selector and validation rule.</param>
    /// <param name="validator5">The fifth property selector and validation rule.</param>
    /// <param name="validator6">The sixth property selector and validation rule.</param>
    /// <param name="validator7">The seventh property selector and validation rule.</param>
    /// <param name="validator8">The eighth property selector and validation rule.</param>
    /// <param name="validator9">The ninth property selector and validation rule.</param>
    /// <returns>The original <paramref name="value"/> when every validator succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, or any validation delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = await person.ValidateAsync(validator1, validator2);</code></example>
    public static Task<FailOr<T>> ValidateAsync<
        T,
        TProp1,
        TResult1,
        TProp2,
        TResult2,
        TProp3,
        TResult3,
        TProp4,
        TResult4,
        TProp5,
        TResult5,
        TProp6,
        TResult6,
        TProp7,
        TResult7,
        TProp8,
        TResult8,
        TProp9,
        TResult9
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, Task<FailOr<TResult1>>> predicateAsync
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, Task<FailOr<TResult2>>> predicateAsync
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, Task<FailOr<TResult3>>> predicateAsync
        ) validator3,
        (
            Expression<Func<T, TProp4>> propertySelector,
            Func<TProp4, Task<FailOr<TResult4>>> predicateAsync
        ) validator4,
        (
            Expression<Func<T, TProp5>> propertySelector,
            Func<TProp5, Task<FailOr<TResult5>>> predicateAsync
        ) validator5,
        (
            Expression<Func<T, TProp6>> propertySelector,
            Func<TProp6, Task<FailOr<TResult6>>> predicateAsync
        ) validator6,
        (
            Expression<Func<T, TProp7>> propertySelector,
            Func<TProp7, Task<FailOr<TResult7>>> predicateAsync
        ) validator7,
        (
            Expression<Func<T, TProp8>> propertySelector,
            Func<TProp8, Task<FailOr<TResult8>>> predicateAsync
        ) validator8,
        (
            Expression<Func<T, TProp9>> propertySelector,
            Func<TProp9, Task<FailOr<TResult9>>> predicateAsync
        ) validator9
    )
    {
        return ValidationPipeline.ValidateAsync(
            value,
            [
                AsyncValidationRule<T>.Create(
                    validator1.propertySelector,
                    validator1.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator2.propertySelector,
                    validator2.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator3.propertySelector,
                    validator3.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator4.propertySelector,
                    validator4.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator5.propertySelector,
                    validator5.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator6.propertySelector,
                    validator6.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator7.propertySelector,
                    validator7.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator8.propertySelector,
                    validator8.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator9.propertySelector,
                    validator9.predicateAsync
                ),
            ]
        );
    }

    /// <summary>
    /// Validates 10 selected properties and returns the original value when every rule succeeds.
    /// </summary>
    /// <param name="value">The object to validate.</param>
    /// <param name="validator1">The first property selector and validation rule.</param>
    /// <param name="validator2">The second property selector and validation rule.</param>
    /// <param name="validator3">The third property selector and validation rule.</param>
    /// <param name="validator4">The fourth property selector and validation rule.</param>
    /// <param name="validator5">The fifth property selector and validation rule.</param>
    /// <param name="validator6">The sixth property selector and validation rule.</param>
    /// <param name="validator7">The seventh property selector and validation rule.</param>
    /// <param name="validator8">The eighth property selector and validation rule.</param>
    /// <param name="validator9">The ninth property selector and validation rule.</param>
    /// <param name="validator10">The tenth property selector and validation rule.</param>
    /// <returns>The original <paramref name="value"/> when every validator succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, or any validation delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = await person.ValidateAsync(validator1, validator2);</code></example>
    public static Task<FailOr<T>> ValidateAsync<
        T,
        TProp1,
        TResult1,
        TProp2,
        TResult2,
        TProp3,
        TResult3,
        TProp4,
        TResult4,
        TProp5,
        TResult5,
        TProp6,
        TResult6,
        TProp7,
        TResult7,
        TProp8,
        TResult8,
        TProp9,
        TResult9,
        TProp10,
        TResult10
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, Task<FailOr<TResult1>>> predicateAsync
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, Task<FailOr<TResult2>>> predicateAsync
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, Task<FailOr<TResult3>>> predicateAsync
        ) validator3,
        (
            Expression<Func<T, TProp4>> propertySelector,
            Func<TProp4, Task<FailOr<TResult4>>> predicateAsync
        ) validator4,
        (
            Expression<Func<T, TProp5>> propertySelector,
            Func<TProp5, Task<FailOr<TResult5>>> predicateAsync
        ) validator5,
        (
            Expression<Func<T, TProp6>> propertySelector,
            Func<TProp6, Task<FailOr<TResult6>>> predicateAsync
        ) validator6,
        (
            Expression<Func<T, TProp7>> propertySelector,
            Func<TProp7, Task<FailOr<TResult7>>> predicateAsync
        ) validator7,
        (
            Expression<Func<T, TProp8>> propertySelector,
            Func<TProp8, Task<FailOr<TResult8>>> predicateAsync
        ) validator8,
        (
            Expression<Func<T, TProp9>> propertySelector,
            Func<TProp9, Task<FailOr<TResult9>>> predicateAsync
        ) validator9,
        (
            Expression<Func<T, TProp10>> propertySelector,
            Func<TProp10, Task<FailOr<TResult10>>> predicateAsync
        ) validator10
    )
    {
        return ValidationPipeline.ValidateAsync(
            value,
            [
                AsyncValidationRule<T>.Create(
                    validator1.propertySelector,
                    validator1.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator2.propertySelector,
                    validator2.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator3.propertySelector,
                    validator3.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator4.propertySelector,
                    validator4.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator5.propertySelector,
                    validator5.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator6.propertySelector,
                    validator6.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator7.propertySelector,
                    validator7.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator8.propertySelector,
                    validator8.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator9.propertySelector,
                    validator9.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator10.propertySelector,
                    validator10.predicateAsync
                ),
            ]
        );
    }

    /// <summary>
    /// Validates 11 selected properties and returns the original value when every rule succeeds.
    /// </summary>
    /// <param name="value">The object to validate.</param>
    /// <param name="validator1">The first property selector and validation rule.</param>
    /// <param name="validator2">The second property selector and validation rule.</param>
    /// <param name="validator3">The third property selector and validation rule.</param>
    /// <param name="validator4">The fourth property selector and validation rule.</param>
    /// <param name="validator5">The fifth property selector and validation rule.</param>
    /// <param name="validator6">The sixth property selector and validation rule.</param>
    /// <param name="validator7">The seventh property selector and validation rule.</param>
    /// <param name="validator8">The eighth property selector and validation rule.</param>
    /// <param name="validator9">The ninth property selector and validation rule.</param>
    /// <param name="validator10">The tenth property selector and validation rule.</param>
    /// <param name="validator11">The eleventh property selector and validation rule.</param>
    /// <returns>The original <paramref name="value"/> when every validator succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, or any validation delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = await person.ValidateAsync(validator1, validator2);</code></example>
    public static Task<FailOr<T>> ValidateAsync<
        T,
        TProp1,
        TResult1,
        TProp2,
        TResult2,
        TProp3,
        TResult3,
        TProp4,
        TResult4,
        TProp5,
        TResult5,
        TProp6,
        TResult6,
        TProp7,
        TResult7,
        TProp8,
        TResult8,
        TProp9,
        TResult9,
        TProp10,
        TResult10,
        TProp11,
        TResult11
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, Task<FailOr<TResult1>>> predicateAsync
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, Task<FailOr<TResult2>>> predicateAsync
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, Task<FailOr<TResult3>>> predicateAsync
        ) validator3,
        (
            Expression<Func<T, TProp4>> propertySelector,
            Func<TProp4, Task<FailOr<TResult4>>> predicateAsync
        ) validator4,
        (
            Expression<Func<T, TProp5>> propertySelector,
            Func<TProp5, Task<FailOr<TResult5>>> predicateAsync
        ) validator5,
        (
            Expression<Func<T, TProp6>> propertySelector,
            Func<TProp6, Task<FailOr<TResult6>>> predicateAsync
        ) validator6,
        (
            Expression<Func<T, TProp7>> propertySelector,
            Func<TProp7, Task<FailOr<TResult7>>> predicateAsync
        ) validator7,
        (
            Expression<Func<T, TProp8>> propertySelector,
            Func<TProp8, Task<FailOr<TResult8>>> predicateAsync
        ) validator8,
        (
            Expression<Func<T, TProp9>> propertySelector,
            Func<TProp9, Task<FailOr<TResult9>>> predicateAsync
        ) validator9,
        (
            Expression<Func<T, TProp10>> propertySelector,
            Func<TProp10, Task<FailOr<TResult10>>> predicateAsync
        ) validator10,
        (
            Expression<Func<T, TProp11>> propertySelector,
            Func<TProp11, Task<FailOr<TResult11>>> predicateAsync
        ) validator11
    )
    {
        return ValidationPipeline.ValidateAsync(
            value,
            [
                AsyncValidationRule<T>.Create(
                    validator1.propertySelector,
                    validator1.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator2.propertySelector,
                    validator2.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator3.propertySelector,
                    validator3.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator4.propertySelector,
                    validator4.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator5.propertySelector,
                    validator5.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator6.propertySelector,
                    validator6.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator7.propertySelector,
                    validator7.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator8.propertySelector,
                    validator8.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator9.propertySelector,
                    validator9.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator10.propertySelector,
                    validator10.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator11.propertySelector,
                    validator11.predicateAsync
                ),
            ]
        );
    }

    /// <summary>
    /// Validates 12 selected properties and returns the original value when every rule succeeds.
    /// </summary>
    /// <param name="value">The object to validate.</param>
    /// <param name="validator1">The first property selector and validation rule.</param>
    /// <param name="validator2">The second property selector and validation rule.</param>
    /// <param name="validator3">The third property selector and validation rule.</param>
    /// <param name="validator4">The fourth property selector and validation rule.</param>
    /// <param name="validator5">The fifth property selector and validation rule.</param>
    /// <param name="validator6">The sixth property selector and validation rule.</param>
    /// <param name="validator7">The seventh property selector and validation rule.</param>
    /// <param name="validator8">The eighth property selector and validation rule.</param>
    /// <param name="validator9">The ninth property selector and validation rule.</param>
    /// <param name="validator10">The tenth property selector and validation rule.</param>
    /// <param name="validator11">The eleventh property selector and validation rule.</param>
    /// <param name="validator12">The twelfth property selector and validation rule.</param>
    /// <returns>The original <paramref name="value"/> when every validator succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, or any validation delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = await person.ValidateAsync(validator1, validator2);</code></example>
    public static Task<FailOr<T>> ValidateAsync<
        T,
        TProp1,
        TResult1,
        TProp2,
        TResult2,
        TProp3,
        TResult3,
        TProp4,
        TResult4,
        TProp5,
        TResult5,
        TProp6,
        TResult6,
        TProp7,
        TResult7,
        TProp8,
        TResult8,
        TProp9,
        TResult9,
        TProp10,
        TResult10,
        TProp11,
        TResult11,
        TProp12,
        TResult12
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, Task<FailOr<TResult1>>> predicateAsync
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, Task<FailOr<TResult2>>> predicateAsync
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, Task<FailOr<TResult3>>> predicateAsync
        ) validator3,
        (
            Expression<Func<T, TProp4>> propertySelector,
            Func<TProp4, Task<FailOr<TResult4>>> predicateAsync
        ) validator4,
        (
            Expression<Func<T, TProp5>> propertySelector,
            Func<TProp5, Task<FailOr<TResult5>>> predicateAsync
        ) validator5,
        (
            Expression<Func<T, TProp6>> propertySelector,
            Func<TProp6, Task<FailOr<TResult6>>> predicateAsync
        ) validator6,
        (
            Expression<Func<T, TProp7>> propertySelector,
            Func<TProp7, Task<FailOr<TResult7>>> predicateAsync
        ) validator7,
        (
            Expression<Func<T, TProp8>> propertySelector,
            Func<TProp8, Task<FailOr<TResult8>>> predicateAsync
        ) validator8,
        (
            Expression<Func<T, TProp9>> propertySelector,
            Func<TProp9, Task<FailOr<TResult9>>> predicateAsync
        ) validator9,
        (
            Expression<Func<T, TProp10>> propertySelector,
            Func<TProp10, Task<FailOr<TResult10>>> predicateAsync
        ) validator10,
        (
            Expression<Func<T, TProp11>> propertySelector,
            Func<TProp11, Task<FailOr<TResult11>>> predicateAsync
        ) validator11,
        (
            Expression<Func<T, TProp12>> propertySelector,
            Func<TProp12, Task<FailOr<TResult12>>> predicateAsync
        ) validator12
    )
    {
        return ValidationPipeline.ValidateAsync(
            value,
            [
                AsyncValidationRule<T>.Create(
                    validator1.propertySelector,
                    validator1.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator2.propertySelector,
                    validator2.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator3.propertySelector,
                    validator3.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator4.propertySelector,
                    validator4.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator5.propertySelector,
                    validator5.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator6.propertySelector,
                    validator6.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator7.propertySelector,
                    validator7.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator8.propertySelector,
                    validator8.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator9.propertySelector,
                    validator9.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator10.propertySelector,
                    validator10.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator11.propertySelector,
                    validator11.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator12.propertySelector,
                    validator12.predicateAsync
                ),
            ]
        );
    }

    /// <summary>
    /// Validates 13 selected properties and returns the original value when every rule succeeds.
    /// </summary>
    /// <param name="value">The object to validate.</param>
    /// <param name="validator1">The first property selector and validation rule.</param>
    /// <param name="validator2">The second property selector and validation rule.</param>
    /// <param name="validator3">The third property selector and validation rule.</param>
    /// <param name="validator4">The fourth property selector and validation rule.</param>
    /// <param name="validator5">The fifth property selector and validation rule.</param>
    /// <param name="validator6">The sixth property selector and validation rule.</param>
    /// <param name="validator7">The seventh property selector and validation rule.</param>
    /// <param name="validator8">The eighth property selector and validation rule.</param>
    /// <param name="validator9">The ninth property selector and validation rule.</param>
    /// <param name="validator10">The tenth property selector and validation rule.</param>
    /// <param name="validator11">The eleventh property selector and validation rule.</param>
    /// <param name="validator12">The twelfth property selector and validation rule.</param>
    /// <param name="validator13">The thirteenth property selector and validation rule.</param>
    /// <returns>The original <paramref name="value"/> when every validator succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, or any validation delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = await person.ValidateAsync(validator1, validator2);</code></example>
    public static Task<FailOr<T>> ValidateAsync<
        T,
        TProp1,
        TResult1,
        TProp2,
        TResult2,
        TProp3,
        TResult3,
        TProp4,
        TResult4,
        TProp5,
        TResult5,
        TProp6,
        TResult6,
        TProp7,
        TResult7,
        TProp8,
        TResult8,
        TProp9,
        TResult9,
        TProp10,
        TResult10,
        TProp11,
        TResult11,
        TProp12,
        TResult12,
        TProp13,
        TResult13
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, Task<FailOr<TResult1>>> predicateAsync
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, Task<FailOr<TResult2>>> predicateAsync
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, Task<FailOr<TResult3>>> predicateAsync
        ) validator3,
        (
            Expression<Func<T, TProp4>> propertySelector,
            Func<TProp4, Task<FailOr<TResult4>>> predicateAsync
        ) validator4,
        (
            Expression<Func<T, TProp5>> propertySelector,
            Func<TProp5, Task<FailOr<TResult5>>> predicateAsync
        ) validator5,
        (
            Expression<Func<T, TProp6>> propertySelector,
            Func<TProp6, Task<FailOr<TResult6>>> predicateAsync
        ) validator6,
        (
            Expression<Func<T, TProp7>> propertySelector,
            Func<TProp7, Task<FailOr<TResult7>>> predicateAsync
        ) validator7,
        (
            Expression<Func<T, TProp8>> propertySelector,
            Func<TProp8, Task<FailOr<TResult8>>> predicateAsync
        ) validator8,
        (
            Expression<Func<T, TProp9>> propertySelector,
            Func<TProp9, Task<FailOr<TResult9>>> predicateAsync
        ) validator9,
        (
            Expression<Func<T, TProp10>> propertySelector,
            Func<TProp10, Task<FailOr<TResult10>>> predicateAsync
        ) validator10,
        (
            Expression<Func<T, TProp11>> propertySelector,
            Func<TProp11, Task<FailOr<TResult11>>> predicateAsync
        ) validator11,
        (
            Expression<Func<T, TProp12>> propertySelector,
            Func<TProp12, Task<FailOr<TResult12>>> predicateAsync
        ) validator12,
        (
            Expression<Func<T, TProp13>> propertySelector,
            Func<TProp13, Task<FailOr<TResult13>>> predicateAsync
        ) validator13
    )
    {
        return ValidationPipeline.ValidateAsync(
            value,
            [
                AsyncValidationRule<T>.Create(
                    validator1.propertySelector,
                    validator1.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator2.propertySelector,
                    validator2.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator3.propertySelector,
                    validator3.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator4.propertySelector,
                    validator4.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator5.propertySelector,
                    validator5.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator6.propertySelector,
                    validator6.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator7.propertySelector,
                    validator7.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator8.propertySelector,
                    validator8.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator9.propertySelector,
                    validator9.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator10.propertySelector,
                    validator10.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator11.propertySelector,
                    validator11.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator12.propertySelector,
                    validator12.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator13.propertySelector,
                    validator13.predicateAsync
                ),
            ]
        );
    }

    /// <summary>
    /// Validates 14 selected properties and returns the original value when every rule succeeds.
    /// </summary>
    /// <param name="value">The object to validate.</param>
    /// <param name="validator1">The first property selector and validation rule.</param>
    /// <param name="validator2">The second property selector and validation rule.</param>
    /// <param name="validator3">The third property selector and validation rule.</param>
    /// <param name="validator4">The fourth property selector and validation rule.</param>
    /// <param name="validator5">The fifth property selector and validation rule.</param>
    /// <param name="validator6">The sixth property selector and validation rule.</param>
    /// <param name="validator7">The seventh property selector and validation rule.</param>
    /// <param name="validator8">The eighth property selector and validation rule.</param>
    /// <param name="validator9">The ninth property selector and validation rule.</param>
    /// <param name="validator10">The tenth property selector and validation rule.</param>
    /// <param name="validator11">The eleventh property selector and validation rule.</param>
    /// <param name="validator12">The twelfth property selector and validation rule.</param>
    /// <param name="validator13">The thirteenth property selector and validation rule.</param>
    /// <param name="validator14">The fourteenth property selector and validation rule.</param>
    /// <returns>The original <paramref name="value"/> when every validator succeeds; otherwise a failed result containing normalized validation failures in declaration order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/>, any selector, or any validation delegate is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when any selector does not resolve to a property access.</exception>
    /// <example><code>var result = await person.ValidateAsync(validator1, validator2);</code></example>
    public static Task<FailOr<T>> ValidateAsync<
        T,
        TProp1,
        TResult1,
        TProp2,
        TResult2,
        TProp3,
        TResult3,
        TProp4,
        TResult4,
        TProp5,
        TResult5,
        TProp6,
        TResult6,
        TProp7,
        TResult7,
        TProp8,
        TResult8,
        TProp9,
        TResult9,
        TProp10,
        TResult10,
        TProp11,
        TResult11,
        TProp12,
        TResult12,
        TProp13,
        TResult13,
        TProp14,
        TResult14
    >(
        this T value,
        (
            Expression<Func<T, TProp1>> propertySelector,
            Func<TProp1, Task<FailOr<TResult1>>> predicateAsync
        ) validator1,
        (
            Expression<Func<T, TProp2>> propertySelector,
            Func<TProp2, Task<FailOr<TResult2>>> predicateAsync
        ) validator2,
        (
            Expression<Func<T, TProp3>> propertySelector,
            Func<TProp3, Task<FailOr<TResult3>>> predicateAsync
        ) validator3,
        (
            Expression<Func<T, TProp4>> propertySelector,
            Func<TProp4, Task<FailOr<TResult4>>> predicateAsync
        ) validator4,
        (
            Expression<Func<T, TProp5>> propertySelector,
            Func<TProp5, Task<FailOr<TResult5>>> predicateAsync
        ) validator5,
        (
            Expression<Func<T, TProp6>> propertySelector,
            Func<TProp6, Task<FailOr<TResult6>>> predicateAsync
        ) validator6,
        (
            Expression<Func<T, TProp7>> propertySelector,
            Func<TProp7, Task<FailOr<TResult7>>> predicateAsync
        ) validator7,
        (
            Expression<Func<T, TProp8>> propertySelector,
            Func<TProp8, Task<FailOr<TResult8>>> predicateAsync
        ) validator8,
        (
            Expression<Func<T, TProp9>> propertySelector,
            Func<TProp9, Task<FailOr<TResult9>>> predicateAsync
        ) validator9,
        (
            Expression<Func<T, TProp10>> propertySelector,
            Func<TProp10, Task<FailOr<TResult10>>> predicateAsync
        ) validator10,
        (
            Expression<Func<T, TProp11>> propertySelector,
            Func<TProp11, Task<FailOr<TResult11>>> predicateAsync
        ) validator11,
        (
            Expression<Func<T, TProp12>> propertySelector,
            Func<TProp12, Task<FailOr<TResult12>>> predicateAsync
        ) validator12,
        (
            Expression<Func<T, TProp13>> propertySelector,
            Func<TProp13, Task<FailOr<TResult13>>> predicateAsync
        ) validator13,
        (
            Expression<Func<T, TProp14>> propertySelector,
            Func<TProp14, Task<FailOr<TResult14>>> predicateAsync
        ) validator14
    )
    {
        return ValidationPipeline.ValidateAsync(
            value,
            [
                AsyncValidationRule<T>.Create(
                    validator1.propertySelector,
                    validator1.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator2.propertySelector,
                    validator2.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator3.propertySelector,
                    validator3.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator4.propertySelector,
                    validator4.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator5.propertySelector,
                    validator5.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator6.propertySelector,
                    validator6.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator7.propertySelector,
                    validator7.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator8.propertySelector,
                    validator8.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator9.propertySelector,
                    validator9.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator10.propertySelector,
                    validator10.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator11.propertySelector,
                    validator11.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator12.propertySelector,
                    validator12.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator13.propertySelector,
                    validator13.predicateAsync
                ),
                AsyncValidationRule<T>.Create(
                    validator14.propertySelector,
                    validator14.predicateAsync
                ),
            ]
        );
    }
}

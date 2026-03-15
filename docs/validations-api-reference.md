# FailOr.Validations API Reference

`FailOr.Validations` is the typed companion package for property-based validation on plain `T` values.

Start with the solution overview in [README.md](../README.md) and the package-specific guide in [src/FailOr.Validations/README.md](../src/FailOr.Validations/README.md).

Namespace:

```csharp
using FailOr.Validation;
```

Install:

```bash
dotnet add package FailOr.Validations
```

`FailOr.Validations` depends on `FailOr`, so adding this package restores the core package automatically.

## Behavior

- `Validate(...)` and `ValidateAsync(...)` run one or more property-based validators and return the original input instance on success.
- `ValidateThenTransform(...)` and `ValidateThenTransformAsync(...)` run one or more property-based mappers and invoke the final transform only when every mapper succeeds.
- Only `Failures.Validation` instances are rewritten to the selected leaf property name such as `Name` or `City`.
- `Failures.General` and `Failures.Exceptional` are preserved unchanged.
- Async validators and async mappers run sequentially and preserve declaration order.

## Public API Families

Representative signatures:

```csharp
public static FailOr<T> Validate<T, TProp1, TResult1>(
    this T value,
    (Expression<Func<T, TProp1>> propertySelector, Func<TProp1, FailOr<TResult1>> predicate) validator1)

public static Task<FailOr<T>> ValidateAsync<T, TProp1, TResult1>(
    this T value,
    (Expression<Func<T, TProp1>> propertySelector, Func<TProp1, Task<FailOr<TResult1>>> predicateAsync) validator1)

public static FailOr<TResult> ValidateThenTransform<T, TProp1, TMapped1, TResult>(
    this T value,
    (Expression<Func<T, TProp1>> propertySelector, Func<TProp1, FailOr<TMapped1>> mapper) validator1,
    Func<TMapped1, TResult> transform)

public static Task<FailOr<TResult>> ValidateThenTransformAsync<T, TProp1, TMapped1, TResult>(
    this T value,
    (Expression<Func<T, TProp1>> propertySelector, Func<TProp1, Task<FailOr<TMapped1>>> mapperAsync) validator1,
    Func<TMapped1, TResult> transform)

public static Task<FailOr<TResult>> ValidateThenTransformAsync<T, TProp1, TMapped1, TResult>(
    this T value,
    (Expression<Func<T, TProp1>> propertySelector, Func<TProp1, Task<FailOr<TMapped1>>> mapperAsync) validator1,
    Func<TMapped1, Task<TResult>> transformAsync)
```

Each family includes typed overloads for arities `1` through `14`.

## Examples

Validate and keep the original value:

```csharp
using FailOr;
using FailOr.Validation;

var result = person.Validate(
    (x => x.Name, name => string.IsNullOrWhiteSpace(name)
        ? FailOr.Fail<int>(Failure.Validation("Ignored", "Name is required."))
        : FailOr.Success(1)),
    (x => x.Address.City, city => string.IsNullOrWhiteSpace(city)
        ? FailOr.Fail<int>(Failure.Validation("Ignored", "City is required."))
        : FailOr.Success(1)));
```

Validate, map, and transform:

```csharp
using FailOr;
using FailOr.Validation;

var result = person.ValidateThenTransform(
    (x => x.Name, name => string.IsNullOrWhiteSpace(name)
        ? FailOr.Fail<string>(Failure.Validation("Ignored", "Name is required."))
        : FailOr.Success(name.ToUpperInvariant())),
    (x => x.Address.City, city => string.IsNullOrWhiteSpace(city)
        ? FailOr.Fail<string>(Failure.Validation("Ignored", "City is required."))
        : FailOr.Success(city.ToUpperInvariant())),
    (name, city) => $"{name}|{city}");
```

Async validation and async transform:

```csharp
using FailOr;
using FailOr.Validation;

var result = await person.ValidateThenTransformAsync(
    (x => x.Name, async name =>
    {
        await Task.Delay(1);
        return string.IsNullOrWhiteSpace(name)
            ? FailOr.Fail<string>(Failure.Validation("Ignored", "Name is required."))
            : FailOr.Success(name.ToUpperInvariant());
    }),
    async name =>
    {
        await Task.Delay(1);
        return $"Hello {name}";
    });
```

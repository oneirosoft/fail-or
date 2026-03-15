# FailOr.Validations

[![NuGet Version](https://img.shields.io/nuget/v/FailOr.Validations?logo=nuget&label=NuGet)](https://www.nuget.org/packages/FailOr.Validations/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/FailOr.Validations?logo=nuget&label=Downloads)](https://www.nuget.org/packages/FailOr.Validations/)
[![CI](https://img.shields.io/github/actions/workflow/status/oneirosoft/fail-or/ci.yml?branch=main&label=CI)](https://github.com/oneirosoft/fail-or/actions/workflows/ci.yml)
[![Targets](https://img.shields.io/badge/Targets-net8.0%20%7C%20net10.0-512bd4?logo=dotnet)](https://github.com/oneirosoft/fail-or)
[![License](https://img.shields.io/github/license/oneirosoft/fail-or?label=License)](https://github.com/oneirosoft/fail-or/blob/main/LICENSE)

`FailOr.Validations` is the typed companion package for property-based validation on plain `T` values. It builds on top of `FailOr` and adds selector-driven validation and validation-plus-transform helpers.

## Install

```bash
dotnet add package FailOr.Validations
```

`FailOr.Validations` depends on `FailOr`, so the core package is restored automatically.

## Behavior

- `Validate(...)` and `ValidateAsync(...)` run one or more property-based validators and return the original input instance on success.
- `ValidateThenTransform(...)` and `ValidateThenTransformAsync(...)` run one or more property-based mappers and invoke the final transform only when every mapper succeeds.
- Only `Failures.Validation` instances are rewritten to the selected leaf property name such as `Name` or `City`.
- `Failures.General` and `Failures.Exceptional` are preserved unchanged.
- Async validators and async mappers run sequentially and preserve declaration order.

## Quick Start

Validate and keep the original value:

```csharp
using FailOr;
using FailOr.Validation;

var person = new Person
{
    Name = "Ada",
    Nickname = "Ace",
    Address = new Address { City = "Boston" }
};

var result = person.Validate(
    (x => x.Name, name => string.IsNullOrWhiteSpace(name)
        ? FailOr.Fail<int>(Failure.Validation("Ignored", "Name is required."))
        : FailOr.Success(1)),
    (x => x.Address.City, city => string.IsNullOrWhiteSpace(city)
        ? FailOr.Fail<int>(Failure.Validation("Ignored", "City is required."))
        : FailOr.Success(1)));
```

Map selected properties and transform the mapped values:

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

Run async validators and async transforms:

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

## Documentation

- Repository overview: [FailOr root README](https://github.com/oneirosoft/fail-or/blob/main/README.md)
- Core package readme: [src/FailOr/README.md](https://github.com/oneirosoft/fail-or/blob/main/src/FailOr/README.md)
- Validations API reference: [docs/validations-api-reference.md](https://github.com/oneirosoft/fail-or/blob/main/docs/validations-api-reference.md)

## License

Licensed under the MIT License. See [LICENSE](https://github.com/oneirosoft/fail-or/blob/main/LICENSE).

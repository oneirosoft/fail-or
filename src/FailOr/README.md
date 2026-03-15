# FailOr

[![NuGet Version](https://img.shields.io/nuget/v/FailOr?logo=nuget&label=NuGet)](https://www.nuget.org/packages/FailOr/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/FailOr?logo=nuget&label=Downloads)](https://www.nuget.org/packages/FailOr/)
[![CI](https://img.shields.io/github/actions/workflow/status/oneirosoft/fail-or/ci.yml?branch=main&label=CI)](https://github.com/oneirosoft/fail-or/actions/workflows/ci.yml)
[![Targets](https://img.shields.io/badge/Targets-net8.0%20%7C%20net10.0-512bd4?logo=dotnet)](https://github.com/oneirosoft/fail-or)
[![License](https://img.shields.io/github/license/oneirosoft/fail-or?label=License)](https://github.com/oneirosoft/fail-or/blob/main/LICENSE)

`FailOr` is the core package for explicit success-and-failure flows in .NET. It gives you a small result type, a failure union, and composable helpers for mapping, matching, chaining, and aggregation.

## Install

```bash
dotnet add package FailOr
```

## Core Surface

- `FailOr<T>` represents either a success value or one-or-more failures.
- `Failures` is the abstract failure union carried by failed results.
- `Failure` is the public factory surface for `Failures.General`, `Failures.Validation`, and `Failures.Exceptional`.
- Chaining and observation helpers include `Then`, `ThenAsync`, `ThenEnsure`, `FailWhen`, `ThenDo`, `IfSuccess`, `IfFail`, `Match`, `MatchFirst`, `Try`, `Zip`, and `Combine`.

## Quick Start

Create success and failure results:

```csharp
using FailOr;

var success = FailOr.Success(42);
var failure = FailOr.Fail<int>(Failure.General("Input was invalid."));

if (success.IsSuccess)
{
    Console.WriteLine(success.UnsafeUnwrap());
}

if (failure.IsFailure)
{
    Console.WriteLine(failure.Failures[0].Details);
}
```

Chain success values:

```csharp
using FailOr;

var result = FailOr.Success(10)
    .Then(value => value + 5)
    .ThenEnsure(value =>
        value >= 0
            ? FailOr.Success(true)
            : FailOr.Fail<bool>(Failure.General("Value must be non-negative.")))
    .Then(value => value * 2);
```

Handle both outcomes with `Match`:

```csharp
using FailOr;

var message = FailOr.Fail<int>(Failure.General("The value could not be produced."))
    .Match(
        success: value => $"Value: {value}",
        failure: failures => failures[0].Details);
```

Convert thrown exceptions into failures with `Try`:

```csharp
using FailOr;

var parsed = FailOr.Success("42")
    .Try(value => int.Parse(value));
```

## Related Package

Use `FailOr.Validations` when you want typed, property-selector based validation helpers that build on top of the core package:

```bash
dotnet add package FailOr.Validations
```

## Documentation

- Repository overview: [FailOr root README](https://github.com/oneirosoft/fail-or/blob/main/README.md)
- Core API reference: [docs/api-reference.md](https://github.com/oneirosoft/fail-or/blob/main/docs/api-reference.md)
- Companion validations package: [src/FailOr.Validations/README.md](https://github.com/oneirosoft/fail-or/blob/main/src/FailOr.Validations/README.md)

## License

Licensed under the MIT License. See [LICENSE](https://github.com/oneirosoft/fail-or/blob/main/LICENSE).

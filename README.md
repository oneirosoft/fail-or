# FailOr

[![CI](https://img.shields.io/github/actions/workflow/status/oneirosoft/fail-or/ci.yml?branch=main&label=CI)](https://github.com/oneirosoft/fail-or/actions/workflows/ci.yml)
[![License](https://img.shields.io/github/license/oneirosoft/fail-or?label=License)](https://github.com/oneirosoft/fail-or/blob/main/LICENSE)
[![Targets](https://img.shields.io/badge/Targets-net8.0%20%7C%20net10.0-512bd4?logo=dotnet)](https://github.com/oneirosoft/fail-or)

`FailOr` is a small .NET result-type toolkit for explicit success and failure flows. The repository contains a core package for result composition and a companion package for typed property-based validation.

## Philosophy

- Use explicit results for expected failure paths instead of exceptions for normal control flow.
- Keep success and failure handling composable through small, focused APIs.
- Preserve rich failure information so validation, general, and exceptional failures can flow through the same pipelines.
- Add validation behavior as an opt-in companion package instead of forcing it into the core result type.

## Packages

### `FailOr`

[![FailOr NuGet Version](https://img.shields.io/nuget/v/FailOr?logo=nuget&label=NuGet)](https://www.nuget.org/packages/FailOr/)
[![FailOr NuGet Downloads](https://img.shields.io/nuget/dt/FailOr?logo=nuget&label=Downloads)](https://www.nuget.org/packages/FailOr/)

The core package for `FailOr<T>`, `Failures`, `Failure`, and the chaining, matching, and aggregation APIs.

- Package readme: [src/FailOr/README.md](src/FailOr/README.md)
- API reference: [docs/api-reference.md](docs/api-reference.md)

### `FailOr.Validations`

[![FailOr.Validations NuGet Version](https://img.shields.io/nuget/v/FailOr.Validations?logo=nuget&label=NuGet)](https://www.nuget.org/packages/FailOr.Validations/)
[![FailOr.Validations NuGet Downloads](https://img.shields.io/nuget/dt/FailOr.Validations?logo=nuget&label=Downloads)](https://www.nuget.org/packages/FailOr.Validations/)

The companion package for property-selector based validation and validation-plus-transform pipelines built on top of `FailOr`.

- Package readme: [src/FailOr.Validations/README.md](src/FailOr.Validations/README.md)
- API reference: [docs/validations-api-reference.md](docs/validations-api-reference.md)

## Solution Quick Start

Install the core package when you want explicit success-or-failure results:

```bash
dotnet add package FailOr
```

Install the validations companion package when you want selector-based validation helpers. It brings `FailOr` as a dependency.

```bash
dotnet add package FailOr.Validations
```

Create success and failure results with the core package:

```csharp
using FailOr;

var success = FailOr.Success(42);
var failure = FailOr.Fail<int>(Failure.General("Input was invalid."));

var message = success.Match(
    value => $"Value: {value}",
    failures => failures[0].Details);
```

Validate selected properties with the companion package:

```csharp
using FailOr;
using FailOr.Validation;

var person = new Person
{
    Name = "Ada",
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

`Failures.Validation` values returned by validators are normalized to the selected leaf property name such as `Name` or `City`, while `Failures.General` and `Failures.Exceptional` are preserved unchanged.

## Documentation

- Core package readme: [src/FailOr/README.md](src/FailOr/README.md)
- Validations package readme: [src/FailOr.Validations/README.md](src/FailOr.Validations/README.md)
- Core API reference: [docs/api-reference.md](docs/api-reference.md)
- Validations API reference: [docs/validations-api-reference.md](docs/validations-api-reference.md)

## Local Development

Restore local tools, packages, build, and test from the repository root:

```bash
dotnet tool restore
dotnet restore fail-or.slnx
dotnet build fail-or.slnx
dotnet test --solution fail-or.slnx
```

Format C# code with the repository-local CSharpier tool:

```bash
dotnet csharpier format .
```

Verify formatting without rewriting files:

```bash
dotnet csharpier check .
```

Create local packages with an explicit version:

```bash
dotnet pack src/FailOr/FailOr.csproj -c Release -p:Version=1.2.3 --output artifacts/packages
dotnet pack src/FailOr.Validations/FailOr.Validations.csproj -c Release -p:Version=1.2.3 --output artifacts/packages
```

## Repository Metadata

- GitHub repository: `https://github.com/oneirosoft/fail-or`
- Issue tracker: `https://github.com/oneirosoft/fail-or/issues`
- Releases: `https://github.com/oneirosoft/fail-or/releases`

## License

This project is licensed under the MIT License. See the repository [LICENSE](LICENSE) file for the full text.

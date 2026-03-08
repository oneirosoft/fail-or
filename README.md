# FailOr

`FailOr` is a small .NET result-type library for representing success or one-or-more failures without relying on exceptions for normal control flow.

It centers on two public types:

- `FailOr<T>` for a successful value or a collection of failures
- `Failure` for a machine-friendly code plus human-readable details

The library also includes convenience APIs for common result workflows:

- `FailOr.Success(...)` and `FailOr.Fail(...)` for construction
- `Then(...)` and `ThenAsync(...)` for chaining
- `Match(...)` and `MatchFirst(...)` for branching
- `Zip(...)` for aggregating multiple results
- `Combine(...)` for choosing a preferred success with fallback

## Target framework

`FailOr` currently targets `net10.0`.

## Installation

Install from NuGet:

```bash
dotnet add package FailOr
```

## Core concepts

`FailOr<T>` is either:

- a success value of type `T`
- a failure result containing one or more `Failure` values

`Failure` currently exposes:

- `Code`
- `Details`

Use `IsSuccess`, `IsFailure`, `UnsafeUnwrap()`, and `Failures` to inspect a result directly when needed.

## Quick start

### Create success and failure results

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

### Chain success values with `Then`

Use `Then` when the next step should only run after success.

```csharp
using FailOr;

var result = FailOr.Success(10)
    .Then(value => value + 5)
    .Then(value => FailOr.Success(value * 2));

var finalValue = result.UnsafeUnwrap(); // 30
```

Async variants are also available:

```csharp
using FailOr;

var result = await FailOr.Success(10)
    .ThenAsync(async value =>
    {
        await Task.Delay(10);
        return value + 5;
    });
```

### Branch with `Match`

Use `Match` when you want a single expression that handles both outcomes.

```csharp
using FailOr;

var message = FailOr.Fail<int>(Failure.General("The value could not be produced."))
    .Match(
        success: value => $"Value: {value}",
        failure: failures => failures[0].Details);
```

Use `MatchFirst` when only the first failure matters:

```csharp
using FailOr;

var message = FailOr.Fail<int>(
        Failure.General("Primary problem"),
        Failure.General("Secondary problem"))
    .MatchFirst(
        success: value => $"Value: {value}",
        failure: firstFailure => firstFailure.Details);
```

### Aggregate with `Zip`

`Zip` combines successful results into tuples and preserves failures in left-to-right order.

```csharp
using FailOr;

var zipped = FailOr.Zip(
    FailOr.Success(1),
    FailOr.Success("two"),
    FailOr.Success(true));

var (number, text, flag) = zipped.UnsafeUnwrap();
```

If any input fails, the returned result is failed and contains every failure from the failed inputs.

### Prefer one result with `Combine`

`Combine` returns the left result when it succeeds; otherwise it returns the right result.

```csharp
using FailOr;

var preferred = FailOr.Combine(
    FailOr.Fail<int>(Failure.General("Primary source unavailable")),
    FailOr.Success(99));

Console.WriteLine(preferred.UnsafeUnwrap()); // 99
```

## Local development

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

Create a local package with an explicit version:

```bash
dotnet pack src/FailOr/FailOr.csproj -c Release -p:Version=1.2.3
```

## GitHub Actions

The repository includes two workflows:

- `CI` runs on pushes to `main` and pull requests targeting `main`
- `Release` runs when a GitHub Release is published

The CI workflow restores, builds, and tests the solution so it can be used as a required branch-protection check.

The release workflow:

1. reads the GitHub Release tag
2. requires the tag to match `v<NuGetSemVer>`
3. strips the leading `v`
4. runs restore, build, test, and pack
5. publishes the resulting package to nuget.org only after all checks pass

Examples of accepted release tags:

- `v1.2.3`
- `v1.2.3-beta.1`

Examples of rejected release tags:

- `1.2.3`
- `release-1.2.3`

## NuGet trusted publishing setup

Before the release workflow can publish to nuget.org, complete this setup:

1. Create the GitHub repository.
2. Verify project metadata and repository links point to `oneirosoft/fail-or`.
3. In nuget.org, configure a trusted publishing policy for this repository and the workflow file `release.yml`.
4. Add a GitHub repository variable named `NUGET_ORG_USERNAME` with the nuget.org account or profile name that owns the package.
5. Optionally configure a GitHub Actions environment if you want additional release approvals or environment-scoped controls.

The workflow requests an OIDC token and exchanges it for a short-lived NuGet API key during the release job. No long-lived NuGet API key is stored in GitHub.

## Releasing

The release process is:

1. Ensure `main` is green.
2. Create and push a tag in the form `v<NuGetSemVer>`.
3. Publish a GitHub Release for that tag.
4. Let the `Release` workflow build, test, pack, and publish the package.

The GitHub Release tag is the package version source of truth. The project file intentionally does not hardcode a package version.

## Repository metadata

Repository metadata:

- GitHub repository: `https://github.com/oneirosoft/fail-or`
- Issue tracker: `https://github.com/oneirosoft/fail-or/issues`
- Releases: `https://github.com/oneirosoft/fail-or/releases`

## License

This project is licensed under the MIT License. See the repository `LICENSE` file for the full text.

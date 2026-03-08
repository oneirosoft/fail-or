# FailOr

`FailOr` is a small .NET result-type library for representing success or one-or-more failures without relying on exceptions for normal control flow.

It centers on three public entry points:

- `FailOr<T>` for a successful value or a collection of failures
- `Failures` for the abstract failure union carried by failed results
- `Failure` for factory methods that create `Failures.General`, `Failures.Validation`, and `Failures.Exceptional`

The library also includes convenience APIs for common result workflows:

- `FailOr.Success(...)` and `FailOr.Fail(...)` for construction
- `Then(...)`, `ThenAsync(...)`, `ThenEnsure(...)`, `ThenEnsureAsync(...)`, `ThenDo(...)`, and `ThenDoAsync(...)` for chaining and success-side effects
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
- a failure result containing one or more `Failures` values

`Failures` always exposes:

- `Code`
- `Details`

Concrete failure values are created through the `Failure` factory surface:

- `Failure.General(...)` for general-purpose failures with optional metadata
- `Failure.Validation(...)` for property-scoped validation failures with one-or-more messages
- `Failure.Exceptional(...)` for failures that wrap exceptions

Use `IsSuccess`, `IsFailure`, `UnsafeUnwrap()`, and `Failures` to inspect a result directly when needed.

## Quick start

### Create success and failure results

```csharp
using FailOr;

var success = FailOr.Success(42);
var failure = FailOr.Fail<int>(Failure.General("Input was invalid."));
FailOr<int> implicitSuccess = 42;
FailOr<int> implicitSingleFailure = Failure.General("Input was invalid.");
Failures[] implicitFailures =
[
    Failure.General("Primary problem"),
    Failure.General("Secondary problem")
];
FailOr<int> implicitFailureArray = implicitFailures;
FailOr<int> implicitFailureList = new List<Failures>
{
    Failure.General("First list problem"),
    Failure.General("Second list problem")
};

if (success.IsSuccess)
{
    Console.WriteLine(success.UnsafeUnwrap());
}

if (failure.IsFailure)
{
    Console.WriteLine(failure.Failures[0].Details);
}
```

The implicit conversions delegate to the same `Success(...)` and `Fail(...)` validation paths, so null reference successes and invalid failure collections still throw the same exceptions as the factory methods.

### Create specific failure cases

```csharp
using FailOr;

var general = Failure.General(
    "Request timed out.",
    code: "Http.Timeout",
    metadata: new Dictionary<string, object?> { ["attempt"] = 3 });

var validation = Failure.Validation(
    "Email",
    "Email is required.",
    "Email must contain '@'.");

var exceptional = Failure.Exceptional(new InvalidOperationException("Operation failed."));
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

### Validate success values with `ThenEnsure`

Use `ThenEnsure` when the next step should validate the current success and keep that original value flowing when validation succeeds.

```csharp
using FailOr;

var result = FailOr.Success(10)
    .ThenEnsure(value =>
        value >= 0
            ? FailOr.Success(true)
            : FailOr.Fail<bool>(Failure.General("Value must be non-negative.")))
    .Then(value => value + 5);

var finalValue = result.UnsafeUnwrap(); // 15
```

Async validation helpers are also available:

```csharp
using FailOr;

var result = await FailOr.Success(10)
    .ThenEnsureAsync(async value =>
    {
        await Task.Delay(10);
        return value % 2 == 0
            ? FailOr.Success(true)
            : FailOr.Fail<bool>(Failure.General("Value must be even."));
    });
```

### Run success-side effects with `ThenDo`

Use `ThenDo` when you want to observe a success without changing the flowing result.

```csharp
using FailOr;

var result = FailOr.Success(10)
    .ThenDo(value => Console.WriteLine($"Observed: {value}"))
    .Then(value => value + 5);

var finalValue = result.UnsafeUnwrap(); // 15
```

The same side-effect helpers are available for task-wrapped results:

```csharp
using FailOr;

var result = await Task.FromResult(FailOr.Success(10))
    .ThenDoAsync(async value =>
    {
        await Task.Delay(10);
        Console.WriteLine($"Observed: {value}");
    })
    .Then(value => value + 5);
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

If any input fails, the returned result is failed and contains every `Failures` value from the failed inputs, including mixed failure cases such as validation and exceptional failures.

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

# FailOr API Reference

`FailOr` is a small result-type library for representing either:

- a successful value of type `T`
- one or more failures

This document is written for consumers of the core `FailOr` package. It focuses on what each API is for, when to use it, and the public signatures you will call most often.

Start with the solution overview in [README.md](../README.md) and the package-specific guide in [src/FailOr/README.md](../src/FailOr/README.md).

For the companion validation package, see [docs/validations-api-reference.md](./validations-api-reference.md) and [src/FailOr.Validations/README.md](../src/FailOr.Validations/README.md).

## Core Types

### `FailOr<T>`

Represents either a success value or one-or-more failures.

```csharp
public readonly record struct FailOr<T>
```

Use it when an operation should return a value on success without throwing exceptions for expected failure cases.

### `Failures`

Represents the abstract failure union carried by failed results.

```csharp
public abstract record Failures
```

Every failure exposes `Code` and `Details`. Concrete failures are:

- `Failures.General`
- `Failures.Validation`
- `Failures.Exceptional`

Additional case-specific members are:

- `Failures.General.Metadata`
- `Failures.Validation.PropertyName`
- `Failures.Exceptional.Exception`

### `Failure`

Provides the public factory entrypoint for creating failure values.

```csharp
public static class Failure
```

Use it when you want to create a specific `Failures` case without calling constructors directly.

## Creating Results

### Create a success

```csharp
public static FailOr<T> Success<T>(T value)
public static FailOr<T> FailOr<T>.Success(T value)
public static implicit operator FailOr<T>(T value)
```

Intent:
Wrap a successful value in a `FailOr<T>`.

Example:

```csharp
var result = FailOr.Success(42);
FailOr<int> implicitResult = 42;
```

### Create a failure from one `Failures` value

```csharp
public static FailOr<T> Fail<T>(Failures failure)
public static FailOr<T> FailOr<T>.Fail(Failures failure)
public static implicit operator FailOr<T>(Failures failure)
```

Intent:
Create a failed result with exactly one failure.

Example:

```csharp
var result = FailOr.Fail<int>(Failure.General("The input was invalid."));
FailOr<int> implicitResult = Failure.General("The input was invalid.");
```

### Create a failure from many `Failures` values

```csharp
public static FailOr<T> Fail<T>(IEnumerable<Failures> failures)
public static FailOr<T> Fail<T>(params Failures[] failures)
public static FailOr<T> FailOr<T>.Fail(IEnumerable<Failures> failures)
public static FailOr<T> FailOr<T>.Fail(params Failures[] failures)
public static implicit operator FailOr<T>(Failures[] failures)
public static implicit operator FailOr<T>(List<Failures> failures)
```

Intent:
Create a failed result with one-or-more failures.

Example:

```csharp
var result = FailOr.Fail<int>(
    Failure.General("Email is required."),
    Failure.General("Password is required."));

Failures[] failures =
[
    Failure.General("Email is required."),
    Failure.General("Password is required.")
];
FailOr<int> implicitArrayResult = failures;

var failureList = new List<Failures>
{
    Failure.General("Email is required."),
    Failure.General("Password is required.")
};
FailOr<int> implicitListResult = failureList;
```

### Create a general-purpose failure

```csharp
public static Failures.General General(
    string details,
    string? code = null,
    Dictionary<string, object?>? metadata = null)
```

Intent:
Create a `Failures.General` value with the default code `"General"` unless you supply a custom code.

Example:

```csharp
var failure = Failure.General("Something went wrong.");
```

### Create a validation failure

```csharp
public static Failures.Validation Validation(string propertyName, params string[] errors)
```

Intent:
Create a property-scoped validation failure. The generated code is `Validation.{PropertyName}` and `Details` is the joined error message string.

Example:

```csharp
var failure = Failure.Validation("Email", "Email is required.", "Email must contain '@'.");
```

### Create an exceptional failure

```csharp
public static Failures.Exceptional Exceptional(
    Exception exception,
    string? details = null,
    string? code = null)
```

Intent:
Create a failure that wraps an exception. The default code is `"Exceptional"` and the default details come from the exception message, or the exception type name when the message is blank.

Example:

```csharp
var failure = Failure.Exceptional(new InvalidOperationException("Operation failed."));
```

## Inspecting Results

### Check whether a result succeeded

```csharp
public bool IsSuccess { get; }
public bool IsFailure { get; }
```

Intent:
Use `IsSuccess` and `IsFailure` for direct branching when you do not want to use `Match`.

Example:

```csharp
if (result.IsFailure)
{
    return result.Failures;
}
```

### Read the success value

```csharp
public T UnsafeUnwrap()
```

Intent:
Return the success value when you already know the result succeeded.

Important:
This throws `InvalidOperationException` if the result is failed.

Example:

```csharp
var value = result.UnsafeUnwrap();
```

### Read the failures

```csharp
public IReadOnlyList<Failures> Failures { get; }
```

Intent:
Access the failures of a failed result without exposing a mutable collection.

Example:

```csharp
foreach (var failure in result.Failures)
{
    Console.WriteLine($"{failure.Code}: {failure.Details}");
}
```

## Transforming Success Values

These APIs only run when the source is successful. If the source is failed, the original failures pass through unchanged.

### Map a success value

```csharp
public FailOr<TResult> Then<TResult>(Func<TSource, TResult> map)
```

Intent:
Transform a success value into another success value.

Example:

```csharp
var result = FailOr.Success(10)
    .Then(x => x + 5);
```

### Bind to another `FailOr`

```csharp
public FailOr<TResult> Then<TResult>(Func<TSource, FailOr<TResult>> bind)
```

Intent:
Chain operations where the next step can also fail.

Example:

```csharp
var result = FailOr.Success("42")
    .Then(ParseNumber);
```

### Validate with another `FailOr` and preserve the success

```csharp
public FailOr<TSource> ThenEnsure<TResult>(Func<TSource, FailOr<TResult>> ensure)
```

Intent:
Run a validation step that can fail while keeping the original success value unchanged when validation succeeds.

Example:

```csharp
var result = FailOr.Success(10)
    .ThenEnsure(value =>
        value >= 0
            ? FailOr.Success(true)
            : FailOr.Fail<bool>(Failure.General("Value must be non-negative.")));
```

### Fail a success when a predicate matches

```csharp
public FailOr<TSource> FailWhen(Func<TSource, bool> predicate, Failures failure)
```

Intent:
Turn a successful value into a failed result when a predicate returns `true`, while preserving the original success when the predicate returns `false`.

Example:

```csharp
var result = FailOr.Success(10)
    .FailWhen(value => value < 0, Failure.General("Value must be non-negative."));
```

Use `FailWhen` for direct predicate-style validation. Use `ThenEnsure` when the validation step naturally produces another `FailOr`.

### Map asynchronously

```csharp
public Task<FailOr<TResult>> ThenAsync<TResult>(Func<TSource, Task<TResult>> mapAsync)
```

Intent:
Transform a success value with an asynchronous operation that returns a plain value.

Example:

```csharp
var result = await FailOr.Success(10)
    .ThenAsync(async x => await GetAdjustedValueAsync(x));
```

### Observe a success and keep chaining

```csharp
public FailOr<TSource> ThenDo(Action<TSource> action)
public Task<FailOr<TSource>> ThenDoAsync(Func<TSource, Task> actionAsync)
public Task<FailOr<TSource>> ThenDo(Action<TSource> action)
public Task<FailOr<TSource>> ThenDoAsync(Func<TSource, Task> actionAsync)
```

Intent:
Run a side effect only when the source is successful and preserve the original result for continued chaining.

Example:

```csharp
var result = FailOr.Success(10)
    .ThenDo(value => Console.WriteLine(value))
    .Then(value => value + 5);
```

### Observe a success terminally

```csharp
public void IfSuccess(Action<TSource> action)
public Task IfSuccessAsync(Func<TSource, Task> actionAsync)
public Task IfSuccess(Action<TSource> action)
public Task IfSuccessAsync(Func<TSource, Task> actionAsync)
```

Intent:
Run a side effect only when the source is successful, but end the pipeline instead of preserving a `FailOr<TSource>` for continued chaining.

Example:

```csharp
FailOr.Success(10)
    .IfSuccess(value => Console.WriteLine(value));
```

### Map a success value with exception handling

```csharp
public FailOr<TResult> Try<TResult>(Func<TSource, TResult> map)
```

Intent:
Transform a success value while converting thrown exceptions to `Failure.Exceptional(...)`.

Example:

```csharp
var result = FailOr.Success("42")
    .Try(int.Parse);
```

### Map a success value with custom exception handling

```csharp
public FailOr<TResult> Try<TResult>(
    Func<TSource, TResult> map,
    Func<Exception, FailOr<TResult>> onException)
```

Intent:
Transform a success value while projecting thrown exceptions into a custom `FailOr<TResult>`.

Example:

```csharp
var result = FailOr.Success("42x")
    .Try(
        int.Parse,
        exception => Failure.General($"Mapping failed: {exception.Message}"));
```

### Map asynchronously with exception handling

```csharp
public Task<FailOr<TResult>> TryAsync<TResult>(Func<TSource, Task<TResult>> mapAsync)
public Task<FailOr<TResult>> TryAsync<TResult>(
    Func<TSource, Task<TResult>> mapAsync,
    Func<Exception, FailOr<TResult>> onException)
```

Intent:
Transform a success value with an asynchronous operation while converting thrown exceptions to either `Failure.Exceptional(...)` or a custom projected result.

Example:

```csharp
var result = await FailOr.Success("42")
    .TryAsync(value => ParseNumberAsync(value));
```

### Bind asynchronously

```csharp
public Task<FailOr<TResult>> ThenAsync<TResult>(Func<TSource, Task<FailOr<TResult>>> bindAsync)
```

Intent:
Chain asynchronous operations where the next step can also fail.

Example:

```csharp
var result = await FailOr.Success("42")
    .ThenAsync(ParseNumberAsync);
```

### Validate asynchronously and preserve the success

```csharp
public Task<FailOr<TSource>> ThenEnsureAsync<TResult>(Func<TSource, Task<FailOr<TResult>>> ensureAsync)
```

Intent:
Run an asynchronous validation step that can fail while keeping the original success value unchanged when validation succeeds.

Example:

```csharp
var result = await FailOr.Success(10)
    .ThenEnsureAsync(async value =>
    {
        await Task.Delay(10);
        return value % 2 == 0
            ? FailOr.Success(true)
            : FailOr.Fail<bool>(Failure.General("Value must be even."));
    });
```

### Fail asynchronously when a predicate matches

```csharp
public Task<FailOr<TSource>> FailWhenAsync(Func<TSource, Task<bool>> predicateAsync, Failures failure)
```

Intent:
Turn a successful value into a failed result when an asynchronous predicate returns `true`, while preserving the original success when the predicate returns `false`.

Example:

```csharp
var result = await FailOr.Success(10)
    .FailWhenAsync(
        async value =>
        {
            await Task.Delay(10);
            return value < 0;
        },
        Failure.General("Value must be non-negative."));
```

### Run a side effect and preserve the success

```csharp
public FailOr<TSource> ThenDo(Action<TSource> action)
```

Intent:
Run a side effect such as logging, metrics, or caching without changing the success value.

Example:

```csharp
var result = FailOr.Success(10)
    .ThenDo(x => Console.WriteLine($"Observed {x}"));
```

### Run a side effect asynchronously and preserve the success

```csharp
public Task<FailOr<TSource>> ThenDoAsync(Func<TSource, Task> actionAsync)
```

Intent:
Run an asynchronous side effect without changing the success value.

Example:

```csharp
var result = await FailOr.Success(10)
    .ThenDoAsync(async x => await AuditAsync(x));
```

## Recovering From Failures

These APIs preserve an existing success. They only apply the fallback when the source is failed.

### Observe failures without recovering

```csharp
public void IfFail(Action<IReadOnlyList<Failures>> action)
public Task IfFailAsync(Func<IReadOnlyList<Failures>, Task> actionAsync)
```

Intent:
Run logging, auditing, metrics, or other side effects only when the result is failed, without replacing or transforming the original result.

Example:

```csharp
result.IfFail(failures => logger.LogError("Primary failure: {Message}", failures[0].Details));

await result.IfFailAsync(async failures =>
{
    await AuditAsync(failures);
});
```

### Provide a fallback result

```csharp
public FailOr<TSource> IfFailThen(FailOr<TSource> alternative)
```

Intent:
Return a replacement result if the original result failed.

Example:

```csharp
var result = ReadFromPrimary()
    .IfFailThen(ReadFromCache());
```

### Lazily create a fallback result

```csharp
public FailOr<TSource> IfFailThen(Func<FailOr<TSource>> alternative)
```

Intent:
Only compute the fallback if the original result failed.

Example:

```csharp
var result = ReadFromPrimary()
    .IfFailThen(() => ReadFromCache());
```

### Create a fallback using the original failures

```csharp
public FailOr<TSource> IfFailThen(Func<IReadOnlyList<Failures>, FailOr<TSource>> alternative)
```

Intent:
Build a fallback that can inspect the original failure list.

Example:

```csharp
var result = ReadFromPrimary()
    .IfFailThen(failures => LogAndBuildFallback(failures));
```

### Async failure recovery

```csharp
public Task<FailOr<TSource>> IfFailThenAsync(Func<Task<FailOr<TSource>>> alternativeAsync)
public Task<FailOr<TSource>> IfFailThenAsync(
    Func<IReadOnlyList<Failures>, Task<FailOr<TSource>>> alternativeAsync)
```

Intent:
Recover from failures using asynchronous fallbacks.

## Matching And Producing Final Values

Use `Match` when you want one expression that handles both success and failure.

### Match with all failures

```csharp
public TResult Match<TResult>(
    Func<TSource, TResult> success,
    Func<IReadOnlyList<Failures>, TResult> failure)
```

Intent:
Project a `FailOr<T>` into a final value, using one branch for success and one branch for failure.

Example:

```csharp
var message = result.Match(
    success: value => $"Value: {value}",
    failure: failures => failures[0].Details);
```

### Async `Match` overloads

```csharp
public Task<TResult> MatchAsync<TResult>(
    Func<TSource, Task<TResult>> successAsync,
    Func<IReadOnlyList<Failures>, TResult> failure)

public Task<TResult> MatchAsync<TResult>(
    Func<TSource, TResult> success,
    Func<IReadOnlyList<Failures>, Task<TResult>> failureAsync)

public Task<TResult> MatchAsync<TResult>(
    Func<TSource, Task<TResult>> successAsync,
    Func<IReadOnlyList<Failures>, Task<TResult>> failureAsync)
```

Intent:
Use asynchronous projections on either or both branches.

### Match using only the first failure

```csharp
public TResult MatchFirst<TResult>(
    Func<TSource, TResult> success,
    Func<Failures, TResult> failure)
```

Intent:
Like `Match`, but only expose the first failure when that is all the caller cares about.

Example:

```csharp
var message = result.MatchFirst(
    success: value => $"Value: {value}",
    failure: firstFailure => firstFailure.Details);
```

### Async `MatchFirst` overloads

```csharp
public Task<TResult> MatchFirstAsync<TResult>(
    Func<TSource, Task<TResult>> successAsync,
    Func<Failures, TResult> failure)

public Task<TResult> MatchFirstAsync<TResult>(
    Func<TSource, TResult> success,
    Func<Failures, Task<TResult>> failureAsync)

public Task<TResult> MatchFirstAsync<TResult>(
    Func<TSource, Task<TResult>> successAsync,
    Func<Failures, Task<TResult>> failureAsync)
```

Intent:
Use asynchronous projections while only exposing the first failure.

## Combining Results

### Prefer one result, with fallback

```csharp
public static FailOr<T> Combine<T>(FailOr<T> left, FailOr<T> right)
```

Intent:
Return `left` when it succeeds. Otherwise return `right`.

This is useful when you have a preferred source and a fallback source.

Example:

```csharp
var result = FailOr.Combine(
    ReadFromPrimary(),
    ReadFromCache());
```

### Zip multiple successful results into a tuple

```csharp
public static FailOr<(T1, T2)> Zip<T1, T2>(FailOr<T1> first, FailOr<T2> second)
public static FailOr<(T1, T2, T3)> Zip<T1, T2, T3>(
    FailOr<T1> first,
    FailOr<T2> second,
    FailOr<T3> third)
public static FailOr<(T1, T2, T3, T4)> Zip<T1, T2, T3, T4>(
    FailOr<T1> first,
    FailOr<T2> second,
    FailOr<T3> third,
    FailOr<T4> fourth)
public static FailOr<(T1, T2, T3, T4, T5)> Zip<T1, T2, T3, T4, T5>(
    FailOr<T1> first,
    FailOr<T2> second,
    FailOr<T3> third,
    FailOr<T4> fourth,
    FailOr<T5> fifth)
public static FailOr<(T1, T2, T3, T4, T5, T6)> Zip<T1, T2, T3, T4, T5, T6>(
    FailOr<T1> first,
    FailOr<T2> second,
    FailOr<T3> third,
    FailOr<T4> fourth,
    FailOr<T5> fifth,
    FailOr<T6> sixth)
public static FailOr<(T1, T2, T3, T4, T5, T6, T7)> Zip<T1, T2, T3, T4, T5, T6, T7>(
    FailOr<T1> first,
    FailOr<T2> second,
    FailOr<T3> third,
    FailOr<T4> fourth,
    FailOr<T5> fifth,
    FailOr<T6> sixth,
    FailOr<T7> seventh)
```

Intent:
Combine several independent `FailOr<T>` results into one tuple result.

Behavior:

- If every input succeeds, the returned value is a success containing a tuple.
- If any input fails, the returned value is failed.
- All failures are preserved in left-to-right order.

Example:

```csharp
var result = FailOr.Zip(
    GetUserId(),
    GetEmail(),
    GetIsActive());
```

## Task-Wrapped Result Extensions

The library also provides the same `Then`, `ThenAsync`, `ThenEnsure`, `ThenEnsureAsync`, `FailWhen`, `FailWhenAsync`, `Try`, `TryAsync`, `ThenDo`, `ThenDoAsync`, `IfFail`, `IfFailAsync`, `IfFailThen`, `IfFailThenAsync`, `Match`, `MatchAsync`, `MatchFirst`, and `MatchFirstAsync` APIs for:

```csharp
Task<FailOr<T>>
```

Intent:
Let you keep chaining and matching without repeatedly awaiting intermediate results yourself.

Example:

```csharp
var message = await GetUserAsync()
    .Then(user => user.Email)
    .Match(
        success: email => $"Email: {email}",
        failure: failures => failures[0].Details);
```

### Fail a task-wrapped success when a predicate matches

```csharp
public Task<FailOr<TSource>> FailWhen(Func<TSource, bool> predicate, Failures failure)
public Task<FailOr<TSource>> FailWhenAsync(Func<TSource, Task<bool>> predicateAsync, Failures failure)
```

Intent:
Apply predicate-style validation after awaiting a `Task<FailOr<T>>`, while preserving the original success when the predicate does not match.

Example:

```csharp
var result = await GetUserAgeAsync()
    .FailWhen(age => age < 0, Failure.General("Age must be non-negative."))
    .FailWhenAsync(async age => await IsBlockedAgeAsync(age), Failure.General("Age is blocked."));
```

### Preserve a task-wrapped success while running side effects

```csharp
public Task<FailOr<TSource>> ThenDo(Action<TSource> action)
public Task<FailOr<TSource>> ThenDoAsync(Func<TSource, Task> actionAsync)
```

Intent:
Observe or audit the successful value from a `Task<FailOr<T>>` without changing the flowing result.

Example:

```csharp
var result = await GetUserAsync()
    .ThenDo(user => Console.WriteLine(user.Email))
    .ThenDoAsync(user => AuditAsync(user))
    .Then(user => user.Email);
```

### Observe task-wrapped failures without recovering

```csharp
public Task IfFail(Action<IReadOnlyList<Failures>> action)
public Task IfFailAsync(Func<IReadOnlyList<Failures>, Task> actionAsync)
```

Intent:
Observe failures from a `Task<FailOr<T>>` after awaiting it, while leaving the original success or failure unchanged.

Example:

```csharp
await GetUserAsync()
    .IfFail(failures => logger.LogWarning("Observed {Count} failure(s)", failures.Count));
```

## Validation Rules And Exceptions

The current API validates a few important invalid states:

- `Success` throws `ArgumentNullException` for `null` reference-type values.
- `Fail` throws `ArgumentNullException` for a `null` single failure or `null` failure sequence.
- `Fail` throws `ArgumentException` when a failure sequence is empty or contains `null` values.
- `Failure.General` throws for `null`, empty, or whitespace details, and for blank custom codes.
- `Failure.Validation` throws for `null` or blank property names and for missing validation messages.
- `Failure.Exceptional` throws for a `null` exception and for blank explicit details or blank custom codes.
- `UnsafeUnwrap` throws `InvalidOperationException` when the result is failed.
- Async delegate-based APIs throw `ArgumentNullException` when the delegate itself is `null`.
- Async delegate-based APIs also throw `ArgumentNullException` when the selected delegate returns a `null` task.
- `FailWhen` and `FailWhenAsync` throw `ArgumentNullException` for a `null` predicate or `null` failure input.
- `Try` and `TryAsync` convert exceptions thrown by mapping delegates into `Failure.Exceptional(...)` unless you provide a custom exception projection.

## Choosing The Right API

- Use `Success` and `Fail` to create results.
- Use `Then` and `ThenAsync` to continue only on success.
- Use `FailWhen` and `FailWhenAsync` for predicate-style validation that should fail on a matching condition.
- Use `ThenDo` and `ThenDoAsync` to run side effects while preserving the success value.
- Use `IfFail` and `IfFailAsync` to run side effects while preserving the original failure.
- Use `IfFailThen` and `IfFailThenAsync` to recover from failures.
- Use `Match` when you want a final value from both branches.
- Use `MatchFirst` when only the first failure matters.
- Use `Combine` for preferred-source fallback.
- Use `Zip` to aggregate several independent results.

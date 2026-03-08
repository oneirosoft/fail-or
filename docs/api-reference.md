# FailOr API Reference

`FailOr` is a small result-type library for representing either:

- a successful value of type `T`
- one or more failures

This document is written for consumers of the package. It focuses on what each API is for, when to use it, and the public signatures you will call most often.

## Core Types

### `FailOr<T>`

Represents either a success value or one-or-more failures.

```csharp
public readonly record struct FailOr<T>
```

Use it when an operation should return a value on success without throwing exceptions for expected failure cases.

### `Failure`

Represents a single failure with a stable code and human-readable details.

```csharp
public readonly record struct Failure
```

Use it when you want to describe why an operation failed in a machine-friendly and user-friendly way.

## Creating Results

### Create a success

```csharp
public static FailOr<T> Success<T>(T value)
public static FailOr<T> FailOr<T>.Success(T value)
```

Intent:
Wrap a successful value in a `FailOr<T>`.

Example:

```csharp
var result = FailOr.Success(42);
```

### Create a failure from one `Failure`

```csharp
public static FailOr<T> Fail<T>(Failure failure)
public static FailOr<T> FailOr<T>.Fail(Failure failure)
```

Intent:
Create a failed result with exactly one failure.

Example:

```csharp
var result = FailOr.Fail<int>(Failure.General("The input was invalid."));
```

### Create a failure from many `Failure` values

```csharp
public static FailOr<T> Fail<T>(IEnumerable<Failure> failures)
public static FailOr<T> Fail<T>(params Failure[] failures)
public static FailOr<T> FailOr<T>.Fail(IEnumerable<Failure> failures)
public static FailOr<T> FailOr<T>.Fail(params Failure[] failures)
```

Intent:
Create a failed result with one-or-more failures.

Example:

```csharp
var result = FailOr.Fail<int>(
    Failure.General("Email is required."),
    Failure.General("Password is required."));
```

### Create a general-purpose failure

```csharp
public static Failure General(string details)
```

Intent:
Create a simple `Failure` with the code `"General"`.

Example:

```csharp
var failure = Failure.General("Something went wrong.");
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
public IReadOnlyList<Failure> Failures { get; }
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

## Recovering From Failures

These APIs preserve an existing success. They only apply the fallback when the source is failed.

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
public FailOr<TSource> IfFailThen(Func<IReadOnlyList<Failure>, FailOr<TSource>> alternative)
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
    Func<IReadOnlyList<Failure>, Task<FailOr<TSource>>> alternativeAsync)
```

Intent:
Recover from failures using asynchronous fallbacks.

## Matching And Producing Final Values

Use `Match` when you want one expression that handles both success and failure.

### Match with all failures

```csharp
public TResult Match<TResult>(
    Func<TSource, TResult> success,
    Func<IReadOnlyList<Failure>, TResult> failure)
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
    Func<IReadOnlyList<Failure>, TResult> failure)

public Task<TResult> MatchAsync<TResult>(
    Func<TSource, TResult> success,
    Func<IReadOnlyList<Failure>, Task<TResult>> failureAsync)

public Task<TResult> MatchAsync<TResult>(
    Func<TSource, Task<TResult>> successAsync,
    Func<IReadOnlyList<Failure>, Task<TResult>> failureAsync)
```

Intent:
Use asynchronous projections on either or both branches.

### Match using only the first failure

```csharp
public TResult MatchFirst<TResult>(
    Func<TSource, TResult> success,
    Func<Failure, TResult> failure)
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
    Func<Failure, TResult> failure)

public Task<TResult> MatchFirstAsync<TResult>(
    Func<TSource, TResult> success,
    Func<Failure, Task<TResult>> failureAsync)

public Task<TResult> MatchFirstAsync<TResult>(
    Func<TSource, Task<TResult>> successAsync,
    Func<Failure, Task<TResult>> failureAsync)
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

The library also provides the same `Then`, `ThenAsync`, `IfFailThen`, `IfFailThenAsync`, `Match`, `MatchAsync`, `MatchFirst`, and `MatchFirstAsync` APIs for:

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

## Validation Rules And Exceptions

The current API validates a few important invalid states:

- `Success` throws `ArgumentNullException` for `null` reference-type values.
- `Fail` throws `ArgumentNullException` when a failure sequence is `null`.
- `Fail` throws `ArgumentException` when a failure sequence is empty.
- `Failure.General` throws for `null`, empty, or whitespace details.
- `UnsafeUnwrap` throws `InvalidOperationException` when the result is failed.
- Async delegate-based APIs throw `ArgumentNullException` when the delegate itself is `null`.
- Async delegate-based APIs also throw `ArgumentNullException` when the selected delegate returns a `null` task.

## Choosing The Right API

- Use `Success` and `Fail` to create results.
- Use `Then` and `ThenAsync` to continue only on success.
- Use `IfFailThen` and `IfFailThenAsync` to recover from failures.
- Use `Match` when you want a final value from both branches.
- Use `MatchFirst` when only the first failure matters.
- Use `Combine` for preferred-source fallback.
- Use `Zip` to aggregate several independent results.

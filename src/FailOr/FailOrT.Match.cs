namespace FailOr;

/// <summary>
/// Provides matching extensions for <see cref="FailOr{T}"/> values.
/// </summary>
public static class FailOrMatchExtensions
{
    extension<TSource>(FailOr<TSource> source)
    {
        /// <summary>
        /// Matches a result by projecting either the successful value or all collected failures.
        /// </summary>
        /// <param name="success">The projection to apply when the result is successful.</param>
        /// <param name="failure">The projection to apply when the result contains one or more failures.</param>
        /// <returns>The value produced by the selected projection.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="success"/> or <paramref name="failure"/> is <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var message = result.Match(
        ///     success: value => $"Value: {value}",
        ///     failure: failures => failures[0].Details);
        /// </code>
        /// </example>
        public TResult Match<TResult>(
            Func<TSource, TResult> success,
            Func<IReadOnlyList<Failures>, TResult> failure
        )
        {
            ArgumentNullException.ThrowIfNull(success);
            ArgumentNullException.ThrowIfNull(failure);

            return source.IsFailure ? failure(source.Failures) : success(source.UnsafeUnwrap());
        }

        /// <summary>
        /// Asynchronously matches a result using an asynchronous success projection and a synchronous failure projection.
        /// </summary>
        /// <param name="successAsync">The asynchronous projection to apply when the result is successful.</param>
        /// <param name="failure">The projection to apply when the result contains one or more failures.</param>
        /// <returns>A task producing the value from the selected projection.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="successAsync"/> or <paramref name="failure"/> is <see langword="null"/>,
        /// or when <paramref name="successAsync"/> returns <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var message = await result.MatchAsync(
        ///     successAsync: value => Task.FromResult($"Value: {value}"),
        ///     failure: failures => failures[0].Details);
        /// </code>
        /// </example>
        public Task<TResult> MatchAsync<TResult>(
            Func<TSource, Task<TResult>> successAsync,
            Func<IReadOnlyList<Failures>, TResult> failure
        )
        {
            ArgumentNullException.ThrowIfNull(successAsync);
            ArgumentNullException.ThrowIfNull(failure);

            return source.IsFailure
                ? Task.FromResult(failure(source.Failures))
                : MatchSuccessAsync(source.UnsafeUnwrap(), successAsync);
        }

        /// <summary>
        /// Asynchronously matches a result using a synchronous success projection and an asynchronous failure projection.
        /// </summary>
        /// <param name="success">The projection to apply when the result is successful.</param>
        /// <param name="failureAsync">The asynchronous projection to apply when the result contains one or more failures.</param>
        /// <returns>A task producing the value from the selected projection.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="success"/> or <paramref name="failureAsync"/> is <see langword="null"/>,
        /// or when <paramref name="failureAsync"/> returns <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var message = await result.MatchAsync(
        ///     success: value => $"Value: {value}",
        ///     failureAsync: failures => Task.FromResult(failures[0].Details));
        /// </code>
        /// </example>
        public Task<TResult> MatchAsync<TResult>(
            Func<TSource, TResult> success,
            Func<IReadOnlyList<Failures>, Task<TResult>> failureAsync
        )
        {
            ArgumentNullException.ThrowIfNull(success);
            ArgumentNullException.ThrowIfNull(failureAsync);

            return source.IsFailure
                ? MatchFailureAsync(source.Failures, failureAsync)
                : Task.FromResult(success(source.UnsafeUnwrap()));
        }

        /// <summary>
        /// Asynchronously matches a result using asynchronous projections for both success and failure.
        /// </summary>
        /// <param name="successAsync">The asynchronous projection to apply when the result is successful.</param>
        /// <param name="failureAsync">The asynchronous projection to apply when the result contains one or more failures.</param>
        /// <returns>A task producing the value from the selected projection.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="successAsync"/> or <paramref name="failureAsync"/> is <see langword="null"/>,
        /// or when the selected delegate returns <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var message = await result.MatchAsync(
        ///     successAsync: value => Task.FromResult($"Value: {value}"),
        ///     failureAsync: failures => Task.FromResult(failures[0].Details));
        /// </code>
        /// </example>
        public Task<TResult> MatchAsync<TResult>(
            Func<TSource, Task<TResult>> successAsync,
            Func<IReadOnlyList<Failures>, Task<TResult>> failureAsync
        )
        {
            ArgumentNullException.ThrowIfNull(successAsync);
            ArgumentNullException.ThrowIfNull(failureAsync);

            return source.IsFailure
                ? MatchFailureAsync(source.Failures, failureAsync)
                : MatchSuccessAsync(source.UnsafeUnwrap(), successAsync);
        }

        /// <summary>
        /// Matches a result by projecting either the successful value or the first collected failure.
        /// </summary>
        /// <param name="success">The projection to apply when the result is successful.</param>
        /// <param name="failure">The projection to apply when the result contains one or more failures.</param>
        /// <returns>The value produced by the selected projection.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="success"/> or <paramref name="failure"/> is <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var message = result.MatchFirst(
        ///     success: value => $"Value: {value}",
        ///     failure: firstFailure => firstFailure.Details);
        /// </code>
        /// </example>
        public TResult MatchFirst<TResult>(
            Func<TSource, TResult> success,
            Func<Failures, TResult> failure
        )
        {
            ArgumentNullException.ThrowIfNull(success);
            ArgumentNullException.ThrowIfNull(failure);

            return source.IsFailure ? failure(source.Failures[0]) : success(source.UnsafeUnwrap());
        }

        /// <summary>
        /// Asynchronously matches a result using an asynchronous success projection and a synchronous first-failure projection.
        /// </summary>
        /// <param name="successAsync">The asynchronous projection to apply when the result is successful.</param>
        /// <param name="failure">The projection to apply when the result contains one or more failures.</param>
        /// <returns>A task producing the value from the selected projection.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="successAsync"/> or <paramref name="failure"/> is <see langword="null"/>,
        /// or when <paramref name="successAsync"/> returns <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var message = await result.MatchFirstAsync(
        ///     successAsync: value => Task.FromResult($"Value: {value}"),
        ///     failure: firstFailure => firstFailure.Details);
        /// </code>
        /// </example>
        public Task<TResult> MatchFirstAsync<TResult>(
            Func<TSource, Task<TResult>> successAsync,
            Func<Failures, TResult> failure
        )
        {
            ArgumentNullException.ThrowIfNull(successAsync);
            ArgumentNullException.ThrowIfNull(failure);

            return source.IsFailure
                ? Task.FromResult(failure(source.Failures[0]))
                : MatchSuccessAsync(source.UnsafeUnwrap(), successAsync);
        }

        /// <summary>
        /// Asynchronously matches a result using a synchronous success projection and an asynchronous first-failure projection.
        /// </summary>
        /// <param name="success">The projection to apply when the result is successful.</param>
        /// <param name="failureAsync">The asynchronous projection to apply when the result contains one or more failures.</param>
        /// <returns>A task producing the value from the selected projection.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="success"/> or <paramref name="failureAsync"/> is <see langword="null"/>,
        /// or when <paramref name="failureAsync"/> returns <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var message = await result.MatchFirstAsync(
        ///     success: value => $"Value: {value}",
        ///     failureAsync: firstFailure => Task.FromResult(firstFailure.Details));
        /// </code>
        /// </example>
        public Task<TResult> MatchFirstAsync<TResult>(
            Func<TSource, TResult> success,
            Func<Failures, Task<TResult>> failureAsync
        )
        {
            ArgumentNullException.ThrowIfNull(success);
            ArgumentNullException.ThrowIfNull(failureAsync);

            return source.IsFailure
                ? MatchFailureAsync(source.Failures[0], failureAsync)
                : Task.FromResult(success(source.UnsafeUnwrap()));
        }

        /// <summary>
        /// Asynchronously matches a result using asynchronous projections for both success and first failure.
        /// </summary>
        /// <param name="successAsync">The asynchronous projection to apply when the result is successful.</param>
        /// <param name="failureAsync">The asynchronous projection to apply when the result contains one or more failures.</param>
        /// <returns>A task producing the value from the selected projection.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="successAsync"/> or <paramref name="failureAsync"/> is <see langword="null"/>,
        /// or when the selected delegate returns <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var message = await result.MatchFirstAsync(
        ///     successAsync: value => Task.FromResult($"Value: {value}"),
        ///     failureAsync: firstFailure => Task.FromResult(firstFailure.Details));
        /// </code>
        /// </example>
        public Task<TResult> MatchFirstAsync<TResult>(
            Func<TSource, Task<TResult>> successAsync,
            Func<Failures, Task<TResult>> failureAsync
        )
        {
            ArgumentNullException.ThrowIfNull(successAsync);
            ArgumentNullException.ThrowIfNull(failureAsync);

            return source.IsFailure
                ? MatchFailureAsync(source.Failures[0], failureAsync)
                : MatchSuccessAsync(source.UnsafeUnwrap(), successAsync);
        }
    }

    extension<TSource>(Task<FailOr<TSource>> sourceTask)
    {
        /// <summary>
        /// Matches a task-wrapped result by projecting either the successful value or all collected failures.
        /// </summary>
        /// <param name="success">The projection to apply when the awaited result is successful.</param>
        /// <param name="failure">The projection to apply when the awaited result contains one or more failures.</param>
        /// <returns>A task producing the value from the selected projection.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the awaited source task, <paramref name="success"/>, or <paramref name="failure"/> is <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var message = await resultTask.Match(
        ///     success: value => $"Value: {value}",
        ///     failure: failures => failures[0].Details);
        /// </code>
        /// </example>
        public Task<TResult> Match<TResult>(
            Func<TSource, TResult> success,
            Func<IReadOnlyList<Failures>, TResult> failure
        )
        {
            ArgumentNullException.ThrowIfNull(sourceTask);
            ArgumentNullException.ThrowIfNull(success);
            ArgumentNullException.ThrowIfNull(failure);

            return MatchCore(sourceTask, source => Task.FromResult(source.Match(success, failure)));
        }

        /// <summary>
        /// Asynchronously matches a task-wrapped result using an asynchronous success projection and a synchronous failure projection.
        /// </summary>
        /// <param name="successAsync">The asynchronous projection to apply when the awaited result is successful.</param>
        /// <param name="failure">The projection to apply when the awaited result contains one or more failures.</param>
        /// <returns>A task producing the value from the selected projection.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the awaited source task, <paramref name="successAsync"/>, or <paramref name="failure"/> is <see langword="null"/>,
        /// or when <paramref name="successAsync"/> returns <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var message = await resultTask.MatchAsync(
        ///     successAsync: value => Task.FromResult($"Value: {value}"),
        ///     failure: failures => failures[0].Details);
        /// </code>
        /// </example>
        public Task<TResult> MatchAsync<TResult>(
            Func<TSource, Task<TResult>> successAsync,
            Func<IReadOnlyList<Failures>, TResult> failure
        )
        {
            ArgumentNullException.ThrowIfNull(sourceTask);
            ArgumentNullException.ThrowIfNull(successAsync);
            ArgumentNullException.ThrowIfNull(failure);

            return MatchCore(sourceTask, source => source.MatchAsync(successAsync, failure));
        }

        /// <summary>
        /// Asynchronously matches a task-wrapped result using a synchronous success projection and an asynchronous failure projection.
        /// </summary>
        /// <param name="success">The projection to apply when the awaited result is successful.</param>
        /// <param name="failureAsync">The asynchronous projection to apply when the awaited result contains one or more failures.</param>
        /// <returns>A task producing the value from the selected projection.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the awaited source task, <paramref name="success"/>, or <paramref name="failureAsync"/> is <see langword="null"/>,
        /// or when <paramref name="failureAsync"/> returns <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var message = await resultTask.MatchAsync(
        ///     success: value => $"Value: {value}",
        ///     failureAsync: failures => Task.FromResult(failures[0].Details));
        /// </code>
        /// </example>
        public Task<TResult> MatchAsync<TResult>(
            Func<TSource, TResult> success,
            Func<IReadOnlyList<Failures>, Task<TResult>> failureAsync
        )
        {
            ArgumentNullException.ThrowIfNull(sourceTask);
            ArgumentNullException.ThrowIfNull(success);
            ArgumentNullException.ThrowIfNull(failureAsync);

            return MatchCore(sourceTask, source => source.MatchAsync(success, failureAsync));
        }

        /// <summary>
        /// Asynchronously matches a task-wrapped result using asynchronous projections for both success and failure.
        /// </summary>
        /// <param name="successAsync">The asynchronous projection to apply when the awaited result is successful.</param>
        /// <param name="failureAsync">The asynchronous projection to apply when the awaited result contains one or more failures.</param>
        /// <returns>A task producing the value from the selected projection.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the awaited source task, <paramref name="successAsync"/>, or <paramref name="failureAsync"/> is <see langword="null"/>,
        /// or when the selected delegate returns <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var message = await resultTask.MatchAsync(
        ///     successAsync: value => Task.FromResult($"Value: {value}"),
        ///     failureAsync: failures => Task.FromResult(failures[0].Details));
        /// </code>
        /// </example>
        public Task<TResult> MatchAsync<TResult>(
            Func<TSource, Task<TResult>> successAsync,
            Func<IReadOnlyList<Failures>, Task<TResult>> failureAsync
        )
        {
            ArgumentNullException.ThrowIfNull(sourceTask);
            ArgumentNullException.ThrowIfNull(successAsync);
            ArgumentNullException.ThrowIfNull(failureAsync);

            return MatchCore(sourceTask, source => source.MatchAsync(successAsync, failureAsync));
        }

        /// <summary>
        /// Matches a task-wrapped result by projecting either the successful value or the first collected failure.
        /// </summary>
        /// <param name="success">The projection to apply when the awaited result is successful.</param>
        /// <param name="failure">The projection to apply when the awaited result contains one or more failures.</param>
        /// <returns>A task producing the value from the selected projection.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the awaited source task, <paramref name="success"/>, or <paramref name="failure"/> is <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var message = await resultTask.MatchFirst(
        ///     success: value => $"Value: {value}",
        ///     failure: firstFailure => firstFailure.Details);
        /// </code>
        /// </example>
        public Task<TResult> MatchFirst<TResult>(
            Func<TSource, TResult> success,
            Func<Failures, TResult> failure
        )
        {
            ArgumentNullException.ThrowIfNull(sourceTask);
            ArgumentNullException.ThrowIfNull(success);
            ArgumentNullException.ThrowIfNull(failure);

            return MatchCore(
                sourceTask,
                source => Task.FromResult(source.MatchFirst(success, failure))
            );
        }

        /// <summary>
        /// Asynchronously matches a task-wrapped result using an asynchronous success projection and a synchronous first-failure projection.
        /// </summary>
        /// <param name="successAsync">The asynchronous projection to apply when the awaited result is successful.</param>
        /// <param name="failure">The projection to apply when the awaited result contains one or more failures.</param>
        /// <returns>A task producing the value from the selected projection.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the awaited source task, <paramref name="successAsync"/>, or <paramref name="failure"/> is <see langword="null"/>,
        /// or when <paramref name="successAsync"/> returns <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var message = await resultTask.MatchFirstAsync(
        ///     successAsync: value => Task.FromResult($"Value: {value}"),
        ///     failure: firstFailure => firstFailure.Details);
        /// </code>
        /// </example>
        public Task<TResult> MatchFirstAsync<TResult>(
            Func<TSource, Task<TResult>> successAsync,
            Func<Failures, TResult> failure
        )
        {
            ArgumentNullException.ThrowIfNull(sourceTask);
            ArgumentNullException.ThrowIfNull(successAsync);
            ArgumentNullException.ThrowIfNull(failure);

            return MatchCore(sourceTask, source => source.MatchFirstAsync(successAsync, failure));
        }

        /// <summary>
        /// Asynchronously matches a task-wrapped result using a synchronous success projection and an asynchronous first-failure projection.
        /// </summary>
        /// <param name="success">The projection to apply when the awaited result is successful.</param>
        /// <param name="failureAsync">The asynchronous projection to apply when the awaited result contains one or more failures.</param>
        /// <returns>A task producing the value from the selected projection.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the awaited source task, <paramref name="success"/>, or <paramref name="failureAsync"/> is <see langword="null"/>,
        /// or when <paramref name="failureAsync"/> returns <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var message = await resultTask.MatchFirstAsync(
        ///     success: value => $"Value: {value}",
        ///     failureAsync: firstFailure => Task.FromResult(firstFailure.Details));
        /// </code>
        /// </example>
        public Task<TResult> MatchFirstAsync<TResult>(
            Func<TSource, TResult> success,
            Func<Failures, Task<TResult>> failureAsync
        )
        {
            ArgumentNullException.ThrowIfNull(sourceTask);
            ArgumentNullException.ThrowIfNull(success);
            ArgumentNullException.ThrowIfNull(failureAsync);

            return MatchCore(sourceTask, source => source.MatchFirstAsync(success, failureAsync));
        }

        /// <summary>
        /// Asynchronously matches a task-wrapped result using asynchronous projections for both success and first failure.
        /// </summary>
        /// <param name="successAsync">The asynchronous projection to apply when the awaited result is successful.</param>
        /// <param name="failureAsync">The asynchronous projection to apply when the awaited result contains one or more failures.</param>
        /// <returns>A task producing the value from the selected projection.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the awaited source task, <paramref name="successAsync"/>, or <paramref name="failureAsync"/> is <see langword="null"/>,
        /// or when the selected delegate returns <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var message = await resultTask.MatchFirstAsync(
        ///     successAsync: value => Task.FromResult($"Value: {value}"),
        ///     failureAsync: firstFailure => Task.FromResult(firstFailure.Details));
        /// </code>
        /// </example>
        public Task<TResult> MatchFirstAsync<TResult>(
            Func<TSource, Task<TResult>> successAsync,
            Func<Failures, Task<TResult>> failureAsync
        )
        {
            ArgumentNullException.ThrowIfNull(sourceTask);
            ArgumentNullException.ThrowIfNull(successAsync);
            ArgumentNullException.ThrowIfNull(failureAsync);

            return MatchCore(
                sourceTask,
                source => source.MatchFirstAsync(successAsync, failureAsync)
            );
        }
    }

    private static Task<TResult> MatchSuccessAsync<TSource, TResult>(
        TSource value,
        Func<TSource, Task<TResult>> successAsync
    )
    {
        var resultTask = successAsync(value);
        ArgumentNullException.ThrowIfNull(resultTask);

        return resultTask;
    }

    private static Task<TResult> MatchFailureAsync<TResult>(
        IReadOnlyList<Failures> failures,
        Func<IReadOnlyList<Failures>, Task<TResult>> failureAsync
    )
    {
        var resultTask = failureAsync(failures);
        ArgumentNullException.ThrowIfNull(resultTask);

        return resultTask;
    }

    private static Task<TResult> MatchFailureAsync<TResult>(
        Failures failure,
        Func<Failures, Task<TResult>> failureAsync
    )
    {
        var resultTask = failureAsync(failure);
        ArgumentNullException.ThrowIfNull(resultTask);

        return resultTask;
    }

    private static async Task<TResult> MatchCore<TSource, TResult>(
        Task<FailOr<TSource>> sourceTask,
        Func<FailOr<TSource>, Task<TResult>> match
    )
    {
        var source = await sourceTask.ConfigureAwait(false);
        return await match(source).ConfigureAwait(false);
    }
}

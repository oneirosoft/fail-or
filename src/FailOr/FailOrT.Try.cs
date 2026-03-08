namespace FailOr;

/// <summary>
/// Provides exception-safe mapping extensions for <see cref="FailOr{T}"/> values.
/// </summary>
public static class FailOrTryExtensions
{
    extension<TSource>(FailOr<TSource> source)
    {
        /// <summary>
        /// Maps a successful value to a new successful result and converts thrown exceptions to an exceptional failure.
        /// </summary>
        /// <param name="map">The projection to apply when the source is successful.</param>
        /// <returns>
        /// A successful result containing the mapped value, the original failures when the source is failed,
        /// or a failed result containing <see cref="Failure.Exceptional(Exception, string?, string?)"/> when
        /// the mapping throws.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="map"/> is <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var next = result.Try(x => x + 1);
        /// </code>
        /// </example>
        public FailOr<TResult> Try<TResult>(Func<TSource, TResult> map)
        {
            ArgumentNullException.ThrowIfNull(map);

            return source.IsFailure
                ? Fail<TSource, TResult>(source)
                : TryCore(source.UnsafeUnwrap(), map, Exceptional<TResult>);
        }

        /// <summary>
        /// Maps a successful value to a new result and converts thrown exceptions with a custom handler.
        /// </summary>
        /// <param name="map">The projection to apply when the source is successful.</param>
        /// <param name="onException">The handler that converts a thrown exception into a result.</param>
        /// <returns>
        /// A successful result containing the mapped value, the original failures when the source is failed,
        /// or the result produced by <paramref name="onException"/> when the mapping throws.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="map"/> or <paramref name="onException"/> is <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var next = result.Try(
        ///     x => x + 1,
        ///     exception => Failure.General("Mapping failed."));
        /// </code>
        /// </example>
        public FailOr<TResult> Try<TResult>(
            Func<TSource, TResult> map,
            Func<Exception, FailOr<TResult>> onException
        )
        {
            ArgumentNullException.ThrowIfNull(map);
            ArgumentNullException.ThrowIfNull(onException);

            return source.IsFailure
                ? Fail<TSource, TResult>(source)
                : TryCore(source.UnsafeUnwrap(), map, onException);
        }

        /// <summary>
        /// Asynchronously maps a successful value to a new result and converts thrown exceptions to an exceptional failure.
        /// </summary>
        /// <param name="mapAsync">The asynchronous projection to apply when the source is successful.</param>
        /// <returns>
        /// A task producing a successful result containing the mapped value, the original failures when the
        /// source is failed, or a failed result containing
        /// <see cref="Failure.Exceptional(Exception, string?, string?)"/> when the mapping throws.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="mapAsync"/> is <see langword="null"/> or returns <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var next = await result.TryAsync(x => GetValueAsync(x));
        /// </code>
        /// </example>
        public Task<FailOr<TResult>> TryAsync<TResult>(Func<TSource, Task<TResult>> mapAsync)
        {
            ArgumentNullException.ThrowIfNull(mapAsync);

            return source.IsFailure
                ? Task.FromResult(Fail<TSource, TResult>(source))
                : TryMapAsync(source.UnsafeUnwrap(), mapAsync, Exceptional<TResult>);
        }

        /// <summary>
        /// Asynchronously maps a successful value to a new result and converts thrown exceptions with a custom handler.
        /// </summary>
        /// <param name="mapAsync">The asynchronous projection to apply when the source is successful.</param>
        /// <param name="onException">The handler that converts a thrown exception into a result.</param>
        /// <returns>
        /// A task producing a successful result containing the mapped value, the original failures when the
        /// source is failed, or the result produced by <paramref name="onException"/> when the mapping throws.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="mapAsync"/> or <paramref name="onException"/> is <see langword="null"/>,
        /// or when <paramref name="mapAsync"/> returns <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var next = await result.TryAsync(
        ///     x => GetValueAsync(x),
        ///     exception => Failure.General("Mapping failed."));
        /// </code>
        /// </example>
        public Task<FailOr<TResult>> TryAsync<TResult>(
            Func<TSource, Task<TResult>> mapAsync,
            Func<Exception, FailOr<TResult>> onException
        )
        {
            ArgumentNullException.ThrowIfNull(mapAsync);
            ArgumentNullException.ThrowIfNull(onException);

            return source.IsFailure
                ? Task.FromResult(Fail<TSource, TResult>(source))
                : TryMapAsync(source.UnsafeUnwrap(), mapAsync, onException);
        }
    }

    extension<TSource>(Task<FailOr<TSource>> sourceTask)
    {
        /// <summary>
        /// Maps the successful value of a task-wrapped result and converts thrown exceptions to an exceptional failure.
        /// </summary>
        /// <param name="map">The projection to apply when the awaited source is successful.</param>
        /// <returns>
        /// A task producing a successful result containing the mapped value, the original failures when the
        /// awaited source is failed, or a failed result containing
        /// <see cref="Failure.Exceptional(Exception, string?, string?)"/> when the mapping throws.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the awaited source task or <paramref name="map"/> is <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var next = await resultTask.Try(x => x + 1);
        /// </code>
        /// </example>
        public Task<FailOr<TResult>> Try<TResult>(Func<TSource, TResult> map)
        {
            ArgumentNullException.ThrowIfNull(sourceTask);
            ArgumentNullException.ThrowIfNull(map);

            return TryCore(sourceTask, source => Task.FromResult(source.Try(map)));
        }

        /// <summary>
        /// Maps the successful value of a task-wrapped result and converts thrown exceptions with a custom handler.
        /// </summary>
        /// <param name="map">The projection to apply when the awaited source is successful.</param>
        /// <param name="onException">The handler that converts a thrown exception into a result.</param>
        /// <returns>
        /// A task producing a successful result containing the mapped value, the original failures when the
        /// awaited source is failed, or the result produced by <paramref name="onException"/> when the mapping throws.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the awaited source task, <paramref name="map"/>, or <paramref name="onException"/> is <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var next = await resultTask.Try(
        ///     x => x + 1,
        ///     exception => Failure.General("Mapping failed."));
        /// </code>
        /// </example>
        public Task<FailOr<TResult>> Try<TResult>(
            Func<TSource, TResult> map,
            Func<Exception, FailOr<TResult>> onException
        )
        {
            ArgumentNullException.ThrowIfNull(sourceTask);
            ArgumentNullException.ThrowIfNull(map);
            ArgumentNullException.ThrowIfNull(onException);

            return TryCore(sourceTask, source => Task.FromResult(source.Try(map, onException)));
        }

        /// <summary>
        /// Asynchronously maps the successful value of a task-wrapped result and converts thrown exceptions to an exceptional failure.
        /// </summary>
        /// <param name="mapAsync">The asynchronous projection to apply when the awaited source is successful.</param>
        /// <returns>
        /// A task producing a successful result containing the mapped value, the original failures when the
        /// awaited source is failed, or a failed result containing
        /// <see cref="Failure.Exceptional(Exception, string?, string?)"/> when the mapping throws.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the awaited source task or <paramref name="mapAsync"/> is <see langword="null"/>,
        /// or when <paramref name="mapAsync"/> returns <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var next = await resultTask.TryAsync(x => GetValueAsync(x));
        /// </code>
        /// </example>
        public Task<FailOr<TResult>> TryAsync<TResult>(Func<TSource, Task<TResult>> mapAsync)
        {
            ArgumentNullException.ThrowIfNull(sourceTask);
            ArgumentNullException.ThrowIfNull(mapAsync);

            return TryCore(sourceTask, source => source.TryAsync(mapAsync));
        }

        /// <summary>
        /// Asynchronously maps the successful value of a task-wrapped result and converts thrown exceptions with a custom handler.
        /// </summary>
        /// <param name="mapAsync">The asynchronous projection to apply when the awaited source is successful.</param>
        /// <param name="onException">The handler that converts a thrown exception into a result.</param>
        /// <returns>
        /// A task producing a successful result containing the mapped value, the original failures when the
        /// awaited source is failed, or the result produced by <paramref name="onException"/> when the mapping throws.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the awaited source task, <paramref name="mapAsync"/>, or <paramref name="onException"/> is <see langword="null"/>,
        /// or when <paramref name="mapAsync"/> returns <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var next = await resultTask.TryAsync(
        ///     x => GetValueAsync(x),
        ///     exception => Failure.General("Mapping failed."));
        /// </code>
        /// </example>
        public Task<FailOr<TResult>> TryAsync<TResult>(
            Func<TSource, Task<TResult>> mapAsync,
            Func<Exception, FailOr<TResult>> onException
        )
        {
            ArgumentNullException.ThrowIfNull(sourceTask);
            ArgumentNullException.ThrowIfNull(mapAsync);
            ArgumentNullException.ThrowIfNull(onException);

            return TryCore(sourceTask, source => source.TryAsync(mapAsync, onException));
        }
    }

    private static FailOr<TResult> Fail<TSource, TResult>(FailOr<TSource> source) =>
        FailOr.Fail<TResult>(source.Failures);

    private static FailOr<TResult> Exceptional<TResult>(Exception exception) =>
        FailOr.Fail<TResult>(Failure.Exceptional(exception));

    private static FailOr<TResult> TryCore<TSource, TResult>(
        TSource value,
        Func<TSource, TResult> map,
        Func<Exception, FailOr<TResult>> onException
    )
    {
        try
        {
            return FailOr.Success(map(value));
        }
        catch (Exception exception)
        {
            return onException(exception);
        }
    }

    private static async Task<FailOr<TResult>> TryMapAsync<TSource, TResult>(
        TSource value,
        Func<TSource, Task<TResult>> mapAsync,
        Func<Exception, FailOr<TResult>> onException
    )
    {
        Task<TResult> resultTask;

        try
        {
            resultTask = mapAsync(value);
        }
        catch (Exception exception)
        {
            return onException(exception);
        }

        ArgumentNullException.ThrowIfNull(resultTask);

        try
        {
            return FailOr.Success(await resultTask.ConfigureAwait(false));
        }
        catch (Exception exception)
        {
            return onException(exception);
        }
    }

    private static async Task<FailOr<TResult>> TryCore<TSource, TResult>(
        Task<FailOr<TSource>> sourceTask,
        Func<FailOr<TSource>, Task<FailOr<TResult>>> then
    )
    {
        var source = await sourceTask.ConfigureAwait(false);
        return await then(source).ConfigureAwait(false);
    }
}

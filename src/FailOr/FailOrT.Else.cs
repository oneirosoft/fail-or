namespace FailOr;

/// <summary>
/// Provides terminal fallback extensions for <see cref="FailOr{T}"/> values.
/// </summary>
public static class FailOrElseExtensions
{
    extension<TSource>(FailOr<TSource> source)
    {
        /// <summary>
        /// Returns the successful value unchanged, or the provided alternative when the source is failed.
        /// </summary>
        /// <param name="alternative">The fallback value to return when the source is failed.</param>
        /// <returns>The wrapped success value, or <paramref name="alternative"/> when the source is failed.</returns>
        /// <example>
        /// <code>
        /// var value = result.Else(42);
        /// </code>
        /// </example>
        public TSource Else(TSource alternative) =>
            source.IsSuccess ? source.UnsafeUnwrap() : alternative;

        /// <summary>
        /// Returns the successful value unchanged, or invokes a fallback factory when the source is failed.
        /// </summary>
        /// <param name="alternative">The fallback factory to invoke when the source is failed.</param>
        /// <returns>The wrapped success value, or the value produced by <paramref name="alternative"/> when the source is failed.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="alternative"/> is <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var value = result.Else(() => 42);
        /// </code>
        /// </example>
        public TSource Else(Func<TSource> alternative)
        {
            ArgumentNullException.ThrowIfNull(alternative);

            return source.IsSuccess ? source.UnsafeUnwrap() : alternative();
        }

        /// <summary>
        /// Returns the successful value unchanged, or invokes a fallback factory with the source failures when the source is failed.
        /// </summary>
        /// <param name="alternative">The fallback factory to invoke with the source failures.</param>
        /// <returns>The wrapped success value, or the value produced by <paramref name="alternative"/> when the source is failed.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="alternative"/> is <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var value = result.Else(failures => failures.Count);
        /// </code>
        /// </example>
        public TSource Else(Func<IReadOnlyList<Failures>, TSource> alternative)
        {
            ArgumentNullException.ThrowIfNull(alternative);

            return source.IsSuccess ? source.UnsafeUnwrap() : alternative(source.Failures);
        }

        /// <summary>
        /// Returns the successful value unchanged, or asynchronously invokes a fallback factory when the source is failed.
        /// </summary>
        /// <param name="alternativeAsync">The asynchronous fallback factory to invoke when the source is failed.</param>
        /// <returns>
        /// A task producing the wrapped success value, or the value produced by <paramref name="alternativeAsync"/> when the source is failed.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="alternativeAsync"/> is <see langword="null"/> or returns <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var value = await result.ElseAsync(() => Task.FromResult(42));
        /// </code>
        /// </example>
        public Task<TSource> ElseAsync(Func<Task<TSource>> alternativeAsync)
        {
            ArgumentNullException.ThrowIfNull(alternativeAsync);

            return source.IsSuccess
                ? Task.FromResult(source.UnsafeUnwrap())
                : ElseAsyncCore(alternativeAsync);
        }

        /// <summary>
        /// Returns the successful value unchanged, or asynchronously invokes a fallback factory with the source failures when the source is failed.
        /// </summary>
        /// <param name="alternativeAsync">The asynchronous fallback factory to invoke with the source failures.</param>
        /// <returns>
        /// A task producing the wrapped success value, or the value produced by <paramref name="alternativeAsync"/> when the source is failed.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="alternativeAsync"/> is <see langword="null"/> or returns <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var value = await result.ElseAsync(failures => Task.FromResult(failures.Count));
        /// </code>
        /// </example>
        public Task<TSource> ElseAsync(
            Func<IReadOnlyList<Failures>, Task<TSource>> alternativeAsync
        )
        {
            ArgumentNullException.ThrowIfNull(alternativeAsync);

            return source.IsSuccess
                ? Task.FromResult(source.UnsafeUnwrap())
                : ElseAsyncCore(source.Failures, alternativeAsync);
        }
    }

    extension<TSource>(Task<FailOr<TSource>> sourceTask)
    {
        /// <summary>
        /// Returns the awaited successful value unchanged, or the provided alternative when the awaited source is failed.
        /// </summary>
        /// <param name="alternative">The fallback value to return when the awaited source is failed.</param>
        /// <returns>
        /// A task producing the wrapped success value, or <paramref name="alternative"/> when the awaited source is failed.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the awaited source task is <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var value = await resultTask.Else(42);
        /// </code>
        /// </example>
        public Task<TSource> Else(TSource alternative)
        {
            ArgumentNullException.ThrowIfNull(sourceTask);

            return ElseCore(sourceTask, source => Task.FromResult(source.Else(alternative)));
        }

        /// <summary>
        /// Returns the awaited successful value unchanged, or invokes a fallback factory when the awaited source is failed.
        /// </summary>
        /// <param name="alternative">The fallback factory to invoke when the awaited source is failed.</param>
        /// <returns>
        /// A task producing the wrapped success value, or the value produced by <paramref name="alternative"/> when the awaited source is failed.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the awaited source task or <paramref name="alternative"/> is <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var value = await resultTask.Else(() => 42);
        /// </code>
        /// </example>
        public Task<TSource> Else(Func<TSource> alternative)
        {
            ArgumentNullException.ThrowIfNull(sourceTask);
            ArgumentNullException.ThrowIfNull(alternative);

            return ElseCore(sourceTask, source => Task.FromResult(source.Else(alternative)));
        }

        /// <summary>
        /// Returns the awaited successful value unchanged, or invokes a fallback factory with the awaited source failures when the awaited source is failed.
        /// </summary>
        /// <param name="alternative">The fallback factory to invoke with the awaited source failures.</param>
        /// <returns>
        /// A task producing the wrapped success value, or the value produced by <paramref name="alternative"/> when the awaited source is failed.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the awaited source task or <paramref name="alternative"/> is <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var value = await resultTask.Else(failures => failures.Count);
        /// </code>
        /// </example>
        public Task<TSource> Else(Func<IReadOnlyList<Failures>, TSource> alternative)
        {
            ArgumentNullException.ThrowIfNull(sourceTask);
            ArgumentNullException.ThrowIfNull(alternative);

            return ElseCore(sourceTask, source => Task.FromResult(source.Else(alternative)));
        }

        /// <summary>
        /// Returns the awaited successful value unchanged, or asynchronously invokes a fallback factory when the awaited source is failed.
        /// </summary>
        /// <param name="alternativeAsync">The asynchronous fallback factory to invoke when the awaited source is failed.</param>
        /// <returns>
        /// A task producing the wrapped success value, or the value produced by <paramref name="alternativeAsync"/> when the awaited source is failed.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the awaited source task or <paramref name="alternativeAsync"/> is <see langword="null"/>,
        /// or when <paramref name="alternativeAsync"/> returns <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var value = await resultTask.ElseAsync(() => Task.FromResult(42));
        /// </code>
        /// </example>
        public Task<TSource> ElseAsync(Func<Task<TSource>> alternativeAsync)
        {
            ArgumentNullException.ThrowIfNull(sourceTask);
            ArgumentNullException.ThrowIfNull(alternativeAsync);

            return ElseCore(sourceTask, source => source.ElseAsync(alternativeAsync));
        }

        /// <summary>
        /// Returns the awaited successful value unchanged, or asynchronously invokes a fallback factory with the awaited source failures when the awaited source is failed.
        /// </summary>
        /// <param name="alternativeAsync">The asynchronous fallback factory to invoke with the awaited source failures.</param>
        /// <returns>
        /// A task producing the wrapped success value, or the value produced by <paramref name="alternativeAsync"/> when the awaited source is failed.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the awaited source task or <paramref name="alternativeAsync"/> is <see langword="null"/>,
        /// or when <paramref name="alternativeAsync"/> returns <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var value = await resultTask.ElseAsync(failures => Task.FromResult(failures.Count));
        /// </code>
        /// </example>
        public Task<TSource> ElseAsync(
            Func<IReadOnlyList<Failures>, Task<TSource>> alternativeAsync
        )
        {
            ArgumentNullException.ThrowIfNull(sourceTask);
            ArgumentNullException.ThrowIfNull(alternativeAsync);

            return ElseCore(sourceTask, source => source.ElseAsync(alternativeAsync));
        }
    }

    private static async Task<TSource> ElseAsyncCore<TSource>(Func<Task<TSource>> alternativeAsync)
    {
        var resultTask = alternativeAsync();
        ArgumentNullException.ThrowIfNull(resultTask);

        return await resultTask.ConfigureAwait(false);
    }

    private static async Task<TSource> ElseAsyncCore<TSource>(
        IReadOnlyList<Failures> failures,
        Func<IReadOnlyList<Failures>, Task<TSource>> alternativeAsync
    )
    {
        var resultTask = alternativeAsync(failures);
        ArgumentNullException.ThrowIfNull(resultTask);

        return await resultTask.ConfigureAwait(false);
    }

    private static async Task<TResult> ElseCore<TSource, TResult>(
        Task<FailOr<TSource>> sourceTask,
        Func<FailOr<TSource>, Task<TResult>> terminate
    )
    {
        var source = await sourceTask.ConfigureAwait(false);
        return await terminate(source).ConfigureAwait(false);
    }
}

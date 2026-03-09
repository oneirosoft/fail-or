namespace FailOr;

/// <summary>
/// Provides predicate-based failure extensions for <see cref="FailOr{T}"/> values.
/// </summary>
public static class FailOrFailWhenExtensions
{
    extension<TSource>(FailOr<TSource> source)
    {
        /// <summary>
        /// Fails a successful value when the supplied predicate matches.
        /// </summary>
        /// <param name="predicate">The predicate that decides whether the success should become a failure.</param>
        /// <param name="failure">The failure to return when <paramref name="predicate"/> evaluates to <see langword="true"/>.</param>
        /// <returns>
        /// The original success unchanged when the predicate evaluates to <see langword="false"/>, the supplied
        /// failure when it evaluates to <see langword="true"/>, or the original failures when the source is failed.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="predicate"/> or <paramref name="failure"/> is <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var next = result.FailWhen(x => x &lt; 0, Failure.General("Value must be non-negative."));
        /// </code>
        /// </example>
        public FailOr<TSource> FailWhen(Func<TSource, bool> predicate, Failures failure)
        {
            ArgumentNullException.ThrowIfNull(predicate);
            ArgumentNullException.ThrowIfNull(failure);

            return source.IsFailure
                ? source
                : FailWhenCore(source.UnsafeUnwrap(), predicate, failure);
        }

        /// <summary>
        /// Asynchronously fails a successful value when the supplied predicate matches.
        /// </summary>
        /// <param name="predicateAsync">The asynchronous predicate that decides whether the success should become a failure.</param>
        /// <param name="failure">The failure to return when <paramref name="predicateAsync"/> evaluates to <see langword="true"/>.</param>
        /// <returns>
        /// A task producing the original success unchanged when the predicate evaluates to <see langword="false"/>,
        /// the supplied failure when it evaluates to <see langword="true"/>, or the original failures when the source is failed.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="predicateAsync"/> or <paramref name="failure"/> is <see langword="null"/>,
        /// or when <paramref name="predicateAsync"/> returns <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var next = await result.FailWhenAsync(
        ///     async x =>
        ///     {
        ///         await Task.Delay(10);
        ///         return x &lt; 0;
        ///     },
        ///     Failure.General("Value must be non-negative."));
        /// </code>
        /// </example>
        public Task<FailOr<TSource>> FailWhenAsync(
            Func<TSource, Task<bool>> predicateAsync,
            Failures failure
        )
        {
            ArgumentNullException.ThrowIfNull(predicateAsync);
            ArgumentNullException.ThrowIfNull(failure);

            return source.IsFailure
                ? Task.FromResult(source)
                : FailWhenAsyncCore(source.UnsafeUnwrap(), predicateAsync, failure);
        }
    }

    extension<TSource>(Task<FailOr<TSource>> sourceTask)
    {
        /// <summary>
        /// Fails the successful value of a task-wrapped result when the supplied predicate matches.
        /// </summary>
        /// <param name="predicate">The predicate that decides whether the success should become a failure.</param>
        /// <param name="failure">The failure to return when <paramref name="predicate"/> evaluates to <see langword="true"/>.</param>
        /// <returns>
        /// A task producing the original success unchanged when the predicate evaluates to <see langword="false"/>,
        /// the supplied failure when it evaluates to <see langword="true"/>, or the original failures when the awaited source is failed.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the awaited source task, <paramref name="predicate"/>, or <paramref name="failure"/> is
        /// <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var next = await resultTask.FailWhen(
        ///     x => x &lt; 0,
        ///     Failure.General("Value must be non-negative."));
        /// </code>
        /// </example>
        public Task<FailOr<TSource>> FailWhen(Func<TSource, bool> predicate, Failures failure)
        {
            ArgumentNullException.ThrowIfNull(sourceTask);
            ArgumentNullException.ThrowIfNull(predicate);
            ArgumentNullException.ThrowIfNull(failure);

            return FailWhenCore(
                sourceTask,
                source => Task.FromResult(source.FailWhen(predicate, failure))
            );
        }

        /// <summary>
        /// Asynchronously fails the successful value of a task-wrapped result when the supplied predicate matches.
        /// </summary>
        /// <param name="predicateAsync">The asynchronous predicate that decides whether the success should become a failure.</param>
        /// <param name="failure">The failure to return when <paramref name="predicateAsync"/> evaluates to <see langword="true"/>.</param>
        /// <returns>
        /// A task producing the original success unchanged when the predicate evaluates to <see langword="false"/>,
        /// the supplied failure when it evaluates to <see langword="true"/>, or the original failures when the awaited source is failed.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the awaited source task, <paramref name="predicateAsync"/>, or <paramref name="failure"/> is
        /// <see langword="null"/>, or when <paramref name="predicateAsync"/> returns <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var next = await resultTask.FailWhenAsync(
        ///     async x =>
        ///     {
        ///         await Task.Delay(10);
        ///         return x &lt; 0;
        ///     },
        ///     Failure.General("Value must be non-negative."));
        /// </code>
        /// </example>
        public Task<FailOr<TSource>> FailWhenAsync(
            Func<TSource, Task<bool>> predicateAsync,
            Failures failure
        )
        {
            ArgumentNullException.ThrowIfNull(sourceTask);
            ArgumentNullException.ThrowIfNull(predicateAsync);
            ArgumentNullException.ThrowIfNull(failure);

            return FailWhenCore(
                sourceTask,
                source => source.FailWhenAsync(predicateAsync, failure)
            );
        }
    }

    private static FailOr<TSource> FailWhenCore<TSource>(
        TSource value,
        Func<TSource, bool> predicate,
        Failures failure
    ) => predicate(value) ? FailOr.Fail<TSource>(failure) : FailOr.Success(value);

    private static async Task<FailOr<TSource>> FailWhenAsyncCore<TSource>(
        TSource value,
        Func<TSource, Task<bool>> predicateAsync,
        Failures failure
    )
    {
        var resultTask = predicateAsync(value);
        ArgumentNullException.ThrowIfNull(resultTask);

        return await resultTask.ConfigureAwait(false)
            ? FailOr.Fail<TSource>(failure)
            : FailOr.Success(value);
    }

    private static async Task<FailOr<TSource>> FailWhenCore<TSource>(
        Task<FailOr<TSource>> sourceTask,
        Func<FailOr<TSource>, Task<FailOr<TSource>>> failWhen
    )
    {
        var source = await sourceTask.ConfigureAwait(false);
        return await failWhen(source).ConfigureAwait(false);
    }
}

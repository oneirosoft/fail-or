namespace FailOr;

/// <summary>
/// Provides terminal success-observation extensions for <see cref="FailOr{T}"/> values.
/// </summary>
public static class FailOrIfSuccessExtensions
{
    extension<TSource>(FailOr<TSource> source)
    {
        /// <summary>
        /// Runs a side effect for a successful value without preserving the result for continued chaining.
        /// </summary>
        /// <param name="action">The side effect to run when the source is successful.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="action"/> is <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// result.IfSuccess(x => Console.WriteLine(x));
        /// </code>
        /// </example>
        public void IfSuccess(Action<TSource> action)
        {
            ArgumentNullException.ThrowIfNull(action);

            if (source.IsFailure)
            {
                return;
            }

            action(source.UnsafeUnwrap());
        }

        /// <summary>
        /// Asynchronously runs a side effect for a successful value without preserving the result for continued chaining.
        /// </summary>
        /// <param name="actionAsync">The asynchronous side effect to run when the source is successful.</param>
        /// <returns>A task that completes after the side effect finishes, or immediately when the source is failed.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="actionAsync"/> is <see langword="null"/> or returns <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// await result.IfSuccessAsync(x => WriteAsync(x));
        /// </code>
        /// </example>
        public Task IfSuccessAsync(Func<TSource, Task> actionAsync)
        {
            ArgumentNullException.ThrowIfNull(actionAsync);

            return source.IsFailure
                ? Task.CompletedTask
                : IfSuccessAsyncCore(source.UnsafeUnwrap(), actionAsync);
        }
    }

    extension<TSource>(Task<FailOr<TSource>> sourceTask)
    {
        /// <summary>
        /// Runs a side effect for the successful value of a task-wrapped result without preserving the result for continued chaining.
        /// </summary>
        /// <param name="action">The side effect to run when the awaited source is successful.</param>
        /// <returns>A task that completes after the side effect finishes, or immediately when the awaited source is failed.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the awaited source task or <paramref name="action"/> is <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// await resultTask.IfSuccess(x => Console.WriteLine(x));
        /// </code>
        /// </example>
        public Task IfSuccess(Action<TSource> action)
        {
            ArgumentNullException.ThrowIfNull(sourceTask);
            ArgumentNullException.ThrowIfNull(action);

            return IfSuccessCore(
                sourceTask,
                source =>
                {
                    source.IfSuccess(action);
                    return Task.CompletedTask;
                }
            );
        }

        /// <summary>
        /// Asynchronously runs a side effect for the successful value of a task-wrapped result without preserving the result for continued chaining.
        /// </summary>
        /// <param name="actionAsync">The asynchronous side effect to run when the awaited source is successful.</param>
        /// <returns>A task that completes after the side effect finishes, or immediately when the awaited source is failed.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the awaited source task or <paramref name="actionAsync"/> is <see langword="null"/>,
        /// or when <paramref name="actionAsync"/> returns <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// await resultTask.IfSuccessAsync(x => WriteAsync(x));
        /// </code>
        /// </example>
        public Task IfSuccessAsync(Func<TSource, Task> actionAsync)
        {
            ArgumentNullException.ThrowIfNull(sourceTask);
            ArgumentNullException.ThrowIfNull(actionAsync);

            return IfSuccessCore(sourceTask, source => source.IfSuccessAsync(actionAsync));
        }
    }

    private static async Task IfSuccessAsyncCore<TSource>(
        TSource value,
        Func<TSource, Task> actionAsync
    )
    {
        var resultTask = actionAsync(value);
        ArgumentNullException.ThrowIfNull(resultTask);

        await resultTask.ConfigureAwait(false);
    }

    private static async Task IfSuccessCore<TSource>(
        Task<FailOr<TSource>> sourceTask,
        Func<FailOr<TSource>, Task> continuation
    )
    {
        var source = await sourceTask.ConfigureAwait(false);
        await continuation(source).ConfigureAwait(false);
    }
}

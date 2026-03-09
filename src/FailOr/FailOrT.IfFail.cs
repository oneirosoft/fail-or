namespace FailOr;

/// <summary>
/// Provides terminal failure-observation extensions for <see cref="FailOr{T}"/> values.
/// </summary>
public static class FailOrIfFailExtensions
{
    extension<TSource>(FailOr<TSource> source)
    {
        /// <summary>
        /// Runs a side effect for the source failures without changing the result.
        /// </summary>
        /// <param name="action">The side effect to run when the source is failed.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="action"/> is <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// result.IfFail(failures => Console.WriteLine(failures[0].Details));
        /// </code>
        /// </example>
        public void IfFail(Action<IReadOnlyList<Failures>> action)
        {
            ArgumentNullException.ThrowIfNull(action);

            if (source.IsSuccess)
            {
                return;
            }

            action(source.Failures);
        }

        /// <summary>
        /// Asynchronously runs a side effect for the source failures without changing the result.
        /// </summary>
        /// <param name="actionAsync">The asynchronous side effect to run when the source is failed.</param>
        /// <returns>A task that completes when the failure side effect has finished.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="actionAsync"/> is <see langword="null"/> or returns <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// await result.IfFailAsync(failures => AuditAsync(failures));
        /// </code>
        /// </example>
        public Task IfFailAsync(Func<IReadOnlyList<Failures>, Task> actionAsync)
        {
            ArgumentNullException.ThrowIfNull(actionAsync);

            return source.IsSuccess
                ? Task.CompletedTask
                : IfFailAsyncCore(source.Failures, actionAsync);
        }
    }

    extension<TSource>(Task<FailOr<TSource>> sourceTask)
    {
        /// <summary>
        /// Runs a side effect for the awaited source failures without changing the result.
        /// </summary>
        /// <param name="action">The side effect to run when the awaited source is failed.</param>
        /// <returns>A task that completes when the failure side effect has finished.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <c>sourceTask</c> or <paramref name="action"/> is <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// await resultTask.IfFail(failures => Console.WriteLine(failures[0].Details));
        /// </code>
        /// </example>
        public Task IfFail(Action<IReadOnlyList<Failures>> action)
        {
            ArgumentNullException.ThrowIfNull(sourceTask);
            ArgumentNullException.ThrowIfNull(action);

            return IfFailCore(
                sourceTask,
                source =>
                {
                    source.IfFail(action);
                    return Task.CompletedTask;
                }
            );
        }

        /// <summary>
        /// Asynchronously runs a side effect for the awaited source failures without changing the result.
        /// </summary>
        /// <param name="actionAsync">The asynchronous side effect to run when the awaited source is failed.</param>
        /// <returns>A task that completes when the failure side effect has finished.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <c>sourceTask</c> or <paramref name="actionAsync"/> is <see langword="null"/>,
        /// or when the awaited source is failed, invokes <paramref name="actionAsync"/>, and it returns <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// await resultTask.IfFailAsync(failures => AuditAsync(failures));
        /// </code>
        /// </example>
        public Task IfFailAsync(Func<IReadOnlyList<Failures>, Task> actionAsync)
        {
            ArgumentNullException.ThrowIfNull(sourceTask);
            ArgumentNullException.ThrowIfNull(actionAsync);

            return IfFailCore(sourceTask, source => source.IfFailAsync(actionAsync));
        }
    }

    private static async Task IfFailAsyncCore(
        IReadOnlyList<Failures> failures,
        Func<IReadOnlyList<Failures>, Task> actionAsync
    )
    {
        var resultTask = actionAsync(failures);
        ArgumentNullException.ThrowIfNull(resultTask);

        await resultTask.ConfigureAwait(false);
    }

    private static async Task IfFailCore<TSource>(
        Task<FailOr<TSource>> sourceTask,
        Func<FailOr<TSource>, Task> ifFail
    )
    {
        var source = await sourceTask.ConfigureAwait(false);
        await ifFail(source).ConfigureAwait(false);
    }
}

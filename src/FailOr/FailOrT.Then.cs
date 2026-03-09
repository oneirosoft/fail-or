namespace FailOr;

/// <summary>
/// Provides chaining and fallback extensions for <see cref="FailOr{T}"/> values.
/// </summary>
public static class FailOrThenExtensions
{
    extension<TSource>(FailOr<TSource> source)
    {
        /// <summary>
        /// Maps a successful value to a new successful result.
        /// </summary>
        /// <param name="map">The projection to apply when the source is successful.</param>
        /// <returns>
        /// A successful result containing the mapped value, or the original failures when the source is failed.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="map"/> is <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var next = result.Then(x => x + 1);
        /// </code>
        /// </example>
        public FailOr<TResult> Then<TResult>(Func<TSource, TResult> map)
        {
            ArgumentNullException.ThrowIfNull(map);

            return source.IsFailure
                ? Fail<TSource, TResult>(source)
                : FailOr.Success(map(source.UnsafeUnwrap()));
        }

        /// <summary>
        /// Binds a successful value to another <see cref="FailOr{T}"/> result.
        /// </summary>
        /// <param name="bind">The bind function to apply when the source is successful.</param>
        /// <returns>
        /// The bound result, or the original failures when the source is failed.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="bind"/> is <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var next = result.Then(x => FailOr.Success(x + 1));
        /// </code>
        /// </example>
        public FailOr<TResult> Then<TResult>(Func<TSource, FailOr<TResult>> bind)
        {
            ArgumentNullException.ThrowIfNull(bind);

            return source.IsFailure ? Fail<TSource, TResult>(source) : bind(source.UnsafeUnwrap());
        }

        /// <summary>
        /// Validates a successful value with another <see cref="FailOr{T}"/> check while preserving the original success.
        /// </summary>
        /// <param name="ensure">The validation function to apply when the source is successful.</param>
        /// <returns>
        /// The original success unchanged when the validation succeeds, the validation failures when it fails,
        /// or the original failures when the source is failed.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="ensure"/> is <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var next = result.ThenEnsure(x => x > 0 ? FailOr.Success(true) : FailOr.Fail&lt;bool&gt;(Failure.General("Must be positive")));
        /// </code>
        /// </example>
        public FailOr<TSource> ThenEnsure<TResult>(Func<TSource, FailOr<TResult>> ensure)
        {
            ArgumentNullException.ThrowIfNull(ensure);

            return source.IsFailure ? source : ThenEnsureCore(source.UnsafeUnwrap(), ensure);
        }

        /// <summary>
        /// Asynchronously maps a successful value to a new successful result.
        /// </summary>
        /// <param name="mapAsync">The asynchronous projection to apply when the source is successful.</param>
        /// <returns>
        /// A task producing a successful result containing the mapped value, or the original failures when the source is failed.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="mapAsync"/> is <see langword="null"/> or returns <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var next = await result.ThenAsync(x => GetValueAsync(x));
        /// </code>
        /// </example>
        public Task<FailOr<TResult>> ThenAsync<TResult>(Func<TSource, Task<TResult>> mapAsync)
        {
            ArgumentNullException.ThrowIfNull(mapAsync);

            return source.IsFailure
                ? Task.FromResult(Fail<TSource, TResult>(source))
                : ThenMapAsync(source.UnsafeUnwrap(), mapAsync);
        }

        /// <summary>
        /// Asynchronously binds a successful value to another <see cref="FailOr{T}"/> result.
        /// </summary>
        /// <param name="bindAsync">The asynchronous bind function to apply when the source is successful.</param>
        /// <returns>
        /// A task producing the bound result, or the original failures when the source is failed.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="bindAsync"/> is <see langword="null"/> or returns <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var next = await result.ThenAsync(x => GetResultAsync(x));
        /// </code>
        /// </example>
        public Task<FailOr<TResult>> ThenAsync<TResult>(
            Func<TSource, Task<FailOr<TResult>>> bindAsync
        )
        {
            ArgumentNullException.ThrowIfNull(bindAsync);

            return source.IsFailure
                ? Task.FromResult(Fail<TSource, TResult>(source))
                : ThenBindAsync(source.UnsafeUnwrap(), bindAsync);
        }

        /// <summary>
        /// Asynchronously validates a successful value with another <see cref="FailOr{T}"/> check while preserving the original success.
        /// </summary>
        /// <param name="ensureAsync">The asynchronous validation function to apply when the source is successful.</param>
        /// <returns>
        /// A task producing the original success unchanged when the validation succeeds, the validation failures when it fails,
        /// or the original failures when the source is failed.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="ensureAsync"/> is <see langword="null"/> or returns <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var next = await result.ThenEnsureAsync(x => Task.FromResult(x > 0 ? FailOr.Success(true) : FailOr.Fail&lt;bool&gt;(Failure.General("Must be positive"))));
        /// </code>
        /// </example>
        public Task<FailOr<TSource>> ThenEnsureAsync<TResult>(
            Func<TSource, Task<FailOr<TResult>>> ensureAsync
        )
        {
            ArgumentNullException.ThrowIfNull(ensureAsync);

            return source.IsFailure
                ? Task.FromResult(source)
                : ThenEnsureAsyncCore(source.UnsafeUnwrap(), ensureAsync);
        }

        /// <summary>
        /// Runs a side effect for a successful value while preserving the original result.
        /// </summary>
        /// <param name="action">The side effect to run when the source is successful.</param>
        /// <returns>
        /// The original success unchanged, or the original failures when the source is failed.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="action"/> is <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var next = result.ThenDo(x => Console.WriteLine(x));
        /// </code>
        /// </example>
        public FailOr<TSource> ThenDo(Action<TSource> action)
        {
            ArgumentNullException.ThrowIfNull(action);

            if (source.IsFailure)
            {
                return source;
            }

            action(source.UnsafeUnwrap());
            return source;
        }

        /// <summary>
        /// Asynchronously runs a side effect for a successful value while preserving the original result.
        /// </summary>
        /// <param name="actionAsync">The asynchronous side effect to run when the source is successful.</param>
        /// <returns>
        /// A task producing the original success unchanged, or the original failures when the source is failed.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="actionAsync"/> is <see langword="null"/> or returns <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var next = await result.ThenDoAsync(x => WriteAsync(x));
        /// </code>
        /// </example>
        public Task<FailOr<TSource>> ThenDoAsync(Func<TSource, Task> actionAsync)
        {
            ArgumentNullException.ThrowIfNull(actionAsync);

            return source.IsFailure
                ? Task.FromResult(source)
                : ThenDoAsyncCore(source.UnsafeUnwrap(), actionAsync);
        }

        /// <summary>
        /// Returns the current success unchanged, or the provided alternative result when the source is failed.
        /// </summary>
        /// <param name="alternative">The fallback result to use when the source is failed.</param>
        /// <returns>
        /// The original success, or <paramref name="alternative"/> when the source is failed.
        /// </returns>
        /// <example>
        /// <code>
        /// var next = result.IfFailThen(FailOr.Success(42));
        /// </code>
        /// </example>
        public FailOr<TSource> IfFailThen(FailOr<TSource> alternative) =>
            FailOr.Combine(source, alternative);

        /// <summary>
        /// Returns the current success unchanged, or invokes a fallback factory when the source is failed.
        /// </summary>
        /// <param name="alternative">The fallback factory to invoke when the source is failed.</param>
        /// <returns>
        /// The original success, or the result produced by <paramref name="alternative"/> when the source is failed.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="alternative"/> is <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var next = result.IfFailThen(() => FailOr.Success(42));
        /// </code>
        /// </example>
        public FailOr<TSource> IfFailThen(Func<FailOr<TSource>> alternative)
        {
            ArgumentNullException.ThrowIfNull(alternative);

            return source.IsSuccess ? source : alternative();
        }

        /// <summary>
        /// Returns the current success unchanged, or invokes a fallback factory with the source failures when the source is failed.
        /// </summary>
        /// <param name="alternative">The fallback factory to invoke with the source failures.</param>
        /// <returns>
        /// The original success, or the result produced by <paramref name="alternative"/> when the source is failed.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="alternative"/> is <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var next = result.IfFailThen(failures => FailOr.Fail&lt;int&gt;(failures));
        /// </code>
        /// </example>
        public FailOr<TSource> IfFailThen(
            Func<IReadOnlyList<Failures>, FailOr<TSource>> alternative
        )
        {
            ArgumentNullException.ThrowIfNull(alternative);

            return source.IsSuccess ? source : alternative(source.Failures);
        }

        /// <summary>
        /// Returns the current success unchanged, or asynchronously invokes a fallback factory when the source is failed.
        /// </summary>
        /// <param name="alternativeAsync">The asynchronous fallback factory to invoke when the source is failed.</param>
        /// <returns>
        /// A task producing the original success, or the fallback result when the source is failed.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="alternativeAsync"/> is <see langword="null"/> or returns <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var next = await result.IfFailThenAsync(() => Task.FromResult(FailOr.Success(42)));
        /// </code>
        /// </example>
        public Task<FailOr<TSource>> IfFailThenAsync(Func<Task<FailOr<TSource>>> alternativeAsync)
        {
            ArgumentNullException.ThrowIfNull(alternativeAsync);

            return source.IsSuccess
                ? Task.FromResult(source)
                : IfFailThenAsyncCore(alternativeAsync);
        }

        /// <summary>
        /// Returns the current success unchanged, or asynchronously invokes a fallback factory with the source failures when the source is failed.
        /// </summary>
        /// <param name="alternativeAsync">The asynchronous fallback factory to invoke with the source failures.</param>
        /// <returns>
        /// A task producing the original success, or the fallback result when the source is failed.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="alternativeAsync"/> is <see langword="null"/> or returns <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var next = await result.IfFailThenAsync(failures => Task.FromResult(FailOr.Fail&lt;int&gt;(failures)));
        /// </code>
        /// </example>
        public Task<FailOr<TSource>> IfFailThenAsync(
            Func<IReadOnlyList<Failures>, Task<FailOr<TSource>>> alternativeAsync
        )
        {
            ArgumentNullException.ThrowIfNull(alternativeAsync);

            return source.IsSuccess
                ? Task.FromResult(source)
                : IfFailThenAsyncCore(source.Failures, alternativeAsync);
        }
    }

    extension<TSource>(Task<FailOr<TSource>> sourceTask)
    {
        /// <summary>
        /// Maps the successful value of a task-wrapped result to a new successful result.
        /// </summary>
        /// <param name="map">The projection to apply when the awaited source is successful.</param>
        /// <returns>
        /// A task producing a successful result containing the mapped value, or the original failures when the awaited source is failed.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the awaited source task or <paramref name="map"/> is <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var next = await resultTask.Then(x => x + 1);
        /// </code>
        /// </example>
        public Task<FailOr<TResult>> Then<TResult>(Func<TSource, TResult> map)
        {
            ArgumentNullException.ThrowIfNull(sourceTask);
            ArgumentNullException.ThrowIfNull(map);

            return ThenCore(sourceTask, source => Task.FromResult(source.Then(map)));
        }

        /// <summary>
        /// Binds the successful value of a task-wrapped result to another <see cref="FailOr{T}"/> result.
        /// </summary>
        /// <param name="bind">The bind function to apply when the awaited source is successful.</param>
        /// <returns>
        /// A task producing the bound result, or the original failures when the awaited source is failed.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the awaited source task or <paramref name="bind"/> is <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var next = await resultTask.Then(x => FailOr.Success(x + 1));
        /// </code>
        /// </example>
        public Task<FailOr<TResult>> Then<TResult>(Func<TSource, FailOr<TResult>> bind)
        {
            ArgumentNullException.ThrowIfNull(sourceTask);
            ArgumentNullException.ThrowIfNull(bind);

            return ThenCore(sourceTask, source => Task.FromResult(source.Then(bind)));
        }

        /// <summary>
        /// Validates the successful value of a task-wrapped result with another <see cref="FailOr{T}"/> check while preserving the original success.
        /// </summary>
        /// <param name="ensure">The validation function to apply when the awaited source is successful.</param>
        /// <returns>
        /// A task producing the original success unchanged when the validation succeeds, the validation failures when it fails,
        /// or the original failures when the awaited source is failed.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the awaited source task or <paramref name="ensure"/> is <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var next = await resultTask.ThenEnsure(x => x > 0 ? FailOr.Success(true) : FailOr.Fail&lt;bool&gt;(Failure.General("Must be positive")));
        /// </code>
        /// </example>
        public Task<FailOr<TSource>> ThenEnsure<TResult>(Func<TSource, FailOr<TResult>> ensure)
        {
            ArgumentNullException.ThrowIfNull(sourceTask);
            ArgumentNullException.ThrowIfNull(ensure);

            return ThenCore(sourceTask, source => Task.FromResult(source.ThenEnsure(ensure)));
        }

        /// <summary>
        /// Asynchronously maps the successful value of a task-wrapped result to a new successful result.
        /// </summary>
        /// <param name="mapAsync">The asynchronous projection to apply when the awaited source is successful.</param>
        /// <returns>
        /// A task producing a successful result containing the mapped value, or the original failures when the awaited source is failed.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the awaited source task or <paramref name="mapAsync"/> is <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var next = await resultTask.ThenAsync(x => GetValueAsync(x));
        /// </code>
        /// </example>
        public Task<FailOr<TResult>> ThenAsync<TResult>(Func<TSource, Task<TResult>> mapAsync)
        {
            ArgumentNullException.ThrowIfNull(sourceTask);
            ArgumentNullException.ThrowIfNull(mapAsync);

            return ThenCore(sourceTask, source => source.ThenAsync(mapAsync));
        }

        /// <summary>
        /// Asynchronously binds the successful value of a task-wrapped result to another <see cref="FailOr{T}"/> result.
        /// </summary>
        /// <param name="bindAsync">The asynchronous bind function to apply when the awaited source is successful.</param>
        /// <returns>
        /// A task producing the bound result, or the original failures when the awaited source is failed.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the awaited source task or <paramref name="bindAsync"/> is <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var next = await resultTask.ThenAsync(x => GetResultAsync(x));
        /// </code>
        /// </example>
        public Task<FailOr<TResult>> ThenAsync<TResult>(
            Func<TSource, Task<FailOr<TResult>>> bindAsync
        )
        {
            ArgumentNullException.ThrowIfNull(sourceTask);
            ArgumentNullException.ThrowIfNull(bindAsync);

            return ThenCore(sourceTask, source => source.ThenAsync(bindAsync));
        }

        /// <summary>
        /// Asynchronously validates the successful value of a task-wrapped result with another <see cref="FailOr{T}"/> check while preserving the original success.
        /// </summary>
        /// <param name="ensureAsync">The asynchronous validation function to apply when the awaited source is successful.</param>
        /// <returns>
        /// A task producing the original success unchanged when the validation succeeds, the validation failures when it fails,
        /// or the original failures when the awaited source is failed.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the awaited source task or <paramref name="ensureAsync"/> is <see langword="null"/>,
        /// or when <paramref name="ensureAsync"/> returns <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var next = await resultTask.ThenEnsureAsync(x => Task.FromResult(x > 0 ? FailOr.Success(true) : FailOr.Fail&lt;bool&gt;(Failure.General("Must be positive"))));
        /// </code>
        /// </example>
        public Task<FailOr<TSource>> ThenEnsureAsync<TResult>(
            Func<TSource, Task<FailOr<TResult>>> ensureAsync
        )
        {
            ArgumentNullException.ThrowIfNull(sourceTask);
            ArgumentNullException.ThrowIfNull(ensureAsync);

            return ThenCore(sourceTask, source => source.ThenEnsureAsync(ensureAsync));
        }

        /// <summary>
        /// Runs a side effect for the successful value of a task-wrapped result while preserving the original result.
        /// </summary>
        /// <param name="action">The side effect to run when the awaited source is successful.</param>
        /// <returns>
        /// A task producing the original success unchanged, or the original failures when the awaited source is failed.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the awaited source task or <paramref name="action"/> is <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var next = await resultTask.ThenDo(x => Console.WriteLine(x));
        /// </code>
        /// </example>
        public Task<FailOr<TSource>> ThenDo(Action<TSource> action)
        {
            ArgumentNullException.ThrowIfNull(sourceTask);
            ArgumentNullException.ThrowIfNull(action);

            return ThenCore(sourceTask, source => Task.FromResult(source.ThenDo(action)));
        }

        /// <summary>
        /// Asynchronously runs a side effect for the successful value of a task-wrapped result while preserving the original result.
        /// </summary>
        /// <param name="actionAsync">The asynchronous side effect to run when the awaited source is successful.</param>
        /// <returns>
        /// A task producing the original success unchanged, or the original failures when the awaited source is failed.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the awaited source task or <paramref name="actionAsync"/> is <see langword="null"/>,
        /// or when <paramref name="actionAsync"/> returns <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var next = await resultTask.ThenDoAsync(x => WriteAsync(x));
        /// </code>
        /// </example>
        public Task<FailOr<TSource>> ThenDoAsync(Func<TSource, Task> actionAsync)
        {
            ArgumentNullException.ThrowIfNull(sourceTask);
            ArgumentNullException.ThrowIfNull(actionAsync);

            return ThenCore(sourceTask, source => source.ThenDoAsync(actionAsync));
        }

        /// <summary>
        /// Returns the awaited success unchanged, or the provided alternative result when the awaited source is failed.
        /// </summary>
        /// <param name="alternative">The fallback result to use when the awaited source is failed.</param>
        /// <returns>
        /// A task producing the original success, or <paramref name="alternative"/> when the awaited source is failed.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the awaited source task is <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var next = await resultTask.IfFailThen(FailOr.Success(42));
        /// </code>
        /// </example>
        public Task<FailOr<TSource>> IfFailThen(FailOr<TSource> alternative)
        {
            ArgumentNullException.ThrowIfNull(sourceTask);

            return ThenCore(sourceTask, source => Task.FromResult(source.IfFailThen(alternative)));
        }

        /// <summary>
        /// Returns the awaited success unchanged, or invokes a fallback factory when the awaited source is failed.
        /// </summary>
        /// <param name="alternative">The fallback factory to invoke when the awaited source is failed.</param>
        /// <returns>
        /// A task producing the original success, or the result produced by <paramref name="alternative"/> when the awaited source is failed.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the awaited source task or <paramref name="alternative"/> is <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var next = await resultTask.IfFailThen(() => FailOr.Success(42));
        /// </code>
        /// </example>
        public Task<FailOr<TSource>> IfFailThen(Func<FailOr<TSource>> alternative)
        {
            ArgumentNullException.ThrowIfNull(sourceTask);
            ArgumentNullException.ThrowIfNull(alternative);

            return ThenCore(sourceTask, source => Task.FromResult(source.IfFailThen(alternative)));
        }

        /// <summary>
        /// Returns the awaited success unchanged, or invokes a fallback factory with the awaited source failures when the awaited source is failed.
        /// </summary>
        /// <param name="alternative">The fallback factory to invoke with the awaited source failures.</param>
        /// <returns>
        /// A task producing the original success, or the result produced by <paramref name="alternative"/> when the awaited source is failed.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the awaited source task or <paramref name="alternative"/> is <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var next = await resultTask.IfFailThen(failures => FailOr.Fail&lt;int&gt;(failures));
        /// </code>
        /// </example>
        public Task<FailOr<TSource>> IfFailThen(
            Func<IReadOnlyList<Failures>, FailOr<TSource>> alternative
        )
        {
            ArgumentNullException.ThrowIfNull(sourceTask);
            ArgumentNullException.ThrowIfNull(alternative);

            return ThenCore(sourceTask, source => Task.FromResult(source.IfFailThen(alternative)));
        }

        /// <summary>
        /// Returns the awaited success unchanged, or asynchronously invokes a fallback factory when the awaited source is failed.
        /// </summary>
        /// <param name="alternativeAsync">The asynchronous fallback factory to invoke when the awaited source is failed.</param>
        /// <returns>
        /// A task producing the original success, or the fallback result when the awaited source is failed.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the awaited source task or <paramref name="alternativeAsync"/> is <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var next = await resultTask.IfFailThenAsync(() => Task.FromResult(FailOr.Success(42)));
        /// </code>
        /// </example>
        public Task<FailOr<TSource>> IfFailThenAsync(Func<Task<FailOr<TSource>>> alternativeAsync)
        {
            ArgumentNullException.ThrowIfNull(sourceTask);
            ArgumentNullException.ThrowIfNull(alternativeAsync);

            return ThenCore(sourceTask, source => source.IfFailThenAsync(alternativeAsync));
        }

        /// <summary>
        /// Returns the awaited success unchanged, or asynchronously invokes a fallback factory with the awaited source failures when the awaited source is failed.
        /// </summary>
        /// <param name="alternativeAsync">The asynchronous fallback factory to invoke with the awaited source failures.</param>
        /// <returns>
        /// A task producing the original success, or the fallback result when the awaited source is failed.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the awaited source task or <paramref name="alternativeAsync"/> is <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code>
        /// var next = await resultTask.IfFailThenAsync(failures => Task.FromResult(FailOr.Fail&lt;int&gt;(failures)));
        /// </code>
        /// </example>
        public Task<FailOr<TSource>> IfFailThenAsync(
            Func<IReadOnlyList<Failures>, Task<FailOr<TSource>>> alternativeAsync
        )
        {
            ArgumentNullException.ThrowIfNull(sourceTask);
            ArgumentNullException.ThrowIfNull(alternativeAsync);

            return ThenCore(sourceTask, source => source.IfFailThenAsync(alternativeAsync));
        }
    }

    private static FailOr<TResult> Fail<TSource, TResult>(FailOr<TSource> source) =>
        FailOr.Fail<TResult>(source.Failures);

    private static async Task<FailOr<TResult>> ThenMapAsync<TSource, TResult>(
        TSource value,
        Func<TSource, Task<TResult>> mapAsync
    )
    {
        var resultTask = mapAsync(value);
        ArgumentNullException.ThrowIfNull(resultTask);

        return FailOr.Success(await resultTask.ConfigureAwait(false));
    }

    private static async Task<FailOr<TResult>> ThenBindAsync<TSource, TResult>(
        TSource value,
        Func<TSource, Task<FailOr<TResult>>> bindAsync
    )
    {
        var resultTask = bindAsync(value);
        ArgumentNullException.ThrowIfNull(resultTask);

        return await resultTask.ConfigureAwait(false);
    }

    private static FailOr<TSource> ThenEnsureCore<TSource, TResult>(
        TSource value,
        Func<TSource, FailOr<TResult>> ensure
    )
    {
        var ensured = ensure(value);
        return ensured.IsFailure ? FailOr.Fail<TSource>(ensured.Failures) : FailOr.Success(value);
    }

    private static async Task<FailOr<TSource>> ThenEnsureAsyncCore<TSource, TResult>(
        TSource value,
        Func<TSource, Task<FailOr<TResult>>> ensureAsync
    )
    {
        var resultTask = ensureAsync(value);
        ArgumentNullException.ThrowIfNull(resultTask);

        var ensured = await resultTask.ConfigureAwait(false);
        return ensured.IsFailure ? FailOr.Fail<TSource>(ensured.Failures) : FailOr.Success(value);
    }

    private static async Task<FailOr<TSource>> ThenDoAsyncCore<TSource>(
        TSource value,
        Func<TSource, Task> actionAsync
    )
    {
        var resultTask = actionAsync(value);
        ArgumentNullException.ThrowIfNull(resultTask);

        await resultTask.ConfigureAwait(false);
        return FailOr.Success(value);
    }

    private static async Task<FailOr<TSource>> IfFailThenAsyncCore<TSource>(
        Func<Task<FailOr<TSource>>> alternativeAsync
    )
    {
        var resultTask = alternativeAsync();
        ArgumentNullException.ThrowIfNull(resultTask);

        return await resultTask.ConfigureAwait(false);
    }

    private static async Task<FailOr<TSource>> IfFailThenAsyncCore<TSource>(
        IReadOnlyList<Failures> failures,
        Func<IReadOnlyList<Failures>, Task<FailOr<TSource>>> alternativeAsync
    )
    {
        var resultTask = alternativeAsync(failures);
        ArgumentNullException.ThrowIfNull(resultTask);

        return await resultTask.ConfigureAwait(false);
    }

    private static async Task<FailOr<TResult>> ThenCore<TSource, TResult>(
        Task<FailOr<TSource>> sourceTask,
        Func<FailOr<TSource>, Task<FailOr<TResult>>> then
    )
    {
        var source = await sourceTask.ConfigureAwait(false);
        return await then(source).ConfigureAwait(false);
    }
}

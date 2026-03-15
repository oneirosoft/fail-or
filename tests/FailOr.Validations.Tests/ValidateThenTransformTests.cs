namespace FailOr.Validations.Tests;

public class ValidateThenTransformTests
{
    [Test]
    public Task ValidateThenTransform_returns_transformed_values_for_all_sync_arities()
    {
        var input = SampleData.CreateValid();

        for (var arity = 1; arity <= 14; arity++)
        {
            var result = InvokeValidateThenTransformSync(arity, input);

            TestAssert.False(
                result.IsFailure,
                $"Expected sync transform validation to succeed for arity {arity}."
            );
            TestAssert.Equal(
                ExpectedJoinedValue(input, arity),
                result.UnsafeUnwrap(),
                $"Unexpected sync transform result for arity {arity}."
            );
        }

        return Task.CompletedTask;
    }

    [Test]
    public async Task ValidateThenTransformAsync_returns_transformed_values_for_all_async_arities_with_sync_transform()
    {
        var input = SampleData.CreateValid();

        for (var arity = 1; arity <= 14; arity++)
        {
            var result = await InvokeValidateThenTransformAsync(arity, input);

            TestAssert.False(
                result.IsFailure,
                $"Expected async transform validation to succeed for arity {arity}."
            );
            TestAssert.Equal(
                ExpectedJoinedValue(input, arity),
                result.UnsafeUnwrap(),
                $"Unexpected async transform result for arity {arity}."
            );
        }
    }

    [Test]
    public async Task ValidateThenTransformAsync_returns_transformed_values_for_all_async_arities_with_async_transform()
    {
        var input = SampleData.CreateValid();

        for (var arity = 1; arity <= 14; arity++)
        {
            var result = await InvokeValidateThenTransformAsyncWithAsyncTransform(arity, input);

            TestAssert.False(
                result.IsFailure,
                $"Expected async transform validation with async transform to succeed for arity {arity}."
            );
            TestAssert.Equal(
                ExpectedJoinedValue(input, arity),
                result.UnsafeUnwrap(),
                $"Unexpected async transform result for arity {arity}."
            );
        }
    }

    [Test]
    public Task ValidateThenTransform_normalizes_validation_failures_and_preserves_non_validation_failures()
    {
        var input = SampleData.CreateValid();
        var result = input.ValidateThenTransform(
            (static x => x.Name, static _ => SampleData.ValidationString("name-invalid")),
            (
                static x => x.Nickname!,
                static _ => SampleData.GeneralString("Nickname.Unexpected", "boom")
            ),
            (static x => x.Address.City, static _ => SampleData.ValidationString("city-invalid")),
            static (_, _, _) => "ignored"
        );

        TestAssert.True(result.IsFailure, "Expected transform validation to fail.");
        TestAssert.Equal(3, result.Failures.Count, "Unexpected transform failure count.");
        TestAssert.ValidationFailure(
            result.Failures[0],
            "Name",
            "name-invalid",
            "Unexpected first transform failure."
        );
        TestAssert.GeneralFailure(
            result.Failures[1],
            "Nickname.Unexpected",
            "boom",
            "Unexpected second transform failure."
        );
        TestAssert.ValidationFailure(
            result.Failures[2],
            "City",
            "city-invalid",
            "Unexpected third transform failure."
        );

        return Task.CompletedTask;
    }

    [Test]
    public Task ValidateThenTransform_skips_the_final_transform_when_any_mapper_fails()
    {
        var input = SampleData.CreateValid();
        var wasInvoked = false;

        var result = input.ValidateThenTransform(
            (static x => x.Name, static _ => SampleData.ValidationString("name-invalid")),
            value =>
            {
                wasInvoked = true;
                return value;
            }
        );

        TestAssert.True(result.IsFailure, "Expected transform validation to fail.");
        TestAssert.False(wasInvoked, "Expected transform to be skipped.");
        TestAssert.ValidationFailure(
            result.Failures[0],
            "Name",
            "name-invalid",
            "Unexpected skipped-transform failure."
        );

        return Task.CompletedTask;
    }

    [Test]
    public async Task ValidateThenTransformAsync_executes_mappers_sequentially_and_skips_the_final_transform_when_any_mapper_fails()
    {
        var input = SampleData.CreateValid();
        var order = new List<string>();
        var wasInvoked = false;

        var result = await input.ValidateThenTransformAsync(
            (
                static x => x.Name,
                async _ =>
                {
                    order.Add("start-1");
                    await Task.Yield();
                    order.Add("end-1");
                    return SampleData.ValidationString("name-invalid");
                }
            ),
            (
                static x => x.Address.City,
                async value =>
                {
                    order.Add("start-2");
                    await Task.Yield();
                    order.Add("end-2");
                    return SampleData.ValidString(value.ToUpperInvariant());
                }
            ),
            (_, _) =>
            {
                wasInvoked = true;
                return "ignored";
            }
        );

        TestAssert.True(result.IsFailure, "Expected async transform validation to fail.");
        TestAssert.False(wasInvoked, "Expected async transform to be skipped.");
        TestAssert.Equal(
            "start-1,end-1,start-2,end-2",
            string.Join(",", order),
            "Expected sequential async mapping."
        );
        TestAssert.ValidationFailure(
            result.Failures[0],
            "Name",
            "name-invalid",
            "Unexpected async skipped-transform failure."
        );
    }

    [Test]
    public Task ValidateThenTransform_throws_for_null_transform()
    {
        var input = SampleData.CreateValid();

        TestAssert.Throws<ArgumentNullException>(
            () =>
                input.ValidateThenTransform(
                    (static x => x.Name, static value => SampleData.ValidString(value)),
                    transform: (Func<string, string>)null!
                ),
            "Expected null transform to throw.",
            "transform"
        );

        return Task.CompletedTask;
    }

    [Test]
    public async Task ValidateThenTransformAsync_throws_for_null_transform_async()
    {
        var input = SampleData.CreateValid();

        await TestAssert.ThrowsAsync<ArgumentNullException>(
            () =>
                input.ValidateThenTransformAsync(
                    (static x => x.Name, static value => SampleData.ValidStringAsync(value)),
                    transformAsync: (Func<string, Task<string>>)null!
                ),
            "Expected null async transform to throw.",
            "transformAsync"
        );
    }

    private static FailOr<string> InvokeValidateThenTransformSync(int arity, SampleInput input) =>
        arity switch
        {
            1 => input.ValidateThenTransform(
                (
                    static x => x.Name,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (v1) => string.Join("|", [v1])
            ),
            2 => input.ValidateThenTransform(
                (
                    static x => x.Name,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Nickname!,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (v1, v2) => string.Join("|", [v1, v2])
            ),
            3 => input.ValidateThenTransform(
                (
                    static x => x.Name,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Nickname!,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Address.City,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (v1, v2, v3) => string.Join("|", [v1, v2, v3])
            ),
            4 => input.ValidateThenTransform(
                (
                    static x => x.Name,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Nickname!,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Address.City,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Country,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (v1, v2, v3, v4) => string.Join("|", [v1, v2, v3, v4])
            ),
            5 => input.ValidateThenTransform(
                (
                    static x => x.Name,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Nickname!,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Address.City,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Country,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.PostalCode,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (v1, v2, v3, v4, v5) => string.Join("|", [v1, v2, v3, v4, v5])
            ),
            6 => input.ValidateThenTransform(
                (
                    static x => x.Name,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Nickname!,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Address.City,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Country,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.PostalCode,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Region,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (v1, v2, v3, v4, v5, v6) => string.Join("|", [v1, v2, v3, v4, v5, v6])
            ),
            7 => input.ValidateThenTransform(
                (
                    static x => x.Name,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Nickname!,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Address.City,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Country,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.PostalCode,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Region,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Email,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (v1, v2, v3, v4, v5, v6, v7) => string.Join("|", [v1, v2, v3, v4, v5, v6, v7])
            ),
            8 => input.ValidateThenTransform(
                (
                    static x => x.Name,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Nickname!,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Address.City,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Country,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.PostalCode,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Region,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Email,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Phone,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (v1, v2, v3, v4, v5, v6, v7, v8) =>
                    string.Join("|", [v1, v2, v3, v4, v5, v6, v7, v8])
            ),
            9 => input.ValidateThenTransform(
                (
                    static x => x.Name,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Nickname!,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Address.City,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Country,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.PostalCode,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Region,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Email,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Phone,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Department,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (v1, v2, v3, v4, v5, v6, v7, v8, v9) =>
                    string.Join("|", [v1, v2, v3, v4, v5, v6, v7, v8, v9])
            ),
            10 => input.ValidateThenTransform(
                (
                    static x => x.Name,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Nickname!,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Address.City,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Country,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.PostalCode,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Region,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Email,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Phone,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Department,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Title,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (v1, v2, v3, v4, v5, v6, v7, v8, v9, v10) =>
                    string.Join("|", [v1, v2, v3, v4, v5, v6, v7, v8, v9, v10])
            ),
            11 => input.ValidateThenTransform(
                (
                    static x => x.Name,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Nickname!,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Address.City,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Country,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.PostalCode,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Region,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Email,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Phone,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Department,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Title,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Language,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11) =>
                    string.Join("|", [v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11])
            ),
            12 => input.ValidateThenTransform(
                (
                    static x => x.Name,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Nickname!,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Address.City,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Country,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.PostalCode,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Region,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Email,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Phone,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Department,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Title,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Language,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.TimeZone,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11, v12) =>
                    string.Join("|", [v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11, v12])
            ),
            13 => input.ValidateThenTransform(
                (
                    static x => x.Name,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Nickname!,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Address.City,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Country,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.PostalCode,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Region,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Email,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Phone,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Department,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Title,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Language,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.TimeZone,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Currency,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11, v12, v13) =>
                    string.Join("|", [v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11, v12, v13])
            ),
            14 => input.ValidateThenTransform(
                (
                    static x => x.Name,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Nickname!,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Address.City,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Country,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.PostalCode,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Region,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Email,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Phone,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Department,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Title,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Language,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.TimeZone,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Currency,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (
                    static x => x.Manager,
                    static value => SampleData.ValidString(value.ToUpperInvariant())
                ),
                (v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11, v12, v13, v14) =>
                    string.Join("|", [v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11, v12, v13, v14])
            ),
            _ => throw new ArgumentOutOfRangeException(nameof(arity)),
        };

    private static Task<FailOr<string>> InvokeValidateThenTransformAsync(
        int arity,
        SampleInput input
    ) =>
        arity switch
        {
            1 => input.ValidateThenTransformAsync(
                (
                    static x => x.Name,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (v1) => string.Join("|", [v1])
            ),
            2 => input.ValidateThenTransformAsync(
                (
                    static x => x.Name,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Nickname!,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (v1, v2) => string.Join("|", [v1, v2])
            ),
            3 => input.ValidateThenTransformAsync(
                (
                    static x => x.Name,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Nickname!,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Address.City,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (v1, v2, v3) => string.Join("|", [v1, v2, v3])
            ),
            4 => input.ValidateThenTransformAsync(
                (
                    static x => x.Name,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Nickname!,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Address.City,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Country,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (v1, v2, v3, v4) => string.Join("|", [v1, v2, v3, v4])
            ),
            5 => input.ValidateThenTransformAsync(
                (
                    static x => x.Name,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Nickname!,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Address.City,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Country,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.PostalCode,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (v1, v2, v3, v4, v5) => string.Join("|", [v1, v2, v3, v4, v5])
            ),
            6 => input.ValidateThenTransformAsync(
                (
                    static x => x.Name,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Nickname!,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Address.City,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Country,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.PostalCode,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Region,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (v1, v2, v3, v4, v5, v6) => string.Join("|", [v1, v2, v3, v4, v5, v6])
            ),
            7 => input.ValidateThenTransformAsync(
                (
                    static x => x.Name,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Nickname!,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Address.City,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Country,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.PostalCode,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Region,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Email,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (v1, v2, v3, v4, v5, v6, v7) => string.Join("|", [v1, v2, v3, v4, v5, v6, v7])
            ),
            8 => input.ValidateThenTransformAsync(
                (
                    static x => x.Name,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Nickname!,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Address.City,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Country,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.PostalCode,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Region,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Email,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Phone,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (v1, v2, v3, v4, v5, v6, v7, v8) =>
                    string.Join("|", [v1, v2, v3, v4, v5, v6, v7, v8])
            ),
            9 => input.ValidateThenTransformAsync(
                (
                    static x => x.Name,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Nickname!,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Address.City,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Country,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.PostalCode,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Region,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Email,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Phone,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Department,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (v1, v2, v3, v4, v5, v6, v7, v8, v9) =>
                    string.Join("|", [v1, v2, v3, v4, v5, v6, v7, v8, v9])
            ),
            10 => input.ValidateThenTransformAsync(
                (
                    static x => x.Name,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Nickname!,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Address.City,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Country,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.PostalCode,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Region,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Email,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Phone,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Department,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Title,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (v1, v2, v3, v4, v5, v6, v7, v8, v9, v10) =>
                    string.Join("|", [v1, v2, v3, v4, v5, v6, v7, v8, v9, v10])
            ),
            11 => input.ValidateThenTransformAsync(
                (
                    static x => x.Name,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Nickname!,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Address.City,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Country,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.PostalCode,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Region,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Email,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Phone,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Department,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Title,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Language,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11) =>
                    string.Join("|", [v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11])
            ),
            12 => input.ValidateThenTransformAsync(
                (
                    static x => x.Name,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Nickname!,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Address.City,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Country,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.PostalCode,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Region,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Email,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Phone,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Department,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Title,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Language,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.TimeZone,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11, v12) =>
                    string.Join("|", [v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11, v12])
            ),
            13 => input.ValidateThenTransformAsync(
                (
                    static x => x.Name,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Nickname!,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Address.City,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Country,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.PostalCode,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Region,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Email,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Phone,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Department,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Title,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Language,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.TimeZone,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Currency,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11, v12, v13) =>
                    string.Join("|", [v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11, v12, v13])
            ),
            14 => input.ValidateThenTransformAsync(
                (
                    static x => x.Name,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Nickname!,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Address.City,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Country,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.PostalCode,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Region,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Email,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Phone,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Department,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Title,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Language,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.TimeZone,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Currency,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Manager,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11, v12, v13, v14) =>
                    string.Join("|", [v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11, v12, v13, v14])
            ),
            _ => throw new ArgumentOutOfRangeException(nameof(arity)),
        };

    private static Task<FailOr<string>> InvokeValidateThenTransformAsyncWithAsyncTransform(
        int arity,
        SampleInput input
    ) =>
        arity switch
        {
            1 => input.ValidateThenTransformAsync(
                (
                    static x => x.Name,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                async (v1) =>
                {
                    await Task.Yield();
                    return string.Join("|", [v1]);
                }
            ),
            2 => input.ValidateThenTransformAsync(
                (
                    static x => x.Name,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Nickname!,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                async (v1, v2) =>
                {
                    await Task.Yield();
                    return string.Join("|", [v1, v2]);
                }
            ),
            3 => input.ValidateThenTransformAsync(
                (
                    static x => x.Name,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Nickname!,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Address.City,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                async (v1, v2, v3) =>
                {
                    await Task.Yield();
                    return string.Join("|", [v1, v2, v3]);
                }
            ),
            4 => input.ValidateThenTransformAsync(
                (
                    static x => x.Name,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Nickname!,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Address.City,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Country,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                async (v1, v2, v3, v4) =>
                {
                    await Task.Yield();
                    return string.Join("|", [v1, v2, v3, v4]);
                }
            ),
            5 => input.ValidateThenTransformAsync(
                (
                    static x => x.Name,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Nickname!,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Address.City,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Country,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.PostalCode,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                async (v1, v2, v3, v4, v5) =>
                {
                    await Task.Yield();
                    return string.Join("|", [v1, v2, v3, v4, v5]);
                }
            ),
            6 => input.ValidateThenTransformAsync(
                (
                    static x => x.Name,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Nickname!,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Address.City,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Country,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.PostalCode,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Region,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                async (v1, v2, v3, v4, v5, v6) =>
                {
                    await Task.Yield();
                    return string.Join("|", [v1, v2, v3, v4, v5, v6]);
                }
            ),
            7 => input.ValidateThenTransformAsync(
                (
                    static x => x.Name,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Nickname!,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Address.City,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Country,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.PostalCode,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Region,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Email,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                async (v1, v2, v3, v4, v5, v6, v7) =>
                {
                    await Task.Yield();
                    return string.Join("|", [v1, v2, v3, v4, v5, v6, v7]);
                }
            ),
            8 => input.ValidateThenTransformAsync(
                (
                    static x => x.Name,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Nickname!,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Address.City,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Country,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.PostalCode,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Region,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Email,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Phone,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                async (v1, v2, v3, v4, v5, v6, v7, v8) =>
                {
                    await Task.Yield();
                    return string.Join("|", [v1, v2, v3, v4, v5, v6, v7, v8]);
                }
            ),
            9 => input.ValidateThenTransformAsync(
                (
                    static x => x.Name,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Nickname!,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Address.City,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Country,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.PostalCode,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Region,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Email,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Phone,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Department,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                async (v1, v2, v3, v4, v5, v6, v7, v8, v9) =>
                {
                    await Task.Yield();
                    return string.Join("|", [v1, v2, v3, v4, v5, v6, v7, v8, v9]);
                }
            ),
            10 => input.ValidateThenTransformAsync(
                (
                    static x => x.Name,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Nickname!,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Address.City,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Country,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.PostalCode,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Region,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Email,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Phone,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Department,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Title,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                async (v1, v2, v3, v4, v5, v6, v7, v8, v9, v10) =>
                {
                    await Task.Yield();
                    return string.Join("|", [v1, v2, v3, v4, v5, v6, v7, v8, v9, v10]);
                }
            ),
            11 => input.ValidateThenTransformAsync(
                (
                    static x => x.Name,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Nickname!,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Address.City,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Country,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.PostalCode,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Region,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Email,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Phone,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Department,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Title,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Language,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                async (v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11) =>
                {
                    await Task.Yield();
                    return string.Join("|", [v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11]);
                }
            ),
            12 => input.ValidateThenTransformAsync(
                (
                    static x => x.Name,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Nickname!,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Address.City,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Country,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.PostalCode,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Region,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Email,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Phone,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Department,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Title,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Language,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.TimeZone,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                async (v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11, v12) =>
                {
                    await Task.Yield();
                    return string.Join("|", [v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11, v12]);
                }
            ),
            13 => input.ValidateThenTransformAsync(
                (
                    static x => x.Name,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Nickname!,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Address.City,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Country,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.PostalCode,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Region,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Email,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Phone,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Department,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Title,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Language,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.TimeZone,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Currency,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                async (v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11, v12, v13) =>
                {
                    await Task.Yield();
                    return string.Join(
                        "|",
                        [v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11, v12, v13]
                    );
                }
            ),
            14 => input.ValidateThenTransformAsync(
                (
                    static x => x.Name,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Nickname!,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Address.City,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Country,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.PostalCode,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Region,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Email,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Phone,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Department,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Title,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Language,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.TimeZone,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Currency,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                (
                    static x => x.Manager,
                    static value => SampleData.ValidStringAsync(value.ToUpperInvariant())
                ),
                async (v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11, v12, v13, v14) =>
                {
                    await Task.Yield();
                    return string.Join(
                        "|",
                        [v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11, v12, v13, v14]
                    );
                }
            ),
            _ => throw new ArgumentOutOfRangeException(nameof(arity)),
        };

    private static string ExpectedJoinedValue(SampleInput input, int arity)
    {
        var values = new[]
        {
            input.Name,
            input.Nickname!,
            input.Address.City,
            input.Country,
            input.PostalCode,
            input.Region,
            input.Email,
            input.Phone,
            input.Department,
            input.Title,
            input.Language,
            input.TimeZone,
            input.Currency,
            input.Manager,
        };

        return string.Join(
            "|",
            values.Take(arity).Select(static value => value.ToUpperInvariant())
        );
    }
}

namespace FailOr.Validations.Tests;

public class ValidatorTests
{
    [Test]
    public Task Validate_returns_the_original_instance_for_all_sync_arities()
    {
        var input = SampleData.CreateValid();

        for (var arity = 1; arity <= 14; arity++)
        {
            var result = InvokeValidateSync(arity, input);

            TestAssert.False(
                result.IsFailure,
                $"Expected sync validation to succeed for arity {arity}."
            );
            TestAssert.Same(
                input,
                result.UnsafeUnwrap(),
                $"Expected original instance for arity {arity}."
            );
        }

        return Task.CompletedTask;
    }

    [Test]
    public async Task ValidateAsync_returns_the_original_instance_for_all_async_arities()
    {
        var input = SampleData.CreateValid();

        for (var arity = 1; arity <= 14; arity++)
        {
            var result = await InvokeValidateAsync(arity, input);

            TestAssert.False(
                result.IsFailure,
                $"Expected async validation to succeed for arity {arity}."
            );
            TestAssert.Same(
                input,
                result.UnsafeUnwrap(),
                $"Expected original instance for async arity {arity}."
            );
        }
    }

    [Test]
    public Task Validate_normalizes_validation_failures_and_preserves_non_validation_failures()
    {
        var input = SampleData.CreateValid();
        var result = input.Validate(
            (static x => x.Name, static _ => SampleData.ValidationInt("name-invalid")),
            (
                static x => x.Nickname!,
                static _ => SampleData.GeneralInt("Nickname.Unexpected", "boom")
            ),
            (static x => x.Address.City, static _ => SampleData.ValidationInt("city-invalid"))
        );

        TestAssert.True(result.IsFailure, "Expected validation to fail.");
        TestAssert.Equal(3, result.Failures.Count, "Unexpected failure count.");
        TestAssert.ValidationFailure(
            result.Failures[0],
            "Name",
            "name-invalid",
            "Unexpected first failure."
        );
        TestAssert.GeneralFailure(
            result.Failures[1],
            "Nickname.Unexpected",
            "boom",
            "Unexpected second failure."
        );
        TestAssert.ValidationFailure(
            result.Failures[2],
            "City",
            "city-invalid",
            "Unexpected third failure."
        );

        return Task.CompletedTask;
    }

    [Test]
    public async Task ValidateAsync_executes_validators_sequentially_and_preserves_failure_order()
    {
        var input = SampleData.CreateValid();
        var order = new List<string>();

        var result = await input.ValidateAsync(
            (
                static x => x.Name,
                async _ =>
                {
                    order.Add("start-1");
                    await Task.Yield();
                    order.Add("end-1");
                    return SampleData.ValidationInt("name-invalid");
                }
            ),
            (
                static x => x.Address.City,
                async _ =>
                {
                    order.Add("start-2");
                    await Task.Yield();
                    order.Add("end-2");
                    return SampleData.ValidationInt("city-invalid");
                }
            )
        );

        TestAssert.True(result.IsFailure, "Expected async validation to fail.");
        TestAssert.Equal(
            "start-1,end-1,start-2,end-2",
            string.Join(",", order),
            "Expected sequential async validation."
        );
        TestAssert.ValidationFailure(
            result.Failures[0],
            "Name",
            "name-invalid",
            "Unexpected first async failure."
        );
        TestAssert.ValidationFailure(
            result.Failures[1],
            "City",
            "city-invalid",
            "Unexpected second async failure."
        );
    }

    [Test]
    public Task Validate_throws_for_null_value()
    {
        TestAssert.Throws<ArgumentNullException>(
            () =>
                Validator.Validate<SampleInput, string, int>(
                    null!,
                    (static x => x.Name, static _ => SampleData.ValidInt())
                ),
            "Expected null value to throw.",
            "value"
        );

        return Task.CompletedTask;
    }

    [Test]
    public Task Validate_throws_for_null_selector()
    {
        var input = SampleData.CreateValid();

        TestAssert.Throws<ArgumentNullException>(
            () =>
                Validator.Validate<SampleInput, string, int>(
                    input,
                    (propertySelector: null!, predicate: static _ => SampleData.ValidInt())
                ),
            "Expected null selector to throw.",
            "propertySelector"
        );

        return Task.CompletedTask;
    }

    [Test]
    public Task Validate_throws_for_null_predicate()
    {
        var input = SampleData.CreateValid();

        TestAssert.Throws<ArgumentNullException>(
            () => input.Validate((static x => x.Name, predicate: (Func<string, FailOr<int>>)null!)),
            "Expected null predicate to throw.",
            "predicate"
        );

        return Task.CompletedTask;
    }

    [Test]
    public async Task ValidateAsync_throws_for_null_predicate()
    {
        var input = SampleData.CreateValid();

        await TestAssert.ThrowsAsync<ArgumentNullException>(
            () =>
                input.ValidateAsync(
                    (static x => x.Name, predicateAsync: (Func<string, Task<FailOr<int>>>)null!)
                ),
            "Expected null async predicate to throw.",
            "predicateAsync"
        );
    }

    [Test]
    public Task Validate_throws_for_invalid_selector()
    {
        var input = SampleData.CreateValid();

        TestAssert.Throws<ArgumentException>(
            () =>
                input.Validate(
                    (static x => x.Name.ToUpperInvariant(), static _ => SampleData.ValidInt())
                ),
            "Expected invalid selector to throw.",
            "propertySelector"
        );

        return Task.CompletedTask;
    }

    private static FailOr<SampleInput> InvokeValidateSync(int arity, SampleInput input) =>
        arity switch
        {
            1 => input.Validate((static x => x.Name, static _ => SampleData.ValidInt())),
            2 => input.Validate(
                (static x => x.Name, static _ => SampleData.ValidInt()),
                (static x => x.Nickname!, static _ => SampleData.ValidInt())
            ),
            3 => input.Validate(
                (static x => x.Name, static _ => SampleData.ValidInt()),
                (static x => x.Nickname!, static _ => SampleData.ValidInt()),
                (static x => x.Address.City, static _ => SampleData.ValidInt())
            ),
            4 => input.Validate(
                (static x => x.Name, static _ => SampleData.ValidInt()),
                (static x => x.Nickname!, static _ => SampleData.ValidInt()),
                (static x => x.Address.City, static _ => SampleData.ValidInt()),
                (static x => x.Country, static _ => SampleData.ValidInt())
            ),
            5 => input.Validate(
                (static x => x.Name, static _ => SampleData.ValidInt()),
                (static x => x.Nickname!, static _ => SampleData.ValidInt()),
                (static x => x.Address.City, static _ => SampleData.ValidInt()),
                (static x => x.Country, static _ => SampleData.ValidInt()),
                (static x => x.PostalCode, static _ => SampleData.ValidInt())
            ),
            6 => input.Validate(
                (static x => x.Name, static _ => SampleData.ValidInt()),
                (static x => x.Nickname!, static _ => SampleData.ValidInt()),
                (static x => x.Address.City, static _ => SampleData.ValidInt()),
                (static x => x.Country, static _ => SampleData.ValidInt()),
                (static x => x.PostalCode, static _ => SampleData.ValidInt()),
                (static x => x.Region, static _ => SampleData.ValidInt())
            ),
            7 => input.Validate(
                (static x => x.Name, static _ => SampleData.ValidInt()),
                (static x => x.Nickname!, static _ => SampleData.ValidInt()),
                (static x => x.Address.City, static _ => SampleData.ValidInt()),
                (static x => x.Country, static _ => SampleData.ValidInt()),
                (static x => x.PostalCode, static _ => SampleData.ValidInt()),
                (static x => x.Region, static _ => SampleData.ValidInt()),
                (static x => x.Email, static _ => SampleData.ValidInt())
            ),
            8 => input.Validate(
                (static x => x.Name, static _ => SampleData.ValidInt()),
                (static x => x.Nickname!, static _ => SampleData.ValidInt()),
                (static x => x.Address.City, static _ => SampleData.ValidInt()),
                (static x => x.Country, static _ => SampleData.ValidInt()),
                (static x => x.PostalCode, static _ => SampleData.ValidInt()),
                (static x => x.Region, static _ => SampleData.ValidInt()),
                (static x => x.Email, static _ => SampleData.ValidInt()),
                (static x => x.Phone, static _ => SampleData.ValidInt())
            ),
            9 => input.Validate(
                (static x => x.Name, static _ => SampleData.ValidInt()),
                (static x => x.Nickname!, static _ => SampleData.ValidInt()),
                (static x => x.Address.City, static _ => SampleData.ValidInt()),
                (static x => x.Country, static _ => SampleData.ValidInt()),
                (static x => x.PostalCode, static _ => SampleData.ValidInt()),
                (static x => x.Region, static _ => SampleData.ValidInt()),
                (static x => x.Email, static _ => SampleData.ValidInt()),
                (static x => x.Phone, static _ => SampleData.ValidInt()),
                (static x => x.Department, static _ => SampleData.ValidInt())
            ),
            10 => input.Validate(
                (static x => x.Name, static _ => SampleData.ValidInt()),
                (static x => x.Nickname!, static _ => SampleData.ValidInt()),
                (static x => x.Address.City, static _ => SampleData.ValidInt()),
                (static x => x.Country, static _ => SampleData.ValidInt()),
                (static x => x.PostalCode, static _ => SampleData.ValidInt()),
                (static x => x.Region, static _ => SampleData.ValidInt()),
                (static x => x.Email, static _ => SampleData.ValidInt()),
                (static x => x.Phone, static _ => SampleData.ValidInt()),
                (static x => x.Department, static _ => SampleData.ValidInt()),
                (static x => x.Title, static _ => SampleData.ValidInt())
            ),
            11 => input.Validate(
                (static x => x.Name, static _ => SampleData.ValidInt()),
                (static x => x.Nickname!, static _ => SampleData.ValidInt()),
                (static x => x.Address.City, static _ => SampleData.ValidInt()),
                (static x => x.Country, static _ => SampleData.ValidInt()),
                (static x => x.PostalCode, static _ => SampleData.ValidInt()),
                (static x => x.Region, static _ => SampleData.ValidInt()),
                (static x => x.Email, static _ => SampleData.ValidInt()),
                (static x => x.Phone, static _ => SampleData.ValidInt()),
                (static x => x.Department, static _ => SampleData.ValidInt()),
                (static x => x.Title, static _ => SampleData.ValidInt()),
                (static x => x.Language, static _ => SampleData.ValidInt())
            ),
            12 => input.Validate(
                (static x => x.Name, static _ => SampleData.ValidInt()),
                (static x => x.Nickname!, static _ => SampleData.ValidInt()),
                (static x => x.Address.City, static _ => SampleData.ValidInt()),
                (static x => x.Country, static _ => SampleData.ValidInt()),
                (static x => x.PostalCode, static _ => SampleData.ValidInt()),
                (static x => x.Region, static _ => SampleData.ValidInt()),
                (static x => x.Email, static _ => SampleData.ValidInt()),
                (static x => x.Phone, static _ => SampleData.ValidInt()),
                (static x => x.Department, static _ => SampleData.ValidInt()),
                (static x => x.Title, static _ => SampleData.ValidInt()),
                (static x => x.Language, static _ => SampleData.ValidInt()),
                (static x => x.TimeZone, static _ => SampleData.ValidInt())
            ),
            13 => input.Validate(
                (static x => x.Name, static _ => SampleData.ValidInt()),
                (static x => x.Nickname!, static _ => SampleData.ValidInt()),
                (static x => x.Address.City, static _ => SampleData.ValidInt()),
                (static x => x.Country, static _ => SampleData.ValidInt()),
                (static x => x.PostalCode, static _ => SampleData.ValidInt()),
                (static x => x.Region, static _ => SampleData.ValidInt()),
                (static x => x.Email, static _ => SampleData.ValidInt()),
                (static x => x.Phone, static _ => SampleData.ValidInt()),
                (static x => x.Department, static _ => SampleData.ValidInt()),
                (static x => x.Title, static _ => SampleData.ValidInt()),
                (static x => x.Language, static _ => SampleData.ValidInt()),
                (static x => x.TimeZone, static _ => SampleData.ValidInt()),
                (static x => x.Currency, static _ => SampleData.ValidInt())
            ),
            14 => input.Validate(
                (static x => x.Name, static _ => SampleData.ValidInt()),
                (static x => x.Nickname!, static _ => SampleData.ValidInt()),
                (static x => x.Address.City, static _ => SampleData.ValidInt()),
                (static x => x.Country, static _ => SampleData.ValidInt()),
                (static x => x.PostalCode, static _ => SampleData.ValidInt()),
                (static x => x.Region, static _ => SampleData.ValidInt()),
                (static x => x.Email, static _ => SampleData.ValidInt()),
                (static x => x.Phone, static _ => SampleData.ValidInt()),
                (static x => x.Department, static _ => SampleData.ValidInt()),
                (static x => x.Title, static _ => SampleData.ValidInt()),
                (static x => x.Language, static _ => SampleData.ValidInt()),
                (static x => x.TimeZone, static _ => SampleData.ValidInt()),
                (static x => x.Currency, static _ => SampleData.ValidInt()),
                (static x => x.Manager, static _ => SampleData.ValidInt())
            ),
            _ => throw new ArgumentOutOfRangeException(nameof(arity)),
        };

    private static Task<FailOr<SampleInput>> InvokeValidateAsync(int arity, SampleInput input) =>
        arity switch
        {
            1 => input.ValidateAsync((static x => x.Name, static _ => SampleData.ValidIntAsync())),
            2 => input.ValidateAsync(
                (static x => x.Name, static _ => SampleData.ValidIntAsync()),
                (static x => x.Nickname!, static _ => SampleData.ValidIntAsync())
            ),
            3 => input.ValidateAsync(
                (static x => x.Name, static _ => SampleData.ValidIntAsync()),
                (static x => x.Nickname!, static _ => SampleData.ValidIntAsync()),
                (static x => x.Address.City, static _ => SampleData.ValidIntAsync())
            ),
            4 => input.ValidateAsync(
                (static x => x.Name, static _ => SampleData.ValidIntAsync()),
                (static x => x.Nickname!, static _ => SampleData.ValidIntAsync()),
                (static x => x.Address.City, static _ => SampleData.ValidIntAsync()),
                (static x => x.Country, static _ => SampleData.ValidIntAsync())
            ),
            5 => input.ValidateAsync(
                (static x => x.Name, static _ => SampleData.ValidIntAsync()),
                (static x => x.Nickname!, static _ => SampleData.ValidIntAsync()),
                (static x => x.Address.City, static _ => SampleData.ValidIntAsync()),
                (static x => x.Country, static _ => SampleData.ValidIntAsync()),
                (static x => x.PostalCode, static _ => SampleData.ValidIntAsync())
            ),
            6 => input.ValidateAsync(
                (static x => x.Name, static _ => SampleData.ValidIntAsync()),
                (static x => x.Nickname!, static _ => SampleData.ValidIntAsync()),
                (static x => x.Address.City, static _ => SampleData.ValidIntAsync()),
                (static x => x.Country, static _ => SampleData.ValidIntAsync()),
                (static x => x.PostalCode, static _ => SampleData.ValidIntAsync()),
                (static x => x.Region, static _ => SampleData.ValidIntAsync())
            ),
            7 => input.ValidateAsync(
                (static x => x.Name, static _ => SampleData.ValidIntAsync()),
                (static x => x.Nickname!, static _ => SampleData.ValidIntAsync()),
                (static x => x.Address.City, static _ => SampleData.ValidIntAsync()),
                (static x => x.Country, static _ => SampleData.ValidIntAsync()),
                (static x => x.PostalCode, static _ => SampleData.ValidIntAsync()),
                (static x => x.Region, static _ => SampleData.ValidIntAsync()),
                (static x => x.Email, static _ => SampleData.ValidIntAsync())
            ),
            8 => input.ValidateAsync(
                (static x => x.Name, static _ => SampleData.ValidIntAsync()),
                (static x => x.Nickname!, static _ => SampleData.ValidIntAsync()),
                (static x => x.Address.City, static _ => SampleData.ValidIntAsync()),
                (static x => x.Country, static _ => SampleData.ValidIntAsync()),
                (static x => x.PostalCode, static _ => SampleData.ValidIntAsync()),
                (static x => x.Region, static _ => SampleData.ValidIntAsync()),
                (static x => x.Email, static _ => SampleData.ValidIntAsync()),
                (static x => x.Phone, static _ => SampleData.ValidIntAsync())
            ),
            9 => input.ValidateAsync(
                (static x => x.Name, static _ => SampleData.ValidIntAsync()),
                (static x => x.Nickname!, static _ => SampleData.ValidIntAsync()),
                (static x => x.Address.City, static _ => SampleData.ValidIntAsync()),
                (static x => x.Country, static _ => SampleData.ValidIntAsync()),
                (static x => x.PostalCode, static _ => SampleData.ValidIntAsync()),
                (static x => x.Region, static _ => SampleData.ValidIntAsync()),
                (static x => x.Email, static _ => SampleData.ValidIntAsync()),
                (static x => x.Phone, static _ => SampleData.ValidIntAsync()),
                (static x => x.Department, static _ => SampleData.ValidIntAsync())
            ),
            10 => input.ValidateAsync(
                (static x => x.Name, static _ => SampleData.ValidIntAsync()),
                (static x => x.Nickname!, static _ => SampleData.ValidIntAsync()),
                (static x => x.Address.City, static _ => SampleData.ValidIntAsync()),
                (static x => x.Country, static _ => SampleData.ValidIntAsync()),
                (static x => x.PostalCode, static _ => SampleData.ValidIntAsync()),
                (static x => x.Region, static _ => SampleData.ValidIntAsync()),
                (static x => x.Email, static _ => SampleData.ValidIntAsync()),
                (static x => x.Phone, static _ => SampleData.ValidIntAsync()),
                (static x => x.Department, static _ => SampleData.ValidIntAsync()),
                (static x => x.Title, static _ => SampleData.ValidIntAsync())
            ),
            11 => input.ValidateAsync(
                (static x => x.Name, static _ => SampleData.ValidIntAsync()),
                (static x => x.Nickname!, static _ => SampleData.ValidIntAsync()),
                (static x => x.Address.City, static _ => SampleData.ValidIntAsync()),
                (static x => x.Country, static _ => SampleData.ValidIntAsync()),
                (static x => x.PostalCode, static _ => SampleData.ValidIntAsync()),
                (static x => x.Region, static _ => SampleData.ValidIntAsync()),
                (static x => x.Email, static _ => SampleData.ValidIntAsync()),
                (static x => x.Phone, static _ => SampleData.ValidIntAsync()),
                (static x => x.Department, static _ => SampleData.ValidIntAsync()),
                (static x => x.Title, static _ => SampleData.ValidIntAsync()),
                (static x => x.Language, static _ => SampleData.ValidIntAsync())
            ),
            12 => input.ValidateAsync(
                (static x => x.Name, static _ => SampleData.ValidIntAsync()),
                (static x => x.Nickname!, static _ => SampleData.ValidIntAsync()),
                (static x => x.Address.City, static _ => SampleData.ValidIntAsync()),
                (static x => x.Country, static _ => SampleData.ValidIntAsync()),
                (static x => x.PostalCode, static _ => SampleData.ValidIntAsync()),
                (static x => x.Region, static _ => SampleData.ValidIntAsync()),
                (static x => x.Email, static _ => SampleData.ValidIntAsync()),
                (static x => x.Phone, static _ => SampleData.ValidIntAsync()),
                (static x => x.Department, static _ => SampleData.ValidIntAsync()),
                (static x => x.Title, static _ => SampleData.ValidIntAsync()),
                (static x => x.Language, static _ => SampleData.ValidIntAsync()),
                (static x => x.TimeZone, static _ => SampleData.ValidIntAsync())
            ),
            13 => input.ValidateAsync(
                (static x => x.Name, static _ => SampleData.ValidIntAsync()),
                (static x => x.Nickname!, static _ => SampleData.ValidIntAsync()),
                (static x => x.Address.City, static _ => SampleData.ValidIntAsync()),
                (static x => x.Country, static _ => SampleData.ValidIntAsync()),
                (static x => x.PostalCode, static _ => SampleData.ValidIntAsync()),
                (static x => x.Region, static _ => SampleData.ValidIntAsync()),
                (static x => x.Email, static _ => SampleData.ValidIntAsync()),
                (static x => x.Phone, static _ => SampleData.ValidIntAsync()),
                (static x => x.Department, static _ => SampleData.ValidIntAsync()),
                (static x => x.Title, static _ => SampleData.ValidIntAsync()),
                (static x => x.Language, static _ => SampleData.ValidIntAsync()),
                (static x => x.TimeZone, static _ => SampleData.ValidIntAsync()),
                (static x => x.Currency, static _ => SampleData.ValidIntAsync())
            ),
            14 => input.ValidateAsync(
                (static x => x.Name, static _ => SampleData.ValidIntAsync()),
                (static x => x.Nickname!, static _ => SampleData.ValidIntAsync()),
                (static x => x.Address.City, static _ => SampleData.ValidIntAsync()),
                (static x => x.Country, static _ => SampleData.ValidIntAsync()),
                (static x => x.PostalCode, static _ => SampleData.ValidIntAsync()),
                (static x => x.Region, static _ => SampleData.ValidIntAsync()),
                (static x => x.Email, static _ => SampleData.ValidIntAsync()),
                (static x => x.Phone, static _ => SampleData.ValidIntAsync()),
                (static x => x.Department, static _ => SampleData.ValidIntAsync()),
                (static x => x.Title, static _ => SampleData.ValidIntAsync()),
                (static x => x.Language, static _ => SampleData.ValidIntAsync()),
                (static x => x.TimeZone, static _ => SampleData.ValidIntAsync()),
                (static x => x.Currency, static _ => SampleData.ValidIntAsync()),
                (static x => x.Manager, static _ => SampleData.ValidIntAsync())
            ),
            _ => throw new ArgumentOutOfRangeException(nameof(arity)),
        };
}

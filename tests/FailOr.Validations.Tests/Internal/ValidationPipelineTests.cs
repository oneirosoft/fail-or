namespace FailOr.Validations.Tests.Internal;

public class ValidationPipelineTests
{
    [Test]
    public Task Validate_returns_the_original_instance_when_all_rules_succeed()
    {
        var input = SampleData.CreateValid();
        var rules = new[]
        {
            ValidationRule<SampleInput>.Create(
                static x => x.Name,
                static _ => SampleData.ValidBool()
            ),
            ValidationRule<SampleInput>.Create(
                static x => x.Nickname!,
                static _ => SampleData.ValidBool()
            ),
        };

        var result = ValidationPipeline.Validate(input, rules);

        TestAssert.False(result.IsFailure, "Expected pipeline validation to succeed.");
        TestAssert.Same(input, result.UnsafeUnwrap(), "Expected the original instance.");

        return Task.CompletedTask;
    }

    [Test]
    public Task Validate_aggregates_failures_in_rule_order()
    {
        var input = SampleData.CreateValid();
        var rules = new[]
        {
            ValidationRule<SampleInput>.Create(
                static x => x.Name,
                static _ => SampleData.ValidationBool("name-invalid")
            ),
            ValidationRule<SampleInput>.Create(
                static x => x.Nickname!,
                static _ => SampleData.GeneralBool("Nickname.Unexpected", "boom")
            ),
            ValidationRule<SampleInput>.Create(
                static x => x.Country,
                static _ => SampleData.ValidationBool("country-invalid")
            ),
        };

        var result = ValidationPipeline.Validate(input, rules);

        TestAssert.True(result.IsFailure, "Expected pipeline validation to fail.");
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
            "Country",
            "country-invalid",
            "Unexpected third failure."
        );

        return Task.CompletedTask;
    }

    [Test]
    public async Task ValidateAsync_executes_rules_sequentially()
    {
        var input = SampleData.CreateValid();
        var order = new List<string>();
        var rules = new[]
        {
            AsyncValidationRule<SampleInput>.Create(
                static x => x.Name,
                async _ =>
                {
                    order.Add("start-1");
                    await Task.Yield();
                    order.Add("end-1");
                    return SampleData.ValidationBool("name-invalid");
                }
            ),
            AsyncValidationRule<SampleInput>.Create(
                static x => x.Country,
                async _ =>
                {
                    order.Add("start-2");
                    await Task.Yield();
                    order.Add("end-2");
                    return SampleData.ValidationBool("country-invalid");
                }
            ),
        };

        var result = await ValidationPipeline.ValidateAsync(input, rules);

        TestAssert.True(result.IsFailure, "Expected async pipeline validation to fail.");
        TestAssert.Equal(
            "start-1,end-1,start-2,end-2",
            string.Join(",", order),
            "Expected sequential async execution."
        );
    }

    [Test]
    public Task NormalizeFailures_returns_no_failures_for_success_results()
    {
        var normalized = ValidationPipeline.NormalizeFailures("Name", SampleData.ValidBool());

        TestAssert.Equal(0, normalized.Length, "Expected no normalized failures.");
        return Task.CompletedTask;
    }

    [Test]
    public Task NormalizeFailures_rewrites_validation_failures_with_the_selected_property_name()
    {
        var normalized = ValidationPipeline.NormalizeFailures(
            "City",
            SampleData.ValidationBool("city-invalid")
        );

        TestAssert.Equal(1, normalized.Length, "Expected one normalized failure.");
        TestAssert.ValidationFailure(
            normalized[0],
            "City",
            "city-invalid",
            "Unexpected normalized validation failure."
        );

        return Task.CompletedTask;
    }

    [Test]
    public Task NormalizeFailures_preserves_non_validation_failures()
    {
        var normalized = ValidationPipeline.NormalizeFailures(
            "City",
            SampleData.GeneralBool("City.Unexpected", "boom")
        );

        TestAssert.Equal(1, normalized.Length, "Expected one normalized failure.");
        TestAssert.GeneralFailure(
            normalized[0],
            "City.Unexpected",
            "boom",
            "Unexpected normalized non-validation failure."
        );

        return Task.CompletedTask;
    }
}

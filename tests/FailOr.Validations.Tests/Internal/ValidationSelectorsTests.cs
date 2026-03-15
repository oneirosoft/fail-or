namespace FailOr.Validations.Tests.Internal;

public class ValidationSelectorsTests
{
    [Test]
    public Task GetPropertyName_returns_the_direct_property_name()
    {
        var propertyName = ValidationSelectors.GetPropertyName<SampleInput, string>(static x =>
            x.Name
        );

        TestAssert.Equal("Name", propertyName, "Unexpected direct property name.");
        return Task.CompletedTask;
    }

    [Test]
    public Task GetPropertyName_returns_the_leaf_property_name_for_nested_access()
    {
        var propertyName = ValidationSelectors.GetPropertyName<SampleInput, string>(static x =>
            x.Address.City
        );

        TestAssert.Equal("City", propertyName, "Unexpected nested property name.");
        return Task.CompletedTask;
    }

    [Test]
    public Task GetPropertyName_throws_for_boxed_value_type_access()
    {
        TestAssert.Throws<ArgumentException>(
            () => ValidationSelectors.GetPropertyName<SampleInput, object?>(static x => x.Age),
            "Expected boxed value-type access to throw.",
            "propertySelector"
        );

        return Task.CompletedTask;
    }

    [Test]
    public Task GetPropertyName_throws_for_non_property_expressions()
    {
        TestAssert.Throws<ArgumentException>(
            () =>
                ValidationSelectors.GetPropertyName<SampleInput, string>(static x =>
                    x.Name.ToUpperInvariant()
                ),
            "Expected method-call selector to throw.",
            "propertySelector"
        );

        return Task.CompletedTask;
    }
}

using TUnit.Assertions;
using TUnit.Assertions.Extensions;
using TUnit.Core;

namespace FailOr.Tests;

public class FailOrFactoryTests
{
    [Test]
    [Arguments(1)]
    [Arguments(42)]
    public async Task Success_creates_success_for_value_types(int value)
    {
        var result = FailOr.Success(value);

        using var _ = Assert.Multiple();
        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(result.IsFailure).IsFalse();
        await Assert.That(result.UnsafeUnwrap()).IsEqualTo(value);
        await Assert.That(result.Failures.Count).IsEqualTo(0);
    }

    [Test]
    [Arguments("alpha")]
    [Arguments("beta")]
    public async Task Success_creates_success_for_reference_types(string value)
    {
        var result = FailOr.Success(value);

        using var _ = Assert.Multiple();
        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(result.IsFailure).IsFalse();
        await Assert.That(result.UnsafeUnwrap()).IsEqualTo(value);
        await Assert.That(result.Failures.Count).IsEqualTo(0);
    }

    [Test]
    public async Task Success_throws_for_null_reference_values()
    {
        Action invoke = () => FailOr.Success<string>(null!);

        await Assert.That(invoke).Throws<ArgumentNullException>().WithParameterName("value");
    }

    [Test]
    [MethodDataSource(typeof(CoreTestData), nameof(CoreTestData.FailureFactoryCases))]
    public async Task Fail_factories_create_failed_results(
        string operation,
        Func<Failure[], FailOr<int>> create
    )
    {
        var firstFailure = Failure.General("first");
        var secondFailure = Failure.General("second");

        var result = create([firstFailure, secondFailure]);

        using var _ = Assert.Multiple();
        await Assert.That(result.IsFailure).IsTrue();
        await Assert.That(result.IsSuccess).IsFalse();
        await Assert.That(result.Failures.Count).IsEqualTo(operation == "single failure" ? 1 : 2);
        await Assert.That(result.Failures[0]).IsEqualTo(firstFailure);

        if (operation != "single failure")
        {
            await Assert.That(result.Failures[1]).IsEqualTo(secondFailure);
        }
    }

    [Test]
    [MethodDataSource(typeof(CoreTestData), nameof(CoreTestData.FailureFactoryGuardCases))]
    public async Task Fail_factories_validate_inputs(
        string operation,
        Action invoke,
        string parameterName
    )
    {
        if (operation == "null enumerable")
        {
            await Assert
                .That(invoke)
                .Throws<ArgumentNullException>()
                .WithParameterName(parameterName);

            return;
        }

        await Assert.That(invoke).Throws<ArgumentException>().WithParameterName(parameterName);
    }

    [Test]
    public async Task UnsafeUnwrap_throws_for_failed_results()
    {
        var result = FailOr<int>.Fail(Failure.General("unwrap failed"));

        await Assert
            .That(() => result.UnsafeUnwrap())
            .Throws<InvalidOperationException>()
            .WithMessage("A failed FailOr does not contain a value.");
    }

    [Test]
    public async Task Failures_exposes_read_only_failure_view()
    {
        var failure = Failure.General("readonly");
        var result = FailOr<int>.Fail(failure);

        using var _ = Assert.Multiple();
        await Assert.That(result.Failures).IsAssignableTo<IReadOnlyList<Failure>>();
        await Assert.That(result.Failures.Count).IsEqualTo(1);
        await Assert.That(result.Failures[0]).IsEqualTo(failure);
    }
}

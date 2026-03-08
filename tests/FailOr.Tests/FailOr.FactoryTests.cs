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
    [Arguments(1)]
    [Arguments(42)]
    public async Task Implicit_success_conversion_creates_success_for_value_types(int value)
    {
        FailOr<int> result = value;

        using var _ = Assert.Multiple();
        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(result.IsFailure).IsFalse();
        await Assert.That(result.UnsafeUnwrap()).IsEqualTo(value);
        await Assert.That(result.Failures.Count).IsEqualTo(0);
    }

    [Test]
    [Arguments("alpha")]
    [Arguments("beta")]
    public async Task Implicit_success_conversion_creates_success_for_reference_types(string value)
    {
        FailOr<string> result = value;

        using var _ = Assert.Multiple();
        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(result.IsFailure).IsFalse();
        await Assert.That(result.UnsafeUnwrap()).IsEqualTo(value);
        await Assert.That(result.Failures.Count).IsEqualTo(0);
    }

    [Test]
    public async Task Implicit_success_conversion_throws_for_null_reference_values()
    {
        Action invoke = () =>
        {
            FailOr<string> result = (string)null!;
            _ = result;
        };

        await Assert.That(invoke).Throws<ArgumentNullException>().WithParameterName("value");
    }

    [Test]
    [MethodDataSource(typeof(CoreTestData), nameof(CoreTestData.FailureFactoryCases))]
    public async Task Fail_factories_create_failed_results(
        string operation,
        Func<Failures[], FailOr<int>> create
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
        await Assert.That(result.Failures).IsAssignableTo<IReadOnlyList<Failures>>();
        await Assert.That(result.Failures.Count).IsEqualTo(1);
        await Assert.That(result.Failures[0]).IsEqualTo(failure);
    }

    [Test]
    public async Task Fail_single_failure_overload_throws_for_null_failure()
    {
        Action invoke = () => FailOr<int>.Fail((Failures)null!);

        await Assert.That(invoke).Throws<ArgumentNullException>().WithParameterName("failure");
    }

    [Test]
    public async Task Fail_params_overload_throws_for_null_array()
    {
        Action invoke = () => FailOr<int>.Fail((Failures[])null!);

        await Assert.That(invoke).Throws<ArgumentNullException>().WithParameterName("failures");
    }

    [Test]
    public async Task Fail_enumerable_overload_throws_for_null_elements()
    {
        Action invoke = () =>
            FailOr<int>.Fail(new Failures[] { Failure.General("first"), null! }.AsEnumerable());

        await Assert.That(invoke).Throws<ArgumentException>().WithParameterName("failures");
    }

    [Test]
    public async Task Fail_params_overload_throws_for_null_elements()
    {
        Action invoke = () => FailOr<int>.Fail(Failure.General("first"), null!);

        await Assert.That(invoke).Throws<ArgumentException>().WithParameterName("failures");
    }

    [Test]
    public async Task Fail_can_capture_heterogeneous_failure_values()
    {
        var validation = Failure.Validation("Email", "Email is required.", "Email is invalid.");
        var exceptional = Failure.Exceptional(new InvalidOperationException("broken"));
        var result = FailOr<int>.Fail(validation, exceptional);

        using var _ = Assert.Multiple();
        await Assert.That(result.IsFailure).IsTrue();
        await Assert.That(result.Failures.Count).IsEqualTo(2);
        await Assert.That(result.Failures[0]).IsEqualTo(validation);
        await Assert.That(result.Failures[1]).IsEqualTo(exceptional);
    }

    [Test]
    public async Task Implicit_single_failure_conversion_creates_failed_result()
    {
        var failure = Failure.General("single");
        FailOr<int> result = failure;

        using var _ = Assert.Multiple();
        await Assert.That(result.IsFailure).IsTrue();
        await Assert.That(result.IsSuccess).IsFalse();
        await Assert.That(result.Failures.Count).IsEqualTo(1);
        await Assert.That(result.Failures[0]).IsEqualTo(failure);
    }

    [Test]
    public async Task Implicit_failure_array_conversion_preserves_order()
    {
        var firstFailure = Failure.General("first");
        var secondFailure = Failure.General("second");
        FailOr<int> result = new Failures[] { firstFailure, secondFailure };

        using var _ = Assert.Multiple();
        await Assert.That(result.IsFailure).IsTrue();
        await Assert.That(result.Failures.Count).IsEqualTo(2);
        await Assert.That(result.Failures[0]).IsEqualTo(firstFailure);
        await Assert.That(result.Failures[1]).IsEqualTo(secondFailure);
    }

    [Test]
    public async Task Implicit_failure_list_conversion_preserves_order()
    {
        var firstFailure = Failure.General("first");
        var secondFailure = Failure.General("second");
        FailOr<int> result = new List<Failures> { firstFailure, secondFailure };

        using var _ = Assert.Multiple();
        await Assert.That(result.IsFailure).IsTrue();
        await Assert.That(result.Failures.Count).IsEqualTo(2);
        await Assert.That(result.Failures[0]).IsEqualTo(firstFailure);
        await Assert.That(result.Failures[1]).IsEqualTo(secondFailure);
    }

    [Test]
    public async Task Implicit_failure_array_conversion_throws_for_null_array()
    {
        Action invoke = () =>
        {
            FailOr<int> result = (Failures[])null!;
            _ = result;
        };

        await Assert.That(invoke).Throws<ArgumentNullException>().WithParameterName("failures");
    }

    [Test]
    public async Task Implicit_failure_list_conversion_throws_for_null_list()
    {
        Action invoke = () =>
        {
            FailOr<int> result = (List<Failures>)null!;
            _ = result;
        };

        await Assert.That(invoke).Throws<ArgumentNullException>().WithParameterName("failures");
    }

    [Test]
    public async Task Implicit_failure_array_conversion_throws_for_empty_array()
    {
        Action invoke = () =>
        {
            var failures = new Failures[] { };
            FailOr<int> result = failures;
            _ = result;
        };

        await Assert.That(invoke).Throws<ArgumentException>().WithParameterName("failures");
    }

    [Test]
    public async Task Implicit_failure_list_conversion_throws_for_empty_list()
    {
        Action invoke = () =>
        {
            FailOr<int> result = new List<Failures>();
            _ = result;
        };

        await Assert.That(invoke).Throws<ArgumentException>().WithParameterName("failures");
    }

    [Test]
    public async Task Implicit_failure_array_conversion_throws_for_null_elements()
    {
        Action invoke = () =>
        {
            var failures = new Failures[] { Failure.General("first"), null! };
            FailOr<int> result = failures;
            _ = result;
        };

        await Assert.That(invoke).Throws<ArgumentException>().WithParameterName("failures");
    }

    [Test]
    public async Task Implicit_failure_list_conversion_throws_for_null_elements()
    {
        Action invoke = () =>
        {
            FailOr<int> result = new List<Failures> { Failure.General("first"), null! };
            _ = result;
        };

        await Assert.That(invoke).Throws<ArgumentException>().WithParameterName("failures");
    }
}

using System.Reflection;
using TUnit.Assertions;
using TUnit.Assertions.Extensions;

namespace FailOr.Tests;

public class FailureTests
{
    [Test]
    [Arguments("boom")]
    [Arguments("bad request")]
    public async Task General_creates_failure_with_default_code(string details)
    {
        var failure = Failure.General(details);

        using var _ = Assert.Multiple();
        await Assert.That(failure.Code).IsEqualTo("General");
        await Assert.That(failure.Details).IsEqualTo(details);
        await Assert.That(failure.Metadata.Count).IsEqualTo(0);
    }

    [Test]
    public async Task General_creates_failure_with_custom_code_and_copied_metadata()
    {
        var metadata = new Dictionary<string, object?> { ["attempt"] = 2 };

        var failure = Failure.General("boom", "Api.Timeout", metadata);
        metadata["attempt"] = 3;
        metadata["extra"] = "later";

        using var _ = Assert.Multiple();
        await Assert.That(failure.Code).IsEqualTo("Api.Timeout");
        await Assert.That(failure.Details).IsEqualTo("boom");
        await Assert.That(failure.Metadata.Count).IsEqualTo(1);
        await Assert.That(failure.Metadata["attempt"]).IsEqualTo(2);

        Action invoke = () => ((IDictionary<string, object?>)failure.Metadata).Add("new", 1);
        await Assert.That(invoke).Throws<NotSupportedException>();
    }

    [Test]
    public async Task General_throws_for_null_details()
    {
        Action invoke = () => Failure.General(null!);

        await Assert.That(invoke).Throws<ArgumentNullException>().WithParameterName("details");
    }

    [Test]
    [Arguments("")]
    [Arguments(" ")]
    [Arguments("\t")]
    public async Task General_throws_for_blank_details(string details)
    {
        Action invoke = () => Failure.General(details);

        await Assert.That(invoke).Throws<ArgumentException>().WithParameterName("details");
    }

    [Test]
    [Arguments("")]
    [Arguments(" ")]
    [Arguments("\t")]
    public async Task General_throws_for_blank_code(string code)
    {
        Action invoke = () => Failure.General("boom", code);

        await Assert.That(invoke).Throws<ArgumentException>().WithParameterName("code");
    }

    [Test]
    public async Task Validation_creates_failure_with_expected_shape()
    {
        var failure = Failure.Validation("Email", "Email is required.", "Email is invalid.");

        using var _ = Assert.Multiple();
        await Assert.That(failure.Code).IsEqualTo("Validation.Email");
        await Assert.That(failure.Details).IsEqualTo("Email is required.; Email is invalid.");
        await Assert.That(failure.PropertyName).IsEqualTo("Email");
    }

    [Test]
    public async Task Validation_throws_for_null_property_name()
    {
        Action invoke = () => Failure.Validation(null!, "Email is required.");

        await Assert.That(invoke).Throws<ArgumentNullException>().WithParameterName("propertyName");
    }

    [Test]
    [Arguments("")]
    [Arguments(" ")]
    [Arguments("\t")]
    public async Task Validation_throws_for_blank_property_name(string propertyName)
    {
        Action invoke = () => Failure.Validation(propertyName, "Email is required.");

        await Assert.That(invoke).Throws<ArgumentException>().WithParameterName("propertyName");
    }

    [Test]
    public async Task Validation_throws_for_null_errors_array()
    {
        Action invoke = () => Failure.Validation("Email", (string[])null!);

        await Assert.That(invoke).Throws<ArgumentNullException>().WithParameterName("errors");
    }

    [Test]
    public async Task Validation_throws_for_empty_errors()
    {
        Action invoke = () => Failure.Validation("Email");

        await Assert.That(invoke).Throws<ArgumentException>().WithParameterName("errors");
    }

    [Test]
    public async Task Validation_throws_for_null_error_messages()
    {
        Action invoke = () => Failure.Validation("Email", "Email is required.", null!);

        await Assert.That(invoke).Throws<ArgumentException>().WithParameterName("errors");
    }

    [Test]
    [Arguments("")]
    [Arguments(" ")]
    [Arguments("\t")]
    public async Task Validation_throws_for_blank_error_messages(string error)
    {
        Action invoke = () => Failure.Validation("Email", "Email is required.", error);

        await Assert.That(invoke).Throws<ArgumentException>().WithParameterName("errors");
    }

    [Test]
    public async Task Exceptional_creates_failure_with_default_values()
    {
        var exception = new InvalidOperationException("Operation failed.");

        var failure = Failure.Exceptional(exception);

        using var _ = Assert.Multiple();
        await Assert.That(failure.Code).IsEqualTo("Exceptional");
        await Assert.That(failure.Details).IsEqualTo("Operation failed.");
        await Assert.That(ReferenceEquals(failure.Exception, exception)).IsTrue();
    }

    [Test]
    public async Task Exceptional_uses_exception_type_name_when_message_is_blank()
    {
        var exception = new InvalidOperationException(" ");

        var failure = Failure.Exceptional(exception);

        using var _ = Assert.Multiple();
        await Assert.That(failure.Code).IsEqualTo("Exceptional");
        await Assert.That(failure.Details).IsEqualTo(nameof(InvalidOperationException));
        await Assert.That(ReferenceEquals(failure.Exception, exception)).IsTrue();
    }

    [Test]
    public async Task Exceptional_creates_failure_with_custom_details_and_code()
    {
        var exception = new InvalidOperationException("Operation failed.");

        var failure = Failure.Exceptional(exception, "Outer failure", "Exceptional.Timeout");

        using var _ = Assert.Multiple();
        await Assert.That(failure.Code).IsEqualTo("Exceptional.Timeout");
        await Assert.That(failure.Details).IsEqualTo("Outer failure");
        await Assert.That(ReferenceEquals(failure.Exception, exception)).IsTrue();
    }

    [Test]
    public async Task Exceptional_throws_for_null_exception()
    {
        Action invoke = () => Failure.Exceptional(null!);

        await Assert.That(invoke).Throws<ArgumentNullException>().WithParameterName("exception");
    }

    [Test]
    [Arguments("")]
    [Arguments(" ")]
    [Arguments("\t")]
    public async Task Exceptional_throws_for_blank_details(string details)
    {
        Action invoke = () =>
            Failure.Exceptional(new InvalidOperationException("Operation failed."), details);

        await Assert.That(invoke).Throws<ArgumentException>().WithParameterName("details");
    }

    [Test]
    [Arguments("")]
    [Arguments(" ")]
    [Arguments("\t")]
    public async Task Exceptional_throws_for_blank_code(string code)
    {
        Action invoke = () =>
            Failure.Exceptional(new InvalidOperationException("Operation failed."), code: code);

        await Assert.That(invoke).Throws<ArgumentException>().WithParameterName("code");
    }

    [Test]
    public async Task Failure_union_cases_expose_non_public_primary_constructors()
    {
        using var _ = Assert.Multiple();

        await Assert.That(GetNonPublicConstructor<Failures.General>()).IsNotNull();
        await Assert.That(GetNonPublicConstructor<Failures.Validation>()).IsNotNull();
        await Assert.That(GetNonPublicConstructor<Failures.Exceptional>()).IsNotNull();

        await Assert.That(typeof(Failures.General).GetConstructors().Length).IsEqualTo(0);
        await Assert.That(typeof(Failures.Validation).GetConstructors().Length).IsEqualTo(0);
        await Assert.That(typeof(Failures.Exceptional).GetConstructors().Length).IsEqualTo(0);
    }

    private static ConstructorInfo? GetNonPublicConstructor<TFailure>()
        where TFailure : Failures =>
        typeof(TFailure)
            .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)
            .FirstOrDefault(static constructor => constructor.IsAssembly);
}

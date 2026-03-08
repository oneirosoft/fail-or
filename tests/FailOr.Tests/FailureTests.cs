using TUnit.Assertions;
using TUnit.Assertions.Extensions;

namespace FailOr.Tests;

public class FailureTests
{
    [Test]
    [Arguments("boom")]
    [Arguments("bad request")]
    public async Task General_creates_failure_with_expected_metadata(string details)
    {
        var failure = Failure.General(details);

        await Assert
            .That(failure)
            .Satisfies(value => value.Code == "General" && value.Details == details);
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
}

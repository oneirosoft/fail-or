namespace FailOr.Tests;

public sealed class FailOrZipTests
{
    [Test]
    public async Task Zip_Two_Returns_Tuple_When_Both_Succeed()
    {
        var result = FailOr.Zip(FailOr.Success(1), FailOr.Success("two"));

        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(result.UnsafeUnwrap()).IsEqualTo((1, "two"));
    }

    [Test]
    public async Task Zip_Two_Aggregates_Failures_In_Left_To_Right_Order()
    {
        var leftFailure = Failure.General("left failed");
        var rightFailure = Failure.General("right failed");

        var result = FailOr.Zip(FailOr.Fail<int>(leftFailure), FailOr.Fail<string>(rightFailure));

        await Assert.That(result.IsFailure).IsTrue();
        await Assert.That(result.Failures.Count).IsEqualTo(2);
        await Assert.That(result.Failures[0]).IsEqualTo(leftFailure);
        await Assert.That(result.Failures[1]).IsEqualTo(rightFailure);
    }

    [Test]
    public async Task Zip_Two_Returns_Single_Failure_Unchanged_When_Only_One_Input_Fails()
    {
        var failure = Failure.General("left failed");

        var result = FailOr.Zip(FailOr.Fail<int>(failure), FailOr.Success("two"));

        await Assert.That(result.IsFailure).IsTrue();
        await Assert.That(result.Failures.Count).IsEqualTo(1);
        await Assert.That(result.Failures[0]).IsEqualTo(failure);
    }

    [Test]
    public async Task Zip_Seven_Returns_Full_Tuple_When_All_Succeed()
    {
        var result = FailOr.Zip(
            FailOr.Success(1),
            FailOr.Success("two"),
            FailOr.Success(true),
            FailOr.Success(4.0m),
            FailOr.Success('5'),
            FailOr.Success(6L),
            FailOr.Success(7u)
        );

        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(result.UnsafeUnwrap()).IsEqualTo((1, "two", true, 4.0m, '5', 6L, 7u));
    }

    [Test]
    public async Task Zip_Seven_Aggregates_Failures_Across_All_Failed_Inputs()
    {
        var firstFailure = Failure.General("first failed");
        var thirdFailure = Failure.General("third failed");
        var sixthFailure = Failure.General("sixth failed");

        var result = FailOr.Zip(
            FailOr.Fail<int>(firstFailure),
            FailOr.Success("two"),
            FailOr.Fail<bool>(thirdFailure),
            FailOr.Success(4.0m),
            FailOr.Success('5'),
            FailOr.Fail<long>(sixthFailure),
            FailOr.Success(7u)
        );

        await Assert.That(result.IsFailure).IsTrue();
        await Assert.That(result.Failures.Count).IsEqualTo(3);
        await Assert.That(result.Failures[0]).IsEqualTo(firstFailure);
        await Assert.That(result.Failures[1]).IsEqualTo(thirdFailure);
        await Assert.That(result.Failures[2]).IsEqualTo(sixthFailure);
    }
}

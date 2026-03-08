namespace FailOr.Tests;

public sealed class FailOrCombineTests
{
    [Test]
    public async Task Combine_Returns_Left_When_Both_Succeed()
    {
        var left = FailOr.Success(1);
        var right = FailOr.Success(2);

        var result = FailOr.Combine(left, right);

        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(result.UnsafeUnwrap()).IsEqualTo(1);
    }

    [Test]
    public async Task Combine_Returns_Left_When_Left_Succeeds_And_Right_Fails()
    {
        var left = FailOr.Success(1);
        var right = FailOr.Fail<int>(Failure.General("right failed"));

        var result = FailOr.Combine(left, right);

        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(result.UnsafeUnwrap()).IsEqualTo(1);
    }

    [Test]
    public async Task Combine_Returns_Right_When_Left_Fails_And_Right_Succeeds()
    {
        var left = FailOr.Fail<int>(Failure.General("left failed"));
        var right = FailOr.Success(2);

        var result = FailOr.Combine(left, right);

        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(result.UnsafeUnwrap()).IsEqualTo(2);
    }

    [Test]
    public async Task Combine_Returns_Right_Failure_When_Both_Fail()
    {
        var left = FailOr.Fail<int>(Failure.General("left failed"));
        var rightFailure = Failure.General("right failed");
        var right = FailOr.Fail<int>(rightFailure);

        var result = FailOr.Combine(left, right);

        await Assert.That(result.IsFailure).IsTrue();
        await Assert.That(result.Failures.Count).IsEqualTo(1);
        await Assert.That(result.Failures[0]).IsEqualTo(rightFailure);
    }
}

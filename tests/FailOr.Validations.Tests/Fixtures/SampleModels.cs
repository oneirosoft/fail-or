namespace FailOr.Validations.Tests.Fixtures;

internal sealed class SampleInput
{
    public string Name { get; init; } = string.Empty;

    public string? Nickname { get; init; }

    public int Age { get; init; }

    public SampleAddress Address { get; init; } = new();

    public string Country { get; init; } = string.Empty;

    public string PostalCode { get; init; } = string.Empty;

    public string Region { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public string Phone { get; init; } = string.Empty;

    public string Department { get; init; } = string.Empty;

    public string Title { get; init; } = string.Empty;

    public string Language { get; init; } = string.Empty;

    public string TimeZone { get; init; } = string.Empty;

    public string Currency { get; init; } = string.Empty;

    public string Manager { get; init; } = string.Empty;
}

internal sealed class SampleAddress
{
    public string City { get; init; } = string.Empty;
}

internal static class SampleData
{
    public static SampleInput CreateValid() =>
        new()
        {
            Name = "Ada",
            Nickname = "Ace",
            Age = 42,
            Address = new SampleAddress { City = "Boston" },
            Country = "US",
            PostalCode = "02110",
            Region = "MA",
            Email = "ada@example.com",
            Phone = "555-0100",
            Department = "R&D",
            Title = "Engineer",
            Language = "en-US",
            TimeZone = "America/New_York",
            Currency = "USD",
            Manager = "Charles",
        };

    public static FailOr<int> ValidInt(int value = 1) => value;

    public static FailOr<bool> ValidBool(bool value = true) => value;

    public static FailOr<string> ValidString(string value) => value;

    public static FailOr<int> ValidationInt(string details) =>
        Failure.Validation("Ignored", details);

    public static FailOr<bool> ValidationBool(string details) =>
        Failure.Validation("Ignored", details);

    public static FailOr<string> ValidationString(string details) =>
        Failure.Validation("Ignored", details);

    public static FailOr<int> GeneralInt(string code, string details) =>
        Failure.General(details, code);

    public static FailOr<bool> GeneralBool(string code, string details) =>
        Failure.General(details, code);

    public static FailOr<string> GeneralString(string code, string details) =>
        Failure.General(details, code);

    public static async Task<FailOr<int>> ValidIntAsync(int value = 1)
    {
        await Task.Yield();
        return value;
    }

    public static async Task<FailOr<bool>> ValidBoolAsync(bool value = true)
    {
        await Task.Yield();
        return value;
    }

    public static async Task<FailOr<string>> ValidStringAsync(string value)
    {
        await Task.Yield();
        return value;
    }

    public static async Task<FailOr<int>> ValidationIntAsync(string details)
    {
        await Task.Yield();
        return Failure.Validation("Ignored", details);
    }

    public static async Task<FailOr<bool>> ValidationBoolAsync(string details)
    {
        await Task.Yield();
        return Failure.Validation("Ignored", details);
    }

    public static async Task<FailOr<string>> ValidationStringAsync(string details)
    {
        await Task.Yield();
        return Failure.Validation("Ignored", details);
    }

    public static async Task<FailOr<int>> GeneralIntAsync(string code, string details)
    {
        await Task.Yield();
        return Failure.General(details, code);
    }

    public static async Task<FailOr<bool>> GeneralBoolAsync(string code, string details)
    {
        await Task.Yield();
        return Failure.General(details, code);
    }

    public static async Task<FailOr<string>> GeneralStringAsync(string code, string details)
    {
        await Task.Yield();
        return Failure.General(details, code);
    }
}

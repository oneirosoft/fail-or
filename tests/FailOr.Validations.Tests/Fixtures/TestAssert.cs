namespace FailOr.Validations.Tests.Fixtures;

internal static class TestAssert
{
    public static void True(bool condition, string message)
    {
        if (!condition)
        {
            throw new InvalidOperationException(message);
        }
    }

    public static void False(bool condition, string message)
    {
        if (condition)
        {
            throw new InvalidOperationException(message);
        }
    }

    public static void Equal<T>(T expected, T actual, string message)
    {
        if (!EqualityComparer<T>.Default.Equals(expected, actual))
        {
            throw new InvalidOperationException(
                $"{message} Expected: {expected ?? default}. Actual: {actual ?? default}."
            );
        }
    }

    public static void Same(object expected, object actual, string message)
    {
        if (!ReferenceEquals(expected, actual))
        {
            throw new InvalidOperationException(message);
        }
    }

    public static void ValidationFailure(
        Failures failure,
        string propertyName,
        string details,
        string message
    )
    {
        if (failure is not Failures.Validation validation)
        {
            throw new InvalidOperationException(message);
        }

        Equal(
            $"Validation.{propertyName}",
            validation.Code,
            $"{message} Unexpected validation code."
        );
        Equal(details, validation.Details, $"{message} Unexpected validation details.");
        Equal(propertyName, validation.PropertyName, $"{message} Unexpected property name.");
    }

    public static void GeneralFailure(Failures failure, string code, string details, string message)
    {
        if (failure is not Failures.General general)
        {
            throw new InvalidOperationException(message);
        }

        Equal(code, general.Code, $"{message} Unexpected general code.");
        Equal(details, general.Details, $"{message} Unexpected general details.");
    }

    public static TException Throws<TException>(
        Action action,
        string message,
        string? parameterName = null
    )
        where TException : Exception
    {
        try
        {
            action();
        }
        catch (TException exception)
        {
            if (parameterName is not null && exception is ArgumentException argumentException)
            {
                Equal(
                    parameterName,
                    argumentException.ParamName,
                    $"{message} Unexpected parameter name."
                );
            }

            return exception;
        }

        throw new InvalidOperationException(message);
    }

    public static async Task<TException> ThrowsAsync<TException>(
        Func<Task> action,
        string message,
        string? parameterName = null
    )
        where TException : Exception
    {
        try
        {
            await action();
        }
        catch (TException exception)
        {
            if (parameterName is not null && exception is ArgumentException argumentException)
            {
                Equal(
                    parameterName,
                    argumentException.ParamName,
                    $"{message} Unexpected parameter name."
                );
            }

            return exception;
        }

        throw new InvalidOperationException(message);
    }
}

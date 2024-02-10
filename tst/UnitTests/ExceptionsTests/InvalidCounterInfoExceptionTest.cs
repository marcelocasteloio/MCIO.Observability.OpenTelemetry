using FluentAssertions;
using MCIO.Observability.OpenTelemetry.Exceptions;

namespace MCIO.Observability.OpenTelemetry.UnitTests.ExceptionsTests;
public class InvalidCounterInfoExceptionTest
{
    [Fact]
    public void InvalidCounterInfoException_Should_Throw()
    {
        // Arrange
        var invalidCounterInfoException = default(InvalidCounterInfoException);

        var outputEnvelop = OutputEnvelop.OutputEnvelop.CreateError();

        // Act
        try
        {
            InvalidCounterInfoException.ThrowIfInvalid(outputEnvelop);
        }
        catch (InvalidCounterInfoException ex)
        {
            invalidCounterInfoException = ex;
        }

        // Assert
        invalidCounterInfoException.Should().NotBeNull();
        invalidCounterInfoException!.OutputEnvelop.Should().Be(outputEnvelop);
    }

    [Fact]
    public void InvalidCounterInfoException_Should_Not_Throw()
    {
        // Arrange
        var invalidCounterInfoException = default(InvalidCounterInfoException);

        var outputEnvelop = OutputEnvelop.OutputEnvelop.CreateSuccess();

        // Act
        try
        {
            InvalidCounterInfoException.ThrowIfInvalid(outputEnvelop);
        }
        catch (InvalidCounterInfoException ex)
        {
            invalidCounterInfoException = ex;
        }

        // Assert
        invalidCounterInfoException.Should().BeNull();
    }
}

using FluentAssertions;
using MCIO.Observability.OpenTelemetry.Exceptions;

namespace MCIO.Observability.OpenTelemetry.UnitTests.ExceptionsTests;
public class InvalidHistogramInfoExceptionTest
{
    [Fact]
    public void InvalidHistogramInfoException_Should_Throw()
    {
        // Arrange
        var invalidHistogramInfoException = default(InvalidHistogramInfoException);

        var outputEnvelop = OutputEnvelop.OutputEnvelop.CreateError();

        // Act
        try
        {
            InvalidHistogramInfoException.ThrowIfInvalid(outputEnvelop);
        }
        catch (InvalidHistogramInfoException ex)
        {
            invalidHistogramInfoException = ex;
        }

        // Assert
        invalidHistogramInfoException.Should().NotBeNull();
        invalidHistogramInfoException!.OutputEnvelop.Should().Be(outputEnvelop);
    }

    [Fact]
    public void InvalidHistogramInfoException_Should_Not_Throw()
    {
        // Arrange
        var invalidHistogramInfoException = default(InvalidHistogramInfoException);

        var outputEnvelop = OutputEnvelop.OutputEnvelop.CreateSuccess();

        // Act
        try
        {
            InvalidHistogramInfoException.ThrowIfInvalid(outputEnvelop);
        }
        catch (InvalidHistogramInfoException ex)
        {
            invalidHistogramInfoException = ex;
        }

        // Assert
        invalidHistogramInfoException.Should().BeNull();
    }
}

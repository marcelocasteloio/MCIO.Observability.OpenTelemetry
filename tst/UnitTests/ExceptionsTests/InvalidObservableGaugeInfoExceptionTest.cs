using FluentAssertions;
using MCIO.Observability.OpenTelemetry.Exceptions;

namespace MCIO.Observability.OpenTelemetry.UnitTests.ExceptionsTests;
public class InvalidObservableGaugeInfoExceptionTest
{
    [Fact]
    public void InvalidObservableGaugeInfoException_Should_Throw()
    {
        // Arrange
        var invalidObservableGaugeInfoException = default(InvalidObservableGaugeInfoException);

        var outputEnvelop = OutputEnvelop.OutputEnvelop.CreateError();

        // Act
        try
        {
            InvalidObservableGaugeInfoException.ThrowIfInvalid(outputEnvelop);
        }
        catch (InvalidObservableGaugeInfoException ex)
        {
            invalidObservableGaugeInfoException = ex;
        }

        // Assert
        invalidObservableGaugeInfoException.Should().NotBeNull();
        invalidObservableGaugeInfoException!.OutputEnvelop.Should().Be(outputEnvelop);
    }

    [Fact]
    public void InvalidObservableGaugeInfoException_Should_Not_Throw()
    {
        // Arrange
        var invalidObservableGaugeInfoException = default(InvalidObservableGaugeInfoException);

        var outputEnvelop = OutputEnvelop.OutputEnvelop.CreateSuccess();

        // Act
        try
        {
            InvalidObservableGaugeInfoException.ThrowIfInvalid(outputEnvelop);
        }
        catch (InvalidObservableGaugeInfoException ex)
        {
            invalidObservableGaugeInfoException = ex;
        }

        // Assert
        invalidObservableGaugeInfoException.Should().BeNull();
    }
}

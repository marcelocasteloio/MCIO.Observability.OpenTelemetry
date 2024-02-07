using FluentAssertions;
using MCIO.Observability.OpenTelemetry.Exceptions;
using System.Diagnostics;

namespace MCIO.Observability.OpenTelemetry.UnitTests.ExceptionsTests;
public class ActivityCannotbeNullExceptionTest
{
    [Fact]
    public void ActivityCannotbeNullException_Should_Throw()
    {
        // Arrange
        var activityName = Guid.NewGuid().ToString();
        var activityKind = ActivityKind.Client;
        var activityCannotbeNullException = default(ActivityCannotbeNullException);

        // Act
        try
        {
            ActivityCannotbeNullException.ThrowIfActivityIsNull(
                activity: null,
                activitySource: null,
                name: activityName,
                kind: activityKind
            );
        }
        catch (ActivityCannotbeNullException ex)
        {
            activityCannotbeNullException = ex;
        }

        // Assert
        activityCannotbeNullException.Should().NotBeNull();
        activityCannotbeNullException!.ActivitySource.Should().BeNull();
        activityCannotbeNullException!.Name.Should().Be(activityName);
        activityCannotbeNullException!.Kind.Should().Be(activityKind);
    }

    [Fact]
    public void ActivityCannotbeNullException_Should_Not_Throw()
    {
        // Arrange
        var activityName = Guid.NewGuid().ToString();
        var activityKind = ActivityKind.Client;

        var activitySource = new ActivitySource(name: Guid.NewGuid().ToString());
        var activity = new Activity(activityName);

        // Act
        var handler = () => ActivityCannotbeNullException.ThrowIfActivityIsNull(
            activity,
            activitySource,
            name: activityName,
            kind: activityKind
        );

        // Assert
        handler.Should().NotThrow<ActivityCannotbeNullException>();
    }
}

using FluentAssertions;
using MCIO.Core.ExecutionInfo;
using MCIO.Core.TenantInfo;
using OpenTelemetry.Trace;
using System.Diagnostics;

namespace MCIO.Observability.OpenTelemetry.UnitTests;
public class TraceManagerTest
{
    // Fields
    private const string OTEL_STATUS_CODE_TAG_KEY = "otel.status_code";

    // Constructors
    static TraceManagerTest()
    {
        ActivitySource.AddActivityListener(
            new ActivityListener
            {
                ShouldListenTo = activitySource => true,
                SampleUsingParentId = (ref ActivityCreationOptions<string> activityOptions) => ActivitySamplingResult.AllData,
                Sample = (ref ActivityCreationOptions<ActivityContext> activityOptions) => ActivitySamplingResult.AllData
            }
        );
    }

    [Fact]
    public void TraceManager_Should_StartActivity()
    {
        foreach (var activityKind in Enum.GetValues<ActivityKind>())
        {
            // Arrange
            var name = Guid.NewGuid().ToString();
            var executionInfo = CreateExecutionInfo();

            var activitySource = CreateActivitySource();
            var traceManager = new TraceManager(activitySource);

            var expectedActivityStatus = Status.Ok;

            // Act
            var handlerActivity = default(Activity);
            var handlerExecutionInfo = default(ExecutionInfo);

            traceManager.StartActivity(
                name,
                activityKind,
                executionInfo,
                handler: (activity, executionInfo) =>
                {
                    handlerActivity = activity;
                    handlerExecutionInfo = executionInfo;
                }
            );

            // Assert
            handlerActivity.Should().NotBeNull();
            handlerActivity!.Source.Should().Be(activitySource);
            handlerActivity!.OperationName.Should().Be(name);
            handlerActivity!.Kind.Should().Be(activityKind);
            ValidateActivityTags(handlerActivity, executionInfo, expectedActivityStatus).Should().BeTrue();

            executionInfo.Should().Be(handlerExecutionInfo);
        }
    }

    [Fact]
    public void TraceManager_Should_StartActivity_WithInput()
    {
        foreach (var activityKind in Enum.GetValues<ActivityKind>())
        {
            // Arrange
            var name = Guid.NewGuid().ToString();
            var executionInfo = CreateExecutionInfo();
            var input = Guid.NewGuid();

            var activitySource = CreateActivitySource();
            var traceManager = new TraceManager(activitySource);

            var expectedActivityStatus = Status.Ok;

            // Act
            var handlerActivity = default(Activity);
            var handlerExecutionInfo = default(ExecutionInfo);
            var handlerInput = Guid.Empty;

            traceManager.StartActivity(
                name,
                activityKind,
                executionInfo,
                input,
                handler: (activity, executionInfo, input) =>
                {
                    handlerActivity = activity;
                    handlerExecutionInfo = executionInfo;
                    handlerInput = input;
                }
            );

            // Assert
            handlerActivity.Should().NotBeNull();
            handlerActivity!.Source.Should().Be(activitySource);
            handlerActivity!.OperationName.Should().Be(name);
            handlerActivity!.Kind.Should().Be(activityKind);
            ValidateActivityTags(handlerActivity, executionInfo, expectedActivityStatus).Should().BeTrue();

            handlerExecutionInfo.Should().Be(executionInfo);
            handlerInput.Should().Be(input);
        }
    }

    [Fact]
    public void TraceManager_Should_StartActivity_WithInputAndOutput()
    {
        foreach (var activityKind in Enum.GetValues<ActivityKind>())
        {
            // Arrange
            var name = Guid.NewGuid().ToString();
            var executionInfo = CreateExecutionInfo();
            var input = Guid.NewGuid();
            var expectedOutput = Guid.NewGuid();

            var activitySource = CreateActivitySource();
            var traceManager = new TraceManager(activitySource);

            var expectedActivityStatus = Status.Ok;

            // Act
            var handlerActivity = default(Activity);
            var handlerExecutionInfo = default(ExecutionInfo);
            var handlerInput = Guid.Empty;

            var handlerOutput = traceManager.StartActivity(
                name,
                activityKind,
                executionInfo,
                input,
                handler: (activity, executionInfo, input) =>
                {
                    handlerActivity = activity;
                    handlerExecutionInfo = executionInfo;
                    handlerInput = input;

                    return expectedOutput;
                }
            );

            // Assert
            handlerActivity.Should().NotBeNull();
            handlerActivity!.Source.Should().Be(activitySource);
            handlerActivity!.OperationName.Should().Be(name);
            handlerActivity!.Kind.Should().Be(activityKind);
            ValidateActivityTags(handlerActivity, executionInfo, expectedActivityStatus).Should().BeTrue();

            handlerExecutionInfo.Should().Be(executionInfo);
            handlerInput.Should().Be(input);
            handlerOutput.Should().Be(expectedOutput);
        }
    }

    [Fact]
    public void TraceManager_Should_StartActivity_WithOutput()
    {
        foreach (var activityKind in Enum.GetValues<ActivityKind>())
        {
            // Arrange
            var name = Guid.NewGuid().ToString();
            var executionInfo = CreateExecutionInfo();
            var expectedOutput = Guid.NewGuid();

            var activitySource = CreateActivitySource();
            var traceManager = new TraceManager(activitySource);

            var expectedActivityStatus = Status.Ok;

            // Act
            var handlerActivity = default(Activity);
            var handlerExecutionInfo = default(ExecutionInfo);
            var handlerInput = Guid.Empty;

            var handlerOutput = traceManager.StartActivity(
                name,
                activityKind,
                executionInfo,
                handler: (activity, executionInfo) =>
                {
                    handlerActivity = activity;
                    handlerExecutionInfo = executionInfo;

                    return expectedOutput;
                }
            );

            // Assert
            handlerActivity.Should().NotBeNull();
            handlerActivity!.Source.Should().Be(activitySource);
            handlerActivity!.OperationName.Should().Be(name);
            handlerActivity!.Kind.Should().Be(activityKind);
            ValidateActivityTags(handlerActivity, executionInfo, expectedActivityStatus).Should().BeTrue();

            handlerExecutionInfo.Should().Be(executionInfo);
            handlerOutput.Should().Be(expectedOutput);
        }
    }

    [Fact]
    public async Task TraceManager_Should_StartActivityAsync()
    {
        foreach (var activityKind in Enum.GetValues<ActivityKind>())
        {
            // Arrange
            var cancellationToken = new CancellationTokenSource().Token;
            var name = Guid.NewGuid().ToString();
            var executionInfo = CreateExecutionInfo();

            var activitySource = CreateActivitySource();
            var traceManager = new TraceManager(activitySource);

            var expectedActivityStatus = Status.Ok;

            // Act
            var handlerActivity = default(Activity);
            var handlerExecutionInfo = default(ExecutionInfo);
            var handlerCancellationToken = default(CancellationToken);

            await traceManager.StartActivityAsync(
                name,
                activityKind,
                executionInfo,
                handler: (activity, executionInfo, cancellationToken) =>
                {
                    handlerActivity = activity;
                    handlerExecutionInfo = executionInfo;
                    handlerCancellationToken = cancellationToken;

                    return Task.CompletedTask;
                },
                cancellationToken
            );

            // Assert
            handlerActivity.Should().NotBeNull();
            handlerActivity!.Source.Should().Be(activitySource);
            handlerActivity!.OperationName.Should().Be(name);
            handlerActivity!.Kind.Should().Be(activityKind);
            ValidateActivityTags(handlerActivity, executionInfo, expectedActivityStatus).Should().BeTrue();

            executionInfo.Should().Be(handlerExecutionInfo);
            handlerCancellationToken.Should().Be(cancellationToken);
        }
    }

    [Fact]
    public async Task TraceManager_Should_StartActivityAsync_WithInput()
    {
        foreach (var activityKind in Enum.GetValues<ActivityKind>())
        {
            // Arrange
            var cancellationToken = new CancellationTokenSource().Token;
            var name = Guid.NewGuid().ToString();
            var executionInfo = CreateExecutionInfo();
            var input = Guid.NewGuid();

            var activitySource = CreateActivitySource();
            var traceManager = new TraceManager(activitySource);

            var expectedActivityStatus = Status.Ok;

            // Act
            var handlerActivity = default(Activity);
            var handlerExecutionInfo = default(ExecutionInfo);
            var handlerInput = Guid.Empty;
            var handlerCancellationToken = default(CancellationToken);

            await traceManager.StartActivityAsync(
                name,
                activityKind,
                executionInfo,
                input,
                handler: (activity, executionInfo, input, cancellationToken) =>
                {
                    handlerActivity = activity;
                    handlerExecutionInfo = executionInfo;
                    handlerInput = input;
                    handlerCancellationToken = cancellationToken;

                    return Task.CompletedTask;
                },
                cancellationToken
            );

            // Assert
            handlerActivity.Should().NotBeNull();
            handlerActivity!.Source.Should().Be(activitySource);
            handlerActivity!.OperationName.Should().Be(name);
            handlerActivity!.Kind.Should().Be(activityKind);
            ValidateActivityTags(handlerActivity, executionInfo, expectedActivityStatus).Should().BeTrue();

            handlerExecutionInfo.Should().Be(executionInfo);
            handlerInput.Should().Be(input);
            handlerCancellationToken.Should().Be(cancellationToken);
        }
    }

    [Fact]
    public async Task TraceManager_Should_StartActivityAsync_WithInputAndOutput()
    {
        foreach (var activityKind in Enum.GetValues<ActivityKind>())
        {
            // Arrange
            var cancellationToken = new CancellationTokenSource().Token;
            var name = Guid.NewGuid().ToString();
            var executionInfo = CreateExecutionInfo();
            var input = Guid.NewGuid();
            var expectedOutput = Guid.NewGuid();

            var activitySource = CreateActivitySource();
            var traceManager = new TraceManager(activitySource);

            var expectedActivityStatus = Status.Ok;

            // Act
            var handlerActivity = default(Activity);
            var handlerExecutionInfo = default(ExecutionInfo);
            var handlerInput = Guid.Empty;
            var handlerCancellationToken = default(CancellationToken);

            var handlerOutput = await traceManager.StartActivityAsync(
                name,
                activityKind,
                executionInfo,
                input,
                handler: (activity, executionInfo, input, CancellationToken) =>
                {
                    handlerActivity = activity;
                    handlerExecutionInfo = executionInfo;
                    handlerInput = input;
                    handlerCancellationToken = cancellationToken;

                    return Task.FromResult(expectedOutput);
                },
                cancellationToken
            );

            // Assert
            handlerActivity.Should().NotBeNull();
            handlerActivity!.Source.Should().Be(activitySource);
            handlerActivity!.OperationName.Should().Be(name);
            handlerActivity!.Kind.Should().Be(activityKind);
            ValidateActivityTags(handlerActivity, executionInfo, expectedActivityStatus).Should().BeTrue();

            handlerExecutionInfo.Should().Be(executionInfo);
            handlerInput.Should().Be(input);
            handlerOutput.Should().Be(expectedOutput);
            handlerCancellationToken.Should().Be(cancellationToken);
        }
    }

    [Fact]
    public async Task TraceManager_Should_StartActivityAsync_WithOutput()
    {
        foreach (var activityKind in Enum.GetValues<ActivityKind>())
        {
            // Arrange
            var cancellationToken = new CancellationTokenSource().Token;
            var name = Guid.NewGuid().ToString();
            var executionInfo = CreateExecutionInfo();
            var expectedOutput = Guid.NewGuid();

            var activitySource = CreateActivitySource();
            var traceManager = new TraceManager(activitySource);

            var expectedActivityStatus = Status.Ok;
            var handlerCancellationToken = default(CancellationToken);

            // Act
            var handlerActivity = default(Activity);
            var handlerExecutionInfo = default(ExecutionInfo);
            var handlerInput = Guid.Empty;

            var handlerOutput = await traceManager.StartActivityAsync(
                name,
                activityKind,
                executionInfo,
                handler: (activity, executionInfo, cancellationToken) =>
                {
                    handlerActivity = activity;
                    handlerExecutionInfo = executionInfo;
                    handlerCancellationToken = cancellationToken;

                    return Task.FromResult(expectedOutput);
                },
                cancellationToken
            );

            // Assert
            handlerActivity.Should().NotBeNull();
            handlerActivity!.Source.Should().Be(activitySource);
            handlerActivity!.OperationName.Should().Be(name);
            handlerActivity!.Kind.Should().Be(activityKind);
            ValidateActivityTags(handlerActivity, executionInfo, expectedActivityStatus).Should().BeTrue();

            handlerExecutionInfo.Should().Be(executionInfo);
            handlerOutput.Should().Be(expectedOutput);
            handlerCancellationToken.Should().Be(cancellationToken);
        }
    }


    // Private Methods
    private static ActivitySource CreateActivitySource()
    {
        return new ActivitySource(name: Guid.NewGuid().ToString(), version: "1");
    }
    private static ExecutionInfo CreateExecutionInfo()
    {
        return ExecutionInfo.Create(
            correlationId: Guid.NewGuid(),
            tenantInfo: TenantInfo.FromExistingCode(code: Guid.NewGuid()).Output!.Value,
            executionUser: Guid.NewGuid().ToString(),
            origin: Guid.NewGuid().ToString()
        ).Output!.Value;
    }
    private static bool ValidateActivityTags(Activity activity, ExecutionInfo executionInfo, Status expectedActivityStatus)
    {
        var tagCollection = activity.TagObjects.ToArray();

        var correlationId = (Guid?) tagCollection.FirstOrDefault(q => q.Key == TraceManager.CORRELATION_ID_TAG_NAME).Value;
        var tenantCode = (Guid?)tagCollection.FirstOrDefault(q => q.Key == TraceManager.TENANT_CODE_TAG_NAME).Value;
        var executionUser = (string?) tagCollection.FirstOrDefault(q => q.Key == TraceManager.EXECUTION_USER_TAG_NAME).Value;
        var origin = (string?) tagCollection.FirstOrDefault(q => q.Key == TraceManager.ORIGIN_TAG_NAME).Value;
        var status = (string?)tagCollection.FirstOrDefault(q => q.Key == OTEL_STATUS_CODE_TAG_KEY).Value;

        return
            executionInfo.CorrelationId == correlationId
            && executionInfo.TenantInfo.Code == tenantCode
            && executionInfo.ExecutionUser == executionUser
            && executionInfo.Origin == origin
            && string.Compare(status, expectedActivityStatus.StatusCode.ToString(), ignoreCase: true) == 0;
    }
}

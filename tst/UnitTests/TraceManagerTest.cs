using FluentAssertions;
using MCIO.Core.ExecutionInfo;
using MCIO.Core.TenantInfo;
using OpenTelemetry.Trace;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        // Arrange
        var name = Guid.NewGuid().ToString();
        var activityKind = ActivityKind.Client;
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

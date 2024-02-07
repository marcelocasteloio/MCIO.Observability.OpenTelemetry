using MCIO.Core.ExecutionInfo;
using MCIO.Observability.Abstractions;
using MCIO.Observability.OpenTelemetry.Exceptions;
using OpenTelemetry.Trace;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace MCIO.Observability.OpenTelemetry
{
    public class TraceManager
        : ITraceManager
    {
        // Constants
        public const string CORRELATION_ID_TAG_NAME = "correlation-id";
        public const string TENANT_CODE_TAG_NAME = "tenant-code";
        public const string EXECUTION_USER_TAG_NAME = "execution-user";
        public const string ORIGIN_TAG_NAME = "origin";

        // Fields
        private readonly ActivitySource _activitySource;

        // Constructors
        public TraceManager(ActivitySource activitySource)
        {
            _activitySource = activitySource;
        }

        // Public Methods
        public void StartActivity(string name, ActivityKind kind, ExecutionInfo executionInfo, Action<Activity, ExecutionInfo> handler)
        {
            using (var activity = CreateActivity(name, kind, _activitySource))
            {
                SetDefaultActivityTags(activity, executionInfo);

                try
                {
                    handler(activity, executionInfo);
                    activity.SetStatus(Status.Ok);
                }
                catch (Exception ex)
                {
                    activity.SetStatus(Status.Error);
                    activity.RecordException(ex);
                    throw;
                } 
            }
        }
        public void StartActivity<TInput>(string name, ActivityKind kind, ExecutionInfo executionInfo, TInput input, Action<Activity, ExecutionInfo, TInput> handler)
        {
            using (var activity = CreateActivity(name, kind, _activitySource))
            {
                SetDefaultActivityTags(activity, executionInfo);

                try
                {
                    handler(activity, executionInfo, input);

                    activity.SetStatus(Status.Ok);
                }
                catch (Exception ex)
                {
                    activity.SetStatus(Status.Error);
                    activity.RecordException(ex);
                    throw;
                }
            }
        }
        public TOutput StartActivity<TInput, TOutput>(string name, ActivityKind kind, ExecutionInfo executionInfo, TInput input, Func<Activity, ExecutionInfo, TInput, TOutput> handler)
        {
            using (var activity = CreateActivity(name, kind, _activitySource))
            {
                SetDefaultActivityTags(activity, executionInfo);

                try
                {
                    var handlerResult = handler(activity, executionInfo, input);

                    activity.SetStatus(Status.Ok);

                    return handlerResult;
                }
                catch (Exception ex)
                {
                    activity.SetStatus(Status.Error);
                    activity.RecordException(ex);
                    throw;
                }
            }
        }
        public TOutput StartActivity<TOutput>(string name, ActivityKind kind, ExecutionInfo executionInfo, Func<Activity, ExecutionInfo, TOutput> handler)
        {
            using (var activity = CreateActivity(name, kind, _activitySource))
            {
                SetDefaultActivityTags(activity, executionInfo);

                try
                {
                    var handlerResult = handler(activity, executionInfo);

                    activity.SetStatus(Status.Ok);

                    return handlerResult;
                }
                catch (Exception ex)
                {
                    activity.SetStatus(Status.Error);
                    activity.RecordException(ex);
                    throw;
                }
            }
        }

        public async Task StartActivityAsync(string name, ActivityKind kind, ExecutionInfo executionInfo, Func<Activity, ExecutionInfo, CancellationToken, Task> handler, CancellationToken cancellationToken)
        {
            using (var activity = CreateActivity(name, kind, _activitySource))
            {
                SetDefaultActivityTags(activity, executionInfo);

                try
                {
                    await handler(activity, executionInfo, cancellationToken);

                    activity.SetStatus(Status.Ok);
                }
                catch (Exception ex)
                {
                    activity.SetStatus(Status.Error);
                    activity.RecordException(ex);
                    throw;
                }
            }
        }
        public async Task StartActivityAsync<TInput>(string name, ActivityKind kind, ExecutionInfo executionInfo, TInput input, Func<Activity, ExecutionInfo, TInput, CancellationToken, Task> handler, CancellationToken cancellationToken)
        {
            using (var activity = CreateActivity(name, kind, _activitySource))
            {
                SetDefaultActivityTags(activity, executionInfo);

                try
                {
                    await handler(activity, executionInfo, input, cancellationToken);

                    activity.SetStatus(Status.Ok);
                }
                catch (Exception ex)
                {
                    activity.SetStatus(Status.Error);
                    activity.RecordException(ex);
                    throw;
                }
            }
        }
        public async Task<TOutput> StartActivityAsync<TOutput>(string name, ActivityKind kind, ExecutionInfo executionInfo, Func<Activity, ExecutionInfo, CancellationToken, Task<TOutput>> handler, CancellationToken cancellationToken)
        {
            using (var activity = CreateActivity(name, kind, _activitySource))
            {
                SetDefaultActivityTags(activity, executionInfo);

                try
                {
                    var handlerResult = await handler(activity, executionInfo, cancellationToken);

                    activity.SetStatus(Status.Ok);

                    return handlerResult;
                }
                catch (Exception ex)
                {
                    activity.SetStatus(Status.Error);
                    activity.RecordException(ex);
                    throw;
                }
            }
        }
        public async Task<TOutput> StartActivityAsync<TInput, TOutput>(string name, ActivityKind kind, ExecutionInfo executionInfo, TInput input, Func<Activity, ExecutionInfo, TInput, CancellationToken, Task<TOutput>> handler, CancellationToken cancellationToken)
        {
            using (var activity = CreateActivity(name, kind, _activitySource))
            {
                SetDefaultActivityTags(activity, executionInfo);

                try
                {
                    var handlerResult = await handler(activity, executionInfo, input, cancellationToken);

                    activity.SetStatus(Status.Ok);

                    return handlerResult;
                }
                catch (Exception ex)
                {
                    activity.SetStatus(Status.Error);
                    activity.RecordException(ex);
                    throw;
                }
            }
        }

        public void StartInternalActivity(string name, ExecutionInfo executionInfo, Action<Activity, ExecutionInfo> handler) => StartActivity(name, kind: ActivityKind.Internal, executionInfo, handler);
        public void StartInternalActivity<TInput>(string name, ExecutionInfo executionInfo, TInput input, Action<Activity, ExecutionInfo, TInput> handler) => StartActivity(name, kind: ActivityKind.Internal, executionInfo, input, handler);
        public TOutput StartInternalActivity<TInput, TOutput>(string name, ExecutionInfo executionInfo, TInput input, Func<Activity, ExecutionInfo, TInput, TOutput> handler) => StartActivity(name, kind: ActivityKind.Internal, executionInfo, input, handler);
        public TOutput StartInternalActivity<TOutput>(string name, ExecutionInfo executionInfo, Func<Activity, ExecutionInfo, TOutput> handler) => StartActivity(name, kind: ActivityKind.Internal, executionInfo, handler);

        public Task StartInternalActivityAsync(string name, ExecutionInfo executionInfo, Func<Activity, ExecutionInfo, CancellationToken, Task> handler, CancellationToken cancellationToken) => StartActivityAsync(name, kind: ActivityKind.Internal, executionInfo, handler, cancellationToken);
        public Task StartInternalActivityAsync<TInput>(string name, ExecutionInfo executionInfo, TInput input, Func<Activity, ExecutionInfo, TInput, CancellationToken, Task> handler, CancellationToken cancellationToken) => StartActivityAsync(name, kind: ActivityKind.Internal, executionInfo, input, handler, cancellationToken);
        public Task<TOutput> StartInternalActivityAsync<TOutput>(string name, ExecutionInfo executionInfo, Func<Activity, ExecutionInfo, CancellationToken, Task<TOutput>> handler, CancellationToken cancellationToken) => StartActivityAsync(name, kind: ActivityKind.Internal, executionInfo, handler, cancellationToken);
        public Task<TOutput> StartInternalActivityAsync<TInput, TOutput>(string name, ExecutionInfo executionInfo, TInput input, Func<Activity, ExecutionInfo, TInput, CancellationToken, Task<TOutput>> handler, CancellationToken cancellationToken) => StartActivityAsync(name, kind: ActivityKind.Internal, executionInfo, input, handler, cancellationToken);

        public void StartServerActivity(string name, ExecutionInfo executionInfo, Action<Activity, ExecutionInfo> handler) => StartActivity(name, kind: ActivityKind.Server, executionInfo, handler);
        public void StartServerActivity<TInput>(string name, ExecutionInfo executionInfo, TInput input, Action<Activity, ExecutionInfo, TInput> handler) => StartActivity(name, kind: ActivityKind.Server, executionInfo, input, handler);
        public TOutput StartServerActivity<TInput, TOutput>(string name, ExecutionInfo executionInfo, TInput input, Func<Activity, ExecutionInfo, TInput, TOutput> handler) => StartActivity(name, kind: ActivityKind.Server, executionInfo, input, handler);
        public TOutput StartServerActivity<TOutput>(string name, ExecutionInfo executionInfo, Func<Activity, ExecutionInfo, TOutput> handler) => StartActivity(name, kind: ActivityKind.Server, executionInfo, handler);

        public Task StartServerActivityAsync(string name, ExecutionInfo executionInfo, Func<Activity, ExecutionInfo, CancellationToken, Task> handler, CancellationToken cancellationToken) => StartActivityAsync(name, kind: ActivityKind.Server, executionInfo, handler, cancellationToken);
        public Task StartServerActivityAsync<TInput>(string name, ExecutionInfo executionInfo, TInput input, Func<Activity, ExecutionInfo, TInput, CancellationToken, Task> handler, CancellationToken cancellationToken) => StartActivityAsync(name, kind: ActivityKind.Server, executionInfo, input, handler, cancellationToken);
        public Task<TOutput> StartServerActivityAsync<TOutput>(string name, ExecutionInfo executionInfo, Func<Activity, ExecutionInfo, CancellationToken, Task<TOutput>> handler, CancellationToken cancellationToken) => StartActivityAsync(name, kind: ActivityKind.Server, executionInfo, handler, cancellationToken);
        public Task<TOutput> StartServerActivityAsync<TInput, TOutput>(string name, ExecutionInfo executionInfo, TInput input, Func<Activity, ExecutionInfo, TInput, CancellationToken, Task<TOutput>> handler, CancellationToken cancellationToken) => StartActivityAsync(name, kind: ActivityKind.Server, executionInfo, input, handler, cancellationToken);

        public void StartClientActivity(string name, ExecutionInfo executionInfo, Action<Activity, ExecutionInfo> handler) => StartActivity(name, kind: ActivityKind.Client, executionInfo, handler);
        public void StartClientActivity<TInput>(string name, ExecutionInfo executionInfo, TInput input, Action<Activity, ExecutionInfo, TInput> handler) => StartActivity(name, kind: ActivityKind.Client, executionInfo, input, handler);
        public TOutput StartClientActivity<TInput, TOutput>(string name, ExecutionInfo executionInfo, TInput input, Func<Activity, ExecutionInfo, TInput, TOutput> handler) => StartActivity(name, kind: ActivityKind.Client, executionInfo, input, handler);
        public TOutput StartClientActivity<TOutput>(string name, ExecutionInfo executionInfo, Func<Activity, ExecutionInfo, TOutput> handler) => StartActivity(name, kind: ActivityKind.Client, executionInfo, handler);

        public Task StartClientActivityAsync(string name, ExecutionInfo executionInfo, Func<Activity, ExecutionInfo, CancellationToken, Task> handler, CancellationToken cancellationToken) => StartActivityAsync(name, kind: ActivityKind.Client, executionInfo, handler, cancellationToken);
        public Task StartClientActivityAsync<TInput>(string name, ExecutionInfo executionInfo, TInput input, Func<Activity, ExecutionInfo, TInput, CancellationToken, Task> handler, CancellationToken cancellationToken) => StartActivityAsync(name, kind: ActivityKind.Client, executionInfo, input, handler, cancellationToken);
        public Task<TOutput> StartClientActivityAsync<TOutput>(string name, ExecutionInfo executionInfo, Func<Activity, ExecutionInfo, CancellationToken, Task<TOutput>> handler, CancellationToken cancellationToken) => StartActivityAsync(name, kind: ActivityKind.Client, executionInfo, handler, cancellationToken);
        public Task<TOutput> StartClientActivityAsync<TInput, TOutput>(string name, ExecutionInfo executionInfo, TInput input, Func<Activity, ExecutionInfo, TInput, CancellationToken, Task<TOutput>> handler, CancellationToken cancellationToken) => StartActivityAsync(name, kind: ActivityKind.Client, executionInfo, input, handler, cancellationToken);

        public void StartProducerActivity(string name, ExecutionInfo executionInfo, Action<Activity, ExecutionInfo> handler) => StartActivity(name, kind: ActivityKind.Producer, executionInfo, handler);
        public void StartProducerActivity<TInput>(string name, ExecutionInfo executionInfo, TInput input, Action<Activity, ExecutionInfo, TInput> handler) => StartActivity(name, kind: ActivityKind.Producer, executionInfo, input, handler);
        public TOutput StartProducerActivity<TInput, TOutput>(string name, ExecutionInfo executionInfo, TInput input, Func<Activity, ExecutionInfo, TInput, TOutput> handler) => StartActivity(name, kind: ActivityKind.Producer, executionInfo, input, handler);
        public TOutput StartProducerActivity<TOutput>(string name, ExecutionInfo executionInfo, Func<Activity, ExecutionInfo, TOutput> handler) => StartActivity(name, kind: ActivityKind.Producer, executionInfo, handler);

        public Task StartProducerActivityAsync(string name, ExecutionInfo executionInfo, Func<Activity, ExecutionInfo, CancellationToken, Task> handler, CancellationToken cancellationToken) => StartActivityAsync(name, kind: ActivityKind.Producer, executionInfo, handler, cancellationToken);
        public Task StartProducerActivityAsync<TInput>(string name, ExecutionInfo executionInfo, TInput input, Func<Activity, ExecutionInfo, TInput, CancellationToken, Task> handler, CancellationToken cancellationToken) => StartActivityAsync(name, kind: ActivityKind.Producer, executionInfo, input, handler, cancellationToken);
        public Task<TOutput> StartProducerActivityAsync<TOutput>(string name, ExecutionInfo executionInfo, Func<Activity, ExecutionInfo, CancellationToken, Task<TOutput>> handler, CancellationToken cancellationToken) => StartActivityAsync(name, kind: ActivityKind.Producer, executionInfo, handler, cancellationToken);
        public Task<TOutput> StartProducerActivityAsync<TInput, TOutput>(string name, ExecutionInfo executionInfo, TInput input, Func<Activity, ExecutionInfo, TInput, CancellationToken, Task<TOutput>> handler, CancellationToken cancellationToken) => StartActivityAsync(name, kind: ActivityKind.Producer, executionInfo, input, handler, cancellationToken);

        public void StartConsumerActivity(string name, ExecutionInfo executionInfo, Action<Activity, ExecutionInfo> handler) => StartActivity(name, kind: ActivityKind.Consumer, executionInfo, handler);
        public void StartConsumerActivity<TInput>(string name, ExecutionInfo executionInfo, TInput input, Action<Activity, ExecutionInfo, TInput> handler) => StartActivity(name, kind: ActivityKind.Consumer, executionInfo, input, handler);
        public TOutput StartConsumerActivity<TInput, TOutput>(string name, ExecutionInfo executionInfo, TInput input, Func<Activity, ExecutionInfo, TInput, TOutput> handler) => StartActivity(name, kind: ActivityKind.Consumer, executionInfo, input, handler);
        public TOutput StartConsumerActivity<TOutput>(string name, ExecutionInfo executionInfo, Func<Activity, ExecutionInfo, TOutput> handler) => StartActivity(name, kind: ActivityKind.Consumer, executionInfo, handler);

        public Task StartConsumerActivityAsync(string name, ExecutionInfo executionInfo, Func<Activity, ExecutionInfo, CancellationToken, Task> handler, CancellationToken cancellationToken) => StartActivityAsync(name, kind: ActivityKind.Consumer, executionInfo, handler, cancellationToken);
        public Task StartConsumerActivityAsync<TInput>(string name, ExecutionInfo executionInfo, TInput input, Func<Activity, ExecutionInfo, TInput, CancellationToken, Task> handler, CancellationToken cancellationToken) => StartActivityAsync(name, kind: ActivityKind.Consumer, executionInfo, input, handler, cancellationToken);
        public Task<TOutput> StartConsumerActivityAsync<TOutput>(string name, ExecutionInfo executionInfo, Func<Activity, ExecutionInfo, CancellationToken, Task<TOutput>> handler, CancellationToken cancellationToken) => StartActivityAsync(name, kind: ActivityKind.Consumer, executionInfo, handler, cancellationToken);
        public Task<TOutput> StartConsumerActivityAsync<TInput, TOutput>(string name, ExecutionInfo executionInfo, TInput input, Func<Activity, ExecutionInfo, TInput, CancellationToken, Task<TOutput>> handler, CancellationToken cancellationToken) => StartActivityAsync(name, kind: ActivityKind.Consumer, executionInfo, input, handler, cancellationToken);

        // Private Methods
        private static Activity CreateActivity(string name, ActivityKind kind, ActivitySource activitySource)
        {
            var activity = activitySource.StartActivity(name, kind);

            ActivityCannotbeNullException.ThrowIfActivityIsNull(activity, activitySource, name, kind);

            return activity;
        }
        private static void SetDefaultActivityTags(Activity activity, ExecutionInfo executionInfo)
        {
            activity.SetTag(CORRELATION_ID_TAG_NAME, executionInfo.CorrelationId);
            activity.SetTag(TENANT_CODE_TAG_NAME, executionInfo.TenantInfo.Code);
            activity.SetTag(EXECUTION_USER_TAG_NAME, executionInfo.ExecutionUser);
            activity.SetTag(ORIGIN_TAG_NAME, executionInfo.Origin);
        }
    }
}
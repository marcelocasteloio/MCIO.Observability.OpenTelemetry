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
        public void StartActivity(string traceName, ActivityKind kind, ExecutionInfo executionInfo, Action<Activity, ExecutionInfo> handler)
        {
            using (var activity = CreateActivity(traceName, kind, _activitySource))
            {
                SetDefaultActivityTags(activity, executionInfo);

                try
                {
                    activity.SetStatus(Status.Ok);
                    handler(activity, executionInfo);
                }
                catch (Exception ex)
                {
                    activity.SetStatus(ActivityStatusCode.Error, ex.Message);
                    activity.RecordException(ex);
                    throw;
                } 
            }
        }
        public void StartInternalActivity(string traceName, ExecutionInfo executionInfo, Action<Activity, ExecutionInfo> handler) => StartActivity(traceName, kind: ActivityKind.Internal, executionInfo, handler);
        public void StartServerActivity(string traceName, ExecutionInfo executionInfo, Action<Activity, ExecutionInfo> handler) => StartActivity(traceName, kind: ActivityKind.Server, executionInfo, handler);
        public void StartClientActivity(string traceName, ExecutionInfo executionInfo, Action<Activity, ExecutionInfo> handler) => StartActivity(traceName, kind: ActivityKind.Client, executionInfo, handler);
        public void StartProducerActivity(string traceName, ExecutionInfo executionInfo, Action<Activity, ExecutionInfo> handler) => StartActivity(traceName, kind: ActivityKind.Producer, executionInfo, handler);
        public void StartConsumerActivity(string traceName, ExecutionInfo executionInfo, Action<Activity, ExecutionInfo> handler) => StartActivity(traceName, kind: ActivityKind.Consumer, executionInfo, handler);

        public void StartActivity<TInput>(string traceName, ActivityKind kind, ExecutionInfo executionInfo, TInput input, Action<Activity, ExecutionInfo, TInput> handler)
        {
            using (var activity = CreateActivity(traceName, kind, _activitySource))
            {
                SetDefaultActivityTags(activity, executionInfo);

                try
                {
                    activity.SetStatus(Status.Ok);
                    handler(activity, executionInfo, input);
                }
                catch (Exception ex)
                {
                    activity.SetStatus(ActivityStatusCode.Error, ex.Message);
                    activity.RecordException(ex);
                    throw;
                } 
            }
        }
        public void StartInternalActivity<TInput>(string traceName, ExecutionInfo executionInfo, TInput input, Action<Activity, ExecutionInfo, TInput> handler) => StartActivity(traceName, kind: ActivityKind.Internal, executionInfo, input, handler);
        public void StartServerActivity<TInput>(string traceName, ExecutionInfo executionInfo, TInput input, Action<Activity, ExecutionInfo, TInput> handler) => StartActivity(traceName, kind: ActivityKind.Server, executionInfo, input, handler);
        public void StartClientActivity<TInput>(string traceName, ExecutionInfo executionInfo, TInput input, Action<Activity, ExecutionInfo, TInput> handler) => StartActivity(traceName, kind: ActivityKind.Client, executionInfo, input, handler);
        public void StartProducerActivity<TInput>(string traceName, ExecutionInfo executionInfo, TInput input, Action<Activity, ExecutionInfo, TInput> handler) => StartActivity(traceName, kind: ActivityKind.Producer, executionInfo, input, handler);
        public void StartConsumerActivity<TInput>(string traceName, ExecutionInfo executionInfo, TInput input, Action<Activity, ExecutionInfo, TInput> handler) => StartActivity(traceName, kind: ActivityKind.Consumer, executionInfo, input, handler);

        public TOutput StartActivity<TInput, TOutput>(string traceName, ActivityKind kind, ExecutionInfo executionInfo, TInput input, Func<Activity, ExecutionInfo, TInput, TOutput> handler)
        {
            using (var activity = CreateActivity(traceName, kind, _activitySource))
            {
                SetDefaultActivityTags(activity, executionInfo);

                try
                {
                    activity.SetStatus(Status.Ok);
                    return handler(activity, executionInfo, input);
                }
                catch (Exception ex)
                {
                    activity.SetStatus(ActivityStatusCode.Error, ex.Message);
                    activity.RecordException(ex);
                    throw;
                } 
            }
        }
        public TOutput StartInternalActivity<TInput, TOutput>(string traceName, ExecutionInfo executionInfo, TInput input, Func<Activity, ExecutionInfo, TInput, TOutput> handler) => StartActivity(traceName, kind: ActivityKind.Internal, executionInfo, input, handler);
        public TOutput StartServerActivity<TInput, TOutput>(string traceName, ExecutionInfo executionInfo, TInput input, Func<Activity, ExecutionInfo, TInput, TOutput> handler) => StartActivity(traceName, kind: ActivityKind.Server, executionInfo, input, handler);
        public TOutput StartClientActivity<TInput, TOutput>(string traceName, ExecutionInfo executionInfo, TInput input, Func<Activity, ExecutionInfo, TInput, TOutput> handler) => StartActivity(traceName, kind: ActivityKind.Client, executionInfo, input, handler);
        public TOutput StartProducerActivity<TInput, TOutput>(string traceName, ExecutionInfo executionInfo, TInput input, Func<Activity, ExecutionInfo, TInput, TOutput> handler) => StartActivity(traceName, kind: ActivityKind.Producer, executionInfo, input, handler);
        public TOutput StartConsumerActivity<TInput, TOutput>(string traceName, ExecutionInfo executionInfo, TInput input, Func<Activity, ExecutionInfo, TInput, TOutput> handler) => StartActivity(traceName, kind: ActivityKind.Consumer, executionInfo, input, handler);

        public TOutput StartActivity<TOutput>(string traceName, ActivityKind kind, ExecutionInfo executionInfo, Func<Activity, ExecutionInfo, TOutput> handler)
        {
            using (var activity = CreateActivity(traceName, kind, _activitySource))
            {
                SetDefaultActivityTags(activity, executionInfo);

                try
                {
                    activity.SetStatus(Status.Ok);
                    return handler(activity, executionInfo);
                }
                catch (Exception ex)
                {
                    activity.SetStatus(ActivityStatusCode.Error, ex.Message);
                    activity.RecordException(ex);
                    throw;
                } 
            }
        }
        public TOutput StartInternalActivity<TOutput>(string traceName, ExecutionInfo executionInfo, Func<Activity, ExecutionInfo, TOutput> handler) => StartActivity(traceName, kind: ActivityKind.Internal, executionInfo, handler);
        public TOutput StartServerActivity<TOutput>(string traceName, ExecutionInfo executionInfo, Func<Activity, ExecutionInfo, TOutput> handler) => StartActivity(traceName, kind: ActivityKind.Server, executionInfo, handler);
        public TOutput StartClientActivity<TOutput>(string traceName, ExecutionInfo executionInfo, Func<Activity, ExecutionInfo, TOutput> handler) => StartActivity(traceName, kind: ActivityKind.Client, executionInfo, handler);
        public TOutput StartProducerActivity<TOutput>(string traceName, ExecutionInfo executionInfo, Func<Activity, ExecutionInfo, TOutput> handler) => StartActivity(traceName, kind: ActivityKind.Producer, executionInfo, handler);
        public TOutput StartConsumerActivity<TOutput>(string traceName, ExecutionInfo executionInfo, Func<Activity, ExecutionInfo, TOutput> handler) => StartActivity(traceName, kind: ActivityKind.Consumer, executionInfo, handler);

        public async Task StartActivityAsync(string traceName, ActivityKind kind, ExecutionInfo executionInfo, Func<Activity, ExecutionInfo, CancellationToken, Task> handler, CancellationToken cancellationToken)
        {
            using (var activity = CreateActivity(traceName, kind, _activitySource))
            {
                SetDefaultActivityTags(activity, executionInfo);

                try
                {
                    activity.SetStatus(Status.Ok);
                    await handler(activity, executionInfo, cancellationToken);
                }
                catch (Exception ex)
                {
                    activity.SetStatus(ActivityStatusCode.Error, ex.Message);
                    activity.RecordException(ex);
                    throw;
                } 
            }
        }
        public Task StartInternalActivityAsync(string traceName, ExecutionInfo executionInfo, Func<Activity, ExecutionInfo, CancellationToken, Task> handler, CancellationToken cancellationToken) => StartActivityAsync(traceName, kind: ActivityKind.Internal, executionInfo, handler, cancellationToken);
        public Task StartServerActivityAsync(string traceName, ExecutionInfo executionInfo, Func<Activity, ExecutionInfo, CancellationToken, Task> handler, CancellationToken cancellationToken) => StartActivityAsync(traceName, kind: ActivityKind.Server, executionInfo, handler, cancellationToken);
        public Task StartClientActivityAsync(string traceName, ExecutionInfo executionInfo, Func<Activity, ExecutionInfo, CancellationToken, Task> handler, CancellationToken cancellationToken) => StartActivityAsync(traceName, kind: ActivityKind.Client, executionInfo, handler, cancellationToken);
        public Task StartProducerActivityAsync(string traceName, ExecutionInfo executionInfo, Func<Activity, ExecutionInfo, CancellationToken, Task> handler, CancellationToken cancellationToken) => StartActivityAsync(traceName, kind: ActivityKind.Producer, executionInfo, handler, cancellationToken);
        public Task StartConsumerActivityAsync(string traceName, ExecutionInfo executionInfo, Func<Activity, ExecutionInfo, CancellationToken, Task> handler, CancellationToken cancellationToken) => StartActivityAsync(traceName, kind: ActivityKind.Consumer, executionInfo, handler, cancellationToken);


        public async Task StartActivityAsync<TInput>(string traceName, ActivityKind kind, ExecutionInfo executionInfo, TInput input, Func<Activity, ExecutionInfo, TInput, CancellationToken, Task> handler, CancellationToken cancellationToken)
        {
            using (var activity = CreateActivity(traceName, kind, _activitySource))
            {
                SetDefaultActivityTags(activity, executionInfo);

                try
                {
                    activity.SetStatus(Status.Ok);
                    await handler(activity, executionInfo, input, cancellationToken);
                }
                catch (Exception ex)
                {
                    activity.SetStatus(ActivityStatusCode.Error, ex.Message);
                    activity.RecordException(ex);
                    throw;
                } 
            }
        }
        public Task StartInternalActivityAsync<TInput>(string traceName, ExecutionInfo executionInfo, TInput input, Func<Activity, ExecutionInfo, TInput, CancellationToken, Task> handler, CancellationToken cancellationToken) => StartActivityAsync(traceName, kind: ActivityKind.Internal, executionInfo, input, handler, cancellationToken);
        public Task StartServerActivityAsync<TInput>(string traceName, ExecutionInfo executionInfo, TInput input, Func<Activity, ExecutionInfo, TInput, CancellationToken, Task> handler, CancellationToken cancellationToken) => StartActivityAsync(traceName, kind: ActivityKind.Server, executionInfo, input, handler, cancellationToken);
        public Task StartClientActivityAsync<TInput>(string traceName, ExecutionInfo executionInfo, TInput input, Func<Activity, ExecutionInfo, TInput, CancellationToken, Task> handler, CancellationToken cancellationToken) => StartActivityAsync(traceName, kind: ActivityKind.Client, executionInfo, input, handler, cancellationToken);
        public Task StartProducerActivityAsync<TInput>(string traceName, ExecutionInfo executionInfo, TInput input, Func<Activity, ExecutionInfo, TInput, CancellationToken, Task> handler, CancellationToken cancellationToken) => StartActivityAsync(traceName, kind: ActivityKind.Producer, executionInfo, input, handler, cancellationToken);
        public Task StartConsumerActivityAsync<TInput>(string traceName, ExecutionInfo executionInfo, TInput input, Func<Activity, ExecutionInfo, TInput, CancellationToken, Task> handler, CancellationToken cancellationToken) => StartActivityAsync(traceName, kind: ActivityKind.Consumer, executionInfo, input, handler, cancellationToken);

        public async Task<TOutput> StartActivityAsync<TOutput>(string traceName, ActivityKind kind, ExecutionInfo executionInfo, Func<Activity, ExecutionInfo, CancellationToken, Task<TOutput>> handler, CancellationToken cancellationToken)
        {
            using (var activity = CreateActivity(traceName, kind, _activitySource))
            {
                SetDefaultActivityTags(activity, executionInfo);

                try
                {
                    activity.SetStatus(Status.Ok);
                    return await handler(activity, executionInfo, cancellationToken);
                }
                catch (Exception ex)
                {
                    activity.SetStatus(ActivityStatusCode.Error, ex.Message);
                    activity.RecordException(ex);
                    throw;
                } 
            }
        }
        public Task<TOutput> StartInternalActivityAsync<TOutput>(string traceName, ExecutionInfo executionInfo, Func<Activity, ExecutionInfo, CancellationToken, Task<TOutput>> handler, CancellationToken cancellationToken) => StartActivityAsync(traceName, kind: ActivityKind.Internal, executionInfo, handler, cancellationToken);
        public Task<TOutput> StartServerActivityAsync<TOutput>(string traceName, ExecutionInfo executionInfo, Func<Activity, ExecutionInfo, CancellationToken, Task<TOutput>> handler, CancellationToken cancellationToken) => StartActivityAsync(traceName, kind: ActivityKind.Server, executionInfo, handler, cancellationToken);
        public Task<TOutput> StartClientActivityAsync<TOutput>(string traceName, ExecutionInfo executionInfo, Func<Activity, ExecutionInfo, CancellationToken, Task<TOutput>> handler, CancellationToken cancellationToken) => StartActivityAsync(traceName, kind: ActivityKind.Client, executionInfo, handler, cancellationToken);
        public Task<TOutput> StartProducerActivityAsync<TOutput>(string traceName, ExecutionInfo executionInfo, Func<Activity, ExecutionInfo, CancellationToken, Task<TOutput>> handler, CancellationToken cancellationToken) => StartActivityAsync(traceName, kind: ActivityKind.Producer, executionInfo, handler, cancellationToken);
        public Task<TOutput> StartConsumerActivityAsync<TOutput>(string traceName, ExecutionInfo executionInfo, Func<Activity, ExecutionInfo, CancellationToken, Task<TOutput>> handler, CancellationToken cancellationToken) => StartActivityAsync(traceName, kind: ActivityKind.Consumer, executionInfo, handler, cancellationToken);

        public async Task<TOutput> StartActivityAsync<TInput, TOutput>(string traceName, ActivityKind kind, ExecutionInfo executionInfo, TInput input, Func<Activity, ExecutionInfo, TInput, CancellationToken, Task<TOutput>> handler, CancellationToken cancellationToken)
        {
            using (var activity = CreateActivity(traceName, kind, _activitySource))
            {
                SetDefaultActivityTags(activity, executionInfo);

                try
                {
                    activity.SetStatus(Status.Ok);
                    return await handler(activity, executionInfo, input, cancellationToken);
                }
                catch (Exception ex)
                {
                    activity.SetStatus(ActivityStatusCode.Error, ex.Message);
                    activity.RecordException(ex);
                    throw;
                } 
            }
        }
        public Task<TOutput> StartInternalActivityAsync<TInput, TOutput>(string traceName, ExecutionInfo executionInfo, TInput input, Func<Activity, ExecutionInfo, TInput, CancellationToken, Task<TOutput>> handler, CancellationToken cancellationToken) => StartActivityAsync(traceName, kind: ActivityKind.Internal, executionInfo, input, handler, cancellationToken);
        public Task<TOutput> StartServerActivityAsync<TInput, TOutput>(string traceName, ExecutionInfo executionInfo, TInput input, Func<Activity, ExecutionInfo, TInput, CancellationToken, Task<TOutput>> handler, CancellationToken cancellationToken) => StartActivityAsync(traceName, kind: ActivityKind.Server, executionInfo, input, handler, cancellationToken);
        public Task<TOutput> StartClientActivityAsync<TInput, TOutput>(string traceName, ExecutionInfo executionInfo, TInput input, Func<Activity, ExecutionInfo, TInput, CancellationToken, Task<TOutput>> handler, CancellationToken cancellationToken) => StartActivityAsync(traceName, kind: ActivityKind.Client, executionInfo, input, handler, cancellationToken);
        public Task<TOutput> StartProducerActivityAsync<TInput, TOutput>(string traceName, ExecutionInfo executionInfo, TInput input, Func<Activity, ExecutionInfo, TInput, CancellationToken, Task<TOutput>> handler, CancellationToken cancellationToken) => StartActivityAsync(traceName, kind: ActivityKind.Producer, executionInfo, input, handler, cancellationToken);
        public Task<TOutput> StartConsumerActivityAsync<TInput, TOutput>(string traceName, ExecutionInfo executionInfo, TInput input, Func<Activity, ExecutionInfo, TInput, CancellationToken, Task<TOutput>> handler, CancellationToken cancellationToken) => StartActivityAsync(traceName, kind: ActivityKind.Consumer, executionInfo, input, handler, cancellationToken);

        // Private Methods
        private static Activity CreateActivity(string traceName, ActivityKind kind, ActivitySource activitySource)
        {
            var activity = activitySource.StartActivity(traceName, kind);

            ActivityCannotbeNullException.ThrowIfActivityIsNull(activity, activitySource, traceName, kind);

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
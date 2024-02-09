using System;
using System.Diagnostics;

namespace MCIO.Observability.OpenTelemetry.Exceptions
{
    public class ActivityCannotbeNullException
        : Exception
    {
        // Properties
        public ActivitySource ActivitySource { get; }
        public string Name { get; }
        public ActivityKind Kind { get; }

        // Constructors
        private ActivityCannotbeNullException(ActivitySource activitySource, string name, ActivityKind kind)
        {
            ActivitySource = activitySource;
            Name = name;
            Kind = kind;
        }

        // Public Methods
        public static void ThrowIfActivityIsNull(Activity activity, ActivitySource activitySource, string name, ActivityKind kind)
        {
            if (activity is null)
                throw new ActivityCannotbeNullException(activitySource, name, kind);
        }
    }
}

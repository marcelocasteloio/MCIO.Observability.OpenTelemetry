using System;

namespace MCIO.Observability.OpenTelemetry.Exceptions
{
    public class InvalidHistogramInfoException
        : Exception
    {
        // Properties
        public OutputEnvelop.OutputEnvelop OutputEnvelop { get; }

        // Constructors
        private InvalidHistogramInfoException(
            OutputEnvelop.OutputEnvelop outputEnvelop
        )
        {
            OutputEnvelop = outputEnvelop;
        }

        // Public Methods
        public static void ThrowIfInvalid(OutputEnvelop.OutputEnvelop outputEnvelop)
        {
            if (!outputEnvelop.IsSuccess)
                throw new InvalidHistogramInfoException(outputEnvelop);
        }
    }
}

using System;

namespace MCIO.Observability.OpenTelemetry.Exceptions
{
    public class InvalidObservableGaugeInfoException
        : Exception
    {
        // Properties
        public OutputEnvelop.OutputEnvelop OutputEnvelop { get; }

        // Constructors
        private InvalidObservableGaugeInfoException(
            OutputEnvelop.OutputEnvelop outputEnvelop
        )
        {
            OutputEnvelop = outputEnvelop;
        }

        // Public Methods
        public static void ThrowIfInvalid(OutputEnvelop.OutputEnvelop outputEnvelop)
        {
            if (!outputEnvelop.IsSuccess)
                throw new InvalidObservableGaugeInfoException(outputEnvelop);
        }
    }
}

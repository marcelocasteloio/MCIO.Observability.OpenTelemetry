using MCIO.Observability.Abstractions;
using MCIO.Observability.Abstractions.Models;
using MCIO.Observability.OpenTelemetry.Exceptions;
using System;
using System.Collections.Generic;

namespace MCIO.Observability.OpenTelemetry
{
    public class MetricsManager
        : IMetricsManager
    {
        // Constants
        public const string COUNTER_ALREADY_EXISTS_ERROR_MESSAGE = "Counter already exists | Name: {0}";
        public const string COUNTER_NOT_FOUND_ERROR_MESSAGE = "Counter not found | Name: {0}";

        public const string HISTOGRAM_ALREADY_EXISTS_ERROR_MESSAGE = "Histogram already exists | Name: {0}";
        public const string HISTOGRAM_NOT_FOUND_ERROR_MESSAGE = "Histogram not found | Name: {0}";

        public const string OBSERVABLE_GAUGE_ALREADY_EXISTS_ERROR_MESSAGE = "Observable gauge already exists | Name: {0}";
        public const string OBSERVABLE_GAUGE_NOT_FOUND_ERROR_MESSAGE = "Observable gauge not found | Name: {0}";

        // Fields
        private readonly System.Diagnostics.Metrics.Meter _meter;
        private readonly Dictionary<string, object> _counterDictionary;
        private readonly List<Counter> _counterColection;

        private readonly Dictionary<string, object> _histogramDictionary;
        private readonly List<Histogram> _histogramColection;

        private readonly Dictionary<string, object> _observableGaugeDictionary;
        private readonly List<ObservableGauge> _observableGaugeColection;

        // Constructors
        public MetricsManager(System.Diagnostics.Metrics.Meter meter)
        {
            _meter = meter;

            _counterDictionary = new Dictionary<string, object>();
            _counterColection = new List<Counter>();

            _histogramDictionary = new Dictionary<string, object>();
            _histogramColection = new List<Histogram>();

            _observableGaugeDictionary = new Dictionary<string, object>();
            _observableGaugeColection = new List<ObservableGauge>();
        }

        // Public Methods
        public void CreateCounter<T>(string name, string unit = null, string description = null)
            where T : struct
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            if (_counterDictionary.ContainsKey(name))
                throw new ArgumentException(message: string.Format(COUNTER_ALREADY_EXISTS_ERROR_MESSAGE, name), paramName: nameof(name));

            var createCounterProcessResult = Counter.Create(name, description, unit);

            // Stryker disable once all
            InvalidCounterInfoException.ThrowIfInvalid(createCounterProcessResult);

            _counterDictionary.Add(name, _meter.CreateCounter<T>(name, unit, description));
            _counterColection.Add(createCounterProcessResult.Output.Value);
        }
        public void IncrementCounter<T>(string name, T delta)
            where T : struct
        {
            if (string.IsNullOrWhiteSpace(name))
                // Stryker disable once all
                throw new ArgumentNullException(nameof(name));

            if (!_counterDictionary.ContainsKey(name))
                // Stryker disable once all
                throw new ArgumentOutOfRangeException(message: string.Format(COUNTER_NOT_FOUND_ERROR_MESSAGE, name), paramName: nameof(name));

            var counter = (System.Diagnostics.Metrics.Counter<T>)_counterDictionary[name];

            // Stryker disable once all
            counter.Add(delta);
        }
        public void IncrementCounter<T>(string name, T delta, params KeyValuePair<string, object>[] tags)
            where T : struct
        {
            if (string.IsNullOrWhiteSpace(name))
                // Stryker disable once all
                throw new ArgumentNullException(nameof(name));

            if (!_counterDictionary.ContainsKey(name))
                // Stryker disable once all
                throw new ArgumentOutOfRangeException(message: string.Format(COUNTER_NOT_FOUND_ERROR_MESSAGE, name), paramName: nameof(name));

            var counter = (System.Diagnostics.Metrics.Counter<T>)_counterDictionary[name];

            // Stryker disable once all
            counter.Add(delta, tags);
        }
        public IEnumerable<Counter> GetCounterCollection()
        {
            return _counterColection.AsReadOnly();
        }

        public void CreateHistogram<T>(string name, string unit = null, string description = null)
            where T : struct
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            if (_histogramDictionary.ContainsKey(name))
                throw new ArgumentException(message: string.Format(HISTOGRAM_ALREADY_EXISTS_ERROR_MESSAGE, name), paramName: nameof(name));

            var createHistogramProcessResult = Histogram.Create(name, description, unit);

            // Stryker disable once all
            InvalidHistogramInfoException.ThrowIfInvalid(createHistogramProcessResult);

            _histogramDictionary.Add(name, _meter.CreateHistogram<T>(name, unit, description));
            _histogramColection.Add(createHistogramProcessResult.Output.Value);
        }
        public void RecordHistogram<T>(string name, T value)
            where T : struct
        {
            if (string.IsNullOrWhiteSpace(name))
                // Stryker disable once all
                throw new ArgumentNullException(nameof(name));

            if (!_histogramDictionary.ContainsKey(name))
                // Stryker disable once all
                throw new ArgumentOutOfRangeException(message: string.Format(HISTOGRAM_NOT_FOUND_ERROR_MESSAGE, name), paramName: nameof(name));

            var histogram = (System.Diagnostics.Metrics.Histogram<T>)_histogramDictionary[name];

            // Stryker disable once all
            histogram.Record(value);
        }
        public void RecordHistogram<T>(string name, T value, KeyValuePair<string, object>[] tags)
            where T : struct
        {
            if (string.IsNullOrWhiteSpace(name))
                // Stryker disable once all
                throw new ArgumentNullException(nameof(name));

            if (!_histogramDictionary.ContainsKey(name))
                // Stryker disable once all
                throw new ArgumentOutOfRangeException(message: string.Format(HISTOGRAM_NOT_FOUND_ERROR_MESSAGE, name), paramName: nameof(name));

            var histogram = (System.Diagnostics.Metrics.Histogram<T>)_histogramDictionary[name];

            // Stryker disable once all
            histogram.Record(value, tags);
        }
        public IEnumerable<Histogram> GetHistogramCollection()
        {
            return _histogramColection.AsReadOnly();
        }

        public void CreateObservableGauge<T>(string name, Func<IEnumerable<System.Diagnostics.Metrics.Measurement<T>>> observeValues, string unit = null, string description = null)
            where T : struct
        {
            if (string.IsNullOrWhiteSpace(name))
                // Stryker disable once all
                throw new ArgumentNullException(nameof(name));

            if (observeValues is null)
                // Stryker disable once all
                throw new ArgumentNullException(nameof(observeValues));

            if (_observableGaugeDictionary.ContainsKey(name))
                throw new ArgumentException(message: string.Format(OBSERVABLE_GAUGE_ALREADY_EXISTS_ERROR_MESSAGE, name), paramName: nameof(name));

            var createObservableGaugeProcessResult = ObservableGauge.Create(name, description, unit);

            // Stryker disable once all
            InvalidObservableGaugeInfoException.ThrowIfInvalid(createObservableGaugeProcessResult);

            _observableGaugeDictionary.Add(name, _meter.CreateObservableGauge(name, observeValues, unit, description));
            _observableGaugeColection.Add(createObservableGaugeProcessResult.Output.Value);
        }
        public IEnumerable<ObservableGauge> GetObservableGaugeCollection()
        {
            return _observableGaugeColection.AsReadOnly();
        }
    }
}

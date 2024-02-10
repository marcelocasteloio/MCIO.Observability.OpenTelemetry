using FluentAssertions;
using System.Diagnostics.Metrics;

namespace MCIO.Observability.OpenTelemetry.UnitTests;
public class MetricsManagerTest
{
    [Fact]
    public void MetricsManager_Should_Create_Counter()
    {
        // Arrange
        var metricsManager = CreateMetricsManager();
        var nameCollection = new[]{
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString()
        };
        var unit = Guid.NewGuid().ToString();
        var description = Guid.NewGuid().ToString();

        // Act
        foreach (var name in nameCollection)
            metricsManager.CreateCounter<int>(name, unit, description);

        var counterCollection = metricsManager.GetCounterCollection();

        // Assert
        counterCollection.Should().NotBeNull();
        counterCollection.Should().HaveCount(nameCollection.Length);

        foreach (var name in nameCollection)
            counterCollection.Any(q => q.Name == name).Should().BeTrue();
    }
    [Fact]
    public void MetricsManager_Should_Not_Create_Counter_With_Empty_Name()
    {
        // Arrange
        var metricsManager = CreateMetricsManager();
        var exceptionThrown = default(ArgumentNullException);

        // Act
        try
        {
            metricsManager.CreateCounter<int>(name: null);
        }
        catch (ArgumentNullException ex)
        {
            exceptionThrown = ex;
        }

        // Assert
        exceptionThrown.Should().NotBeNull();
        exceptionThrown!.ParamName.Should().Be("name");
    }
    [Fact]
    public void MetricsManager_Should_Not_Create_Counter_With_Duplicated_Name()
    {
        // Arrange
        var metricsManager = CreateMetricsManager();
        var exceptionThrown = default(ArgumentException);
        var name = Guid.NewGuid().ToString();
        var expectedMessage = string.Format(MetricsManager.COUNTER_ALREADY_EXISTS_ERROR_MESSAGE, name);

        // Act
        try
        {
            metricsManager.CreateCounter<int>(name);
            metricsManager.CreateCounter<int>(name);
        }
        catch (ArgumentException ex)
        {
            exceptionThrown = ex;
        }

        // Assert
        exceptionThrown.Should().NotBeNull();
        exceptionThrown!.ParamName.Should().Be("name");
        exceptionThrown!.Message.Should().Contain(expectedMessage);
    }
    [Fact]
    public void MetricsManager_Should_Increment_Counter()
    {
        // Arrange
        var metricsManager = CreateMetricsManager();
        var name = Guid.NewGuid().ToString();
        var unit = Guid.NewGuid().ToString();
        var description = Guid.NewGuid().ToString();
        var tags = new KeyValuePair<string, object>[]
        {
            new(key: "A", value: 1 ),
            new(key: "B", value: 2 ),
        };

        metricsManager.CreateCounter<int>(name, unit, description);

        // Act
        var handler = () =>
        {
            metricsManager.IncrementCounter(name, delta: 1);
            metricsManager.IncrementCounter(name, delta: 1, tags);
        };

        // Assert
        handler.Should().NotThrow();
    }
    [Fact]
    public void MetricsManager_Should_Not_Increment_Counter()
    {
        // Arrange
        var metricsManager = CreateMetricsManager();
        var name = Guid.NewGuid().ToString();
        var unit = Guid.NewGuid().ToString();
        var description = Guid.NewGuid().ToString();

        metricsManager.CreateCounter<int>(name, unit, description);

        // Act
        var handlerCollection = new Action[] {
            () => metricsManager.IncrementCounter(name: null, delta: 1),
            () => metricsManager.IncrementCounter(name: Guid.NewGuid().ToString(), delta: 1)
        };

        // Assert
        foreach (var handler in handlerCollection)
            handler.Should().Throw<Exception>();
    }

    [Fact]
    public void MetricsManager_Should_Create_Histogram()
    {
        // Arrange
        var metricsManager = CreateMetricsManager();
        var nameCollection = new[]{
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString()
        };
        var unit = Guid.NewGuid().ToString();
        var description = Guid.NewGuid().ToString();

        // Act
        foreach (var name in nameCollection)
            metricsManager.CreateHistogram<int>(name, unit, description);

        var histogramCollection = metricsManager.GetHistogramCollection();

        // Assert
        histogramCollection.Should().NotBeNull();
        histogramCollection.Should().HaveCount(nameCollection.Length);

        foreach (var name in nameCollection)
            histogramCollection.Any(q => q.Name == name).Should().BeTrue();
    }
    [Fact]
    public void MetricsManager_Should_Not_Create_Histogram_With_Empty_Name()
    {
        // Arrange
        var metricsManager = CreateMetricsManager();
        var exceptionThrown = default(ArgumentNullException);

        // Act
        try
        {
            metricsManager.CreateHistogram<int>(name: null);
        }
        catch (ArgumentNullException ex)
        {
            exceptionThrown = ex;
        }

        // Assert
        exceptionThrown.Should().NotBeNull();
        exceptionThrown!.ParamName.Should().Be("name");
    }
    [Fact]
    public void MetricsManager_Should_Not_Create_Histogram_With_Duplicated_Name()
    {
        // Arrange
        var metricsManager = CreateMetricsManager();
        var exceptionThrown = default(ArgumentException);
        var name = Guid.NewGuid().ToString();
        var expectedMessage = string.Format(MetricsManager.HISTOGRAM_ALREADY_EXISTS_ERROR_MESSAGE, name);

        // Act
        try
        {
            metricsManager.CreateHistogram<int>(name);
            metricsManager.CreateHistogram<int>(name);
        }
        catch (ArgumentException ex)
        {
            exceptionThrown = ex;
        }

        // Assert
        exceptionThrown.Should().NotBeNull();
        exceptionThrown!.ParamName.Should().Be("name");
        exceptionThrown!.Message.Should().Contain(expectedMessage);
    }

    [Fact]
    public void MetricsManager_Should_Create_ObservableGauge()
    {
        // Arrange
        var metricsManager = CreateMetricsManager();
        var nameCollection = new[]{
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString()
        };
        var observeValues = Enumerable.Range(1, 10).Select(q => new Measurement<int>(q)).ToArray();
        var unit = Guid.NewGuid().ToString();
        var description = Guid.NewGuid().ToString();

        // Act
        foreach (var name in nameCollection)
            metricsManager.CreateObservableGauge(name, observeValues: () => observeValues, unit, description);

        var observableGaugeCollection = metricsManager.GetObservableGaugeCollection();

        // Assert
        observableGaugeCollection.Should().NotBeNull();
        observableGaugeCollection.Should().HaveCount(nameCollection.Length);

        foreach (var name in nameCollection)
            observableGaugeCollection.Any(q => q.Name == name).Should().BeTrue();
    }
    [Fact]
    public void MetricsManager_Should_Not_Create_ObservableGauge_With_Empty_Name()
    {
        // Arrange
        var metricsManager = CreateMetricsManager();
        var exceptionThrown = default(ArgumentNullException);

        // Act
        try
        {
            metricsManager.CreateObservableGauge<int>(name: null, observeValues: default);
        }
        catch (ArgumentNullException ex)
        {
            exceptionThrown = ex;
        }

        // Assert
        exceptionThrown.Should().NotBeNull();
        exceptionThrown!.ParamName.Should().Be("name");
    }
    [Fact]
    public void MetricsManager_Should_Not_Create_ObservableGauge_With_Null_ObserveValues()
    {
        // Arrange
        var metricsManager = CreateMetricsManager();
        var exceptionThrown = default(ArgumentNullException);

        // Act
        try
        {
            metricsManager.CreateObservableGauge<int>(name: Guid.NewGuid().ToString(), observeValues: null);
        }
        catch (ArgumentNullException ex)
        {
            exceptionThrown = ex;
        }

        // Assert
        exceptionThrown.Should().NotBeNull();
        exceptionThrown!.ParamName.Should().Be("observeValues");
    }
    [Fact]
    public void MetricsManager_Should_Not_Create_ObservableGauge_With_Duplicated_Name()
    {
        // Arrange
        var metricsManager = CreateMetricsManager();
        var exceptionThrown = default(ArgumentException);
        var name = Guid.NewGuid().ToString();
        var observeValues = Enumerable.Range(1, 10).Select(q => new Measurement<int>(q)).ToArray();
        var expectedMessage = string.Format(MetricsManager.OBSERVABLE_GAUGE_ALREADY_EXISTS_ERROR_MESSAGE, name);

        // Act
        try
        {
            metricsManager.CreateObservableGauge(name, observeValues: () => observeValues);
            metricsManager.CreateObservableGauge(name, observeValues: () => observeValues);
        }
        catch (ArgumentException ex)
        {
            exceptionThrown = ex;
        }

        // Assert
        exceptionThrown.Should().NotBeNull();
        exceptionThrown!.ParamName.Should().Be("name");
        exceptionThrown!.Message.Should().Contain(expectedMessage);
    }

    // Private Methods
    private static MetricsManager CreateMetricsManager()
    {
        return new MetricsManager(
            new Meter(name: Guid.NewGuid().ToString(), version: "1")
        );
    }
}

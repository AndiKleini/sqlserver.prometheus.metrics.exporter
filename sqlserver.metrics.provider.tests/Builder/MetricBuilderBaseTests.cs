using FluentAssertions;
using NUnit.Framework;
using SqlServer.Metrics.Provider.Tests.Builder.Exposals;

namespace SqlServer.Metrics.Provider.Tests.Builder
{
    [TestFixture]
    public class MetricBuilderBaseTests
    {
        [Test]
        public void GetMetricName_ProcMetricValueProvided_ReturnsMetricName()
        {
            const string ProcedureName = "myProc";
            const string MetricsName = "myMetrics";
            string metricsName = new MetricBuilderBaseExposal().GetMetricsName(ProcedureName, MetricsName);
            metricsName.Should().Be($"MSSQL_{MetricsName}{{storedprocedure=\"{ProcedureName}\"}}");
        }
    }
}

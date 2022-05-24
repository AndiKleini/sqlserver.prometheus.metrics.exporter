using FluentAssertions;
using Moq;
using NUnit.Framework;
using SqlServer.Metrics.Provider.Builder;
using SqlServer.Metrics.Provider;
using System.Linq;

namespace SqlServer.Metrics.Provider.Tests.Builder
{
    [TestFixture]
    public class StandardMetricsBuilderTests
    {
        [Test]
        public void Build_WithRegisteredMetricsBuilder_Executed()
        {
            string storedProcedureName = "MySp";
            const int maxElapsedTime = 150;
            const int minElapsedTime = 10;
            const int executionCount = 23;
            MetricItem maxMetricItem = MetricItemFactoryMethod.GetMetricItem(storedProcedureName, "ElapsedTimeMax", maxElapsedTime);
            MetricItem minMetricItem = MetricItemFactoryMethod.GetMetricItem(storedProcedureName, "ElapsedTimeMin", minElapsedTime);
            MetricItem executionCountMetricItem = MetricItemFactoryMethod.GetMetricItem(storedProcedureName, "ExecutionCount", executionCount);
            
            var elapsedTimeMayMockery = new Mock<IMetricsBuilder>();
            elapsedTimeMayMockery.Setup(s => s.Build(It.IsAny<IGrouping<string, PlanCacheItem>>()))
                .Returns(new[] { maxMetricItem });
            var elapsedTimeMinMockery = new Mock<IMetricsBuilder>();
            elapsedTimeMinMockery.Setup(s => s.Build(It.IsAny<IGrouping<string, PlanCacheItem>>()))
                .Returns(new[] { minMetricItem });

            StandardMetricsBuilder instanceUnderTest = new StandardMetricsBuilder();
            instanceUnderTest.Include(elapsedTimeMayMockery.Object);
            instanceUnderTest.Include(elapsedTimeMinMockery.Object);

            var resultedItemsBuild = instanceUnderTest.Build(null);

            resultedItemsBuild.Should().BeEquivalentTo(new[] { maxMetricItem, minMetricItem });
        }
    }
}

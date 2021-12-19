using FluentAssertions;
using Moq;
using NUnit.Framework;
using Sqlserver.Metrics.Provider.Builder;
using SqlServer.Metrics.Provider;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sqlserver.Metrics.Provider.Tests
{
    [TestFixture]
    public class StandardMetricsBuilderTests
    {
        [Test]
        public void Build_WithRegisteredSubMetricsBuilder_Executed()
        {
            string storedProcedureName = "MySp";
            int maxElapsedTime = 150;
            int minElapsedTime = 10;
            MetricItem maxMetricItem = new MetricItem()
            {
                Name = $"{storedProcedureName}_ElapsedTimeMax",
                Value = maxElapsedTime
            };
            MetricItem minMetricItem = new MetricItem()
            {
                Name = $"{storedProcedureName}_ElapsedTimeMin",
                Value = minElapsedTime
            };
         
            var elapsedTimeMayMockery = new Mock<IMetricsBuilder>();
            elapsedTimeMayMockery.Setup(s => s.Build(It.IsAny<IGrouping<string, PlanCacheItem>>())).Returns(
                new[] { maxMetricItem });
            var elapsedTimeMinMockery = new Mock<IMetricsBuilder>();
            elapsedTimeMinMockery.Setup(s => s.Build(It.IsAny<IGrouping<string, PlanCacheItem>>())).Returns(
                 new[] { minMetricItem });

            StandardMetricsBuilder instanceUnderTest = new StandardMetricsBuilder();
            instanceUnderTest.Include(elapsedTimeMayMockery.Object);
            instanceUnderTest.Include(elapsedTimeMinMockery.Object);

            var resultedItems = instanceUnderTest.Build(null);

            resultedItems.Should().BeEquivalentTo(new[] { maxMetricItem, minMetricItem });
        }
    }
}

using NUnit.Framework;
using SqlServer.Metrics.Provider;
using System.Collections.Generic;
using FluentAssertions.Collections;
using FluentAssertions;
using System;
using Moq;

namespace sqlserver.metrics.exporter.engine.tests
{
    [TestFixture]
    public class StoredProcedureMetricsProviderTest
    {
        [Test]
        public void Export_RetreieveStatisticsOnlyFromPlanCache_MetricsEmitted()
        {
            List<MetricItem> expectedItems = new List<MetricItem>() { new MetricItem() };
            DateTime from = DateTime.Parse("2021-12-12 13:00");
            DateTime to = DateTime.Parse("2021-12-12 13:05");
            var mockery = new Mock<IPlanCacheRepository>();
            mockery
                .Setup(s => s.GetPlanCache(from, to))
                .Returns(new List<PlanCacheItem>()
                {
                    new PlanCacheItem() {  } 
                });
            IPlanCacheRepository planCacherepository = mockery.Object;
            var instanceUnderTest = new StoredProcedureMetricsProvider(planCacherepository);

            List<MetricItem> items = instanceUnderTest.Collect(from, to);

            items.Should().BeEquivalentTo(expectedItems);
        }
    }
}

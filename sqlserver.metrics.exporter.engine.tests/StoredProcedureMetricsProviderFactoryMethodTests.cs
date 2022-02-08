using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using Sqlserver.Metrics.Provider.Builder;
using SqlServer.Metrics.Provider;
using System.Collections.Generic;

namespace Sqlserver.Metrics.Provider.Tests
{
    [TestFixture]
    public class StoredProcedureMetricsProviderFactoryMethodTests
    {
        [Test]
        public void Create_MetricsBuilderAndPlanCacherepoAdded_ReturnsProperProviderInstance()
        {
            List<IMetricsBuilder> expectedMetricsBuilders = 
                new List<IMetricsBuilder>()
                    {
                        new AverageElapsedTimeMetricsBuilder(),
                        new MaxElapsedTimeMetricsBuilder(),
                        new MinElapsedTimeMetricsBuilder()
                    };
            BuilderTypes[] builderInUse =
                new BuilderTypes[]
                {
                    BuilderTypes.AverageElapsedTimeMetricsBuilder,
                    BuilderTypes.ExecutionCountMetricsBuilder,
                    BuilderTypes.MaxElapsedTimeMetricsBuilder,
                    BuilderTypes.MinElapsedTimeMetricsBuilder
                };
            var repositoryCache = new Mock<IPlanCacheRepository>();
            var previousItemCache = new Mock<IPreviousItemCache>();

            IStoredProcedureMetricsProvider createdInstance = 
                StoredProcedureMetricsProviderFactoryMethod.Create(repositoryCache.Object, previousItemCache.Object, builderInUse);

            using (new AssertionScope())
            {
                List<IMetricsBuilder> builders = createdInstance.GetMetricBuilders();
                builders.Should().Contain(s => s.GetType() == typeof(AverageElapsedTimeMetricsBuilder)).
                    And.Contain(s => s.GetType() == typeof(MaxElapsedTimeMetricsBuilder)).
                    And.Contain(s => s.GetType() == typeof(MinElapsedTimeMetricsBuilder)).
                    And.Contain(s => s.GetType() == typeof(ExecutionCountMetricsBuilder));
            }
        }
    }
}

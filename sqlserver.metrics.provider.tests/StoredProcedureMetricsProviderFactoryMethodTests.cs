using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SqlServer.Metrics.Provider.Builder;
using System.Collections.Generic;
using Sqlserver.Metrics.Provider.Builder;

namespace SqlServer.Metrics.Provider.Tests
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
                    BuilderTypes.MinElapsedTimeMetricsBuilder,
                    BuilderTypes.LastElapsedTimeMetricsBuilder,
                    BuilderTypes.MaxPhysicalReadsMetricsBuilder,
                    BuilderTypes.MinPhysicalReadsMetricsBuilder,
                    BuilderTypes.LastPhysicalReadsMetricsBuilder,
                    BuilderTypes.AveragePhysicalRreadsMetricsBuilder,
                    BuilderTypes.MaxLogicalReadsMetricsBuilder,
                    BuilderTypes.MinLogicalReadsMetricsBuilder,
                    BuilderTypes.LastLogicalReadsMetricsBuilder,
                    BuilderTypes.AverageLogicalRreadsMetricsBuilder,
                    BuilderTypes.MaxPageServerReadsMetricsBuilder,
                    BuilderTypes.MinPageServerReadsMetricsBuilder,
                    BuilderTypes.LastPageServerReadsMetricsBuilder,
                    BuilderTypes.AveragePageServerReadsMetricsBuilder,
                    BuilderTypes.MaxLogicalWritesMetricsBuilder,
                    BuilderTypes.MinLogicalWritesMetricsBuilder,
                    BuilderTypes.LastLogicalWritesMetricsBuilder,
                    BuilderTypes.AverageLogicalWritesMetricsBuilder,
                    BuilderTypes.MaxWorkerTimeMetricsBuilder,
                    BuilderTypes.MinWorkerTimeMetricsBuilder,
                    BuilderTypes.LastWorkerTimeMetricsBuilder,
                    BuilderTypes.AverageWorkerTimeMetricsBuilder,
                    BuilderTypes.MaxPageSpillsMetricsBuilder,
                    BuilderTypes.MinPageSpillsMetricsBuilder,
                    BuilderTypes.LastPageSpillsMetricsBuilder,
                    BuilderTypes.AveragePageSpillsMetricsBuilder,
                    BuilderTypes.EstimatedExecutionCountBuilder
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
                    And.Contain(s => s.GetType() == typeof(ExecutionCountMetricsBuilder)).
                    And.Contain(s => s.GetType() == typeof(LastElapsedTimeMetricsBuilder)).
                    And.Contain(s => s.GetType() == typeof(GenericMaxMetricsBuilder) && ((GenericMaxMetricsBuilder)s).MetricsName == "PhysicalReadsMax").
                    And.Contain(s => s.GetType() == typeof(GenericMinMetricsBuilder) && ((GenericMinMetricsBuilder)s).MetricsName == "PhysicalReadsMin").
                    And.Contain(s => s.GetType() == typeof(GenericLastMetricsBuilder) && ((GenericLastMetricsBuilder)s).MetricsName == "PhysicalReadsLast").
                    And.Contain(s => s.GetType() == typeof(GenericAverageMetricsBuilder) && ((GenericAverageMetricsBuilder)s).MetricsName == "PhysicalReadsAverage").
                    And.Contain(s => s.GetType() == typeof(GenericMaxMetricsBuilder) && ((GenericMaxMetricsBuilder)s).MetricsName == "LogicalReadsMax").
                    And.Contain(s => s.GetType() == typeof(GenericMinMetricsBuilder) && ((GenericMinMetricsBuilder)s).MetricsName == "LogicalReadsMin").
                    And.Contain(s => s.GetType() == typeof(GenericLastMetricsBuilder) && ((GenericLastMetricsBuilder)s).MetricsName == "LogicalReadsLast").
                    And.Contain(s => s.GetType() == typeof(GenericAverageMetricsBuilder) && ((GenericAverageMetricsBuilder)s).MetricsName == "LogicalReadsAverage").
                    And.Contain(s => s.GetType() == typeof(GenericMaxMetricsBuilder) && ((GenericMaxMetricsBuilder)s).MetricsName == "PageServerReadsMax").
                    And.Contain(s => s.GetType() == typeof(GenericMinMetricsBuilder) && ((GenericMinMetricsBuilder)s).MetricsName == "PageServerReadsMin").
                    And.Contain(s => s.GetType() == typeof(GenericLastMetricsBuilder) && ((GenericLastMetricsBuilder)s).MetricsName == "PageServerReadsLast").
                    And.Contain(s => s.GetType() == typeof(GenericAverageMetricsBuilder) && ((GenericAverageMetricsBuilder)s).MetricsName == "PageServerReadsAverage").
                    And.Contain(s => s.GetType() == typeof(GenericMaxMetricsBuilder) && ((GenericMaxMetricsBuilder)s).MetricsName == "LogicalWritesMax").
                    And.Contain(s => s.GetType() == typeof(GenericMinMetricsBuilder) && ((GenericMinMetricsBuilder)s).MetricsName == "LogicalWritesMin").
                    And.Contain(s => s.GetType() == typeof(GenericLastMetricsBuilder) && ((GenericLastMetricsBuilder)s).MetricsName == "LogicalWritesLast").
                    And.Contain(s => s.GetType() == typeof(GenericAverageMetricsBuilder) && ((GenericAverageMetricsBuilder)s).MetricsName == "LogicalWritesAverage").
                    And.Contain(s => s.GetType() == typeof(GenericMaxMetricsBuilder) && ((GenericMaxMetricsBuilder)s).MetricsName == "WorkerTimeMax").
                    And.Contain(s => s.GetType() == typeof(GenericMinMetricsBuilder) && ((GenericMinMetricsBuilder)s).MetricsName == "WorkerTimeMin").
                    And.Contain(s => s.GetType() == typeof(GenericLastMetricsBuilder) && ((GenericLastMetricsBuilder)s).MetricsName == "WorkerTimeLast").
                    And.Contain(s => s.GetType() == typeof(GenericAverageMetricsBuilder) && ((GenericAverageMetricsBuilder)s).MetricsName == "WorkerTimeAverage").
                    And.Contain(s => s.GetType() == typeof(GenericMaxMetricsBuilder) && ((GenericMaxMetricsBuilder)s).MetricsName == "PageSpillsMax").
                    And.Contain(s => s.GetType() == typeof(GenericMinMetricsBuilder) && ((GenericMinMetricsBuilder)s).MetricsName == "PageSpillsMin").
                    And.Contain(s => s.GetType() == typeof(GenericLastMetricsBuilder) && ((GenericLastMetricsBuilder)s).MetricsName == "PageSpillsLast").
                    And.Contain(s => s.GetType() == typeof(GenericAverageMetricsBuilder) && ((GenericAverageMetricsBuilder)s).MetricsName == "PageSpillsAverage").
                    And.Contain(s => s.GetType() == typeof(EstimatedExecutionCountMetricsBuilder));
            }
        }
    }
}

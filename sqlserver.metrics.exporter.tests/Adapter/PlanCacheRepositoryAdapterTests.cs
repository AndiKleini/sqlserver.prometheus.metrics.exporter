using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SqlServer.Metrics.Exporter.Adapters;
using SqlServer.Metrics.Exporter.Database;
using SqlServer.Metrics.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sqlserver.Metrics.Exporter.Tests.Adapter
{
    [TestFixture]
    public class PlanCacheRepositoryAdapterTests
    {
        [Test]
        public async Task GetPlanCache_PlanCacheDataRetrieved_ReturnsProperListOfPlanCacheItems()
        {
            Mock<IDbPlanCacheRepository> planCacheRepositoryMockery = new Mock<IDbPlanCacheRepository>();
            const int objectIdSp1 = 1234;
            const int objectIdSp2 = 3456;
            const string sp1Name = "myschema1.myProcedure1";
            const string sp2Name = "myschema2.myProcedure2";
            DateTime includeHistoricalItemsFrom = DateTime.Now;
            DateTime collectMetricsFrom = DateTime.Now;
            Dictionary<int, string> lookup
                = new Dictionary<int, string>()
                {
                    { objectIdSp1, sp1Name },
                    { objectIdSp2, sp2Name }
                };
            planCacheRepositoryMockery.Setup(s => s.GetObjectIdAndProcedureNames()).ReturnsAsync(lookup);
            List<PlanCacheItem> currentPlanCacheItems = new List<PlanCacheItem>() { new PlanCacheItem() { ObjectId = objectIdSp1 } };
            planCacheRepositoryMockery.Setup(s => s.GetCurrentPlanCache(collectMetricsFrom)).ReturnsAsync(currentPlanCacheItems);
            List<PlanCacheItem> historicalPlanCacheItem = new List<PlanCacheItem>() { new PlanCacheItem() { ObjectId = objectIdSp2 } };
            planCacheRepositoryMockery.Setup(s => s.GetHistoricalPlanCache(includeHistoricalItemsFrom)).ReturnsAsync(historicalPlanCacheItem);
            var instanceUnderTest = new PlanCacheRepositoryAdapter(planCacheRepositoryMockery.Object);

            var yieldPlanCacheItem = await instanceUnderTest.GetPlanCache(collectMetricsFrom, includeHistoricalItemsFrom);

            using (var scope = new AssertionScope())
            {
                yieldPlanCacheItem.Should().BeEquivalentTo(currentPlanCacheItems.Concat(historicalPlanCacheItem));
                yieldPlanCacheItem.Should().Contain(p => p.SpName == sp1Name).And.Contain(p => p.SpName == sp2Name);
            }
            planCacheRepositoryMockery.VerifyAll();
        }

        [Test]
        public async Task GetPlanCache_PlanCacheDataRetrievedButOneObjectIdIsMissing_OmitsProcedureWithMissingObjectIdInResult()
        {
            Mock<IDbPlanCacheRepository> planCacheRepositoryMockery = new Mock<IDbPlanCacheRepository>();
            const int objectIdSp1 = 1234;
            const int missingObjectIdSp2 = 3456;
            const string sp1Name = "myProcedure1";
            const string sp2Name = "myProcedure2";
            DateTime includeHistoricalItemsFrom = DateTime.Now;
            DateTime collectMetricsFrom = DateTime.Now;
            Dictionary<int, string> lookup
                = new Dictionary<int, string>()
                {
                    { objectIdSp1, sp1Name }
                };
            planCacheRepositoryMockery.Setup(s => s.GetObjectIdAndProcedureNames()).ReturnsAsync(lookup);
            List<PlanCacheItem> currentPlanCacheItems = new List<PlanCacheItem>() { new PlanCacheItem() { ObjectId = objectIdSp1 } };
            planCacheRepositoryMockery.Setup(s => s.GetCurrentPlanCache(collectMetricsFrom)).ReturnsAsync(currentPlanCacheItems);
            List<PlanCacheItem> historicalPlanCacheItem = new List<PlanCacheItem>() { new PlanCacheItem() { ObjectId = missingObjectIdSp2 } };
            planCacheRepositoryMockery.Setup(s => s.GetHistoricalPlanCache(includeHistoricalItemsFrom)).ReturnsAsync(historicalPlanCacheItem);
            var instanceUnderTest = new PlanCacheRepositoryAdapter(planCacheRepositoryMockery.Object);

            var yieldPlanCacheItem = await instanceUnderTest.GetPlanCache(collectMetricsFrom, includeHistoricalItemsFrom);

            using (var scope = new AssertionScope())
            {
                yieldPlanCacheItem.Should().BeEquivalentTo(currentPlanCacheItems);
                yieldPlanCacheItem.Should().Contain(p => p.SpName == sp1Name);
            }
            planCacheRepositoryMockery.VerifyAll();
        }
    }
}

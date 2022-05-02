using FluentAssertions;
using NUnit.Framework;
using SqlServer.Metrics.Exporter.Adapters;
using SqlServer.Metrics.Provider;
using SqlServer.Metrics.Provider;

namespace SqlServer.Metrics.Exporter.Tests.Adapter
{
    [TestFixture]
    public class InMemoryPreviousItemCacheAdapterTests
    {
        [Test]
        public void StoreAndRetrievePreviousItem() 
        {
            IPreviousItemCache instanceUnderTest = new InMemoryPreviousItemCacheAdapter();
            PlanCacheItem previousPlanCacheItem = new PlanCacheItem() { ObjectId = 12345 };
            const string storedProcedureName = "myProc";

            instanceUnderTest.StorePreviousCacheItem(storedProcedureName, previousPlanCacheItem);
            var result = instanceUnderTest.GetPreviousCacheItem(storedProcedureName);

            result.Should().BeEquivalentTo(previousPlanCacheItem);
        }

        [Test]
        public void GetPreviousCacheItem_StoredProcedureNameNotExisting_ReturnsNull()
        {
            var instanceUnderTest = new InMemoryPreviousItemCacheAdapter();

            var result = instanceUnderTest.GetPreviousCacheItem("DoesNotExist");

            result.Should().BeNull();
        }

        [Test]
        public void StoreAndRetrievePreviousItem_CalledTwice_SecondValueOverwritesTheFirstValue()
        {
            var instanceUnderTest = new InMemoryPreviousItemCacheAdapter();
            PlanCacheItem firstPlanCacheItem = new PlanCacheItem() { ObjectId = 1 };
            PlanCacheItem secondPlanCacheItem = new PlanCacheItem() { ObjectId = 2 };
            const string storedProcedureName = "myProc";
            instanceUnderTest.StorePreviousCacheItem(storedProcedureName, firstPlanCacheItem);
            instanceUnderTest.StorePreviousCacheItem(storedProcedureName, secondPlanCacheItem);

            var result = instanceUnderTest.GetPreviousCacheItem(storedProcedureName);

            result.Should().BeEquivalentTo(secondPlanCacheItem);
        }
    }
}

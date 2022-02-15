using FluentAssertions;
using NUnit.Framework;
using SqlServer.Metrics.Provider;

namespace Sqlserver.Metrics.Provider.Tests
{
    [TestFixture]
    public class PlanCacheItemTests
    {
        [Test]
        public void Zero_ReturnsPlanCacheItemWithZeroExecutionCount()
        {
            PlanCacheItem expectedZeroItem = 
                new PlanCacheItem() 
                { 
                    ExecutionStatistics = 
                        new ProcedureExecutionStatistics() 
                        { 
                            GeneralStats = 
                                new GeneralStats() 
                                { 
                                    ExecutionCount = 0 
                                } 
                        } 
                };

            PlanCacheItem.Zero().Should().BeEquivalentTo(expectedZeroItem);
        }
    }
}

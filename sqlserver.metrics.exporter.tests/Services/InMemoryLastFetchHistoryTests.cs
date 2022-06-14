using FluentAssertions;
using NUnit.Framework;
using Sqlserver.Metrics.Exporter.Services;
using System;

namespace Sqlserver.Metrics.Exporter.Tests.Services
{
    [TestFixture]
    public class InMemoryLastFetchHistoryTests
    {
        [Test]
        public void GetPreviousFetch_SetPreviousFetchToWasNotCalledBefore_ReturnsNull()
        {
            Assert.IsNull(new InMemoryLastFetchHistory().GetPreviousFetch());
        }

        [Test]
        public void GetPreviousFetch_SetPreviousFetchToWasCalledBefore_ReturnsPreviousValues()
        {
            HistoricalFetch previousFetch = new HistoricalFetch() { IncludedHistoricalItemsFrom = DateTime.Now, LastFetchTime = DateTime.Now };
            var instanceUnderTest = new InMemoryLastFetchHistory();

            instanceUnderTest.SetPreviousFetchTo(previousFetch);
            var yield = instanceUnderTest.GetPreviousFetch();

            yield.Should().BeSameAs(previousFetch);
        }
    }
}

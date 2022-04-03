using FluentAssertions;
using NUnit.Framework;
using SqlServer.Metrics.Exporter.Controllers;
using System;

namespace Sqlserver.Metrics.Exporter.Tests.Controller
{
    [TestFixture]
    public class CollectionRangeTests
    {
        [Test]
        public void ToString_RetunsStringifiedInstance()
        {

            DateTime lastFetchTime = DateTime.Now;
            DateTime includedHistoricalItemsUntil = DateTime.Now;
            string expected = $"CollectionRange: LastFetchTime {lastFetchTime}, IncludedHistoricalItemsUntil = {includedHistoricalItemsUntil}";

            string stringifiedInstance = (new CollectionRange() { LastFetchTime = lastFetchTime, IncludedHistoricalItemsUntil = includedHistoricalItemsUntil }).ToString();

            stringifiedInstance.Should().Be(expected);
        }
    }
}

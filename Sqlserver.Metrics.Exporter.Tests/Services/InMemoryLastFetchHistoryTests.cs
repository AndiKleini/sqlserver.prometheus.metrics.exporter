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
        public void GetPreviousFetchAndResetToNow_CalledForTheFirstTime_ReturnsNull()
        {
            Assert.IsNull(new InMemoryLastFetchHistory().GetPreviousFetchAndResetToNow());
        }

        [Test]
        public void GetPreviousFetchAndResetToNow_CalledForTheSecondTime_ReturnsTimeStampOfPreviousCall()
        {
            var instanceUnderTest = new InMemoryLastFetchHistory();
            DateTime timeBeforeFirstFetch = DateTime.Now;
            
            _ = instanceUnderTest.GetPreviousFetchAndResetToNow();
            DateTime? resultOfSecondCall = instanceUnderTest.GetPreviousFetchAndResetToNow();

            resultOfSecondCall.Should().BeAfter(timeBeforeFirstFetch);
        }
    }
}

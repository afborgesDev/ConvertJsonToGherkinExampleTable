using FluentAssertions;

namespace ConvertJsonToGherkinExampleTable.Test.Common
{
    public static class CommonTestsHelper
    {
        public static void AssertValidTable(string ExpectedTableResult, string sut)
        {
            sut.Should().NotBeNullOrEmpty();
            sut.Should().BeEquivalentTo(ExpectedTableResult);
        }
    }
}

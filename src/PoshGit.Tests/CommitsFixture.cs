using PoshGit.Tests.TestHelpers;
using Xunit;

namespace PoshGit.Tests
{
    public class CommitsFixture : BaseFixture
    {
        [Fact]
        public void GetCommitsWithNoReferenceSpecified()
        {
            GitGommitHelper.
            Assert.Equal(0, 0);
        }
    }
}
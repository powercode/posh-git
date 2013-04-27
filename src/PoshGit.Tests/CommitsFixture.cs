using LibGit2Sharp;
using PoshGit.Tests.TestHelpers;
using Xunit;
using System.Linq;

namespace PoshGit.Tests
{
    public class CommitsFixture : BaseFixture
    {
        [Fact]
        public void GetCommitsWithNoReferenceSpecified()
        {
            string path = CloneStandardTestRepo();
            using (var commitHelper = Model.GitCommitHelper.EnumerateCommits(path, null, new Filter()))
            {
                //Assert.Equal(7, commitHelper.Commits.Count());
                var first = commitHelper.Commits.First();
                Assert.Equal("testing", first.Message);
            }
        }
    }
}
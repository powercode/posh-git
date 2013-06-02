using System.IO;
using System.Management.Automation;
using LibGit2Sharp;
using PoshGit.Tests.TestHelpers;
using Xunit;

namespace PoshGit.Tests
{
    public class StatusFixture : BaseFixture
    {
        [Fact]
        public void CanRetrieveTheStatusOfAFile()
        {
            using (var ps = PowerShell.Create())
            {
                ps.AddCommand("Get-GitStatus");
                var filePath = Path.Combine(StandardTestRepoWorkingDirPath, "new_tracked_file.txt");
                ps.AddParameter("LiteralPath", filePath);
                var output = ps.Invoke<StatusEntry>();
                Assert.Equal(1, output.Count);
                Assert.Equal(FileStatus.Added, output[0].State);
                Assert.Equal("new_tracked_file.txt", output[0].FilePath);
            }
        }
    }
}
namespace PoshGit.Tests
{
    using System.Linq;
    using System.Management.Automation;

    using LibGit2Sharp;

    using PoshGit.Model;
    using PoshGit.Tests.TestHelpers;

    using Xunit;

    /// <summary>
    /// The commits fixture.
    /// </summary>
    public class CommitsFixture : BaseFixture
    {
        /// <summary>
        /// The get commits with cmdlet.
        /// </summary>
        [Fact]
        public void GetCommitsWithCmdlet()
        {
            var path = CloneStandardTestRepo();
            using (var ps = PowerShell.Create())
            {
                ps.AddCommand("Get-GitLog");
                ps.AddParameter("LiteralPath", path);
                var output = ps.Invoke<Commit>();
                Assert.Equal(9, output.Count());

                var first = output.First();
                Assert.Equal("32eab9cb1f450b5fe7ab663462b77d7f4b703344", first.Id.Sha);
            }
        }

        /// <summary>
        /// The get cmdlet commits test reference.
        /// </summary>
        [Fact]
        public void GetCmdletCommitsTestReference()
        {
            var path = CloneStandardTestRepo();
            using (var ps = PowerShell.Create())
            {
                ps.AddCommand("Get-GitLog");
                ps.AddParameter("LiteralPath", path);
                ps.AddParameter("Reference", "test");
                var output = ps.Invoke<Commit>();
                Assert.Equal(2, output.Count());

                var first = output.First();
                Assert.Equal("e90810b8df3e80c413d903f631643c716887138d", first.Id.Sha);
            }
        }

        /// <summary>
        /// The get cmdlet commits first skip.
        /// </summary>
        [Fact]
        public void GetCmdletCommitsFirstSkip()
        {
            var path = CloneStandardTestRepo();
            using (var ps = PowerShell.Create())
            {
                ps.AddCommand("Get-GitLog");
                ps.AddParameter("LiteralPath", path);
                ps.AddParameter("First", 4);
                ps.AddParameter("Skip", 2);
                ps.AddParameter("Reference", "master");
                var output = ps.Invoke<Commit>();
                Assert.Equal(4, output.Count());

                var first = output.First();
                Assert.Equal("4c062a6361ae6959e06292c1fa5e2822d9c96345", first.Id.Sha);
            }
        }
    }
}